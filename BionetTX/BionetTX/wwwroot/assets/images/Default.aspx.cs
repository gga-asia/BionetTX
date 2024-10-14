using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        submit1.ServerClick += new System.EventHandler(this.Button_Click);
		 
    }
    void Button_Click(Object sender, EventArgs e)
    {
        string UserName = name.Value;
        string UserTel = Tel.Value;
        string UserEmail = email.Value;
        string UserSubjecte = subject.Value;
        string UserQuestiontype = questiontype.Value+"<br>資訊來源 : "+infosource.Value;
        string UserMessage = message.Value;
		
		if (UserName =="" || UserName =="" || UserEmail =="") {
		Response.Write("<Script language='JavaScript'>alert('請填寫必填欄位');</Script>");
		Response.Write("<Script language='JavaScript'>history.go(-1);</Script>");
		}else{
		
			//寫入SMA主機 DB
			using (SqlConnection openCon = new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["BBCMSConnectionString"].ConnectionString))
			{
				string saveStaff = "INSERT into WEB_GGA2 (UserName, UserTel, UserEmail, UserSubjecte, UserQuestiontype, UserMessage) VALUES (@UserName, @UserTel, @UserEmail, @UserSubjecte, @UserQuestiontype, @UserMessage)";

				using (SqlCommand querySaveStaff = new SqlCommand(saveStaff))
				{
					querySaveStaff.Connection = openCon;
					querySaveStaff.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = UserName;
					querySaveStaff.Parameters.Add("@UserTel", SqlDbType.NVarChar, 50).Value = UserTel;
					querySaveStaff.Parameters.Add("@UserEmail", SqlDbType.NVarChar, 50).Value = UserEmail;
					querySaveStaff.Parameters.Add("@UserSubjecte", SqlDbType.NVarChar, 50).Value = UserSubjecte;
					querySaveStaff.Parameters.Add("@UserQuestiontype", SqlDbType.NVarChar, 50).Value = UserQuestiontype;
					querySaveStaff.Parameters.Add("@UserMessage", SqlDbType.NVarChar, 1000).Value = UserMessage;
					openCon.Open();
					querySaveStaff.ExecuteNonQuery();
					openCon.Close();
					openCon.Dispose();
				}
			}
			String Mail_BodyString = "姓名：" + UserName + "<br />連絡電話：" + UserTel + "<br />連絡Email：" + UserEmail + "<br />公司名稱：" + UserSubjecte + "<br />問題類別：" + UserQuestiontype + "<br />細節說明：" + UserMessage + "<br />";
			SMAServer.WebMail ws = new SMAServer.WebMail();
			//string restring = ws.SendMail("訊聯智藥官網來信", "Service@BionetCorp.com", "kennysung@gga.asia", "","", Mail_BodyString, true);
			string restring = ws.SendMail("訊聯智藥官網來信", "Service@BionetCorp.com", "service@bionettx.com", "","", Mail_BodyString, true);

			Response.Write("<Script language='JavaScript'>alert('訊息發送成功');</Script>");
			Response.Write("<Script language='JavaScript'>location.href='https://www.bionettx.com';</Script>");
		
		}
    }

}