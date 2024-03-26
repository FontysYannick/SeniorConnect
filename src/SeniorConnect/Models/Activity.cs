namespace SeniorConnect.Models
{
    public class Activity
    {
        public Guid ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string? ActivityDescription { get; set; }
        public string? ActivityImg { get; set; }
        public List<int>? ActivityType { get; set; }
    }
}
