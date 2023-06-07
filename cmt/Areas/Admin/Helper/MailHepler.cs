using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

/// <summary>
/// MailService 的摘要描述
/// </summary>
namespace cmt.Areas.Admin.Helper
{
    public class MailHepler
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //

        public void SendPwd(string Email, string pwd)
        {
            string Subject = "交易與資訊揭露平台";
            
            
            StringBuilder html = new StringBuilder();
            html.Append("<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /><title>"+ Subject + "</title></head>");
            html.Append("<body>");
            html.Append("<table width=\"750\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-family:Arial, Helvetica, '微軟正黑體',  sans-serif; line-height:155%;\">");
            html.Append("<tr><td height=\"75\"></td></tr>");
            html.Append("<tr><td style=\"background:#F7FAF3; border-bottom:#0092D0 1px solid; padding:25px 70px 70px;\">");
            html.Append("<table><tr>");
            html.Append("<td width=\"75\" valign=\"top\"></td>");
            html.Append("<td valign=\"top\"><p><strong style=\"font-size:25px; color:#0093CE\">" + Email + "您好!</strong></p>");
            html.Append("<br>");
            html.Append("<br>");
            html.Append("<p>您的密碼:"+ pwd + "</p>");
            html.Append("<br>");
            html.Append("<br>");
            html.Append("</tr></table>");
            html.Append("</td></tr>");
            html.Append("<tr><td style=\"background:#F6F1EA; padding:10px 35px; padding-left:150px; font-size:14px; color:#B20000;\">此信件由系統發出，因此請勿回覆本電子郵件。</td></tr>");
            html.Append("</table></body></html>");
            senMail(Subject, html.ToString(), Email);
        }

        void senMail(string Subject, string MailBody, string ToEmail)
        {

            
            var MailAccount = ConfigurationManager.AppSettings["MailAccount"];
            var MailPwd = ConfigurationManager.AppSettings["MailPwd"];

            MailMessage msg = new MailMessage();
            //收件者，以逗號分隔不同收件者 ex "test@gmail.com,test2@gmail.com"
            msg.To.Add(ToEmail);
            msg.From = new MailAddress("test2@gmail.com", Subject, System.Text.Encoding.UTF8);
            //郵件標題 
            msg.Subject = Subject;
            //郵件標題編碼  
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            //郵件內容
            msg.Body = MailBody;
            msg.IsBodyHtml = true;
            msg.BodyEncoding = System.Text.Encoding.UTF8;//郵件內容編碼 
            msg.Priority = MailPriority.Normal;//郵件優先級 
                                               //建立 SmtpClient 物件 並設定 Gmail的smtp主機及Port 
            #region 其它 Host
            /*
             *  outlook.com smtp.live.com port:25
             *  yahoo smtp.mail.yahoo.com.tw port:465
            */
            #endregion
            SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);
            //設定你的帳號密碼
            MySmtp.Credentials = new System.Net.NetworkCredential(MailAccount, MailPwd);
            //Gmial 的 smtp 使用 SSL
            MySmtp.EnableSsl = true;
            MySmtp.Send(msg);
        }
    }

  


}