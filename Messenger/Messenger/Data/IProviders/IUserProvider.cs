using Messenger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Data.IProviders
{
    public interface IUserProvider
    {
        ParticipantModel GetUser(string email);
        ParticipantModel GetUser(Guid uuid);
    }
}
