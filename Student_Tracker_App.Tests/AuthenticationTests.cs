using Xunit;
using Student_Tracker_App.Services;
using System.IO;
using System.Threading.Tasks;
using System;

namespace Student_Tracker_App.Tests
{
    public class AuthenticationTests
    {
        // Helper method to create an isolated database for each test
        private DatabaseServices GetTestDatabase()
        {
            // Create a unique temporary database file for each test
            string testDbPath = Path.Combine(Path.GetTempPath(), $"testdb_{Guid.NewGuid()}.db3");
            return new DatabaseServices(testDbPath);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnTrue_WhenUserIsNew()
        {
            //Arrange
            var db = GetTestDatabase();
            string testusername = "TestUser";
            string testpassword = "SecurePassword123";

            //Act
            bool isRegistered = await db.RegisterUserAsync(testusername, testpassword);

            //Assert
            Assert.True(isRegistered);
        }

        [Fact]
        public async Task ValidateLoginAsync_ShouldReturnTrue_WhenCredentialsMatch()
        {
            //Arrange
            var db = GetTestDatabase();

            string testusername = "TestUser2";
            string testpassword = "AnotherSecurePassword456";

            // Seed the database with the test user first
            await db.RegisterUserAsync(testusername, testpassword);

            //Act
            bool isValidLogin = await db.ValidateLoginAsync(testusername, testpassword);

            //Assert
            Assert.True(isValidLogin);
        }

        [Fact]
        public async Task ValidateLoginAsync_ShouldReturnFalse_WhenCredentialsDoNotMatch()
        {
            //Arrange
            var db = GetTestDatabase();
            string testusername = "TestUser3";
            string testpassword = "CorrectPassword789";

            // Seed the database with the test user first
            await db.RegisterUserAsync(testusername, testpassword);

            //Act - Try to login with the correct username but wrong password
            bool isValidLogin = await db.ValidateLoginAsync(testusername, "WrongPassword");
            //Assert
            Assert.False(isValidLogin);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnFalse_WhenUsernameAlreadyExists()
        {
            //Arrange
            var db = GetTestDatabase();
            string testusername = "TestUser4";
            string testpassword = "UniquePassword321";

            //Register the user for the first time
            await db.RegisterUserAsync(testusername, testpassword);

            //Act - Try to register the same username again
            bool isRegisteredAgain = await db.RegisterUserAsync(testusername, "AnotherPassword987");

            //Assert
            Assert.False(isRegisteredAgain);
        }
    }
}
