using System.ComponentModel.DataAnnotations;

namespace Bingo.Common.DomainModel.Timing
{
    public class SectorTime
    {
        [Required]
        public Guid Id { get; set; }

        public Guid? CompetitorId { get; set; }

        public Guid? SectorId { get; set; }

        public DateTime EntryTime { get; set; } = DateTime.MinValue;

        public DateTime ExitTime { get; set; } = DateTime.MinValue;

        public long? PenaltyTimeTicks { get; set; }

        public Int32? PenaltyPositions { get; set; }

        public string? PenaltyNote { get; set; }

        public DateTime CreationTimeStamp { get; set; }

        public DateTime LastUpdateTimeStamp { get; set; }
    }
}
