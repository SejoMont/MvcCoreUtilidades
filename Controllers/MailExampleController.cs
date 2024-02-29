using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace MvcCoreUtilidades.Controllers
{
    public class MailExampleController : Controller
    {
        private IConfiguration configuration;

        public MailExampleController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IActionResult SendMail()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SendMail(string para, string asunto, string mensaje, IFormFile file)
        {
            MailMessage mail = new MailMessage();

            string user = this.configuration.GetValue<string>("MailSettings:Credentials:User");
            mail.From = new MailAddress(user);
            mail.To.Add(para);
            mail.Subject = asunto;
            mail.Body = mensaje;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;

            //Configuramos nuestro SMTP Server
            string password = this.configuration.GetValue<string>("MailSettings:Credentials:Password");
            string hostName = this.configuration.GetValue<string>("MailSettings:ServerSmtp:Host");
            int port = this.configuration.GetValue<int>("MailSettings:ServerSmtp:Port");
            bool enableSSL = this.configuration.GetValue<bool>("MailSettings:ServerSmtp:EnableSsl");
            bool defaultCredentials = this.configuration.GetValue<bool>("MailSettings:ServerSmtp:DefaultCredentials");

            //Creamos el servidor para enviar los mails
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = hostName;
            smtpClient.Port = port;
            smtpClient.EnableSsl = enableSSL;
            smtpClient.UseDefaultCredentials = defaultCredentials;

            //Creamos las Credenciales
            NetworkCredential credentials = new NetworkCredential(user, password);
            smtpClient.Credentials = credentials;
            smtpClient.Send(mail);
            ViewData["MENSAJE"] = "Email enviado correctamente";

            return View();
        }
    }
}
