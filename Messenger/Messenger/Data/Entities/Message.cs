using Messenger.Data.IProviders;
using Messenger.Models;
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

        public MessageModel ToMessageModel(IUserProvider userProvider, Guid user)
        {
            var outcome = new MessageModel()
            {
                CreatedAt = CreatedAt,
                Text = Text,
                Sender = userProvider.GetUser(Sender),
                IsSender = Sender==user
            };
            return outcome;
        }
    }
}
