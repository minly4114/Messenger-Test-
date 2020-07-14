using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Models
{
    public class MessageModel
    {
        public ParticipantModel Sender { get; set; }
        public bool IsSender { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Text { get; set; }
    }
}
