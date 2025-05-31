using FluentAssertions;
using LibraryManagement.Domain.Entities;
using Xunit;

namespace LibraryManagement.Tests
{
    public class UserTests
    {
        [Fact]
        public void User_WhenCreated_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            var user = new User
            {
                Username = "testuser",
                Password = "password123",
                FullName = "Test User",
                Email = "test@example.com",
                Role = "User"
            };

            // Assert
            user.IsActive.Should().BeTrue();
            user.BorrowRecords.Should().NotBeNull();
            user.BorrowRecords.Should().BeEmpty();
        }

        [Fact]
        public void User_WhenCreated_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var username = "testuser";
            var password = "password123";
            var fullName = "Test User";
            var email = "test@example.com";
            var role = "User";

            // Act
            var user = new User
            {
                Username = username,
                Password = password,
                FullName = fullName,
                Email = email,
                Role = role
            };

            // Assert
            user.Username.Should().Be(username);
            user.Password.Should().Be(password);
            user.FullName.Should().Be(fullName);
            user.Email.Should().Be(email);
            user.Role.Should().Be(role);
        }

        [Fact]
        public void User_WhenDeactivated_ShouldUpdateActiveStatus()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                Password = "password123",
                FullName = "Test User",
                Email = "test@example.com",
                Role = "User",
                IsActive = true
            };

            // Act
            user.IsActive = false;

            // Assert
            user.IsActive.Should().BeFalse();
        }

        [Fact]
        public void User_WhenCreated_ShouldInitializeBorrowRecordsCollection()
        {
            // Arrange & Act
            var user = new User();

            // Assert
            user.BorrowRecords.Should().NotBeNull();
            user.BorrowRecords.Should().BeEmpty();
        }

        [Fact]
        public void User_WhenAddingBorrowRecord_ShouldUpdateCollection()
        {
            // Arrange
            var user = new User();
            var borrowRecord = new BorrowRecord();

            // Act
            user.BorrowRecords.Add(borrowRecord);

            // Assert
            user.BorrowRecords.Should().Contain(borrowRecord);
            user.BorrowRecords.Should().HaveCount(1);
        }
    }
} 