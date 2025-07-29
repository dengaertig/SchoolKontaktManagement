namespace KontaktManagement
{
    /// <summary>
    /// Verarbeitet Befehle aus der Kommandozeile und ruft Repository-Methoden auf.
    /// </summary>
    public class HandleArgsInput
    {
        private readonly ContactRepository repository;

        public HandleArgsInput(ContactRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Startet die Verarbeitung der übergebenen Befehlsargumente.
        /// </summary>
        /// <param name="args">Kommandozeilenargumente</param>
        public void Process(string[] args)
        {
            if (args.Length == 0)
            {
                PrintHelp();
                return;
            }

            string command = args[0].ToLower();

            switch (command)
            {
                case "create":
                    if (args.Length < 7)
                    {
                        Console.WriteLine("❌ Fehlende Argumente: create <FirstName> <LastName> <Email> <Phone> <City> <Birthdate>");
                        return;
                    }

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
                    Console.WriteLine($"✅ Kontakt erstellt mit ID {newId}");
                    break;

                case "read":
                    if (args.Length < 2 || !int.TryParse(args[1], out int readId))
                    {
                        Console.WriteLine("❌ Ungültige ID: read <ContactID>");
                        return;
                    }
                    repository.ReadContact(readId);
                    break;

                case "list":
                    repository.ReadContacts();
                    break;

                case "update":
                    if (args.Length < 8 || !int.TryParse(args[1], out int updateId))
                    {
                        Console.WriteLine("❌ Ungültige Argumente: update <ID> <FirstName> <LastName> <Email> <Phone> <City> <Birthdate>");
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
                    Console.WriteLine(updated ? "✅ Kontakt aktualisiert." : "❌ Update fehlgeschlagen.");
                    break;

                case "delete":
                    if (args.Length < 2 || !int.TryParse(args[1], out int deleteId))
                    {
                        Console.WriteLine("❌ Ungültige ID: delete <ContactID>");
                        return;
                    }

                    bool deleted = repository.DeleteContact(deleteId);
                    Console.WriteLine(deleted ? "🗑️ Kontakt gelöscht." : "❌ Löschen fehlgeschlagen.");
                    break;

                case "import":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("❌ Pfad zur CSV-Datei fehlt. Beispiel: import contacts.csv");
                        return;
                    }

                    repository.ImportContactsFromCsv(args[1]);
                    break;

                case "export":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("❌ Pfad für Export fehlt. Beispiel: export export.csv");
                        return;
                    }

                    repository.ExportContactsToCsv(args[1]);
                    break;

                case "help":
                    PrintHelp();
                    break;

                default:
                    Console.WriteLine("❓ Unbekannter Befehl.");
                    PrintHelp();
                    break;
            }
        }

        /// <summary>
        /// Gibt eine Übersicht aller verfügbaren CLI-Befehle aus.
        /// </summary>
        private void PrintHelp()
        {
            Console.WriteLine("📚 Verfügbare Befehle:");
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
