using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.Database.Mocks;

namespace UnitTests
{
    [TestClass]
    public class RegistrationTests
    {
        private InMemoryDatabaseHelper db;

        [TestInitialize]
        public void Init() => db = new InMemoryDatabaseHelper();

        [TestMethod]
        public void Register_NewUser_Succeeds()
        {
            var res = db.RegisterUser("newuser", "new@example.com", "Pass123", "New User");
            Assert.IsTrue(res.Success);
            Assert.IsTrue(res.UserID > 0);
        }

        [TestMethod]
        public void Register_ExistingUsername_Fails()
        {
            var res = db.RegisterUser("existinguser", "other@example.com", "Pass123", "Someone");
            Assert.IsFalse(res.Success);
        }

        [TestMethod]
        public void Register_ExistingEmail_Fails()
        {
            var res = db.RegisterUser("anotheruser", "exist@example.com", "Pass123", "Someone");
            Assert.IsFalse(res.Success);
        }
    }
}
