using System;
using System.Data;
using CommandProject.Database;

namespace UnitTests.Database.Mocks
{
    // Minimal in-memory substitute implementing only used methods for tests
    public class InMemoryDatabaseHelper
    {
        private DataTable books = new DataTable();
        private DataTable users = new DataTable();

        public InMemoryDatabaseHelper()
        {
            // setup columns similar to DatabaseHelper expectations
            books.Columns.Add("ID", typeof(int));
            books.Columns.Add("Title", typeof(string));
            books.Columns.Add("Description", typeof(string));
            books.Columns.Add("CoverPath", typeof(string));
            books.Columns.Add("Author", typeof(string));
            books.Columns.Add("Genres", typeof(string));
            books.Columns.Add("PublishedYear", typeof(int));
            books.Columns.Add("Rating", typeof(decimal));

            users.Columns.Add("UserID", typeof(int));
            users.Columns.Add("Username", typeof(string));
            users.Columns.Add("Email", typeof(string));
            users.Columns.Add("FullName", typeof(string));

            // seed some books
            books.Rows.Add(1, "The Hobbit", "A fantasy novel", "", "J.R.R. Tolkien", "Fantasy", 1937, 4.8m);
            books.Rows.Add(2, "Clean Code", "A guide to writing clean code", "", "Robert C. Martin", "Programming", 2008, 4.6m);
            books.Rows.Add(3, "The Pragmatic Programmer", "Practical programming tips", "", "Andrew Hunt", "Programming", 1999, 4.5m);
            books.Rows.Add(4, "Cooking 101", "Beginner recipes", "", "Jane Doe", "Cooking", 2015, 3.8m);

            // seed users
            users.Rows.Add(1, "existinguser", "exist@example.com", "Existing User");
        }

        public DataTable GetAllBooks() => books.Copy();
        public DataTable GetBooksByFilter(int? authorId, int? genreId, decimal? minRating, decimal? maxRating)
        {
            // we'll implement simple filter by minRating/maxRating and genre string match
            var dt = books.Clone();
            foreach (DataRow r in books.Rows)
            {
                decimal rating = r.Field<decimal>("Rating");
                string genres = r.Field<string>("Genres") ?? string.Empty;
                if (minRating.HasValue && rating < minRating.Value) continue;
                if (maxRating.HasValue && rating > maxRating.Value) continue;
                if (genreId.HasValue)
                {
                    // for tests we'll treat genreId as index into an array: 1=Programming,2=Fantasy,3=Cooking
                    string want = genreId.Value == 1 ? "Programming" : genreId.Value == 2 ? "Fantasy" : "Cooking";
                    if (!genres.Contains(want)) continue;
                }
                dt.ImportRow(r);
            }
            return dt;
        }

        public (bool Success, string Message, int UserID) RegisterUser(string username, string email, string password, string fullName)
        {
            // simulate existing user
            foreach (DataRow r in users.Rows)
            {
                if (string.Equals(r.Field<string>("Username"), username, StringComparison.OrdinalIgnoreCase))
                    return (false, "Пользователь с таким именем уже существует", -1);
                if (string.Equals(r.Field<string>("Email"), email, StringComparison.OrdinalIgnoreCase))
                    return (false, "Email уже зарегистрирован", -1);
            }

            int newId = users.Rows.Count + 1;
            users.Rows.Add(newId, username, email, fullName);
            return (true, "OK", newId);
        }

        public bool UserExists(string username)
        {
            foreach (DataRow r in users.Rows)
                if (string.Equals(r.Field<string>("Username"), username, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        public bool EmailExists(string email)
        {
            foreach (DataRow r in users.Rows)
                if (string.Equals(r.Field<string>("Email"), email, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }
    }
}
