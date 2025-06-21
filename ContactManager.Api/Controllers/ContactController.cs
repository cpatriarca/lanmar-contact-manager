using ContactManager.Application;
using ContactManager.Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ContactManager.Api.Controllers
{
    public class ContactController : Controller
    {
        private readonly ILogger<ContactController> _logger;
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService, ILogger<ContactController> logger)
        {
            _contactService = contactService;
            _logger = logger;
        }

        public IActionResult Index(string searchTerm)
        {
            ViewBag.Title = "Contacts List";

            return View();
        }

        public IActionResult Search(string searchTerm)
        {
            var contacts = string.IsNullOrWhiteSpace(searchTerm)
                ? _contactService.GetAllContacts()
                : _contactService.SearchContacts(searchTerm);

            return PartialView("_SearchResults", contacts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ContactDto contact)
        {
            if (!ModelState.IsValid) return View(contact);
            _contactService.AddContact(contact);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var contact = _contactService.GetContactById(id);
            if (contact == null) return NotFound();

            return View(contact);
        }

        [HttpPost]
        public IActionResult Edit(ContactDto contact)
        {
            if (!ModelState.IsValid) return View(contact);
            _contactService.UpdateContact(contact);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var contact = _contactService.GetContactById(id);
            if (contact == null) 
                return NotFound();
            else
                _contactService.DeleteContact(id);

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}