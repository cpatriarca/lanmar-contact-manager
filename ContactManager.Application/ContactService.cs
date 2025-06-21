using ContactManager.Application.Models;
using ContactManager.Infrastructure;
using ContactManager.Infrastructure.Entities;

namespace ContactManager.Application
{
    public class ContactService : IContactService
    {
        private readonly ContactDbContext _context;

        public ContactService(ContactDbContext context)
        {
            _context = context;
        }

        public void AddContact(ContactDto dto)
        {
            var contact = new Contact
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                CompanyName = dto.CompanyName,
                Mobile = dto.Mobile,
                Email = dto.Email
            };
            _context.Contacts.Add(contact);
            _context.SaveChanges();
        }

        public void UpdateContact(ContactDto dto)
        {
            var contact = _context.Contacts.Find(dto.Id);
            if (contact == null) return;
            contact.FirstName = dto.FirstName;
            contact.LastName = dto.LastName;
            contact.CompanyName = dto.CompanyName;
            contact.Mobile = dto.Mobile;
            contact.Email = dto.Email;
            _context.SaveChanges();
        }

        public ContactDto? GetContactById(int id)
        {
            var c = _context.Contacts.Find(id);
            return c == null ? null : new ContactDto
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                CompanyName = c.CompanyName,
                Mobile = c.Mobile,
                Email = c.Email
            };
        }

        public IEnumerable<ContactDto> GetAllContacts() => _context.Contacts
            .Select(c => new ContactDto
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                CompanyName = c.CompanyName,
                Mobile = c.Mobile,
                Email = c.Email
            }).ToList();

        
        public IEnumerable<ContactDto> SearchContacts(string term)
        {
            /*
             * I have implemented a lowered term search to ensure as friendly a user experience as possible.
             * The technical drawback to this approach is that indexing is normally disabled on ToLower, which
             * has a perfomance impact on large data sets.
            */
            var loweredTerm = term.ToLower();

            return _context.Contacts
                .Where(c =>
                    c.FirstName.ToLower().Contains(loweredTerm) ||
                    c.LastName.ToLower().Contains(loweredTerm) ||
                    c.CompanyName.ToLower().Contains(loweredTerm) ||
                    c.Email.ToLower().Contains(loweredTerm))
                .Select(c => new ContactDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    CompanyName = c.CompanyName,
                    Mobile = c.Mobile,
                    Email = c.Email
                })
                .ToList();
        }

        public void DeleteContact(int id)
        {
            var c = _context.Contacts.Find(id);
            if (c != null)
            {
                _context.Contacts.Remove(c);
                _context.SaveChanges();
            }
        }
    }
}