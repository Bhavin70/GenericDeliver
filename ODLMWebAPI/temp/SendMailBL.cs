using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.Models;
using System.IO;
using Microsoft.AspNetCore;
using System.Net;
using MailKit.Net.Smtp;
using MimeKit;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
namespace ODLMWebAPI.DAL
{
    #region sending the mail through the gmail account by vinod Thorat Dated:2/10/2017

    public class SendMailBL : ISendMailBL
    {
        public ResultMessage SendEmail(SendMail tblsendTO, String fileName)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                #region Set configration of mail  

                TblEmailConfigrationTO DimEmailConfigrationTO = BL.TblEmailConfigrationBL.SelectDimEmailConfigrationTO();
                if (DimEmailConfigrationTO != null)
                {

                    tblsendTO.From = DimEmailConfigrationTO.EmailId;
                    tblsendTO.UserName = DimEmailConfigrationTO.EmailId;
                    tblsendTO.Password = DimEmailConfigrationTO.Password;
                    tblsendTO.Port = DimEmailConfigrationTO.PortNumber;
                }
                else
                {
                    resultMessage.DefaultBehaviour("DimEmailConfigrationTO Found Null");
                    resultMessage.DisplayMessage = "Error While Sending Email";
                    resultMessage.MessageType = ResultMessageE.Error;
                    return resultMessage;

                }
                #endregion
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(tblsendTO.FromTitle, tblsendTO.From));
                mimeMessage.To.Add(new MailboxAddress(tblsendTO.ToTitle, tblsendTO.To));
                //mimeMessage.Subject = "Regards New Visit Details ";
                mimeMessage.Subject = tblsendTO.Subject;
                var bodybuilder = new BodyBuilder();
                bodybuilder.HtmlBody = "<h4>Dear Sir, </h4><p>I am sharing  Visit information with  you in regard to a new Visit Details that has been captured during  visit.   You may find the pdf file attached.</p><h4>Kind Regards,";
                mimeMessage.Body = bodybuilder.ToMessageBody();
                byte[] bytes = System.Convert.FromBase64String(tblsendTO.Message.Replace("data:application/pdf;base64,", String.Empty));
                bodybuilder.Attachments.Add(fileName, bytes, ContentType.Parse("application/pdf"));
                mimeMessage.Body = bodybuilder.ToMessageBody();
                using (SmtpClient client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", tblsendTO.Port, false);
                    client.Authenticate(tblsendTO.UserName, tblsendTO.Password);
                    client.Send(mimeMessage);
                    client.Disconnect(true);
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage = "Email Sent Succesfully";
                    resultMessage.MessageType = ResultMessageE.Information;
                    return resultMessage;

                }

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SendEmail");
                resultMessage.DisplayMessage = "Error While Sending Email";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;

            }
            finally
            {

            }
        }
      
        public int SendEmail(SendMail tblsendTO)
          {
             try
             {
                #region Set configration of mail  

                TblEmailConfigrationTO DimEmailConfigrationTO = BL.TblEmailConfigrationBL.SelectDimEmailConfigrationTO();
                if (DimEmailConfigrationTO != null)
                {
                
                        tblsendTO.From = DimEmailConfigrationTO.EmailId;
                        tblsendTO.UserName = DimEmailConfigrationTO.EmailId;
                        tblsendTO.Password = DimEmailConfigrationTO.Password;
                        tblsendTO.Port = DimEmailConfigrationTO.PortNumber;
                }
                else
                {
                    return -1;
                }
                #endregion
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(tblsendTO.FromTitle, tblsendTO.From));
                mimeMessage.To.Add(new MailboxAddress(tblsendTO.ToTitle, tblsendTO.To));
                mimeMessage.Subject = tblsendTO.Subject;
                var bodybuilder = new BodyBuilder();
                bodybuilder.HtmlBody = "<h4>Dear Client, </h4><p>We are contacting you in regard to a new invoice that has been created on your account. You may find the invoice attached.</p><h4>Kind Regards,</h4>";
                mimeMessage.Body = bodybuilder.ToMessageBody();
                byte[] bytes = System.Convert.FromBase64String(tblsendTO.Message.Replace("data:application/pdf;base64,", String.Empty));
                bodybuilder.Attachments.Add("Invoice.pdf", bytes, ContentType.Parse("application/pdf"));
                mimeMessage.Body = bodybuilder.ToMessageBody();
                using (SmtpClient client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", tblsendTO.Port, false);                   
                    client.Authenticate(tblsendTO.UserName, tblsendTO.Password);
                    client.Send(mimeMessage);                                     
                    client.Disconnect(true);
                    return 1;
                }
               
            }        
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {

            }
        }
        //added by aniket
        public int SendEmailNotification(SendMail tblsendTO)
        {
            try
            {
                #region Set configration of mail  

                TblEmailConfigrationTO DimEmailConfigrationTO = BL.TblEmailConfigrationBL.SelectDimEmailConfigrationTO();
                if (DimEmailConfigrationTO != null)
                {

                    tblsendTO.From = DimEmailConfigrationTO.EmailId;
                    tblsendTO.UserName = DimEmailConfigrationTO.EmailId;
                    tblsendTO.Password = DimEmailConfigrationTO.Password;
                    tblsendTO.Port = DimEmailConfigrationTO.PortNumber;
                }
                else
                {
                    return -1;
                }
                #endregion
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(tblsendTO.FromTitle, tblsendTO.From));
                mimeMessage.To.Add(new MailboxAddress(tblsendTO.ToTitle, tblsendTO.To));
                mimeMessage.Subject = tblsendTO.Subject;
                var bodybuilder = new BodyBuilder();
                bodybuilder.HtmlBody = tblsendTO.BodyContent;
                //commented by aniket 
                //bodybuilder.HtmlBody = "<h4>Dear Client, </h4><p>We are contacting you in regard to a new invoice that has been created on your account. You may find the invoice attached.</p><h4>Kind Regards,</h4>";
                mimeMessage.Body = bodybuilder.ToMessageBody();
               // byte[] bytes = System.Convert.FromBase64String(tblsendTO.Message.Replace("data:application/pdf;base64,", String.Empty));
               // bodybuilder.Attachments.Add("Invoice.pdf", bytes, ContentType.Parse("application/pdf"));
                mimeMessage.Body = bodybuilder.ToMessageBody();
                using (SmtpClient client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", tblsendTO.Port, false);
                    client.Authenticate(tblsendTO.UserName, tblsendTO.Password);
                   // client.SendAsync(mimeMessage); // added by aniket
                  client.Send(mimeMessage);
                  
                    client.Disconnect(true);
                    return 1;
                }

            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {

            }
        }
    }
    #endregion
}
