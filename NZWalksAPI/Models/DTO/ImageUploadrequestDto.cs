using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTO
{
    public class ImageUploadrequestDto
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string FileName { get; set; }
        public string? FileDescription { get; set; }
    }
}
