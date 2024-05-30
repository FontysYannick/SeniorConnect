using SeniorConnect.Models;
using System.ComponentModel;

namespace SeniorConnect.Helpers
{
    public class SetData
    {
        public List<Activity> Activitys = new();

        public List<Activity> setActivty()
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

            Activitys.Add(new Activity
            {
                ActivityId = 4,
                ActivityName = "Kookworkshop",
                ActivityDescription = "Een kookworkshop is een uitstekende manier voor volwassenen om nieuwe culinaire vaardigheden te leren en creatief te zijn in de keuken. Onder begeleiding van een ervaren chef-kok leren deelnemers verschillende gerechten bereiden, van voorgerechten tot desserts. Het is niet alleen leerzaam, maar ook een geweldige gelegenheid om samen met anderen te koken, te proeven en te genieten van heerlijke maaltijden. Bovendien kan het koken ontspannend werken en bijdragen aan een gezonde levensstijl door het bevorderen van vers koken met verse ingrediënten.",
                ActivityImg = "https://www.jannskeuken.nl/uploads/resized/small/4-3/d05ad8f9-3f0f-4867-a0e5-2b9ef455ef70_2.jpg",
            });

            Activitys.Add(new Activity
            {
                ActivityId = 5,
                ActivityName = "Yoga",
                ActivityDescription = "Yoga is een eeuwenoude praktijk die fysieke houdingen, ademhalingstechnieken en meditatie combineert om zowel lichaam als geest in balans te brengen. Het is een toegankelijke activiteit voor mensen van alle leeftijden en fitnessniveaus. Door regelmatig aan yoga te doen, kunnen deelnemers hun flexibiliteit, kracht en balans verbeteren, evenals stress verminderen en een gevoel van innerlijke rust en welzijn bevorderen. Yoga kan worden beoefend in groepslessen of individueel, en biedt een waardevolle manier om tijd voor jezelf te nemen en te focussen op persoonlijke gezondheid.",
                ActivityImg = "https://cdn.vilgain.com/image/article-cover/cover-desktop/jak-vam-joga-zmeni-zivot-8-duvodu-proc-s-jogou-zacit-2.jpeg?v=1592398763",
            });

            Activitys.Add(new Activity
            {
                ActivityId = 6,
                ActivityName = "Schilderworkshop",
                ActivityDescription = "Een schilderworkshop biedt volwassenen de kans om hun creatieve kant te ontdekken en uit te drukken. Onder begeleiding van een ervaren kunstenaar leren deelnemers verschillende schildertechnieken en werken ze aan hun eigen kunstwerken. Het is een ontspannende en meditatieve activiteit die helpt bij het verminderen van stress en het bevorderen van mindfulness. Bovendien kunnen deelnemers trots zijn op hun unieke creaties, die ze mee naar huis kunnen nemen als blijvende herinnering aan een inspirerende en leuke ervaring.",
                ActivityImg = "https://cdn1.emesa-static.com/uploads/Schildercursus-VakantieVeilingen.jpg",
            });

            return Activitys;
        }
    }
}
