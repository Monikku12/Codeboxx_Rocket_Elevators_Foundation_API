namespace RocketElevators.Models
{
    public class Column
    {
        public long Id { get; set; }
        public string? Status { get; set; }
        public string? Column_type { get; set; }
        public long number_of_floors_served { get; set; }
        public string? Information { get; set; }
        public string? Notes { get; set; }
        public long Battery_id { get; set; }
    }
}