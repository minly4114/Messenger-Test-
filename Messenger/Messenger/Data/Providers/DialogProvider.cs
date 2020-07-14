using Messenger.Data.Entities;
using Messenger.Data.IProviders;
using Messenger.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Data.Providers
{
    public class DialogProvider : IDialogProvider
    {
        private readonly IDbSetProvider<Dialog> _dialogsProvider;
        public DialogProvider(DialogsContext context
            , ILog4netProvider log4NetProvider,
            IDbSetProvider<Dialog> dialogsProvider)
        {

            _dialogsProvider = dialogsProvider.Build(context.Dialogs, context);
        }
        public void CreateDialog()
        {

        }
    }
}
