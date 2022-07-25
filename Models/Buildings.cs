namespace RocketElevators.Models
{
    public class Building
    {
        public long Id { get; set; }
        public string? Building_administrator_full_name { get; set; }
        public string? Building_administrator_email { get; set; }
        public string? Building_administrator_phone { get; set; }
        public string? Building_technical_contact_full_name { get; set; } 
        public string? Building_technical_contact_email { get; set; }
        public string? Building_technical_contact_phone { get; set;}
        public long Customer_id { get; set; }
        public long Address_id { get; set; }
    }
}