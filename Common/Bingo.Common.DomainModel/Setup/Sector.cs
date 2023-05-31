using System.ComponentModel.DataAnnotations;

namespace Bingo.Common.DomainModel.MasterData
{
    public class Sector
    {
        [Required]
        public Guid Id { get; set; }

        [Required, StringLength(50, ErrorMessage = "Lunghezza massima del nome settore di gara è 50 caratteri")]
        public string Name { get; set; }

        public string? ImportName { get; set; }

        public decimal? Length { get; set; }

        public decimal? TargetAverageSpeed { get; set; }

        public long? MinTimeTicks { get; set; }

        public long? MaxTimeTicks { get; set; }

        public DateTime CreationTimeStamp { get; set; }

        public DateTime LastUpdateTimeStamp { get; set; }
    }
}
