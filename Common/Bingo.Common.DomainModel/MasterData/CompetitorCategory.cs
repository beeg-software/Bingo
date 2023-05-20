using System.ComponentModel.DataAnnotations;

namespace Bingo.Common.DomainModel.MasterData
{
    public class CompetitorCategory
    {
        [Required]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime CreationTimeStamp { get; set; }

        public DateTime LastUpdateTimeStamp { get; set; }
    }
}
