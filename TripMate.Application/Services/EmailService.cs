using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace TripMate.Application.Services;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendOtpAsync(
        string toEmail,
        string otp)
    {
        var email = new MimeMessage();

        email.From.Add(
            MailboxAddress.Parse(
                _config["EmailSettings:Email"]));

        email.To.Add(
            MailboxAddress.Parse(toEmail));

        email.Subject = "TripMate OTP";

        email.Body = new TextPart("plain")
        {
            Text = $"Your OTP Code is: {otp}"
        };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            _config["EmailSettings:Host"],
            int.Parse(_config["EmailSettings:Port"]),
            MailKit.Security.SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(
            _config["EmailSettings:Email"],
            _config["EmailSettings:Password"]);

        await smtp.SendAsync(email);

        await smtp.DisconnectAsync(true);
    }
}