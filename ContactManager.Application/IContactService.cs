using ContactManager.Application.Models;

namespace ContactManager.Application
{
    public interface IContactService
    {
        void AddContact(ContactDto contact);
        void UpdateContact(ContactDto contact);
        ContactDto GetContactById(int id);
        IEnumerable<ContactDto> GetAllContacts();
        IEnumerable<ContactDto> SearchContacts(string term);
        void DeleteContact(int id);
    }
}