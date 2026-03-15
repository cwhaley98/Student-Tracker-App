using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Student_Tracker_App.Services;

namespace Student_Tracker_App.Tests
{
    public class SecurityTests
    {
        [Fact]
        public void HashPassword_ShouldReturnConsistentHash_ForSameInput()
        {
            //Arrange
            string password = "TestPassword123";

            //Act
            string hash1 = SecurityHelper.HashPassword(password);
            string hash2 = SecurityHelper.HashPassword(password);

            //Assert
            Assert.NotNull(hash1);
            Assert.Equal(hash1, hash2); // The same input should produce the same hash
        }

        [Fact]
        public void HashPassword_ShouldReturnDifferentHash_ForDifferentInput()
        {
            //Arrange
            string password1 = "TestPassword123";
            string password2 = "AnotherPassword456";
            //Act
            string hash1 = SecurityHelper.HashPassword(password1);
            string hash2 = SecurityHelper.HashPassword(password2);
            //Assert
            Assert.NotNull(hash1);
            Assert.NotNull(hash2);
            Assert.NotEqual(hash1, hash2); // Different inputs should produce different hashes
        }
    }
}
