using System.ComponentModel.DataAnnotations;

namespace Bingo.Common.DomainModel.Setup
{
    public class Session
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime CreationTimeStamp { get; set; }

        public DateTime LastUpdateTimeStamp { get; set; }
    }
}
