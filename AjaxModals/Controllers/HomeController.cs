using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AjaxModals.Models;

namespace AjaxModals.Controllers
{
    public class HomeController : Controller
    {
        //Como parametrizar o modal
        //https://softdevpractice.com/blog/asp-net-core-mvc-ajax-modals/
        //https://softdevpractice.com/blog/asp-net-core-ajax-modals-part-2/

        private readonly static List<Contact> Contacts = new List<Contact>();

        public IActionResult Index()
        {
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return PartialView("_Table", Contacts);
            }

            return View(Contacts);
        }

        public IActionResult Contact()
        {
            var model = new Contact { };

            return PartialView("_ContactModalPartial", model);
        }

        [HttpPost]
        public IActionResult Contact(Contact model)
        {
            if (ModelState.IsValid)
            {
                Contacts.Add(model);
                CreateNotification("Contact saved!");
            }

            return PartialView("_ContactModalPartial", model);
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
