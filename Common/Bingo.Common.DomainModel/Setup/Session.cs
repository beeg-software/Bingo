using System.ComponentModel.DataAnnotations;

namespace Bingo.Common.DomainModel.MasterData
{
    public class Session
    {
        [Required]
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
