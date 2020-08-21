using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using TicketManagement.Common.Algorithm;
using TicketManagement.Concrete;
using TicketManagement.Interface;
using TicketManagement.ViewModels;
using System.Web;
using Elmah;

namespace TicketManagement.Common.Emails
{
    public class SendingEmailhelper
    {
        public string TestEmail(string toList)
        {
            try
            {
                string subject = "###### Test Email ######";
                string from = ConfigurationManager.AppSettings["Smtp_from"].ToString();
                IProcessSettings processSettings = new ProcessSettingsConcrete();
                var settingobject = processSettings.GetDefaultEmailSettings();
                if (settingobject != null)
                {
                    MailMessage message = new MailMessage();
                    SmtpClient smtpClient = new SmtpClient();
                    string msg;
                    try
                    {
                        MailAddress fromAddress = new MailAddress(from);
                        message.From = fromAddress;
                        message.To.Add(toList);
                        message.Subject = subject;
                        message.IsBodyHtml = true;
                        message.Body = "###### Test Email for Ticket ######";
                        // We use gmail as our smtp client
                        smtpClient.Host = settingobject.Host;
                        smtpClient.Port = Convert.ToInt32(settingobject.Port);
                        smtpClient.EnableSsl = settingobject.SslProtocol == "Y" ? true : false;
                        smtpClient.UseDefaultCredentials = true;
                        smtpClient.Credentials = new System.Net.NetworkCredential(settingobject.Username, settingobject.Password);
                        smtpClient.Send(message);
                        msg = "Successful";
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                    }
                    return msg;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return string.Empty;
            }
        }

        public string Send(string trackingId)
        {
            try
            {
                if (trackingId != null)
                {
                    var generalsetting = GetGeneralSetting();
                    if (generalsetting != null)
                    {
                        if (generalsetting.EnableEmailFeature)
                        {
                            IProcessSettings processSettings = new ProcessSettingsConcrete();
                            var settingobject = processSettings.GetDefaultEmailSettings();

                            string from = generalsetting.Email;

                            if (settingobject != null)
                            {
                                MailMessage message = new MailMessage();
                                SmtpClient smtpClient = new SmtpClient();
                                string msg;
                                try
                                {
                                    MailAddress fromAddress = new MailAddress(from);
                                    var data = PreparingEmailtoSend(trackingId);
                                    if (data != null)
                                    {
                                        message.From = fromAddress;
                                        message.To.Add(data.UserEmailId);
                                        message.Subject = data.Subject;
                                        message.IsBodyHtml = true;
                                        message.Body = data.EmailBody;
                                        // We use gmail as our smtp client
                                        smtpClient.Host = settingobject.Host;
                                        smtpClient.Port = Convert.ToInt32(settingobject.Port);
                                        smtpClient.EnableSsl = settingobject.SslProtocol == "Y" ? true : false;
                                        smtpClient.UseDefaultCredentials = true;
                                        smtpClient.Credentials =
                                            new System.Net.NetworkCredential(settingobject.Username, settingobject.Password);
                                        smtpClient.Send(message);
                                        msg = "Successful";
                                        return msg;
                                    }
                                    else
                                    {
                                        return "SMTP Setting is Not Configured ";
                                    }

                                }
                                catch (Exception ex)
                                {
                                    msg = ex.Message;
                                }
                            }
                            else
                            {
                                return "SMTP Setting is Not Configured ";
                            }
                        }
                        else
                        {
                            return "SMTP Setting Feature is Disabled";
                        }
                    }

                    return "General Setting is Not Configured ";
                }
                else
                {
                    return "Please try Again";
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return string.Empty;
            }
        }

        public async Task SendEmailasync(string trackingId)
        {
            try
            {
                if (trackingId != null)
                {
                    var generalsetting = GetGeneralSetting();
                    if (generalsetting != null)
                    {
                        if (generalsetting.EnableEmailFeature)
                        {
                            IProcessSettings processSettings = new ProcessSettingsConcrete();
                            var settingobject = processSettings.GetDefaultEmailSettings();

                            string from = generalsetting.Email;

                            if (settingobject != null)
                            {
                                MailMessage message = new MailMessage();
                                SmtpClient smtpClient = new SmtpClient();
                                try
                                {
                                    MailAddress fromAddress = new MailAddress(from);
                                    message.From = fromAddress;
                                    message.To.Add(PreparingEmailtoSend(trackingId).UserEmailId);
                                    message.Subject = PreparingEmailtoSend(trackingId).Subject;
                                    message.IsBodyHtml = true;
                                    message.Body = PreparingEmailtoSend(trackingId).EmailBody;
                                    // We use mail as our smtp client
                                    smtpClient.Host = settingobject.Host;
                                    smtpClient.Port = Convert.ToInt32(settingobject.Port);
                                    smtpClient.EnableSsl = settingobject.SslProtocol == "Y" ? true : false;
                                    smtpClient.UseDefaultCredentials = true;
                                    smtpClient.Credentials =
                                        new System.Net.NetworkCredential(settingobject.Username, settingobject.Password);
                                    await smtpClient.SendMailAsync(message);
                                }
                                catch (Exception ex)
                                {
                                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private Emailstruct PreparingEmailtoSend(string trackingId)
        {
            try
            {
                var stringtemplate = new StringBuilder();
                var mainticket = TicketsDetailsbyticketId(trackingId);

                if (mainticket != null)
                {
                    stringtemplate.Append("<table>");
                    stringtemplate.Append("<tr>");
                    stringtemplate.Append("<td style='width:20%'><b>Name</b> :</td>");
                    stringtemplate.Append("<td>" + mainticket.Name + "</td>");
                    stringtemplate.Append("</tr>");

                    stringtemplate.Append("<tr>");
                    stringtemplate.Append("<td> <b>CategoryName</b> :</td>");
                    stringtemplate.Append("<td>" + mainticket.CategoryName + "</td>");
                    stringtemplate.Append("</tr>");

                    stringtemplate.Append("<tr>");
                    stringtemplate.Append("<td> <b>PriorityName</b> :</td>");
                    stringtemplate.Append("<td>" + mainticket.PriorityName + "</td>");
                    stringtemplate.Append("</tr>");

                    stringtemplate.Append("<tr>");
                    stringtemplate.Append("<td> <b>Subject</b> :</td>");
                    stringtemplate.Append("<td>" + mainticket.Subject + "</td>");
                    stringtemplate.Append("</tr>");
                    stringtemplate.Append("<tr>");

                    stringtemplate.Append("<td> <b>Message</b> :</td>");
                    stringtemplate.Append("<td>" + mainticket.Message + "</td>");
                    stringtemplate.Append("</tr>");
                    stringtemplate.Append("</table>");
                    stringtemplate.Append("<hr/>");
                }

                var ticketreplyList = ListofHistoryTicketReplies(trackingId);
                if (ticketreplyList != null)
                {
                    foreach (var reply in ticketreplyList)
                    {
                        stringtemplate.Append("<table>");
                        stringtemplate.Append("<tr>");
                        stringtemplate.Append("<td style='width:10%'><b>Replied_Date</b>  :</td>");
                        stringtemplate.Append("<td>" + reply.CreatedDateDisplay + "</td>");
                        stringtemplate.Append("</tr>");

                        stringtemplate.Append("<tr>");
                        stringtemplate.Append("<td> <b>Replied User</b>  :</td>");
                        stringtemplate.Append("<td>" + reply.RepliedUserName + "</td>");
                        stringtemplate.Append("</tr>");

                        stringtemplate.Append("<tr>");
                        stringtemplate.Append("<td> <b>Email</b>  :</td>");
                        stringtemplate.Append("<td>" + reply.Email + "</td>");
                        stringtemplate.Append("</tr>");

                        stringtemplate.Append("<tr>");
                        stringtemplate.Append("<td> <b>Message</b>  :</td>");
                        stringtemplate.Append("<td>" + reply.Message + "</td>");
                        stringtemplate.Append("</tr>");
                        stringtemplate.Append("</table>");
                        stringtemplate.Append("<hr/>");
                    }
                }

                Emailstruct emailstruct = new Emailstruct { EmailBody = stringtemplate.ToString() };
                if (mainticket != null) emailstruct.Subject = mainticket.Subject;
                if (mainticket != null) emailstruct.UserEmailId = mainticket.Email;

                return emailstruct;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return null;
            }
        }

        private DisplayTicketEmailViewModel TicketsDetailsbyticketId(string trackingId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    var param = new DynamicParameters();
                    param.Add("@TrackingId", trackingId);
                    return con.Query<DisplayTicketEmailViewModel>("Usp_Tickets_TicketsDetailsbyticketId", param, null, false, 0, CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return null;
            }
        }

        private List<ViewTicketReplyHistoryModel> ListofHistoryTicketReplies(string trackingId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@TrackingId", trackingId);
                    return con.Query<ViewTicketReplyHistoryModel>("Usp_Tickets_HistoryTicketRepliesbytrackingId", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return null;
            }
        }

        private class Emailstruct
        {
            public string Subject;
            public string EmailBody;
            public string UserEmailId;
        }

        private GeneralSettingsViewModel GetGeneralSetting()
        {
            try
            {
                using (var context = new DatabaseContext())
                {
                    var getsetting = (from general in context.GeneralSettings
                                      select new GeneralSettingsViewModel()
                                      {
                                          Name = general.Name,
                                          Email = general.Email,
                                          GeneralSettingsId = general.GeneralSettingsId,
                                          SupportEmailId = general.SupportEmailId,
                                          WebsiteTitle = general.WebsiteTitle,
                                          WebsiteUrl = general.WebsiteUrl,
                                          EnableEmailFeature = general.EnableEmailFeature,
                                          EnableSignatureFeature = general.EnableSignatureFeature,
                                          EnableSmsFeature = general.EnableSmsFeature,
                                      }).FirstOrDefault();

                    return getsetting;
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return null;
            }
        }

        public async Task SendVerificationEmailasync(string emailid, string name, string token, string sendingType, string userid)
        {
            try
            {
                AesAlgorithm aesAlgorithm = new AesAlgorithm();
                var key = string.Join(":", new string[] { DateTime.Now.Ticks.ToString(), userid });
                var encrypt = aesAlgorithm.EncryptToBase64String(key);

                var linktoverify = ConfigurationManager.AppSettings["VerifyRegistrationURL"] + "?key=" + HttpUtility.UrlEncode(encrypt) + "&hashtoken=" + HttpUtility.UrlEncode(token);

                if (emailid != null)
                {
                    var generalsetting = GetGeneralSetting();
                    if (generalsetting != null)
                    {
                        if (generalsetting.EnableEmailFeature)
                        {
                            IProcessSettings processSettings = new ProcessSettingsConcrete();
                            var settingobject = processSettings.GetDefaultEmailSettings();

                            string from = generalsetting.Email;

                            if (settingobject != null)
                            {
                                MailMessage message = new MailMessage();
                                SmtpClient smtpClient = new SmtpClient();
                                try
                                {
                                    MailAddress fromAddress = new MailAddress(from);
                                    message.From = fromAddress;
                                    message.To.Add(emailid);
                                    message.Subject = "Welcome to Ticket's";
                                    message.IsBodyHtml = true;
                                    message.Body = SendVerificationEmail(name, linktoverify);
                                    // We use mail as our smtp client
                                    smtpClient.Host = settingobject.Host;
                                    smtpClient.Port = Convert.ToInt32(settingobject.Port);
                                    smtpClient.EnableSsl = settingobject.SslProtocol == "Y" ? true : false;
                                    smtpClient.UseDefaultCredentials = true;
                                    smtpClient.Credentials =
                                        new System.Net.NetworkCredential(settingobject.Username, settingobject.Password);
                                    await smtpClient.SendMailAsync(message);
                                }
                                catch (Exception ex)
                                {
                                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        public string SendVerificationEmail(string name, string link)
        {
            var stringtemplate = new StringBuilder();
            stringtemplate.Append("Welcome");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("Dear " + name);
            stringtemplate.Append("<br/>");
            stringtemplate.Append("Thanks for joining Ticket's.");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("To activate your Ticket's account, please confirm your email address.");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("<a target='_blank' href=" + link + ">Confirm Email</a>");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("Yours sincerely,");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("Ticket's");
            stringtemplate.Append("<br/>");
            return stringtemplate.ToString();
        }

        public async Task SendForgotPasswordVerificationEmailasync(string emailid, string token, string sendingType, string userId)
        {
            try
            {
                AesAlgorithm aesAlgorithm = new AesAlgorithm();
                var key = string.Join(":", new string[] { DateTime.Now.Ticks.ToString(), userId });
                var encrypt = aesAlgorithm.EncryptToBase64String(key);
                var link = ConfigurationManager.AppSettings["VerifyResetPasswordURL"] + "?key=" + HttpUtility.UrlEncode(encrypt) + "&hashtoken=" + HttpUtility.UrlEncode(token);

                if (emailid != null)
                {
                    var generalsetting = GetGeneralSetting();
                    if (generalsetting != null)
                    {
                        if (generalsetting.EnableEmailFeature)
                        {
                            IProcessSettings processSettings = new ProcessSettingsConcrete();
                            var settingobject = processSettings.GetDefaultEmailSettings();

                            string from = generalsetting.Email;

                            if (settingobject != null)
                            {
                                MailMessage message = new MailMessage();
                                SmtpClient smtpClient = new SmtpClient();
                                try
                                {
                                    MailAddress fromAddress = new MailAddress(from);
                                    message.From = fromAddress;
                                    message.To.Add(emailid);
                                    message.Subject = "Password Reset";
                                    message.IsBodyHtml = true;
                                    message.Body = SendForgotVerificationEmail(link);
                                    // We use mail as our smtp client
                                    smtpClient.Host = settingobject.Host;
                                    smtpClient.Port = Convert.ToInt32(settingobject.Port);
                                    smtpClient.EnableSsl = settingobject.SslProtocol == "Y" ? true : false;
                                    smtpClient.UseDefaultCredentials = true;
                                    smtpClient.Credentials =
                                        new System.Net.NetworkCredential(settingobject.Username, settingobject.Password);
                                    await smtpClient.SendMailAsync(message);
                                }
                                catch (Exception ex)
                                {
                                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string SendForgotVerificationEmail(string link)
        {
            var stringtemplate = new StringBuilder();
            stringtemplate.Append("Hello");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("You have just initiated a request to reset the password in Ticket's account.");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("To set a new password,please click the button below:");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("<a target='_blank' href=" + link + ">Reset Password</a>");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("Yours sincerely,");
            stringtemplate.Append("<br/>");
            stringtemplate.Append("Ticket's");
            stringtemplate.Append("<br/>");
            return stringtemplate.ToString();
        }

     
    }
}
