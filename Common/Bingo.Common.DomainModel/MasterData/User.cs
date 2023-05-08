using System.ComponentModel.DataAnnotations;

namespace Bingo.Common.DomainModel.MasterData
{
    public class User
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public DateTime TimeStamp { get; set; }

        public User(Guid id, string name, DateTime timeStamp)
        {
            Id = id;
            Name = name;
            TimeStamp = timeStamp;
        }
    }
}
