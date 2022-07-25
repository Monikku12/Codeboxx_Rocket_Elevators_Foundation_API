namespace RocketElevators.Models
{
    public class Lead
    {
        public long Id { get; set; }
        public DateTime? Lead_created_at { get; set; }
        public string? Full_name { get; set; }
        public string? Company_name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Project_name { get; set; }
        public string? Project_description { get; set; }
        public string? Department { get; set; }
        public string? Message { get; set; }
    }
}