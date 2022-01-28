using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communicator
{
    public class EmailTemplateRequest
    {
        public string ToEmail { get; set; }
        public string UserName { get; set; }
        public string EmailConfirmationLink { get; set; }
        public string OTP { get; set; }
        public string GeneratedPassword { get; set; }
        public string Subject { get; set; }
        public string MailBody { get; set; }
        public string CompanyNameorSignature { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
