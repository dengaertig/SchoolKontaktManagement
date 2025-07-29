using System;

namespace KontaktManagement;

class Program
{
    static void Main(string[] args)
    {
        var connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Contactmanagement;Trusted_Connection=True;";
        var repository = new ContactRepository(connectionString);
        var handler = new HandleArgsInput(repository);

        handler.Process(args);
    }
}
