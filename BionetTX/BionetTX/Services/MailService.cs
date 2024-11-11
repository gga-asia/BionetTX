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
    public class MailService : IMailService
    {

        private readonly IDBDapper _dBDapper;
        private readonly HttpClient _httpClient;

        public MailService(IDBDapper dBDapper, HttpClient httpClient)
        {
            _dBDapper = dBDapper;
            _httpClient = httpClient;
        }


        // 直接寫進資料庫
        //public void SendToTX(string infoStr)
        //{
        //    // 物件化
        //    var info = JsonConvert.DeserializeObject<contactInfoModel>(infoStr);

        //    // 發信的主旨
        //    var subject = "訊聯智藥官網來信";

        //    // 生成收件人信箱列表 
        //    // 記得換 你要的收件者!!!  MillyKuo@gga.asia;AlusaLu@GGA.ASIA;JennyLee2@BionetCorp.com
        //    var emailMap = new Dictionary<string, string>()
        //    {
        //        { "CRDMO委託製造" , "MillyKuo@gga.asia; Milly.Kuo@sunrise.tw;" },
        //        { "原料合作 / 採購" , "MillyKuo@gga.asia; Milly.Kuo@sunrise.tw;" },
        //        { "商品合作 / 採購" , "MillyKuo@gga.asia; Milly.Kuo@sunrise.tw;" },
        //        { "資訊詢問" , "MillyKuo@gga.asia; Milly.Kuo@sunrise.tw;" },
        //        { "其他" , "MillyKuo@gga.asia; Milly.Kuo@sunrise.tw;" },
        //        { "Business Cooperation" , "MillyKuo@gga.asia; Milly.Kuo@sunrise.tw;" },
        //        { "Other" , "MillyKuo@gga.asia; Milly.Kuo@sunrise.tw;" },
        //    };

        //    var receiveType = new List<string>();

        //    Console.WriteLine("Selected Type: " + info.typeSelected);

        //    if (emailMap.TryGetValue(info.typeSelected, out var email))
        //    {
        //        receiveType.Add(email);
        //    }

        //    Console.WriteLine("Emails to send: " + string.Join(", ", receiveType));

        //    var to = string.Join(";", receiveType);

        //    // 生成BCC收件人信箱列表 
        //    var bcc = "";
        //    bcc = "MillyKuo@GGA.ASIA";


        //    // 組合 body 信件內容
        //    var UserName = info.name;
        //    var UserTel = info.tel;
        //    var UserEmail = info.email;
        //    var UserSubjecte = info.company;
        //    var UserQuestiontype = info.typeSelected;
        //    var UserSourceSelected = info.sourceSelected;
        //    var UserMessage = info.description;

        //    // 構建 HTML 文本
        //    StringBuilder bodyBuilder = new StringBuilder();
        //    bodyBuilder.AppendLine("<html>");
        //    bodyBuilder.AppendLine("<body>");
        //    bodyBuilder.AppendLine("<h2>訊聯智藥官網來信</h2>");
        //    bodyBuilder.AppendLine("<p><b>姓名：</b>" + UserName + "</p>");
        //    bodyBuilder.AppendLine("<p><b>連絡電話：</b>" + UserTel + "</p>");
        //    bodyBuilder.AppendLine("<p><b>連絡Email：</b>" + UserEmail + "</p>");
        //    bodyBuilder.AppendLine("<p><b>公司名稱：</b>" + UserSubjecte + "</p>");
        //    bodyBuilder.AppendLine("<p><b>問題類別：</b>" + UserQuestiontype + "</p>");
        //    bodyBuilder.AppendLine("<p><b>資訊來源：</b>" + UserSourceSelected + "</p>");
        //    bodyBuilder.AppendLine("<p><b>詳細描述：</b>" + UserMessage + "</p>");
        //    bodyBuilder.AppendLine("</body>");
        //    bodyBuilder.AppendLine("</html>");
        //    string body = bodyBuilder.ToString();

        //    var sql = @"INSERT INTO [dbo].[Mail_Log]
        //                                     ([Subject]
        //                                     ,[From]
        //                                     ,[To]
        //                                     ,[Cc]
        //                                     ,[Bcc]
        //                                     ,[Body]
        //                                     ,[IsBodyHtml]
        //                                     ,[SendUtcTime]
        //                                     ,[SendStatus])
        //                                VALUES
        //                                     (@subject
        //                                     ,'Service@bionetcorp.com'
        //                                     ,@to
        //                                     ,''
        //                                     ,@bcc
        //                                     ,@body
        //                                     ,1
        //                                     ,GETUTCDATE() 
        //                                     ,0 ) ";

        //    try
        //    {
        //        // 發信
        //        var mailParams = new { subject, to, bcc, body };
        //        // Execute the SQL query to log the email
        //        dBDapper.Exec(dbname: "API_LOG", sql: sql, mailParams);


        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions
        //        throw new Exception("Error sending the message", ex);
        //    }

        //}


        // API 寫法
        public void SendToTX(string infoStr)
        {
            // 物件化
            var info = JsonConvert.DeserializeObject<contactInfoModel>(infoStr);

            // 發信的主旨
            var subject = "訊聯智藥官網來信";

            // 生成收件人信箱列表 
            // 記得換 你要的收件者!!!  MillyKuo@gga.asia;AlusaLu@GGA.ASIA;JennyLee2@BionetCorp.com
            var emailMap = new Dictionary<string, string>()
            {
                { "CRDMO委託製造" , "AlusaLu@GGA.ASIA;JennyLee2@BionetCorp.com;" },
                { "原料合作 / 採購" , "AlusaLu@GGA.ASIA;JennyLee2@BionetCorp.com;" },
                { "商品合作 / 採購" , "AlusaLu@GGA.ASIA;JennyLee2@BionetCorp.com;" },
                { "資訊詢問" , "AlusaLu@GGA.ASIA;JennyLee2@BionetCorp.com;" },
                { "其他" , "AlusaLu@GGA.ASIA;JennyLee2@BionetCorp.com;" },
                { "Business Cooperation" , "AlusaLu@GGA.ASIA;JennyLee2@BionetCorp.com;" },
                { "Other" , "AlusaLu@GGA.ASIA;JennyLee2@BionetCorp.com;" },
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

            // 構建 HTML 文本
            var body = $@"
            <html><body>
            <h2>訊聯智藥官網來信</h2>
            <p><b>姓名：</b>{info.name}</p>
            <p><b>連絡電話：</b>{info.tel}</p>
            <p><b>連絡Email：</b>{info.email}</p>
            <p><b>公司名稱：</b>{info.company}</p>
            <p><b>問題類別：</b>{info.typeSelected}</p>
            <p><b>資訊來源：</b>{info.sourceSelected}</p>
            <p><b>詳細描述：</b>{info.description}</p>
            </body></html>";

            // API 請求內容
            var mailContent = new
            {
                Subject = subject,
                From = "Service@bionetcorp.com",
                To = emailMap[info.typeSelected].Split(';').Select(e => e.Trim()).ToList(),
                Cc = new List<string> { "" },
                Bcc = new List<string> { "MillyKuo@GGA.ASIA" },
                Body = body,
                IsBodyHtml = true,
                SendStatus = 0,
            };


            var content = new StringContent(JsonConvert.SerializeObject(mailContent), Encoding.UTF8, "application/json"); 



            var sql = "";

            try
            {
                // 發送郵件

                var response = _httpClient.PostAsync("/Api/Mail/Send", content).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();

                // 發信
                var mailParams = new { subject, to, bcc, body };
                // Execute the SQL query to log the email
                //_dBDapper.Exec(dbname: "API_LOG", sql: sql, mailParams);

                // 如果需要，可以處理 API 的響應
                var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                Console.WriteLine($"Response from API: {responseContent}");


            }
            catch (Exception ex)
            {
                // 處理 HTTP 請求錯誤
                Console.WriteLine($"Request error: {ex.Message}");
                // Handle exceptions
                throw new Exception("Error sending the message", ex);


            }

        }

    }
}
