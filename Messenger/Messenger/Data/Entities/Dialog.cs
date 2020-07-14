using Messenger.Data.IProviders;
using Messenger.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Messenger.Data.Entities
{
    public class Dialog
    {
        [Key]
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public List<Message> Messages { get; set; }
        public Guid Creator { get; set; }
        public List<string> Participants { get; set; }
        public DateTime CreatedAt { get; set; }

        public Dialog()
        {
            CreatedAt = DateTime.UtcNow;
            Uuid = Guid.NewGuid();
        }

        public ShortDialogModel ToShortDialog(IUserProvider userProvider, Guid user)
        {
            var outcome = new ShortDialogModel()
            {
                Uuid = Uuid,
                Name = Name,
                Participants = Participants.ToList().ConvertAll(x => userProvider.GetUser(Guid.Parse(x))?.Name ),
                IsCreator = Creator == user,
                NumberUnread = 0,
                CreatedAt = CreatedAt
            };

            foreach (var message in Messages)
            {
                bool isRead=false,isSender=false;
                if (message.Sender != user)
                {
                    isRead = message.Reads.FirstOrDefault(x => x.UuidParticipant == user).IsRead;
                }
                else
                {
                    isRead = message.Reads.Where(x => x.UuidParticipant != user).FirstOrDefault(x => x.IsRead) != null;
                    isSender = true;
                }
                if (!isSender && !isRead)
                {
                    outcome.NumberUnread++;
                }
            }
            return outcome;
        }

        public DialogDetailsModel ToDialogDetails(IUserProvider userProvider, Guid user)
        {
            var outcome = new DialogDetailsModel()
            {
                CreatedAt = CreatedAt,
                Name = Name,
                Uuid = Uuid,
                Participants = Participants.ToList().ConvertAll(x => userProvider.GetUser(Guid.Parse(x))),
                Messages = Messages.ConvertAll(x => x.ToMessageModel(userProvider, user)),
                Creator = Creator
            };
            return outcome;
        }
    }
}
