using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorConnect.Models;

namespace SeniorConnect.Pages
{
    public class IndexModel : PageModel
    {
        public Activity Activity { get; set; }

        public IndexModel()
        {

        }

        public void OnGet()
        {
            Activity = new Activity { 
                ActivityId = new Guid(), 
                ActivityName = "Bingo voor Senioren", 
                ActivityDescription = "Ouderenbingo is een geliefde activiteit onder senioren, waarbij ze samenkomen om nummers af te strepen en gezelligheid te delen. Het spel brengt niet alleen opwinding, maar ook sociale verbinding en therapeutische voordelen, zoals het stimuleren van cognitieve functies. Kortom, ouderenbingo is een waardevolle gelegenheid voor ouderen om uit hun kamers te komen, interactie te hebben en vriendschappen te koesteren.",
                ActivityImg = "https://cdn-efkdob.nitrocdn.com/myhgDcCvqygVBxRpjTaycdopPbcYzKxm/assets/images/optimized/rev-ba2f213/www.riddlevillage.com/wp-content/uploads/Senior-woman-winning-at-the-game-of-bingo-concept-image-for-bingo-prize-ideas-for-seniors-800x533.jpg",
            };
        }
    }
}
