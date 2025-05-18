using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public void EnviarCodigo(string destino, string codigo)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_config["EmailSettings:SenderEmail"]));
        email.To.Add(MailboxAddress.Parse(destino));
        email.Subject = "Código de recuperación de contraseña";

        email.Body = new TextPart("plain")
        {
            Text = $"Hola, tu código de recuperación es: {codigo}"
        };

        using var smtp = new SmtpClient();
        smtp.Connect(_config["EmailSettings:SmtpServer"], int.Parse(_config["EmailSettings:Port"]), false);
        smtp.Authenticate(_config["EmailSettings:SenderEmail"], _config["EmailSettings:SenderPassword"]);
        smtp.Send(email);
        smtp.Disconnect(true);
    }
}
