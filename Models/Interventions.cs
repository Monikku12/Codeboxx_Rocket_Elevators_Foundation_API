namespace RocketElevators.Models
{
    public class Intervention
    {
        public long Id { get; set; }
        public long Author { get; set; }
        public long Customer_id { get; set; }
        public long Building_id { get; set; }
        public long Battery_id { get; set; }
        public long Column_id { get; set; }
        public long Elevator_id { get; set; }
        public long Employee_id { get; set; }
        public DateTime Intervention_started_at { get; set; }
        public DateTime Intervention_ended_at { get; set; }
        public string? Result { get; set; }
        public string? Report { get; set; }
        public string? Status { get; set; }
    }
}