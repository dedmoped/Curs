using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResourceAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ResourceAuth.Help
{
    public class Email
    {
        public static async Task EmailSend(string selleremail, string subj, string attach,IServiceScope scope)
        {
            try
            {
                IConfiguration _configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(_configuration["Email:FromEmail"]);
                        mail.To.Add("vilbicos2000@gmail.com");
                        mail.Subject = subj;
                        mail.Body = attach;

                        using (SmtpClient smtp = new SmtpClient(_configuration["Email:SmtpHost"], Convert.ToInt32(_configuration["Email:SmtpPort"])))
                        {
                            smtp.Credentials = new System.Net.NetworkCredential(_configuration["Email:FromEmail"], _configuration["Email:Password"]);
                            smtp.EnableSsl = true;
                           await smtp.SendMailAsync(mail);
                        }
                    }
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }

        }
    }
}
