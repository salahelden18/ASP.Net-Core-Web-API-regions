using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Models.DTO
{
    public class UpdateWalkRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; } 
        public string LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }
    }
}
