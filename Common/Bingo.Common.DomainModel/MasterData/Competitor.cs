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

        [MaxLength(20, ErrorMessage = "Lunghezza massima del numero di gara da import è 20 caratteri")]
        public string? ImportNumber { get; set; }

        public Guid? CompetitorCategoryId { get; set; }

        [Required]
        public string Name1 { get; set; } = "";

        public string? Name2 { get; set; }

        public string? Name3 { get; set; }

        public string? Name4 { get; set; }

        public string? Nationality { get; set; }

        public string? Team { get; set; }

        public string? Boat { get; set; }

        public string? Engine { get; set; }

        public string? Status { get; set; }

        public DateTime CreationTimeStamp { get; set; }

        public DateTime LastUpdateTimeStamp { get; set; }
    }
}
