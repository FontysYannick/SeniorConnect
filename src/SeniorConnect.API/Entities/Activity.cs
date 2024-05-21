namespace SeniorConnect.API.Entities
{
    public class Activity
    {
        public long ActivityId { get; set; }
        public string Title { get; set; }
        public BaseListItem Catagory { get; set; }
        public User Organizer { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Place { get; set; }
        public int? MaxParticipants { get; set; }
        public decimal? Price { get; set; }

        public Activity() { }
    }
}
