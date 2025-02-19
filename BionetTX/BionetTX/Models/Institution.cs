namespace BionetTX.Models
{
    public class Institution
    {
        public int InstitutionId { get; set; }
        public string InstitutionName { get; set; }
        public string Address { get; set;}
        public string GoogleMap {  get; set; }
        public string Phone {  get; set; }
        public string Website {  get; set; }
        public string CountryCode {  get; set; }
        public int RegionId{  get; set; }
        public string CityCode { get; set;} 


    }
}
