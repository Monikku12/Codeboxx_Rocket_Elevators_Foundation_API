namespace RocketElevators.Models
{
    public class Elevator
    {
        public long Id { get; set; }
        public string? Status { get; set; }
        public string? Serial_number { get; set; }
        public string? Model { get; set; }
        public string? Elevator_type { get; set; }
        public DateTime Commissioning_date { get; set; }
        public DateTime Last_inspection_date { get; set; }
        public string? Inspection_certificate { get; set; }
        public string? Information { get; set; }
        public string? Notes { get; set; }
        public long Column_id { get; set; }
    }
}