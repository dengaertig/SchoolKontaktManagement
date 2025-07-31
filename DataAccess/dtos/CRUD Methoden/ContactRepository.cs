using Microsoft.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;

namespace KontaktManagement
{
    /// <summary>
    /// Stellt Methoden für das Verwalten von Kontakten bereit, einschließlich Datenbankzugriff und CSV-Import/Export.
    /// </summary>
    public class ContactRepository : IContactRepository
    {
        private readonly string connectionString;
        /// <summary>
        /// Initialisiert eine neue Instanz des <see cref="ContactRepository"/> mit einer Verbindungszeichenfolge.
        /// </summary>
        /// <param name="connectionString">Die SQL-Verbindungszeichenfolge.</param>
        public ContactRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// Fügt einen neuen Kontakt in die Datenbank ein.
        /// </summary>
        /// <param name="contact">Der zu speichernde Kontakt.</param>
        /// <returns>Die generierte ContactID des neuen Kontakts.</returns>
        public int CreateContact(Contact contact)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                                INSERT INTO dbo.Contact (FirstName, LastName, Email, Phonenumber, City, Birthdate)
                                OUTPUT INSERTED.ContactId
                                VALUES (@FirstName, @LastName, @Email, @Phonenumber, @City, @Birthdate)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", contact.FirstName);
                    command.Parameters.AddWithValue("@LastName", contact.LastName);
                    command.Parameters.AddWithValue("@Email", contact.Email);
                    command.Parameters.AddWithValue("@Phonenumber", string.IsNullOrWhiteSpace(contact.Phonenumber));
                    command.Parameters.AddWithValue("@City", string.IsNullOrWhiteSpace(contact.City));
                    command.Parameters.AddWithValue("@Birthdate", contact.Birthdate.HasValue ? contact.Birthdate.Value.ToDateTime(TimeOnly.MinValue) : (object)DBNull.Value);

                    // Gibt die generierte ContactID zurück
                    int newId = (int)command.ExecuteScalar();
                    return newId;
                }
            }
        }

        /// <summary>
        /// Liest einen Kontakt anhand seiner ID aus der Datenbank und gibt ihn in der Konsole aus.
        /// </summary>
        /// <param name="contactID">Die ID des Kontakts.</param>
        /// <returns>Ein <see cref="Contact"/>-Objekt, oder ein leerer Kontakt, wenn nicht gefunden.</returns>
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
        /// <summary>
        /// Liest alle Kontakte aus der Datenbank, zeigt sie in der Konsole und gibt sie als Liste zurück.
        /// </summary>
        /// <returns>Eine Liste aller vorhandenen Kontakte.</returns>
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
        /// <summary>
        /// Aktualisiert einen bestehenden Kontakt in der Datenbank anhand seiner ID.
        /// </summary>
        /// <param name="contact">Das aktualisierte Kontaktobjekt.</param>
        /// <returns> true, wenn die Aktualisierung erfolgreich war; andernfalls false.</returns>
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
        /// <summary>
        /// Löscht einen Kontakt anhand seiner ID.
        /// </summary>
        /// <param name="contactID">Die ID des zu löschenden Kontakts.</param>
        /// <returns><c>true</c>, wenn ein Kontakt gelöscht wurde; andernfalls <c>false</c>.</returns>
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
        /// <summary>
        /// Importiert Kontakte aus einer CSV-Datei (UTF-8, Semikolon-separiert).
        /// Doppelte Einträge (nach E-Mail) werden übersprungen.
        /// </summary>
        /// <param name="filePath">Pfad zur CSV-Datei.</param>
        /// <exception cref="IOException">Wird ausgelöst, wenn die Datei nicht gelesen werden kann.</exception>
        public void ImportContactsFromCsv(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("CSV-Datei nicht gefunden.");
                return;
            }

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var lines = File.ReadAllLines(filePath);
                int imported = 0, skipped = 0;

                foreach (var line in lines.Skip(1))
                {
                    var columns = line.Split(';');
                    if (columns.Length < 6) continue;

                    string email = columns[2];

                    string checkQuery = "SELECT COUNT(*) FROM dbo.Contact WHERE Email = @Email";
                    using (var checkCmd = new SqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", email);
                        int exists = (int)checkCmd.ExecuteScalar();
                        if (exists > 0)
                        {
                            skipped++;
                            continue;
                        }
                    }

                    // Einfügen
                    string insertQuery = @"
                INSERT INTO dbo.Contact (FirstName, LastName, Email, Phonenumber, City, Birthdate)
                VALUES (@FirstName, @LastName, @Email, @Phonenumber, @City, @Birthdate)";

                    using (var insertCmd = new SqlCommand(insertQuery, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@FirstName", columns[0]);
                        insertCmd.Parameters.AddWithValue("@LastName", columns[1]);
                        insertCmd.Parameters.AddWithValue("@Email", columns[2]);
                        insertCmd.Parameters.AddWithValue("@Phonenumber", string.IsNullOrWhiteSpace(columns[3]) ? (object)DBNull.Value : columns[3]);
                        insertCmd.Parameters.AddWithValue("@City", string.IsNullOrWhiteSpace(columns[4]) ? (object)DBNull.Value : columns[4]);
                        insertCmd.Parameters.AddWithValue("@Birthdate", DateTime.TryParse(columns[5], out var bd) ? bd : (object)DBNull.Value);

                        insertCmd.ExecuteNonQuery();
                        imported++;
                    }
                }

                Console.WriteLine($"Import abgeschlossen. {imported} importiert, {skipped} übersprungen (bereits vorhanden).");
            }
        }
        /// <summary>
        /// Exportiert alle Kontakte aus der Datenbank in eine neue CSV-Datei.
        /// Felder mit null-Werten werden leer geschrieben.
        /// </summary>
        /// <param name="filePath">Zielpfad der zu erstellenden CSV-Datei.</param>
        /// <exception cref="IOException">Wird ausgelöst, wenn die Datei nicht geschrieben werden kann.</exception>
        public void ExportContactsToCsv(string filePath)
        {
            var contacts = ReadContacts();
            var sb = new StringBuilder();
            sb.AppendLine("FirstName;LastName;Email;Phonenumber;City;Birthdate");

            foreach (var contact in contacts)
            {
                string birthdateStr = contact.Birthdate.HasValue && contact.Birthdate.Value >= new DateOnly(1753, 1, 1)
                    ? contact.Birthdate.Value.ToString("yyyy-MM-dd")
                    : "";

                sb.AppendLine($"{contact.FirstName};{contact.LastName};{contact.Email};{contact.Phonenumber};{contact.City};{birthdateStr}");
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
            Console.WriteLine($"Export abgeschlossen: {filePath}");
        }


    }
}