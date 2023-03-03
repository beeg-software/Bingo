using System.ComponentModel.DataAnnotations;

namespace Blank7.Common.DomainModel
{
    public class User
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public DateTime TimeStamp { get; set; }
    }
}
