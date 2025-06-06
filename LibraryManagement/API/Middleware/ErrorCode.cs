namespace LibraryManagement.API.Middleware
{
    public record ErrorCode(int code, string message, int httpStatusCode)
    {
        public string FormattedMessage(params object[] args)
        {
            return string.Format(message, args);
        }
    }

    public static class ErrorCodes
    {
        public static ErrorCode BOOK_ID_NOT_FOUND => new (1001, "Book ID not found", StatusCodes.Status404NotFound);
        public static ErrorCode CATEGORY_ID_NOT_FOUND => new(1002, "Category ID not found", StatusCodes.Status404NotFound);
        public static ErrorCode BORROWER_NOT_FOUND => new(1003, "Borrower not found", StatusCodes.Status404NotFound);
        public static ErrorCode BOOK_NOT_AVAILABLE => new(1004, "Book is not available for borrowing", StatusCodes.Status400BadRequest);
        public static ErrorCode BORROW_RECORD_NOT_FOUND => new(1005, "Borrow record not found", StatusCodes.Status404NotFound);
        public static ErrorCode SERVER_ERROR => new(1006, "An unexpected error occurred", StatusCodes.Status500InternalServerError);
        public static ErrorCode USERNAME_ALREADY_EXISTS => new(1007, "Username already exists", StatusCodes.Status400BadRequest);
        public static ErrorCode PASSWORD_MISMATCH => new(1008, "Passwords do not match", StatusCodes.Status400BadRequest);
        public static ErrorCode USER_ID_NOT_FOUND => new(1009, "User ID not found", StatusCodes.Status404NotFound);
        public static ErrorCode CATEGORY_NAME_ALREADY_EXISTS => new(1010, "Category name already exists", StatusCodes.Status400BadRequest);
        public static ErrorCode INVALID_CREDENTIALS => new(1011, "Invalid credentials", StatusCodes.Status401Unauthorized);
        public static ErrorCode INVALID_TOKEN => new(1012, "Invalid token", StatusCodes.Status401Unauthorized);
        public static ErrorCode USER_NOT_FOUND => new(1013, "User not found", StatusCodes.Status404NotFound);
        public static ErrorCode BOOK_IMAGE_NOT_FOUND => new(1014, "Book image not found", StatusCodes.Status404NotFound);
        public static ErrorCode INVALID_IMAGE_FORMAT => new(1015, "Invalid image format. Allowed formats: JPG, JPEG, PNG, GIF", StatusCodes.Status400BadRequest);
    }
}
