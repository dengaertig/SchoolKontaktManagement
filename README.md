# 📇 KontaktManagement (C# Konsolen-App mit SQL Server)

Ein einfaches Kommandozeilen-Tool zur Verwaltung von Kontakten mit C# und SQL Server (LocalDB). Die Anwendung unterstützt das Erstellen, Lesen, Aktualisieren und Löschen (CRUD) von Kontaktdaten über Befehlszeilenargumente.

---

## 🛠 Voraussetzungen

- [.NET 6 oder höher](https://dotnet.microsoft.com/)
- SQL Server LocalDB (mit `Contactmanagement`-Datenbank)
- Visual Studio / VS Code / beliebige IDE

---

## 🗃️ Datenbankstruktur

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

## 🚀 Projekt starten

1. **Klonen oder öffnen**
2. **Datenbank sicherstellen** (`Contactmanagement` muss existieren)
3. **Konfigurierte Connection String prüfen in `Program.cs`:**

```csharp
"Server=(localdb)\\MSSQLLocalDB;Database=Contactmanagement;Trusted_Connection=True;"
```

4. **Kompilieren und ausführen mit Argumenten:**

```bash
dotnet run [befehl] [parameter...]
```

---

## 🧾 Verfügbare Befehle

| Befehl        | Beschreibung             | Beispiel                                                      |
| ------------- | ------------------------ | ------------------------------------------------------------- |
| `create`      | Neuen Kontakt anlegen    | `create Max Mustermann max@test.de 01234 Berlin 1990-01-01`   |
| `read <id>`   | Kontakt nach ID anzeigen | `read 1`                                                      |
| `readall`     | Alle Kontakte anzeigen   | `readall`                                                     |
| `update <id>` | Kontakt aktualisieren    | `update 1 Maxine Musterfrau max@neu.de 09876 Köln 1991-02-02` |
| `delete <id>` | Kontakt löschen          | `delete 1`                                                    |

---

## 📦 Projektstruktur

```text
KontaktManagement/
│
├── Program.cs              // Einstiegspunkt (Main)
├── Contact.cs              // Datenmodell
├── ContactRepository.cs    // Datenbankzugriff (CRUD)
├── HandleArgsInput.cs      // Kommandoverarbeitung (Business-Logik)
├── KontaktManagement.csproj
```

---

## ✅ Beispiel

```bash
dotnet run create Anna Schmidt anna@example.com 017612345678 Berlin 1990-05-15
dotnet run read 1
dotnet run update 1 Anna Mustermann anna.neu@example.com 017612300000 München 1991-04-01
dotnet run delete 1
dotnet run readall
```

---

## 🧑‍💻 Motivation / Lernziele

- Arbeiten mit SQL-Datenbanken in C#
- Aufbau einer Repository-Struktur
- Kommandozeilenverarbeitung
- Trennung von Datenzugriff und Steuerlogik (Business Layer)
- Umgang mit Nullable-Typen (`DateOnly?`, `string?`)

---

## 📝 Lizenz

Dies ist ein Lernprojekt – frei verwendbar für Schulung, Übung und persönliche Weiterentwicklung.
