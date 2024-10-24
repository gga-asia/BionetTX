namespace BionetTX.Models
{
    public class MailHelper
    {
        public string Subject { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public string Cc { get; set; } = string.Empty;
        public string Bcc { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public int IsBodyHtml { get; set; } = 1;
        public DateTime SendUtcTime { get; set; } = DateTime.UtcNow;
        public int SendStatus { get; set; } = 0;
    }
}
