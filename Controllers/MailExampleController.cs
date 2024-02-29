using Microsoft.AspNetCore.Mvc;
using MvcCoreUtilidades.Helpers;
using System.Net;
using System.Net.Mail;

namespace MvcCoreUtilidades.Controllers
{
    public class MailExampleController : Controller
    {
        private IConfiguration configuration;
        private HelperPathProvider helperPathProvider;

        public MailExampleController(IConfiguration configuration, HelperPathProvider helperPathProvider)
        {
            this.configuration = configuration;
            this.helperPathProvider = helperPathProvider;
        }

        public IActionResult SendMail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendMail(string para, string asunto, string mensaje, IFormFile file)
        {
            MailMessage mail = new MailMessage();

            string user = this.configuration.GetValue<string>("MailSettings:Credentials:User");
            mail.From = new MailAddress(user);
            mail.To.Add(para);
            mail.Subject = asunto;
            mail.Body = mensaje;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;
            //Preguntamos si tenemos ficheros adjuntos
            if (file != null)
            {
                string fileName = file.FileName;

                string path = this.helperPathProvider.MapPath(fileName, Folders.Mails);
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                Attachment attachment = new Attachment(path);
                mail.Attachments.Add(attachment);
            }

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
            await smtpClient.SendMailAsync(mail);
            ViewData["MENSAJE"] = "Email enviado correctamente";

            return View();
        }
    }
}
