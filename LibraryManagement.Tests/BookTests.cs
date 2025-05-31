using FluentAssertions;
using LibraryManagement.Domain.Entities;
using Xunit;

namespace LibraryManagement.Tests
{
    public class BookTests
    {
        [Fact]
        public void Book_WhenCreated_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            var book = new Book
            {
                Title = "Test Book",
                Author = "Test Author"
            };

            // Assert
            book.IsAvailable.Should().BeTrue();
            book.IsActive.Should().BeTrue();
            book.BorrowRecords.Should().NotBeNull();
            book.BorrowRecords.Should().BeEmpty();
        }

        [Fact]
        public void Book_WhenCreated_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var title = "Test Book";
            var author = "Test Author";
            var categoryId = 1;

            // Act
            var book = new Book
            {
                Title = title,
                Author = author,
                CategoryId = categoryId
            };

            // Assert
            book.Title.Should().Be(title);
            book.Author.Should().Be(author);
            book.CategoryId.Should().Be(categoryId);
        }

        [Fact]
        public void Book_WhenBorrowed_ShouldUpdateAvailability()
        {
            // Arrange
            var book = new Book
            {
                Title = "Test Book",
                Author = "Test Author",
                IsAvailable = true
            };

            // Act
            book.IsAvailable = false;

            // Assert
            book.IsAvailable.Should().BeFalse();
        }

        [Fact]
        public void Book_WhenDeactivated_ShouldUpdateActiveStatus()
        {
            // Arrange
            var book = new Book
            {
                Title = "Test Book",
                Author = "Test Author",
                IsActive = true
            };

            // Act
            book.IsActive = false;

            // Assert
            book.IsActive.Should().BeFalse();
        }
    }
} 