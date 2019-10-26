using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AjaxModals.Models;
using Microsoft.AspNetCore.Mvc;

namespace AjaxModals.Controllers
{
    public class ClientController : Controller
    {
        private readonly static List<Client> Clients = new List<Client>();

        public IActionResult Index()
        {
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return PartialView("_Table", Clients);
            }

            return View(Clients);
        }

        public IActionResult Client()
        {
            var model = new Client { };

            return PartialView("_ClientModalPartial", model);
        }

        [HttpPost]
        public IActionResult Client(Client model)
        {
            if (ModelState.IsValid)
            {
                Clients.Add(model);
                CreateNotification("Client saved!");
            }

            return PartialView("_ClientModalPartial", model);
        }

        [NonAction]
        private void CreateNotification(string message)
        {
            TempData.TryGetValue("Notifications", out object value);
            var notifications = value as List<string> ?? new List<string>();
            notifications.Add(message);
            TempData["Notifications"] = notifications;
        }

        public IActionResult Notifications()
        {
            TempData.TryGetValue("Notifications", out object value);
            var notifications = value as IEnumerable<string> ?? Enumerable.Empty<string>();
            return PartialView("_NotificationsPartial", notifications);
        }
    }
}