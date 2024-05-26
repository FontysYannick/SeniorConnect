namespace SeniorConnect.API.Entities
{
    public class ActivityUsers
    {
        public int ActivityUserId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int ActivityId { get; set; }
        public Activity Activity { get; set; }
    }
}
