using System;
using System.Collections.Generic;

namespace Messenger.Models
{
    public class EditDialogModel
    {
        public string Name { get; set; }
        public List<ParticipantModel> Participants { get; set; }
        public List<string> EmailParticipants { get; set; }
        public Guid Creator { get; set; }
    }
}
