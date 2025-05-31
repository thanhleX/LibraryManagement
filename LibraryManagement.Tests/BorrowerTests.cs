using FluentAssertions;
using LibraryManagement.Domain.Entities;
using Xunit;

namespace LibraryManagement.Tests
{
    public class BorrowerTests
    {
        [Fact]
        public void Borrower_WhenCreated_ShouldInitializeBorrowRecordsCollection()
        {
            // Arrange & Act
            var borrower = new Borrower();

            // Assert
            borrower.BorrowRecords.Should().NotBeNull();
            borrower.BorrowRecords.Should().BeEmpty();
        }

        [Fact]
        public void Borrower_WhenCreated_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var fullName = "John Doe";
            var email = "john.doe@example.com";

            // Act
            var borrower = new Borrower
            {
                FullName = fullName,
                Email = email
            };

            // Assert
            borrower.FullName.Should().Be(fullName);
            borrower.Email.Should().Be(email);
        }

        [Fact]
        public void Borrower_WhenAddingBorrowRecord_ShouldUpdateCollection()
        {
            // Arrange
            var borrower = new Borrower();
            var borrowRecord = new BorrowRecord();

            // Act
            borrower.BorrowRecords.Add(borrowRecord);

            // Assert
            borrower.BorrowRecords.Should().Contain(borrowRecord);
            borrower.BorrowRecords.Should().HaveCount(1);
        }

        [Fact]
        public void Borrower_WhenAddingMultipleBorrowRecords_ShouldUpdateCollection()
        {
            // Arrange
            var borrower = new Borrower();
            var borrowRecord1 = new BorrowRecord();
            var borrowRecord2 = new BorrowRecord();

            // Act
            borrower.BorrowRecords.Add(borrowRecord1);
            borrower.BorrowRecords.Add(borrowRecord2);

            // Assert
            borrower.BorrowRecords.Should().Contain(borrowRecord1);
            borrower.BorrowRecords.Should().Contain(borrowRecord2);
            borrower.BorrowRecords.Should().HaveCount(2);
        }
    }
} 