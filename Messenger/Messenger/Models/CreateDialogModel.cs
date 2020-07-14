using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Models
{
    public class CreateDialogModel
    {
        public string Name { get; set; }
        public List<string> Participants { get; set; }
        public Guid Creator { get; set; }
    }
}
