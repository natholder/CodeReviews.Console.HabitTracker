# HabitTracker Console App

A console application to log occurrences of miles (runs). The app stores entries in a SQLite database using ADO.NET and supports inserting, viewing, updating and deleting records.

---

## Quick overview

- Record format: `Date` (when the habit occurred) and `Miles` (quantity).
- Database: `HabitTracker.db` is created automatically when the app runs.
- Storage access: ADO.NET only (no EF / Dapper). Parameterized SQL is used to prevent injection.

---

## How to run

1. Open a terminal in the project folder `HabitTracker.natholder`.
2. Build & run:

   ```bash
   dotnet run
   ```

3. The app will create `HabitTracker.db` (if missing) and the `Habits` table.
4. Use the menu to Insert, View, Update, or Delete records.

---

## Features

- Creates a sqlite db and habit table on first run or if they have been deleted.
- Insert records with validated values..
- View (select) all records
- Update an existing record
- Delete a record
- input validation and error handling

---

## Thought process / retrospective

I think that once i got started and made a little bit of progress, that the rest of the project came a lot easier. I had some trouble implementing the view command and had to use the video tutorial. I feel like I am learning though, and am continuting to improve.

---

## Potential improvements (next steps)

- Add unit tests for parsing/validation logic.

---
