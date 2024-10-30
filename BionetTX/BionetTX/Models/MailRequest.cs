namespace BionetTX.Models
{
    public class MailRequest
    {
        public string Subject { get; set; }
        public string From { get; set; }
        public List<string> To { get; set; } = new List<string>();
        public List<string> Cc { get; set; } = new List<string>();
        public List<string> Bcc { get; set; } = new List<string>();
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
        public int SendStatus { get; set; }

    }
}
