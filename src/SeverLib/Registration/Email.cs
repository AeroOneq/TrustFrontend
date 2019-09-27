using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace ServerLib
{
    public static class Email
    {
        private const int codeLength = 6;
        #region Properties
        private static Random Random { get; } = new Random();
        public static string CurrentCode { get; private set; }
        #endregion
        public static void SendEmailWIthCode(UserInfo user)
        {
            MailAddress fromAdress = new MailAddress("aeroone90@gmail.com");
            MailAddress toAdress = new MailAddress(user.Email);

            MailMessage message = new MailMessage(fromAdress, toAdress)
            {
                Subject = "Activation code for TRUST",
                Body = $"Your code is <h2>{CreateNewCode()}</h2>",
                IsBodyHtml = true
            };

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("aeroone90@gmail.com", "AeroOne1"),
                EnableSsl = true
            };

            smtpClient.Send(message);
        }

        private static string CreateNewCode()
        {
            string code = string.Empty;
            for (int i = 0; i<codeLength; i++)
                code += (char)Random.Next('A', 'Z' + 1);
            CurrentCode = code;
            return code;
        }
    }
}
