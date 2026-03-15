using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Student_Tracker_App.Schemas;
using Student_Tracker_App.Services;
using Xunit;
using System.IO;

namespace Student_Tracker_App.Tests
{
    public class CrudTests
    {
        //Helper method to create a isolated database for each test

        private DatabaseServices GetTestDatabase()
        {
            string testDbPath = Path.Combine(Path.GetTempPath(), $"testdb_{Guid.NewGuid()}.db3");
            return new DatabaseServices(testDbPath);

        }

        [Fact]
        public async Task SaveTermAsync_ShouldAddTerm_WhenTermIsNew()  // CREATE
        {
            // Arrange
            var db = GetTestDatabase();
            var term = new Term { TermTitle = "Fall 2026", TermStart = DateTime.Now, TermEnd = DateTime.Now.AddMonths(6) };
            // Act
            int result = await db.SaveTermAsync(term);
            var terms = await db.GetTermsAsync();
            // Assert
            Assert.Equal(1, result); // SQLite should return 1 for a new term when successfully saved
            Assert.Single(terms); // Ensures there should be exactly one term in the database
        }

        [Fact]
        public async Task GetTermAsync_ShouldRetrieveCorrectTerm() // READ
        {
            // Arrange
            var db = GetTestDatabase();
            var term = new Term { TermTitle = "Spring 2027", TermStart = DateTime.Now, TermEnd = DateTime.Now.AddMonths(6) };
            await db.SaveTermAsync(term);

            // Act
            var retrievedTerm = await db.GetTermAsync(1);

            // Assert
            Assert.NotNull(retrievedTerm);

            // The retrieved term should have the same properties as the original term and avoids SQLite DateTime metadata stripping issues
            Assert.Equal(term.TermId, retrievedTerm.TermId);
            Assert.Equal(term.TermTitle, retrievedTerm.TermTitle);

            // Converts TermStart to a string format that excludes milliseconds for comparison, as SQLite may strip milliseconds from DateTime values
            Assert.Equal(term.TermStart.ToString("yyyy-MM-dd HH:mm"), retrievedTerm.TermStart.ToString("yyyy-MM-dd HH:mm"));
        }

        [Fact]
        public async Task SaveTermAsync_ShouldUpdateTerm_WhenTermExists() // UPDATE
        {
            // Arrange
            var db = GetTestDatabase();
            var term = new Term { TermTitle = "Summer 2027", TermStart = DateTime.Now, TermEnd = DateTime.Now.AddMonths(6) };
            int termId = await db.SaveTermAsync(term);
            term.TermId = termId; // Ensure the term has the correct ID for updating
            term.TermTitle = "Updated Summer 2027"; // Change title to verify update
            // Act
            int result = await db.SaveTermAsync(term);
            var updatedTerm = await db.GetTermAsync(termId);
            // Assert
            Assert.Equal(1, result); // Should return 1 for a successful update
            Assert.Equal("Updated Summer 2027", updatedTerm.TermTitle); // Verify the title was updated
        }

        [Fact]
        public async Task DeleteTermAsync_ShouldRemoveTerm() // DELETE
        {
            // Arrange
            var db = GetTestDatabase();
            var term = new Term { TermTitle = "Winter 2027", TermStart = DateTime.Now, TermEnd = DateTime.Now.AddMonths(6) };
            await db.SaveTermAsync(term);
            // Act
            int result = await db.DeleteTermAsync(term);
            var remainingTerms = await db.GetTermsAsync();
            // Assert
            Assert.Equal(1, result); // Should return 1 for a successful deletion
            Assert.Empty(remainingTerms); // The term should no longer exist in the database
        }

    }
}
