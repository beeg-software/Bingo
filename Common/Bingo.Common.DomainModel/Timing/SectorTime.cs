using System.ComponentModel.DataAnnotations;

namespace Bingo.Common.DomainModel.MasterData
{
    public class SectorTime
    {
        [Required]
        public Guid Id { get; set; }

        public Guid CompetitorId { get; set; }

        [Required]
        public Guid SectorId { get; set; }

        public DateTime TimeStamp { get; set; }

        public DateTime EntryTime { get; set; }

        public DateTime ExitTime { get; set; }

        public long PenaltyTimeTicks { get; set; }

        public Int32 PenaltyPositions { get; set; }

        public string PenaltyNote { get; set; }
    }
}
