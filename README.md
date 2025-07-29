# 📇 KONAKTMANAGEMENT – C# Konsolenanwendung zur Kontaktverwaltung

Ein praktisches Übungsprojekt mit .NET und SQL Server LocalDB, um eine einfache CRUD-Kontaktverwaltung über die Kommandozeile zu realisieren.

---

## 📁 Projektstruktur

```text
KONAKTMANAGEMENT/
├── Businesslogic/
│   └── HandleArgsInput.cs         # Verarbeitung der CLI-Befehle
│
├── DataAccess/
│   ├── dtos/
│   │   └── CRUD Methoden/
│   │       └── ContactRepository.cs  # Datenbankzugriffe
│   └── SQLStatements/             # (Optional für ausgelagerte SQL)
│   └── IContactRepository.cs      # Interface für Repository
│
├── Models/
│   └── Contact.cs                 # Datenmodell der Kontakte
│
├── Program.cs                     # Einstiegspunkt der App
├── KontaktManagement.csproj       # Projektdatei
├── KonaktManagement.sln           # Solution-Datei
├── export_2025.csv                # Exportierte Kontakte als CSV
├── README.md                      # Diese Projektbeschreibung
```

---

## 🗃️ Datenbankstruktur (SQL)

```sql
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
```

---

## 🚀 Anwendung starten

1. Stelle sicher, dass SQL Server LocalDB installiert ist.
2. Datenbank erzeugen (siehe SQL oben).
3. Konfiguriere den Connection String in `Program.cs`:

```csharp
"Server=(localdb)\\MSSQLLocalDB;Database=Contactmanagement;Trusted_Connection=True;"
```

4. Anwendung über CLI starten:

```bash
dotnet run <befehl> <parameter>
```

---

## 🧾 Befehle

| Befehl        | Beschreibung                                                                           |
| ------------- | -------------------------------------------------------------------------------------- |
| `create`      | Erstellt einen Kontakt:<br>`create Max Mustermann max@mail.de 01234 Berlin 1990-01-01` |
| `read <id>`   | Zeigt einen Kontakt mit angegebener ID                                                 |
| `readall`     | Listet alle Kontakte                                                                   |
| `update`      | Aktualisiert einen Kontakt:<br>`update 1 Max Neu max@neu.de 0123 Berlin 1991-01-01`    |
| `delete <id>` | Löscht den Kontakt mit der angegebenen ID                                              |
| `import`      | Importiert Kontakte aus einer CSV-Datei: `import contacts.csv`                         |
| `export`      | Exportiert alle Kontakte als CSV-Datei: `export export_2025.csv`                       |

---

## 📝 CSV-Format

Die CSV-Datei sollte UTF-8-codiert und durch Semikolon (`;`) getrennt sein.

### Beispiel:

```csv
FirstName;LastName;Email;Phonenumber;City;Birthdate
Anna;Schmidt;anna@example.com;017612345678;Berlin;1990-05-15
Max;Müller;max@example.com;017698765432;München;1985-11-30
```

- Leere oder ungültige `Birthdate`-Felder werden als `NULL` gespeichert.
- Doppelte E-Mail-Adressen werden beim Import übersprungen.

---

## 🎯 Lernziele

- Umgang mit SQL Server in C#
- Aufbau von Repository-Strukturen
- Verwendung von Interfaces
- Trennung von Businesslogik und Datenzugriff
- Arbeiten mit `DateOnly?` und Nullable-Typen
- Verarbeitung von CSV-Dateien (Import & Export)
- Argumentverarbeitung in Konsolen-Apps

---

## 🧑‍💻 Hinweis

Dies ist ein Übungsprojekt und dient zur Veranschaulichung von CRUD-Operationen, Datenbankanbindung und CLI-Verarbeitung in C#.
