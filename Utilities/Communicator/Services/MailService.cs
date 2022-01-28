using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicator.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public Microsoft.Extensions.Hosting.IHostingEnvironment Env { get; }

        public MailService(IOptions<MailSettings> mailSettings, Microsoft.Extensions.Hosting.IHostingEnvironment env)
        {
            _mailSettings = mailSettings.Value;
            Env = env;
        }
        //public async Task SendEmailWithAttachmentAsync(SimpleMailRequest mailRequest)
        //{
        //    var email = new MimeMessage();
        //    email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
        //    email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
        //    email.Subject = mailRequest.Subject;
        //    var builder = new BodyBuilder();
        //    if (mailRequest.Attachments != null)
        //    {
        //        byte[] fileBytes;
        //        foreach (var file in mailRequest.Attachments)
        //        {
        //            if (file.Length > 0)
        //            {
        //                using (var ms = new MemoryStream())
        //                {
        //                    file.CopyTo(ms);
        //                    fileBytes = ms.ToArray();
        //                }
        //                builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
        //            }
        //        }
        //    }
        //    builder.HtmlBody = mailRequest.Body;
        //    email.Body = builder.ToMessageBody();
        //    using var smtp = new SmtpClient();
        //    smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
        //    smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
        //    await smtp.SendAsync(email);
        //    smtp.Disconnect(true);
        //}
        //public async Task SendGeneralPurposeEmailWithAttachmentAsync(EmailTemplateRequest request)//, string File)
        //{
        //    //string FilePath = string.IsNullOrEmpty(File) ? @"\Templates\EmailTemplate\General Purpose Email.html" : File;// Directory.GetCurrentDirectory() + @"\Templates\WelcomeTemplate.html";
        //    string FilePath = Directory.GetCurrentDirectory() + @"\Templates\EmailTemplate\General Purpose Email.html" ;// Directory.GetCurrentDirectory() + @"\Templates\WelcomeTemplate.html";
        //    StreamReader str = new StreamReader(FilePath);
        //    string MailText = str.ReadToEnd();
        //    str.Close();
        //    MailText = MailText.Replace("{Subject}", request.Subject)//.Replace("[email]", request.ToEmail)
        //        .Replace("{MailBody}", request.MailBody)
        //           .Replace("{CompanyNameorSignature}", request.CompanyNameorSignature); ;
        //    var email = new MimeMessage();
        //    email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
        //    email.To.Add(MailboxAddress.Parse(request.ToEmail));
        //    email.Subject = request.Subject;
        //    var builder = new BodyBuilder();
        //    if (request.Attachments != null)
        //    {
        //        byte[] fileBytes;
        //        foreach (var file in request.Attachments)
        //        {
        //            if (file.Length > 0)
        //            {
        //                using (var ms = new MemoryStream())
        //                {
        //                    file.CopyTo(ms);
        //                    fileBytes = ms.ToArray();
        //                }
        //                builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
        //            }
        //        }
        //    }
        //    builder.HtmlBody = MailText;
        //    email.Body = builder.ToMessageBody();
        //    using var smtp = new SmtpClient();
        //    smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
        //    smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
        //    await smtp.SendAsync(email);
        //    smtp.Disconnect(true);
        //}
        //public async Task SendWelcomeEmailAsync(EmailTemplateRequest request,string File)
        //{
        //    string FilePath = File;// Directory.GetCurrentDirectory() + @"\Templates\WelcomeTemplate.html";
        //    StreamReader str = new StreamReader(FilePath);
        //    string MailText = str.ReadToEnd();
        //    str.Close();
        //    MailText = MailText.Replace("[username]", request.UserName).Replace("[email]", request.ToEmail)
        //        .Replace("[EmailConfirmationLink]", request.EmailConfirmationLink);
        //    var email = new MimeMessage();
        //    email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
        //    email.To.Add(MailboxAddress.Parse(request.ToEmail));
        //    email.Subject = $"Welcome {request.UserName}";
        //    var builder = new BodyBuilder();
        //    builder.HtmlBody = MailText;
        //    email.Body = builder.ToMessageBody();
        //    using var smtp = new SmtpClient();
        //    smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
        //    smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
        //    await smtp.SendAsync(email);
        //    smtp.Disconnect(true);
        //}

        public async Task SendConfirmAccountEmailAsync(EmailTemplateRequest request)//, string File)
        {
       //   < Content Include = "Templates\**" >
       //    < CopyToOutputDirectory > PreserveNewest </ CopyToOutputDirectory >
       //</ Content >
             var pathToFile = Env.ContentRootPath
                     +@"Templates\EmailTemplate\Account Confirmation Email.html";
            //string FilePath = string.IsNullOrEmpty( File)? Directory.GetCurrentDirectory() + @"\Templates\EmailTemplate\Account Confirmation Email.html" : File;// Directory.GetCurrentDirectory() + @"\Templates\WelcomeTemplate.html";
            string FilePath =   Directory.GetCurrentDirectory() + @"\Templates\EmailTemplate\Account Confirmation Email.html" ;// Directory.GetCurrentDirectory() + @"\Templates\WelcomeTemplate.html";

            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("{UserName}", request.UserName)//.Replace("[email]", request.ToEmail)
                .Replace("{ConfirmationLink}", request.EmailConfirmationLink)
                .Replace("{CompanyNameorSignature}",request.CompanyNameorSignature);
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject =  string.IsNullOrEmpty( request.Subject)? "Confirm Account Email": request.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        public async Task SendGeneralPurposeEmailAsync(EmailTemplateRequest request)//, string File)
        {

            //string FilePath = string.IsNullOrEmpty(File) ? @"\Templates\EmailTemplate\General Purpose Email.html" : File;// Directory.GetCurrentDirectory() + @"\Templates\WelcomeTemplate.html";
            string FilePath = Directory.GetCurrentDirectory() + @"\Templates\EmailTemplate\General Purpose Email.html" ;// Directory.GetCurrentDirectory() + @"\Templates\WelcomeTemplate.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("{Subject}", request.Subject)//.Replace("[email]", request.ToEmail)
                .Replace("{MailBody}", request.MailBody)
                   .Replace("{CompanyNameorSignature}", request.CompanyNameorSignature); ;
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = request.Subject;
            var builder = new BodyBuilder();
            if (request.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in request.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        public async Task SendResetPasswordLinkEmailAsync(EmailTemplateRequest request)//, string File)
        {

            //string FilePath = string.IsNullOrEmpty(File) ? @"\Templates\EmailTemplate\Password Reset Link Email.html" : File;// Directory.GetCurrentDirectory() + @"\Templates\WelcomeTemplate.html";
            string FilePath = Directory.GetCurrentDirectory() + @"\Templates\EmailTemplate\Password Reset Link Email.html" ;// Directory.GetCurrentDirectory() + @"\Templates\WelcomeTemplate.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("{UserName}", request.UserName)//.Replace("[email]", request.ToEmail)
                .Replace("{VerificationLink}", request.EmailConfirmationLink)
                 .Replace("{CompanyNameorSignature}", request.CompanyNameorSignature); ; ;
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = string.IsNullOrEmpty(request.Subject) ? "Reset Password Link" : request.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        public async Task SendRegistrationSuccessEmailAsync(EmailTemplateRequest request)//, string File)
        {

            //string FilePath = string.IsNullOrEmpty(File) ? @"\Templates\EmailTemplate\Registration Success EMail.html" : File;// Directory.GetCurrentDirectory() + @"\Templates\WelcomeTemplate.html";
            string FilePath = Directory.GetCurrentDirectory() + @"\Templates\EmailTemplate\Registration Success EMail.html" ;// Directory.GetCurrentDirectory() + @"\Templates\WelcomeTemplate.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("{UserName}", request.UserName)//.Replace("[email]", request.ToEmail)
                                                                       //.Replace("[EmailConfirmationLink]", request.EmailConfirmationLink);
                   .Replace("{CompanyNameorSignature}", request.CompanyNameorSignature); ; ;
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            //email.Subject = $"Welcome {request.UserName}";
            email.Subject = string.IsNullOrEmpty(request.Subject) ? "Account Registration Success" : request.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
