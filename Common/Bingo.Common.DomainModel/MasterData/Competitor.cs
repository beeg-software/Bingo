using System.ComponentModel.DataAnnotations;

namespace Bingo.Common.DomainModel.MasterData
{
    public class Competitor
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required, MaxLength(15, ErrorMessage = "Lunghezza massima del numero di gara è 20 caratteri")]
        public string Number { get; set; }

        public DateTime CreationTimeStamp { get; set; }

        public DateTime LastUpdateTimeStamp { get; set; }
    }
}
