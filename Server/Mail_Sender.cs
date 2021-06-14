
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.IO;
using System.Net;
using System.ComponentModel;

namespace Login_Server
{
    class Mail_Sender
    {
        private Random random = new Random();
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
        public int send_Mail(string _email, string _nickname)
        {
            //111,111 ~ 999,999사이의 숫자를 return
            int vertinum = random.Next(111111, 999999);
            MailMessage message = new MailMessage()
            {
                //보내는사람
                From = new MailAddress("sw.contents.defencegame@gmail.com", "softgames"),
                Subject = "E-mail verification",
                Body = "To complete your verification, enter the verification code on Client\n" +
                "Verification code: " + vertinum + "\n" +
                "If this wasn't you, please reset your password to secure your account.",
            };

            //메일 받는사람
            message.To.Add(new MailAddress(_email, _nickname));
            try
            {
                Client.SendCompleted += Client_sendCompleted;
                Client.SendMailAsync(message);
                //메일을 실제로 보내는 부분
                return vertinum;
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {

            }
        }
        private void Client_sendCompleted(Object sender, AsyncCompletedEventArgs e)
        {
            //아무것도 하지 않음
        }
    }
}



