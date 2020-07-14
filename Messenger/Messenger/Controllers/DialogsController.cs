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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DialogsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DialogsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DialogsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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
