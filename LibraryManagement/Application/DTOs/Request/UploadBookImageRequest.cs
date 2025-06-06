using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Application.DTOs.Request
{
    public class UploadBookImageRequest
    {
        public IFormFile File { get; set; }
    }
} 