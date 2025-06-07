using System.Net;
using System.Text.Json;
using LibraryManagement.Application.DTOs.Response;
using LibraryManagement.Application.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.API.Middleware
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GlobalExceptionHandler(
            RequestDelegate next, 
            ILogger<GlobalExceptionHandler> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Check for blacklisted token
                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (!string.IsNullOrEmpty(token))
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var invalidTokenRepository = scope.ServiceProvider.GetRequiredService<IInvalidTokenRepository>();
                        var isInvalid = await invalidTokenRepository.IsTokenInvalidAsync(token);
                        if (isInvalid)
                        {
                            throw new AppException(ErrorCodes.TOKEN_REVOKED);
                        }
                    }
                }

                await _next(context);
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, $"AppException: {ex.Message}");
                await HandleExceptionAsync(context, ex.ErrorCode, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unhandled exception: {ex.Message}");
                await HandleExceptionAsync(context, ErrorCodes.SERVER_ERROR, ex.Message);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, ErrorCode error, string? detail)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = error.httpStatusCode;

            var response = ApiResponse<object>.Fail(error.code, error.message);

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
            return context.Response.WriteAsync(json);
        }
    }
}
