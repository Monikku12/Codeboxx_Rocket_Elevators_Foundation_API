namespace RocketElevators.Models
{
    public class Battery
    {
        public long Id { get; set; }
        public string? Status { get; set; }
        public string? Batterie_type { get; set; }
        public DateTime Commissioning_date { get; set; }
        public DateTime Last_inspection_date { get; set; }
        public string? Certificate_of_operation { get; set; }
        public string? Informations { get; set; }
        public string? Notes { get; set; }
        public long Building_id { get; set; }
        public long Employee_id { get; set; }
    }
}