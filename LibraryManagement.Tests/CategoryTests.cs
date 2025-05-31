using FluentAssertions;
using LibraryManagement.Domain.Entities;
using Xunit;

namespace LibraryManagement.Tests
{
    public class CategoryTests
    {
        [Fact]
        public void Category_WhenCreated_ShouldInitializeBooksCollection()
        {
            // Arrange & Act
            var category = new Category();

            // Assert
            category.Books.Should().NotBeNull();
            category.Books.Should().BeEmpty();
        }

        [Fact]
        public void Category_WhenCreated_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var name = "Fiction";

            // Act
            var category = new Category
            {
                Name = name
            };

            // Assert
            category.Name.Should().Be(name);
        }

        [Fact]
        public void Category_WhenAddingBook_ShouldUpdateBooksCollection()
        {
            // Arrange
            var category = new Category();
            var book = new Book
            {
                Title = "Test Book",
                Author = "Test Author"
            };

            // Act
            category.Books.Add(book);

            // Assert
            category.Books.Should().Contain(book);
            category.Books.Should().HaveCount(1);
        }

        [Fact]
        public void Category_WhenAddingMultipleBooks_ShouldUpdateBooksCollection()
        {
            // Arrange
            var category = new Category();
            var book1 = new Book { Title = "Book 1", Author = "Author 1" };
            var book2 = new Book { Title = "Book 2", Author = "Author 2" };

            // Act
            category.Books.Add(book1);
            category.Books.Add(book2);

            // Assert
            category.Books.Should().Contain(book1);
            category.Books.Should().Contain(book2);
            category.Books.Should().HaveCount(2);
        }
    }
} 