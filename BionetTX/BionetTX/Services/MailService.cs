using Azure;
using BionetTX.Models;
using BionetTX.Services.IServices;
using Dapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Text;
using static BionetTX.Services.MailService;
using static System.Net.Mime.MediaTypeNames;

namespace BionetTX.Services
{
    public class MailService(IDBDapper dBDapper) : IMailService
    {

        public IDBDapper DBDapper { get; set; } = dBDapper;

        public void  SendToTX(string infoStr)
        {
            // 物件化
            var info = JsonConvert.DeserializeObject<contactInfoModel>(infoStr);

            // 發信的主旨
            var subject = "訊聯智藥官網來信";

            // 生成收件人信箱列表 
            // 記得換 你要的收件者!!!
            var emailMap = new Dictionary<string, string>()
            {
                { "CRDMO委託製造" , "MillyKuo@gga.asia" },
                { "原料合作 / 採購" , "MillyKuo@gga.asia" },
                { "商品合作 / 採購" , "MillyKuo@gga.asia" },
                { "資訊詢問" , "MillyKuo@gga.asia" },
                { "其他" , "MillyKuo@gga.asia" },
                { "Business Cooperation" , "MillyKuo@gga.asia" },
                { "Other" , "MillyKuo@gga.asia" },
            };

            var receiveType = new List<string>();

            Console.WriteLine("Selected Type: " + info.typeSelected);

            if (emailMap.TryGetValue(info.typeSelected, out var email))
            {
                receiveType.Add(email);
            }

            Console.WriteLine("Emails to send: " + string.Join(", ", receiveType));

            var to = string.Join(";", receiveType);

            // 生成BCC收件人信箱列表 
            var bcc = "";
            bcc = "MillyKuo@GGA.ASIA";


            // 組合 body 信件內容
            var UserName = info.name;
            var UserTel = info.tel;
            var UserEmail = info.email;
            var UserSubjecte = info.company;
            var UserQuestiontype = info.typeSelected;
            var UserSourceSelected = info.sourceSelected;
            var UserMessage = info.description;

            // 構建 HTML 文本
            StringBuilder bodyBuilder = new StringBuilder();
            bodyBuilder.AppendLine("<html>");
            bodyBuilder.AppendLine("<body>");
            bodyBuilder.AppendLine("<h2>訊聯智藥官網來信</h2>");
            bodyBuilder.AppendLine("<p><b>姓名：</b>" + UserName + "</p>");
            bodyBuilder.AppendLine("<p><b>連絡電話：</b>" + UserTel + "</p>");
            bodyBuilder.AppendLine("<p><b>連絡Email：</b>" + UserEmail + "</p>");
            bodyBuilder.AppendLine("<p><b>公司名稱：</b>" + UserSubjecte + "</p>");
            bodyBuilder.AppendLine("<p><b>問題類別：</b>" + UserQuestiontype + "</p>");
            bodyBuilder.AppendLine("<p><b>資訊來源：</b>" + UserSourceSelected + "</p>");
            bodyBuilder.AppendLine("<p><b>詳細描述：</b>" + UserMessage + "</p>");
            bodyBuilder.AppendLine("</body>");
            bodyBuilder.AppendLine("</html>");
            string body = bodyBuilder.ToString();

            var sql = @"INSERT INTO [dbo].[Mail_Log]
                                             ([Subject]
                                             ,[From]
                                             ,[To]
                                             ,[Cc]
                                             ,[Bcc]
                                             ,[Body]
                                             ,[IsBodyHtml]
                                             ,[SendUtcTime]
                                             ,[SendStatus])
                                        VALUES
                                             (@subject
                                             ,'Service@bionetcorp.com'
                                             ,@to
                                             ,''
                                             ,@bcc
                                             ,@body
                                             ,1
                                             ,GETUTCDATE() 
                                             ,0 ) ";

            try
            {
                // 發信
                var mailParams = new { subject, to, bcc, body };
                // Execute the SQL query to log the email
                dBDapper.Exec(dbname: "API_LOG", sql: sql, mailParams);

               
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new Exception("Error sending the message", ex);
            }

        }

    }
}
