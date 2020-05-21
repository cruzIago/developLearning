using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;

/*
 * This is used to send logs via email
 */
public class mono_gmail
{
    //https://stackoverflow.com/questions/44739094/send-log-file-with-email-unity
    public void SendLog(string path)
    {
        string copy = "logFile_" + string.Format("{0:dd-MMM-yyyy}", DateTime.Now) + ".txt";
        File.Copy(path, copy, true);
        SendMail(copy);
    }

    void SendMail(string logPath)
    {
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress("developlearningtfg@gmail.com");
        mail.To.Add("iagocruzgarcia@gmail.com");
        mail.Subject = "LogFile from DevelopLearning App";
        mail.Body = "Logfile";
        mail.Attachments.Add(new Attachment(logPath));
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential("developlearningtfg@gmail.com", "findegrado") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        smtpServer.Send(mail);
        Debug.Log("success");
        //File.Delete(logPath);

    }
}