using System;

//TODO: You need to add these using statements to get mail to work
using System.Net.Mail;
using System.Net;

namespace Team1_FinalProject.Utilities
{
    public static class EmailMessaging
    {
        public static void SendEmail(String toEmailAddress, String emailSubject, String emailBody)
        {
            //Create an email client to send the emails
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                UseDefaultCredentials = false,
                //This is the SENDING email address and password
                Credentials = new NetworkCredential("mis333k.team1@gmail.com", "password123!abC"),
                EnableSsl = true
            };
            //Add anything that you need to the body of the message
            // /n is a new line – this will add some white space after the main body of the message
            String finalMessage = emailBody + "\n\nThanks for visiting our website!";


            //Create an email address object for the sender address
            MailAddress senderEmail = new MailAddress("mis333k.team1@gmail.com", "Main Street Movies");
            MailMessage mm = new MailMessage();
            mm.Subject = "Team 1 - " + emailSubject;
            mm.Sender = senderEmail;
            mm.From = senderEmail;
            mm.To.Add(new MailAddress(toEmailAddress));
            mm.Body = finalMessage;
            client.Send(mm);
        }
    }
}