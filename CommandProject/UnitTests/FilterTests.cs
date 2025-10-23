using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.Database.Mocks;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class FilterTests
    {
        private InMemoryDatabaseHelper db;

        [TestInitialize]
        public void Init() => db = new InMemoryDatabaseHelper();

        [TestMethod]
        public void Filter_ByMinRating_ReturnsExpected()
        {
            var dt = db.GetBooksByFilter(null, null, 4.6m, null);
            Assert.IsTrue(dt.Rows.Count >= 1);
            var titles = new List<string>();
            foreach (System.Data.DataRow r in dt.Rows) titles.Add(r["Title"] as string);
            Assert.IsTrue(titles.Contains("Clean Code"));
            Assert.IsTrue(titles.Contains("The Hobbit"));
        }

        [TestMethod]
        public void Filter_ByGenre_ReturnsProgrammingBooks()
        {
            var dt = db.GetBooksByFilter(null, 1, null, null);
            Assert.IsTrue(dt.Rows.Count >= 1);
            var titles = new List<string>();
            foreach (System.Data.DataRow r in dt.Rows) titles.Add(r["Title"] as string);
            Assert.IsTrue(titles.Contains("Clean Code"));
            Assert.IsTrue(titles.Contains("The Pragmatic Programmer"));
        }
    }
}
