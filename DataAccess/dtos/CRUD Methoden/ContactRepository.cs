using Microsoft.Data.SqlClient;
using System.Globalization;

namespace KontaktManagement
{
    public class ContactRepository : IContactRepository
    {
        private readonly string connectionString;

        public ContactRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public int CreateContact(Contact contact)
        {
            return 0;
        }

        public Contact ReadContact(int contactID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM dbo.Contact WHERE ContactID = @contactID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@contactID", contactID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine("Kontaktdetails:");
                            Console.WriteLine($"ContactID: {reader["ContactID"]}");
                            Console.WriteLine($"Vorname: {reader["FirstName"]}");
                            Console.WriteLine($"Nachname: {reader["LastName"]}");
                            Console.WriteLine($"Email: {reader["Email"]}");
                            Console.WriteLine($"Telefon: {reader["Phonenumber"]}");
                            Console.WriteLine($"Stadt: {reader["City"]}");
                            Console.WriteLine($"Geburtsdatum: {reader["Birthdate"]}");
                        }
                    }
                }
            }
            return new Contact();
        }

        public List<Contact> ReadContacts()
        {
            var contacts = new List<Contact>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM dbo.Contact";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var contact = new Contact
                        {
                            ContactID = reader.GetInt32(0),
                            FirstName = reader.IsDBNull(1) ? null : reader.GetString(1),
                            LastName = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Email = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Phonenumber = reader.IsDBNull(4) ? null : reader.GetString(4),
                            City = reader.IsDBNull(5) ? null : reader.GetString(5),
                            Birthdate = reader.IsDBNull(6) ? default : DateOnly.FromDateTime(reader.GetDateTime(6))
                        };

                        contacts.Add(contact);
                    }
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(
                    $"{"ID",-4}{"Vorname",-12}{"Nachname",-14}{"Email",-30}{"Telefon",-18}{"Stadt",-16}{"Geburtsdatum",-12}"
                );
                Console.ResetColor();
                Console.WriteLine(new string('-', 106));

                int rowCount = 0;

                foreach (var item in contacts)
                {
                    Console.ForegroundColor = (rowCount++ % 2 == 0) ? ConsoleColor.Gray : ConsoleColor.DarkGray;

                    Console.WriteLine(
                        $"{item.ContactID,-4}" +
                        $"{item.FirstName,-12}" +
                        $"{item.LastName,-14}" +
                        $"{item.Email,-30}" +
                        $"{item.Phonenumber,-18}" +
                        $"{item.City,-16}" +
                        $"{item.Birthdate:dd-MM-yyyy}"
                    );

                    Console.ResetColor();
                }
            }

            return contacts;
        }

        public bool UpdateContact(Contact contact)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    UPDATE dbo.Contact
                    SET 
                        FirstName = @FirstName,
                        LastName = @LastName,
                        Email = @Email,
                        Phonenumber = @Phonenumber,
                        City = @City,
                        Birthdate = @Birthdate
                    WHERE ContactID = @ContactID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", contact.FirstName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", contact.LastName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Email", contact.Email ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Phonenumber", contact.Phonenumber ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@City", contact.City ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Birthdate", contact.Birthdate.HasValue ? contact.Birthdate.Value.ToDateTime(TimeOnly.MinValue) : (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ContactID", contact.ContactID);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public bool DeleteContact(int contactID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM dbo.Contact WHERE ContactID = @ContactID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ContactID", contactID);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}