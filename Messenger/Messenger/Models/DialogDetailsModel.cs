using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Models
{
    public class DialogDetailsModel
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public List<MessageModel> Messages { get; set; }
        public bool IsCreator { get; set; }
        public List<ParticipantModel> Participants { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CurentMessage { get; set; }
    }
}
