using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        public string? RegionImageUrl { get; set; }

    }
}
