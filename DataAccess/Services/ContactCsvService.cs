using System.Globalization;
using System.Text;

namespace KontaktManagement
{
    /// <summary>
    /// Bietet Funktionen zum Importieren und Exportieren von Kontakten im CSV-Format.
    /// </summary>
    public class ContactCsvService
    {
        private readonly IContactRepository repository;

        /// <summary>
        /// Erstellt eine neue Instanz von <see cref="ContactCsvService"/> mit einem angegebenen Repository.
        /// </summary>
        /// <param name="repository">Ein Objekt, das das <see cref="IContactRepository"/> implementiert.</param>
        public ContactCsvService(IContactRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Importiert Kontakte aus einer CSV-Datei.
        /// Bereits vorhandene Kontakte (nach E-Mail verglichen) werden übersprungen.
        /// </summary>
        /// <param name="filePath">Pfad zur CSV-Datei.</param>
        public void ImportContactsFromCsv(string filePath)
        {
            // Prüfe, ob die Datei existiert
            if (!File.Exists(filePath))
            {
                Console.WriteLine("CSV-Datei nicht gefunden.");
                return;
            }

            // Lese alle Zeilen der Datei
            var lines = File.ReadAllLines(filePath);
            int imported = 0, skipped = 0;

            // Überspringe die Kopfzeile (erste Zeile)
            foreach (var line in lines.Skip(1))
            {
                // Trenne die Spalten anhand des Semikolons
                var columns = line.Split(';');
                if (columns.Length < 6) continue;

                var email = columns[2];

                // Prüfe, ob ein Kontakt mit dieser E-Mail bereits existiert
                var existing = repository.ReadContacts().FirstOrDefault(c =>
                    string.Equals(c.Email?.Trim(), email.Trim(), StringComparison.OrdinalIgnoreCase)
                );

                if (existing != null)
                {
                    // Wenn vorhanden, überspringe den Eintrag
                    skipped++;
                    continue;
                }

                // Erzeuge neuen Kontakt aus den CSV-Daten
                var contact = new Contact
                {
                    FirstName = columns[0],
                    LastName = columns[1],
                    Email = email,
                    Phonenumber = string.IsNullOrWhiteSpace(columns[3]) ? null : columns[3],
                    City = string.IsNullOrWhiteSpace(columns[4]) ? null : columns[4],
                    Birthdate = DateTime.TryParse(columns[5], out var bd) ? DateOnly.FromDateTime(bd) : null
                };

                // Füge den neuen Kontakt über das Repository ein
                repository.CreateContact(contact);
                imported++;
            }

            // Zusammenfassung ausgeben
            Console.WriteLine($"Import abgeschlossen: {imported} importiert, {skipped} übersprungen.");
        }

        /// <summary>
        /// Exportiert alle Kontakte in eine CSV-Datei.
        /// Leere Felder werden als leere Strings geschrieben.
        /// </summary>
        /// <param name="filePath">Pfad zur Ausgabedatei (CSV).</param>
        public void ExportContactsToCsv(string filePath)
        {
            // Alle Kontakte aus der Datenquelle abrufen
            var contacts = repository.ReadContacts();

            // StringBuilder für die CSV-Zeilen verwenden
            var sb = new StringBuilder();

            // Kopfzeile einfügen
            sb.AppendLine("FirstName;LastName;Email;Phonenumber;City;Birthdate");

            // Jeden Kontakt als CSV-Zeile einfügen
            foreach (var contact in contacts)
            {
                string birthdateStr = contact.Birthdate.HasValue
                    ? contact.Birthdate.Value.ToString("yyyy-MM-dd")
                    : "";

                sb.AppendLine($"{contact.FirstName};{contact.LastName};{contact.Email};{contact.Phonenumber};{contact.City};{birthdateStr}");
            }

            // Die CSV-Datei schreiben (UTF-8)
            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);

            // Rückmeldung an den Benutzer
            Console.WriteLine($"Export abgeschlossen: {filePath}");
        }
    }
}
