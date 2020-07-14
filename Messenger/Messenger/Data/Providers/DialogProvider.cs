using Messenger.Data.Entities;
using Messenger.Data.IProviders;
using Messenger.HelperEntities;
using Messenger.Logging;
using Messenger.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.Data.Providers
{
    public class DialogProvider : IDialogProvider
    {
        private readonly IDbSetProvider<Dialog> _dialogsProvider;

        private readonly IUserProvider _userProvider;
        public DialogProvider(DialogsContext context
            , ILog4netProvider log4NetProvider,
            IDbSetProvider<Dialog> dialogsProvider,
            IUserProvider userProvider)
        {
            _userProvider = userProvider;
            _dialogsProvider = dialogsProvider.Build(context.Dialogs, context);
        }

        public StatusExecution ChangeDialog(Guid uuid, EditDialogModel edDialog)
        {
            var dialog = _dialogsProvider.GetQueryable().FirstOrDefault(x => x.Uuid == uuid);
            if (dialog == null)
            {
                return new StatusExecution()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Диалога не существует"
                };
            }
            if (dialog.Creator != edDialog.Creator)
            {
                return new StatusExecution()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Данный пользователь не может изменить диалог"
                };
            }
            dialog.Name = edDialog.Name;
            edDialog.EmailParticipants.RemoveAll(x => x == null);
            dialog.Participants = edDialog.EmailParticipants.ConvertAll(x => _userProvider.GetUser(x).Uuid);
            dialog.Participants.Add(edDialog.Creator.ToString());
            try
            {
                _dialogsProvider.UpdateAsync(dialog).Wait();
            }
            catch
            {
                return new StatusExecution()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Диалог не изменён"
                };
            }
            return new StatusExecution()
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Диалог успешно изменён"
            };
        }

        public StatusExecution CreateDialog(CreateDialogModel crDialog)
        {
            Dialog dialog = new Dialog()
            {
                Name = crDialog.Name,
                Participants = crDialog.Participants.ConvertAll(x => _userProvider.GetUser(x).Uuid),
                Creator = crDialog.Creator
            };
            dialog.Participants.Add(crDialog.Creator.ToString());
            try
            {
                _dialogsProvider.InsertAsync(dialog).Wait();
            }
            catch
            {
                return new StatusExecution()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Диалог не создан"
                };
            }
            return new StatusExecution()
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Диалог успешно создан"
            };
        }

        public DialogDetailsModel GetDialog(Guid uuidDialog, Guid uuidUser)
        {
            var dialog = _dialogsProvider.GetQueryable().Include(x => x.Messages).FirstOrDefault(x => x.Uuid == uuidDialog).ToDialogDetails(_userProvider, uuidUser);
            return dialog;
        }

        public EditDialogModel GetDialogForEdit(Guid uuidDialog, Guid uuidCreator)
        {
            var dialog = _dialogsProvider.GetQueryable().FirstOrDefault(x => x.Uuid == uuidDialog).ToEditDialog(_userProvider);
            dialog.Participants.RemoveAll(x => x.Uuid == uuidCreator.ToString());
            return dialog;
        }

        public List<ShortDialogModel> GetDialogs(Guid uuidUser)
        {
            var uuidString = uuidUser.ToString();
            var dialogs = _dialogsProvider.GetQueryable().Include(x => x.Messages).ThenInclude(x => x.Reads).ToList()
                .Where(r => r.Participants.Contains(uuidString))
                .ToList().ConvertAll(x => x.ToShortDialog(_userProvider, uuidUser));
            return dialogs;
        }

        public StatusExecution SendMessage(Guid uuidDialog, MessageModel messageModel)
        {
            var dialog = _dialogsProvider.GetQueryable().Include(x => x.Messages).FirstOrDefault(x => x.Uuid == uuidDialog);
            if (dialog == null)
            {
                return new StatusExecution()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Диалога не существует"
                };
            }
            dialog.Messages.Add(new Message()
            {
                Text = messageModel.Text,
                Reads = dialog.Participants.ConvertAll(x => new Read()
                {
                    IsRead = false,
                    UuidParticipant = Guid.Parse(x)
                }),
                Sender = Guid.Parse(messageModel.Sender.Uuid),
            });
            try
            {
                _dialogsProvider.UpdateAsync(dialog).Wait();
            }
            catch
            {
                return new StatusExecution()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Сообщение не отправлено"
                };
            }
            return new StatusExecution()
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Сообщение успешно отправлено"
            };

        }

        public StatusExecution StatusIsRead(Guid uuidDialog, Guid uuidUser)
        {
            var dialog = _dialogsProvider.GetQueryable().Include(x => x.Messages).ThenInclude(x=>x.Reads).FirstOrDefault(x => x.Uuid == uuidDialog);
            if (dialog == null)
            {
                return new StatusExecution()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Диалога не существует"
                };
            }
            dialog.Messages.ForEach(x => x.Reads.FirstOrDefault(y => y.UuidParticipant == uuidUser).IsRead = true);
            try
            {
                _dialogsProvider.UpdateAsync(dialog).Wait();
            }
            catch
            {
                return new StatusExecution()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Не удалось прочесть сообщение"
                };
            }
            return new StatusExecution()
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Диалог прочитан"
            };
        }
    }
}
