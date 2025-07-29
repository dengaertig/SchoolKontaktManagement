# 📁 KONAKTMANAGEMENT – C# Konsolenanwendung zur Kontaktverwaltung

Ein kleines Übungsprojekt in C#, das CRUD-Funktionalität (Create, Read, Update, Delete) für Kontakte bietet. Die Anwendung nutzt SQL Server LocalDB und verarbeitet Kommandozeilenargumente zur Steuerung.

---

## 📦 Projektstruktur

KONAKTMANAGEMENT/
│
├── Businesslogic/
│ └── HandleArgsInput.cs → verarbeitet Befehle aus der Kommandozeile
│
├── DataAccess/
│ ├── dtos/
│ │ └── CRUD Methoden/
│ │ └── ContactRepository.cs → Datenbankzugriffe (SQL)
│ └── SQLStatements/ → (optional, z.B. für ausgelagerte SQL-Queries)
│ └── IContactRepository.cs → Interface für Repository
│
├── Models/
│ └── Contact.cs → Datenmodell für Kontakte
│
├── Program.cs → Einstiegspunkt / Main-Methode
├── KontaktManagement.csproj → Projektdatei
├── KonaktManagement.sln → Solution-Datei
├── export_2025.csv → Exportierte Kontakte (CSV)
├── README.md → (Dieselbe Beschreibung, aber als Markdown für GitHub)

---

## 🗃 Datenbankstruktur (SQL Server)

CREATE DATABASE Contactmanagement;
USE Contactmanagement;

CREATE TABLE Contact (
ContactId INT PRIMARY KEY IDENTITY(1,1),
FirstName VARCHAR(100) NOT NULL,
LastName VARCHAR(100) NOT NULL,
Email VARCHAR(100) UNIQUE NOT NULL,
Phonenumber VARCHAR(50),
City VARCHAR(100),
Birthdate DATE
);

---

## ▶ Anwendung starten

1. Stelle sicher, dass LocalDB läuft und die Datenbank vorhanden ist.
2. Passe ggf. die Verbindungszeichenfolge in `Program.cs` an:

   "Server=(localdb)\\MSSQLLocalDB;Database=Contactmanagement;Trusted_Connection=True;"

3. Kompiliere und führe die Anwendung aus:

   dotnet run <befehl> <parameter>

---

## 💡 Unterstützte Befehle

- `create <FirstName> <LastName> <Email> <Phone> <City> <Birthdate>`
- `read <ContactID>` → Zeigt einen Kontakt
- `readall` → Listet alle Kontakte auf
- `update <ID> <...>`→ Aktualisiert einen Kontakt
- `delete <ContactID>`
- `import <PfadZurCSV>` → CSV-Datei importieren, doppelte E-Mails werden übersprungen
- `export <PfadZurCSV>` → Exportiert alle Kontakte als CSV

---

## 📌 CSV Format (UTF-8, mit ; getrennt)

FirstName;LastName;Email;Phonenumber;City;Birthdate  
Anna;Schmidt;anna@example.com;0176...;Berlin;1990-05-15  
Max;Müller;max@example.com;0176...;München;1985-11-30

Leere Datumswerte oder ungültige Formate werden beim Import ignoriert.

---

## 📚 Lernziele

✔ Arbeiten mit SQL Server aus C#  
✔ Nutzung von Repositories und Interfaces  
✔ Trennung von Logikschichten (Models / Business / DataAccess)  
✔ Umgang mit DateOnly?, Nullable, CSV-Dateien und Formatprüfungen  
✔ CLI-Eingabeverarbeitung mit Switch-Logik

---

📝 Hinweis

Dies ist ein Übungsprojekt zur Konsolenentwicklung mit Datenbankanbindung in .NET.
