using System.ComponentModel.DataAnnotations;

namespace SeniorConnect.API.Models.Activity
{
    public class AbstractActivity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public int OrganizerId { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? Image { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Place { get; set; }
        public int? MaxParticipants { get; set; }
        public string? Awards { get; set; }
    }
}
