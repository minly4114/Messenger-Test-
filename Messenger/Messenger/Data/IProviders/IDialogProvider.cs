﻿using Messenger.HelperEntities;
using Messenger.Models;
using System;
using System.Collections.Generic;

namespace Messenger.Data.IProviders
{
    public interface IDialogProvider
    {
        List<ShortDialogModel> GetDialogs(Guid uuidUser);
        DialogDetailsModel GetDialog(Guid uuidDialog, Guid uuidUser);
        StatusExecution CreateDialog(CreateDialogModel crDialog);
        StatusExecution ChangeDialog(Guid uuid, CreateDialogModel crDialog);
        StatusExecution SendMessage(Guid uuidDialog, MessageModel messageModel);
        StatusExecution StatusIsRead(Guid uuidDialog, Guid uuidUser);
    }
}
