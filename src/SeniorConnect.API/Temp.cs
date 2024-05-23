using SeniorConnect.API.Entities;

namespace SeniorConnect.API
{
    public class Temp
    {
        public List<Activity> Activitys = new();
        public List<User> Users = new();
        public List<BaseList> baseLists = new();
        public List<BaseListItem> baseListItems = new();


        public List<User> setUser()
        {
            User piet = new User
            {
                UserId = 1,
                FirstName = "yannick",
                LastName = "test",
                Email = "test",
                Phonenumber = "test"
            };

            Users.Add(piet);

            return Users;
        }

        public List<Activity> setActivty()
        {
            Users = setUser();
            baseListItems = setBaseListItems();

            Activitys.Add(new Activity
            { 
                ActivityId = 1,
                Title = "Bingo",
                //Catagory = baseListItems[0],
                Organizer = Users[0],
                Description = "is bingo",
                Date = DateTime.Now,
                Place = "Eindhoven",
                MaxParticipants = 10,
                Awards = "5 euro"
            });

            Activitys.Add(new Activity
            {
                ActivityId = 2,
                Title = "Zwemmen",
                //Catagory = baseListItems[1],
                Organizer = Users[0],
                Description = "is zwemmen",
                Date = DateTime.Now,
                Place = "Eindhoven",
                MaxParticipants = 50,
                Awards = "10 euro"
            });

            return Activitys;
        }

        public List<BaseListItem> setBaseListItems()
        {
            BaseList baseList = new BaseList
            {
                BaselistId = 1,
                Code = "CATAGORY",
                Description = "Category"
            };

            baseListItems.Add(new BaseListItem
            {
                BaselistId = 1,
                BaselistItemId = 1,
                Code = "GAMES",
                Description = "Spellen"
            });

            baseListItems.Add(new BaseListItem
            {
                BaselistId = 1,
                BaselistItemId = 2,
                Code = "WATER",
                Description = "Water"
            });

            return baseListItems;
        }
    }
}
