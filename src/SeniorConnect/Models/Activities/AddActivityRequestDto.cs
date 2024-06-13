using System.ComponentModel.DataAnnotations;

namespace SeniorConnect.Models.Activities
{
    public class AddActivityRequestDto
    {
        [Required(ErrorMessage = "De titel is verplicht")]
        public string Title { get; set; }

        [Required]
        public int OrganizerId { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        [Required(ErrorMessage = "De Datum & tijd is verplicht")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Geef een geldig adres aan")]
        public string Place { get; set; }

        [Range(1, 999, ErrorMessage = "Er moet minimaal 1 deelnemer")]
        public int MaxParticipants { get; set; }

        public string? Awards { get; set; }
    }
}
