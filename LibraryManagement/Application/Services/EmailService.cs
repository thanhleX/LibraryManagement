using System.Net.Mail;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace LibraryManagement.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _smtpServer = _configuration["EmailSettings:SmtpServer"] ?? throw new ArgumentNullException("SmtpServer");
            _smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            _smtpUsername = _configuration["EmailSettings:SmtpUsername"] ?? throw new ArgumentNullException("SmtpUsername");
            _smtpPassword = _configuration["EmailSettings:SmtpPassword"] ?? throw new ArgumentNullException("SmtpPassword");
            _fromEmail = _configuration["EmailSettings:FromEmail"] ?? throw new ArgumentNullException("FromEmail");
            _fromName = _configuration["EmailSettings:FromName"] ?? "Library Management System";
        }

        public async Task SendReturnReminderEmailAsync(BorrowRecord borrowRecord)
        {
            var dueDate = borrowRecord.BorrowedAt.AddDays(14); // Giả sử thời hạn mượn là 14 ngày
            var daysUntilDue = (dueDate - DateTime.UtcNow).Days;

            if (daysUntilDue <= 3 && daysUntilDue > 0)
            {
                var subject = $"Reminder: Book Return Due in {daysUntilDue} Days";
                var body = GenerateReturnReminderEmailBody(borrowRecord, daysUntilDue);
                var recipientEmail = borrowRecord.User?.Email ?? borrowRecord.Borrower?.Email;

                if (!string.IsNullOrEmpty(recipientEmail))
                {
                    await SendEmailAsync(recipientEmail, subject, body);
                }
            }
        }

        public async Task SendOverdueEmailAsync(BorrowRecord borrowRecord)
        {
            var dueDate = borrowRecord.BorrowedAt.AddDays(14);
            var daysOverdue = (DateTime.UtcNow - dueDate).Days;

            if (daysOverdue > 0)
            {
                var subject = $"Overdue Notice: Book Return {daysOverdue} Days Late";
                var body = GenerateOverdueEmailBody(borrowRecord, daysOverdue);
                var recipientEmail = borrowRecord.User?.Email ?? borrowRecord.Borrower?.Email;

                if (!string.IsNullOrEmpty(recipientEmail))
                {
                    await SendEmailAsync(recipientEmail, subject, body);
                }
            }
        }

        private string GenerateReturnReminderEmailBody(BorrowRecord borrowRecord, int daysUntilDue)
        {
            var bookTitle = borrowRecord.Book?.Title ?? "Unknown Book";
            var dueDate = borrowRecord.BorrowedAt.AddDays(14).ToString("dd/MM/yyyy");

            return $@"
                Dear {borrowRecord.User?.FullName ?? borrowRecord.Borrower?.FullName},

                This is a friendly reminder that the book '{bookTitle}' you borrowed is due for return in {daysUntilDue} days (by {dueDate}).

                Please return the book to the library before the due date to avoid any late fees.

                Thank you for your cooperation.

                Best regards,
                Library Management System";
        }

        private string GenerateOverdueEmailBody(BorrowRecord borrowRecord, int daysOverdue)
        {
            var bookTitle = borrowRecord.Book?.Title ?? "Unknown Book";
            var dueDate = borrowRecord.BorrowedAt.AddDays(14).ToString("dd/MM/yyyy");

            return $@"
                Dear {borrowRecord.User?.FullName ?? borrowRecord.Borrower?.FullName},

                This is to inform you that the book '{bookTitle}' you borrowed is now {daysOverdue} days overdue.
                The book was due for return on {dueDate}.

                Please return the book to the library as soon as possible to avoid accumulating late fees.

                Thank you for your immediate attention to this matter.

                Best regards,
                Library Management System";
        }

        private async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                EnableSsl = true,
                Credentials = new System.Net.NetworkCredential(_smtpUsername, _smtpPassword)
            };

            using var message = new MailMessage
            {
                From = new MailAddress(_fromEmail, _fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };
            message.To.Add(to);

            await client.SendMailAsync(message);
        }
    }
} 