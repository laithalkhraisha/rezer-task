using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BlazorApp1.Shared;

namespace BlazorApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private static List<Contact> _contacts = new List<Contact>(); // In-memory storage for contacts
        private static int _nextId = 1; // To generate unique Ids for new contacts

        [HttpGet]
        public ActionResult<IEnumerable<Contact>> Get(string sortBy = "Id", string sortOrder = "asc")
        {
            // Sorting
            var sortedContacts = SortContacts(_contacts, sortBy, sortOrder);
            return Ok(sortedContacts);
        }

        [HttpGet("{id}")]
        public ActionResult<Contact> Get(int id)
        {
            var contact = _contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        [HttpPost]
        public IActionResult Post(Contact contact)
        {
            contact.Id = _nextId++; // Assigning a unique Id
            _contacts.Add(contact);
            return CreatedAtAction(nameof(Get), new { id = contact.Id }, contact);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Contact updatedContact)
        {
            var existingContact = _contacts.FirstOrDefault(c => c.Id == id);
            if (existingContact == null)
            {
                return NotFound();
            }

            existingContact.FirstName = updatedContact.FirstName;
            existingContact.LastName = updatedContact.LastName;
            existingContact.Email = updatedContact.Email;
            existingContact.PhoneNumber = updatedContact.PhoneNumber;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var contactToRemove = _contacts.FirstOrDefault(c => c.Id == id);
            if (contactToRemove == null)
            {
                return NotFound();
            }

            _contacts.Remove(contactToRemove);
            return NoContent();
        }

        private List<Contact> SortContacts(List<Contact> contacts, string sortBy, string sortOrder)
        {
            switch (sortBy.ToLower())
            {
                case "firstname":
                    return sortOrder.ToLower() == "asc" ? contacts.OrderBy(c => c.FirstName).ToList() :
                                                           contacts.OrderByDescending(c => c.FirstName).ToList();
                case "lastname":
                    return sortOrder.ToLower() == "asc" ? contacts.OrderBy(c => c.LastName).ToList() :
                                                           contacts.OrderByDescending(c => c.LastName).ToList();
                case "email":
                    return sortOrder.ToLower() == "asc" ? contacts.OrderBy(c => c.Email).ToList() :
                                                           contacts.OrderByDescending(c => c.Email).ToList();
                case "phonenumber":
                    return sortOrder.ToLower() == "asc" ? contacts.OrderBy(c => c.PhoneNumber).ToList() :
                                                           contacts.OrderByDescending(c => c.PhoneNumber).ToList();
                // Default sorting by Id
                default:
                    return sortOrder.ToLower() == "asc" ? contacts.OrderBy(c => c.Id).ToList() :
                                                           contacts.OrderByDescending(c => c.Id).ToList();
            }
        }
    }
}
