using System.ComponentModel.DataAnnotations;

namespace Bingo.Common.DomainModel.Setup
{
    public class SessionSector
    {
        [Required]
        public Guid Id { get; set; }

        public Guid? SessionId { get; set; }

        public Guid? SectorId { get; set; }

        [Required]
        public bool RaceEnabled { get; set; } = false;

        public DateTime CreationTimeStamp { get; set; }

        public DateTime LastUpdateTimeStamp { get; set; }
    }
}
