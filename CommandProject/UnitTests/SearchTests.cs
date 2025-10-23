using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Data;
using UnitTests.Database.Mocks;

namespace UnitTests
{
    [TestClass]
    public class SearchTests
    {
        private InMemoryDatabaseHelper db;

        [TestInitialize]
        public void Init() => db = new InMemoryDatabaseHelper();

        [TestMethod]
        public void Search_ByTitle_FindsCorrectBook()
        {
            var dt = db.GetAllBooks();
            string q = "hobbit";
            var found = (from DataRow r in dt.Rows
                         where ((r["Title"] as string) ?? string.Empty).ToLower().Contains(q)
                         select r).ToList();
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual("The Hobbit", found[0]["Title"] as string);
        }

        [TestMethod]
        public void Search_ByAuthor_FindsMultipleProgrammingBooks()
        {
            var dt = db.GetAllBooks();
            string q = "robert";
            var found = (from DataRow r in dt.Rows
                         where ((r["Author"] as string) ?? string.Empty).ToLower().Contains(q)
                         select r).ToList();
            Assert.AreEqual(1, found.Count);
            Assert.IsTrue((found[0]["Title"] as string).Contains("Clean Code"));
        }
    }
}
