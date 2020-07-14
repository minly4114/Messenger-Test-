using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Messenger.Data.Entities
{
    public class Dialog
    {
        [Key]
        public Guid Uuid { get; set; }
        public List<Message> Messages { get; set; }
        public List<Guid> Participants { get; set; }
        public DateTime CreatedAt { get; set; }

        public Dialog()
        {
            CreatedAt = DateTime.UtcNow;
            Uuid = Guid.NewGuid();
        }
    }
}
