using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Messenger.Data.Entities
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public Dialog Dialog { get; set; }
        public Guid Sender { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Text { get; set; }
        public List<Read> Reads { get; set; }
        public Message()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}
