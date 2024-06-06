using SeniorConnect.Models.User;

namespace SeniorConnect.Models.Activities
{
    public class ActivityDto
    {
        public int ActivityId { get; set; }
        public string Title { get; set; }
        public int OrganizerId { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public DateTime Date { get; set; }
        public string Place { get; set; }
        public int? MaxParticipants { get; set; }
        public string? Awards { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public UserDto Organizer { get; set; }
    }
}
