using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Models
{
    public class ShortDialogModel
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public List<string> Participants { get; set; }
        public int NumberUnread { get; set; }
        public bool IsCreator { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
