using System.ComponentModel.DataAnnotations;

namespace Bingo.Common.DomainModel.MasterData
{
    public class SessionSector
    {
        [Required]
        public Guid Id { get; set; }

        public Guid SessionId { get; set; }

        public Guid SectorId { get; set; }

        public bool RaceEnabled { get; set; }
    }
}
