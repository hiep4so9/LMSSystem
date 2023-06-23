using LMSSystem.Data;
using LMSSystem.Repositories.IRepository;
using MimeKit;
namespace LMSSystem.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        public async Task SendEmailAsync(EmailDTO request, string filepath = null!)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("hiep4so9@gmail.com"));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;

            if (filepath != null)
            {
                var image = new MimePart("image", "jpeg")
                {
                    Content = new MimeContent(File.OpenRead(filepath)),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = Path.GetFileName(filepath)
                };
                var multipart = new Multipart("mixed");
                multipart.Add(image);
                multipart.Add(new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = request.Body,
                });
                email.Body = multipart;
            }
            else
            {
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = request.Body,

                };
            }
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("hiep4so9@gmail.com", "ijndbyqxenojhzcn");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
