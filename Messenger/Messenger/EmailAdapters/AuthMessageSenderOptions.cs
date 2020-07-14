using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.EmailAdapters
{
    public class AuthMessageSenderOptions
    {
        public string SenderUser { get; set; }
        public string SendPassword { get; set; }
        public string SendName { get; set; }
    }
}
