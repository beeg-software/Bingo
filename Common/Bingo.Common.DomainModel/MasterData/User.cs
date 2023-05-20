﻿using System.ComponentModel.DataAnnotations;

namespace Bingo.Common.DomainModel.MasterData
{
    public class User
    {
        [Required]
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime CreationTimeStamp { get; set; }

        public DateTime LastUpdateTimeStamp { get; set; }
    }
}
