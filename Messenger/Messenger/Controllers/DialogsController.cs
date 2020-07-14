using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Messenger.Data.IProviders;
using Messenger.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers
{
    public class DialogsController : Controller
    {
        private readonly IDialogProvider _dialogProvider;
        private readonly IUserProvider _userProvider;
        public DialogsController(IDialogProvider dialogProvider, IUserProvider userProvider)
        {
            _dialogProvider = dialogProvider;
            _userProvider = userProvider;
        }
        // GET: DialogsController
        public ActionResult Index()
        {
            return View(_dialogProvider.GetDialogs(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))));
        }

        // GET: DialogsController/Details/5
        public ActionResult Details(string id)
        {
            var result =_dialogProvider.GetDialog(Guid.Parse(id), Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return View(result);
        }

        // GET: DialogsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DialogsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateDialogModel model)
        {
            try
            {
                model.Creator = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var status = _dialogProvider.CreateDialog(model);
                return Ok(status.Message);
            }
            catch
            {
                return View(model);
            }
        }

        // GET: DialogsController/Edit/5
        public ActionResult Edit(string id)
        {
            return View(_dialogProvider.GetDialogForEdit(Guid.Parse(id), Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))));
        }

        // POST: DialogsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, EditDialogModel model)
        {
            try
            {
                var status = _dialogProvider.ChangeDialog(Guid.Parse(id), model);
                if (status.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(model);
                }
                
            }
            catch
            {
                return View(model);
            }
        }

        [HttpGet("/Dialogs/GetMember/{email}")]
        public ActionResult GetMember(string email)
        {
            var participant = _userProvider.GetUser(email);
            return Ok(participant);
        }

        [HttpPost]
        public ActionResult SendMessage(string id, MessageModel message)
        {
            try
            {
                message.Sender = new ParticipantModel()
                {
                    Uuid = User.FindFirstValue(ClaimTypes.NameIdentifier),
                };
                var result = _dialogProvider.SendMessage(Guid.Parse(id), message);
                return Ok();
            }
            catch
            {
                return Ok("Неудалось отправить сообщение!");
            }
        }
        [HttpPost]
        public ActionResult StatusIsRead(string id)
        {
            try
            {
                var result = _dialogProvider.StatusIsRead(Guid.Parse(id) ,Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
                return Ok();
            }
            catch
            {
                return Ok("Неудалось отправить оповещение о прочтении!");
            }
        }
    }
}
