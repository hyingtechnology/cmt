using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net.Mail;
using System.IO;
using System.Configuration;

namespace cmt.Extensions.Mail
{
    /// <summary>
    /// SMTPMail 的摘要描述
    /// </summary>
    public class SMTPMail
    {
        #region 發信Info
        /// <summary>
        /// SMTP
        /// </summary>
        public string getSMTP
        {
            get
            {
                if (ConfigurationManager.AppSettings["MailServer"] == null)
                    throw new System.ArgumentNullException();
                else
                    return ConfigurationManager.AppSettings["MailServer"].ToString();
            }
        }

        /// <summary>
        /// 發信信箱
        /// </summary>
        public string getFromMail
        {
            get
            {
                if (ConfigurationManager.AppSettings["FromMail"] == null)
                    throw new System.ArgumentNullException();
                else
                    return ConfigurationManager.AppSettings["FromMail"].ToString();
            }
        }

        /// <summary>
        /// 發信名稱
        /// </summary>
        public string getFromName
        {
            get
            {
                if (ConfigurationManager.AppSettings["FromName"] == null)
                    throw new System.ArgumentNullException();
                else
                    return ConfigurationManager.AppSettings["FromName"].ToString();
            }
        }
        #endregion

        public SMTPMail()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        string Status = ConfigurationManager.AppSettings["SendMailStatus"].ToString();

        private string Host { set; get; }
        private MailAddress From { set; get; }
        private MailAddressCollection To { set; get; }
        private MailAddressCollection Cc { set; get; }
        private MailAddressCollection Bcc { set; get; }
        private string Subject { set; get; }
        private string Body { set; get; }
        private bool IsBodyHtml { set; get; }
        private Attachment AttachFile { set; get; }

        private void Send()
        {
            SmtpClient sc = new SmtpClient();
            sc.Host = this.Host;
            //sc.Credentials = new System.Net.NetworkCredential("account", "password");

            MailMessage mmessage = new MailMessage();
            mmessage.From = this.From;

            if (this.To.Count > 0)
            {
                foreach (MailAddress addr in this.To)
                    mmessage.To.Add(addr);
            }

            if (this.Cc != null && this.Cc.Count > 0)
            {
                foreach (MailAddress addr in this.Cc)
                    mmessage.CC.Add(addr);
            }

            if (this.Bcc != null && this.Bcc.Count > 0)
            {
                foreach (MailAddress addr in this.Bcc)
                    mmessage.Bcc.Add(addr);
            }

            mmessage.Subject = this.Subject;
            mmessage.Body = this.Body;
            mmessage.IsBodyHtml = this.IsBodyHtml;

            if (this.AttachFile != null)
                mmessage.Attachments.Add(this.AttachFile);

            sc.Send(mmessage);

            mmessage.Attachments.Dispose();
        }

        public void SendMail(string Host, string From, string displayName,
            string[] To, string[] Cc, string[] Bcc,
            string Subject, string Body, Attachment AttachFile, bool IsBodyHtml)
        {
            try
            {
                if (Status == "Y")
                {
                    this.Host = Host;
                    this.From = new MailAddress(From, displayName);
                    if (From.Length > 0)
                    {
                        MailAddressCollection to;

                        if (To != null)
                        {
                            to = new MailAddressCollection();
                            foreach (string mto in To)
                            {
                                if (mto.Length > 0)
                                    to.Add(new MailAddress(mto));
                            }
                            this.To = to;
                        }
 

                        if (Cc != null)
                        {
                            to = new MailAddressCollection();
                            foreach (string mto in Cc)
                            {
                                if (mto.Length > 0)
                                    to.Add(new MailAddress(mto));
                            }
                            this.Cc = to;
                        }

                        if (Bcc != null)
                        {
                            to = new MailAddressCollection();
                            foreach (string mto in Bcc)
                            {
                                if (mto.Length > 0)
                                    to.Add(new MailAddress(mto));
                            }
                            this.Bcc = to;
                        }
                    }
                    this.Subject = Subject;
                    this.Body = Body;
                    this.IsBodyHtml = IsBodyHtml;
                    this.AttachFile = AttachFile;

                    this.Send();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}