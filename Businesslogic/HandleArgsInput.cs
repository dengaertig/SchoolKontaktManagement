namespace KontaktManagement
{
    /// <summary>
    /// Verarbeitet Befehle aus der Kommandozeile und ruft Repository- und CSV-Methoden auf.
    /// </summary>
    public class HandleArgsInput
    {
        private readonly IContactRepository repository;
        private readonly ContactCsvService csvService;

        /// <summary>
        /// Erstellt eine neue Instanz von <see cref="HandleArgsInput"/>.
        /// </summary>
        /// <param name="repository">Ein Objekt zur Verwaltung von Kontakten (z. B. Datenbank).</param>
        /// <param name="csvService">Ein Objekt zur Verarbeitung von CSV-Dateien.</param>
        public HandleArgsInput(IContactRepository repository, ContactCsvService csvService)
        {
            this.repository = repository;
            this.csvService = csvService;
        }

        /// <summary>
        /// Führt die Verarbeitung der übergebenen CLI-Argumente durch.
        /// </summary>
        /// <param name="args">Ein Array mit Kommandozeilenargumenten.</param>
        public void Process(string[] args)
        {
            if (args.Length == 0)
            {
                PrintHelp();
                return;
            }

            // Ersten Befehl lesen (z. B. "create", "read", ...)
            string command = args[0].ToLower();

            switch (command)
            {
                case "create":
                    // Erwartet mindestens 6 Argumente + Befehl = 7
                    if (args.Length < 7)
                    {
                        Console.WriteLine("Fehlende Argumente: create <FirstName> <LastName> <Email> <Phone> <City> <Birthdate>");
                        return;
                    }

                    // Erstellt ein neues Contact-Objekt aus den übergebenen Argumenten
                    var newContact = new Contact
                    {
                        FirstName = args[1],
                        LastName = args[2],
                        Email = args[3],
                        Phonenumber = args[4],
                        City = args[5],
                        Birthdate = DateOnly.TryParse(args[6], out var date) ? date : null
                    };

                    int newId = repository.CreateContact(newContact);
                    Console.WriteLine($"Kontakt erstellt mit ID {newId}");
                    break;

                case "read":
                    // Erwartet eine gültige ID als Argument
                    if (args.Length < 2 || !int.TryParse(args[1], out int readId))
                    {
                        Console.WriteLine("Ungültige ID: read <ContactID>");
                        return;
                    }

                    repository.ReadContact(readId);
                    break;

                case "list":
                    // Gibt alle Kontakte aus
                    repository.ReadContacts();
                    break;

                case "update":
                    // Erwartet 7 Argumente + Befehl + ID = 8
                    if (args.Length < 8 || !int.TryParse(args[1], out int updateId))
                    {
                        Console.WriteLine("Ungültige Argumente: update <ID> <FirstName> <LastName> <Email> <Phone> <City> <Birthdate>");
                        return;
                    }

                    var updateContact = new Contact
                    {
                        ContactID = updateId,
                        FirstName = args[2],
                        LastName = args[3],
                        Email = args[4],
                        Phonenumber = args[5],
                        City = args[6],
                        Birthdate = DateOnly.TryParse(args[7], out var uDate) ? uDate : null
                    };

                    bool updated = repository.UpdateContact(updateContact);
                    Console.WriteLine(updated ? "Kontakt aktualisiert." : "Update fehlgeschlagen.");
                    break;

                case "delete":
                    // Erwartet eine ID als Argument
                    if (args.Length < 2 || !int.TryParse(args[1], out int deleteId))
                    {
                        Console.WriteLine("Ungültige ID: delete <ContactID>");
                        return;
                    }

                    bool deleted = repository.DeleteContact(deleteId);
                    Console.WriteLine(deleted ? "Kontakt gelöscht." : "Löschen fehlgeschlagen.");
                    break;

                case "import":
                    // Erwartet Pfad zur CSV-Datei
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Pfad zur CSV-Datei fehlt. Beispiel: import contacts.csv");
                        return;
                    }

                    csvService.ImportContactsFromCsv(args[1]);
                    break;

                case "export":
                    // Erwartet Pfad zur Ausgabedatei
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Pfad für Export fehlt. Beispiel: export export.csv");
                        return;
                    }

                    csvService.ExportContactsToCsv(args[1]);
                    break;

                case "help":
                    PrintHelp();
                    break;

                default:
                    Console.WriteLine("Unbekannter Befehl.");
                    PrintHelp();
                    break;
            }
        }

        /// <summary>
        /// Gibt eine Liste aller verfügbaren CLI-Befehle und deren Syntax aus.
        /// </summary>
        private void PrintHelp()
        {
            Console.WriteLine("  Verfügbare Befehle:");
            Console.WriteLine("  create <Vorname> <Nachname> <Email> <Telefon> <Stadt> <Geburtsdatum>");
            Console.WriteLine("  read <ID>");
            Console.WriteLine("  list");
            Console.WriteLine("  update <ID> <Vorname> <Nachname> <Email> <Telefon> <Stadt> <Geburtsdatum>");
            Console.WriteLine("  delete <ID>");
            Console.WriteLine("  import <Dateipfad.csv>");
            Console.WriteLine("  export <Zieldatei.csv>");
            Console.WriteLine("  help");
        }
    }
}
