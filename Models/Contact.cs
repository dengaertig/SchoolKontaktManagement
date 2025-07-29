namespace KontaktManagement
{
    public class Contact
    {
        public int? ContactID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phonenumber { get; set; }
        public string? City { get; set; }
        public DateOnly? Birthdate { get; set; }
    }
}