using Messenger.Data.IProviders;
using Messenger.Logging;
using Messenger.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Messenger.Data.Providers
{
    public class UserProvider : IUserProvider
    {

        private readonly ILog4netProvider _log;
        private readonly UserManager<IdentityUser> _userManager;
        public UserProvider(ILog4netProvider log, IServiceProvider serviceProvider)
        {
            _log = log;
            _userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        }
        public ParticipantModel GetUser(string email)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Email == email);
            return user != null ? new ParticipantModel() { Name = user.UserName, Uuid = user.Id, Email= user.Email } : null;
        }

        public ParticipantModel GetUser(Guid uuid)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id==uuid.ToString());
            return user!=null?new ParticipantModel() { Name = user.UserName, Uuid = user.Id, Email = user.Email } :null;
        }
    }
}
