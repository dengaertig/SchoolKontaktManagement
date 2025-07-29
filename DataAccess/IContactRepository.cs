namespace KontaktManagement
{
    public interface IContactRepository
    {
        int CreateContact(Contact contact);
        Contact ReadContact(int contactID);
        List<Contact> ReadContacts();
        bool UpdateContact(Contact contact);
        bool DeleteContact(int contactID);
    }
}