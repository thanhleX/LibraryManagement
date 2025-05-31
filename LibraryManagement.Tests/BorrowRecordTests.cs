using FluentAssertions;
using LibraryManagement.Domain.Entities;
using Xunit;

namespace LibraryManagement.Tests
{
    public class BorrowRecordTests
    {
        [Fact]
        public void BorrowRecord_WhenCreated_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            var borrowRecord = new BorrowRecord();

            // Assert
            borrowRecord.BorrowedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            borrowRecord.ReturnedAt.Should().BeNull();
            borrowRecord.IsReturned.Should().BeFalse();
        }

        [Fact]
        public void BorrowRecord_WhenReturned_ShouldUpdateReturnStatus()
        {
            // Arrange
            var borrowRecord = new BorrowRecord();
            var returnTime = DateTime.Now.AddDays(7);

            // Act
            borrowRecord.ReturnedAt = returnTime;

            // Assert
            borrowRecord.IsReturned.Should().BeTrue();
            borrowRecord.ReturnedAt.Should().Be(returnTime);
        }

        [Fact]
        public void BorrowRecord_WhenCreatedWithUser_ShouldSetUserProperties()
        {
            // Arrange
            var userId = 1;
            var bookId = 1;

            // Act
            var borrowRecord = new BorrowRecord
            {
                UserId = userId,
                BookId = bookId
            };

            // Assert
            borrowRecord.UserId.Should().Be(userId);
            borrowRecord.BookId.Should().Be(bookId);
            borrowRecord.BorrowerId.Should().BeNull();
        }

        [Fact]
        public void BorrowRecord_WhenCreatedWithBorrower_ShouldSetBorrowerProperties()
        {
            // Arrange
            var borrowerId = 1;
            var bookId = 1;

            // Act
            var borrowRecord = new BorrowRecord
            {
                BorrowerId = borrowerId,
                BookId = bookId
            };

            // Assert
            borrowRecord.BorrowerId.Should().Be(borrowerId);
            borrowRecord.BookId.Should().Be(bookId);
            borrowRecord.UserId.Should().BeNull();
        }
    }
} 