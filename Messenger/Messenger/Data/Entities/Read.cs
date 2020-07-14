using System;
using System.ComponentModel.DataAnnotations;

namespace Messenger.Data.Entities
{
    public class Read
    {
        [Key]
        public int Id { get; set; }
        public Guid UuidParticipant { get; set; }
        public bool IsRead { get; set; }
    }
}
