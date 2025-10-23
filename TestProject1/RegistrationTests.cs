using UnitTests.Database.Mocks;

namespace UnitTests
{
    public class RegistrationTests
    {
        private InMemoryDatabaseHelper db;

        [SetUp]
        public void Init() => db = new InMemoryDatabaseHelper();

        [Test]
        public void Register_NewUser_Succeeds()
        {
            var res = db.RegisterUser("newuser", "new@example.com", "Pass123", "New User");
            Assert.IsTrue(res.Success);
            Assert.IsTrue(res.UserID > 0);
        }

        [Test]
        public void Register_ExistingUsername_Fails()
        {
            var res = db.RegisterUser("existinguser", "other@example.com", "Pass123", "Someone");
            Assert.IsFalse(res.Success);
        }

        [Test]
        public void Register_ExistingEmail_Fails()
        {
            var res = db.RegisterUser("anotheruser", "exist@example.com", "Pass123", "Someone");
            Assert.IsFalse(res.Success);
        }
    }
}
