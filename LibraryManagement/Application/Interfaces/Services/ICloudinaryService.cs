using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Application.Interfaces.Services
{
    public interface ICloudinaryService
    {
        Task<(string PublicId, string Url)> UploadImageAsync(IFormFile file, string folder);
        Task DeleteImageAsync(string publicId);
    }
} 