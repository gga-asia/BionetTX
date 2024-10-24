using System.ComponentModel.DataAnnotations.Schema;

namespace BionetTX.Models
{
    public class contactInfoModel
    {
        [Column("UserName")] // 資料庫中的欄位名稱
        public string name { get; set; } = string.Empty;

        [Column("UserTele")] // 資料庫中的欄位名稱
        public string tel { get; set; } = string.Empty;

        [Column("UserEmail")] // 資料庫中的欄位名稱
        public string email { get; set; } = string.Empty;

        [Column("UserSubjecte")] // 資料庫中的欄位名稱
        public string company { get; set; } = string.Empty;

        [NotMapped] // 這個屬性不直接映射到資料庫
        public string typeSelected { get; set; } = string.Empty;

        [NotMapped] // 這個屬性不直接映射到資料庫
        public string sourceSelected { get; set; } = string.Empty;

        [Column("UserQuestiontype")]
        public string combinedQuestionType
        {
            get { return typeSelected + " | 資訊來源: " + sourceSelected; }
        }

        [Column("UserMessage")] // 資料庫中的欄位名稱
        public string description { get; set; } = string.Empty;

        [Column("CreDate")] // 資料庫中的欄位名稱
        public DateTime CreDate { get; set; }

        [Column("ID")] // 資料庫中的欄位名稱
        public int ID { get; set; }
    }
}
