using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.IO;
using System.Net;

namespace Login_Server
{
    
    class Mail_Sender
    {
        static SmtpClient Client = new SmtpClient()
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential()
            {
                UserName = "sw.contents.defencegame@gmail.com",
                Password = "swcontents#2"
            }
        };
        public bool send_Mail(string _email, string _nickname)
        {
            MailMessage message = new MailMessage()
            {
                //보내는사람
                From = new MailAddress("sw.contents.defencegame@gmail.com", "softgames"),
                Subject = "E-mail verification",
                Body = "To complete your verification, enter the verification code on Client\n" +
                "Verification code: " + "\n" +
                "If this wasn't you, please reset your password to secure your account.",
            };
            //메일 받는사람
            message.To.Add(new MailAddress(_email, _nickname));

            try
            {
                //메일을 실제로 보내는 부분
                Client.Send(message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                
            }
        }
    }
}
