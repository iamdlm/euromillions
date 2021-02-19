using EuromillionsCore.Interfaces;
using EuromillionsCore.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EuromillionsCore.Services
{
    public class MailService : IMailService
    {
        IConfiguration config;

        public MailService(IConfiguration _config)
        {
            this.config = _config;
        }


        public void Send(List<Draw> draws)
        {
            var smtpClient = new SmtpClient(config.GetSection("SmtpConfiguration:Host").Value)
            {
                Port = Convert.ToInt32(config.GetSection("SmtpConfiguration:Port").Value),
                Credentials = new NetworkCredential(
                    config.GetSection("SmtpConfiguration:Username").Value,
                    config.GetSection("SmtpConfiguration:Password").Value),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(config.GetSection("SmtpConfiguration:Username").Value),
                Subject = "Euromillions keys",
                Body = GenerateBody(draws),
                IsBodyHtml = true,
            };

            mailMessage.To.Add(config.GetSection("SmtpConfiguration:Username").Value);

            smtpClient.Send(mailMessage);
        }

        private static string GenerateBody(List<Draw> draws)
        {
            string body = "<h2>Your euromillions keys for the next draw:</h2>";

            foreach (Draw draw in draws)
            {
                body += $"{string.Join(" ", draw.Numbers)} ¤ {string.Join(" ", draw.Stars)}";
                body += "<br>";
            }

            body += "<h3>Good luck!</h3>";

            return body;
        }
    }
}
