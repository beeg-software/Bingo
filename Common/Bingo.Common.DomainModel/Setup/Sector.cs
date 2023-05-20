using System.ComponentModel.DataAnnotations;

namespace Bingo.Common.DomainModel.MasterData
{
    public class Sector
    {
        [Required]
        public Guid Id { get; set; }

        [Required, StringLength(50, ErrorMessage = "Lunghezza massima del nome settore di gara è 50 caratteri")]
        public string Name { get; set; }

        public string ImportName { get; set; }

        public Decimal Length { get; set; }

        public Decimal TargetAverageSpeed { get; set; }

        public long MinTimeTicks { get; set; }

        public long MaxTimeTicks { get; set; }
    }
}
