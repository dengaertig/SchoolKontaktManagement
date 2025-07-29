namespace KontaktManagement
{
    public class HandleArgsInput
    {
        private readonly ContactRepository repository;

        public HandleArgsInput(ContactRepository repository)
        {
            this.repository = repository;
        }

        public void Process(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Keine Argumente übergeben. Nutze: create | read | update | delete | list |export | import");
                return;
            }

            string command = args[0].ToLower();

            switch (command)
            {
                case "create":
                    if (args.Length < 6)
                    {
                        Console.WriteLine("Fehlende Argumente: create <FirstName> <LastName> <Email> <Phone> <City> <Birthdate>");
                        return;
                    }
                    var contact = new Contact
                    {
                        FirstName = args[1],
                        LastName = args[2],
                        Email = args[3],
                        Phonenumber = args[4],
                        City = args[5],
                        Birthdate = DateOnly.TryParse(args[6], out var date) ? date : null
                    };
                    int id = repository.CreateContact(contact);
                    Console.WriteLine($"Kontakt erstellt mit ID {id}");
                    break;

                case "read":
                    if (args.Length < 2 || !int.TryParse(args[1], out int readId))
                    {
                        Console.WriteLine("Ungültige ID: read <ContactID>");
                        return;
                    }
                    repository.ReadContact(readId);
                    break;

                case "list":
                    repository.ReadContacts();
                    break;

                case "update":
                    if (args.Length < 7 || !int.TryParse(args[1], out int updateId))
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
                    if (args.Length < 2 || !int.TryParse(args[1], out int deleteId))
                    {
                        Console.WriteLine("Ungültige ID: delete <ContactID>");
                        return;
                    }
                    bool deleted = repository.DeleteContact(deleteId);
                    Console.WriteLine(deleted ? "Kontakt gelöscht." : "Löschen fehlgeschlagen.");
                    break;
                case "import":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("import <PfadZurCSV>");
                        return;
                    }
                    repository.ImportContactsFromCsv(args[1]);
                    break;

                case "export":
                    if (args.Length < 2)
                    {
                        Console.WriteLine("export <ZielDatei.csv>");
                        return;
                    }
                    repository.ExportContactsToCsv(args[1]);
                    break;

                default:
                    Console.WriteLine("Unbekanntes Kommando. Nutze: create | read | update | delete | list | export | import");
                    break;
            }
        }
    }
}
