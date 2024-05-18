using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorConnect.Models;

namespace SeniorConnect.Pages.activity
{
    public class ActivityDetailModel : PageModel
    {
        public List<Activity> Activitys = new();
        public Activity Activity = new();

        public ActivityDetailModel()
        {

        }

        public void OnGet(int Id)
        {
            Activitys.Add(new Activity
            {
                ActivityId = 1,
                ActivityName = "Bingo voor Senioren",
                ActivityDescription = "Ouderenbingo is een geliefde activiteit onder senioren, waarbij ze samenkomen om nummers af te strepen en gezelligheid te delen. Het spel brengt niet alleen opwinding, maar ook sociale verbinding en therapeutische voordelen, zoals het stimuleren van cognitieve functies. Kortom, ouderenbingo is een waardevolle gelegenheid voor ouderen om uit hun kamers te komen, interactie te hebben en vriendschappen te koesteren.",
                ActivityImg = "https://cdn-efkdob.nitrocdn.com/myhgDcCvqygVBxRpjTaycdopPbcYzKxm/assets/images/optimized/rev-ba2f213/www.riddlevillage.com/wp-content/uploads/Senior-woman-winning-at-the-game-of-bingo-concept-image-for-bingo-prize-ideas-for-seniors-800x533.jpg",
            });

            Activitys.Add(new Activity
            {
                ActivityId = 2,
                ActivityName = "Zwemmen",
                ActivityDescription = "Zwemmen is een van de meest veelzijdige en toegankelijke sporten voor volwassenen. Of je nu een doorgewinterde atleet bent of net begint, zwemmen biedt tal van voordelen voor zowel lichaam als geest.\r\n\r\nAllereerst is zwemmen een uitstekende vorm van lichaamsbeweging die alle belangrijke spiergroepen traint. Het is een low-impact sport, wat betekent dat het zacht is voor de gewrichten en ideaal voor mensen die last hebben van blessures of chronische pijn. Door het water voelen je lichaam en gewrichten minder belasting, waardoor je op een comfortabele en veilige manier kunt trainen.",
                ActivityImg = "https://d21buns5ku92am.cloudfront.net/67995/images/356723-newsroom-piscine-b598ed-large-1591952868.jpg",
            });

            Activitys.Add(new Activity
            {
                ActivityId = 3,
                ActivityName = "Golfen",
                ActivityDescription = "Golfen is een sport die zowel lichaam als geest aanspreekt en biedt een unieke combinatie van fysieke activiteit, mentale uitdaging en sociale interactie. Het is een geweldige keuze voor volwassenen die op zoek zijn naar een plezierige en gezonde manier om tijd door te brengen.\r\n\r\nEén van de grootste voordelen van golfen is dat het een laagdrempelige manier is om aan lichaamsbeweging te doen. Tijdens een rondje golf loop je gemiddeld zo'n 6 tot 10 kilometer, wat bijdraagt aan je cardiovasculaire gezondheid en helpt bij het onderhouden van een gezond gewicht. Daarnaast verbetert het slaan van de bal je spierkracht en flexibiliteit, vooral in de kernspieren, armen en rug.",
                ActivityImg = "https://mijnslovenie.com/wp-content/uploads/2018/11/golfist-1024x696.jpg",
            });

            Activity = Activitys.Where(a => a.ActivityId == Id).FirstOrDefault();
        }
    }
}
