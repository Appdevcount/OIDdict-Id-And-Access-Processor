using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicator.Services
{
    public interface IMailService
    {
        //Task SendEmailAsync(SimpleMailRequest mailRequest);
        //Task SendWelcomeEmailAsync(EmailTemplateRequest request,String File);
        Task SendConfirmAccountEmailAsync(EmailTemplateRequest request);//, string File);
        Task SendGeneralPurposeEmailAsync(EmailTemplateRequest request);//, string File);
        //Task SendGeneralPurposeEmailWithAttachmentAsync(EmailTemplateRequest request);//, string File);
        Task SendResetPasswordLinkEmailAsync(EmailTemplateRequest request);//, string File);
        Task SendRegistrationSuccessEmailAsync(EmailTemplateRequest request);//, string File);
    }
}
