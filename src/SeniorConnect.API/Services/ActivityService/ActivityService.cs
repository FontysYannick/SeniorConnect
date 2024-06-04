using SeniorConnect.API.Data;
using SeniorConnect.API.Entities;
using SeniorConnect.API.Models.Activity;

namespace SeniorConnect.API.Services.ActivityService
{
    public class ActivityService
    {
        private readonly DataContext _dataContext;

        public ActivityService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<Activity> GetActivities()
        {
            return _dataContext.Activities.ToList();
        }

        public Activity GetSingleActivity(int ActivityId)
        {
            return _dataContext.Activities.Where(a => a.ActivityId == ActivityId).FirstOrDefault();
        }


        public void setActivty(AbstractActivity activity)
        {
            var newActivty = new Activity
            {
                Title = activity.Title,
                OrganizerId = activity.OrganizerId,
                Description = activity.Description,
                Image = activity.Image,
                Date = activity.Date,
                Place = activity.Place,
                MaxParticipants = activity.MaxParticipants,
                Awards = activity.Awards
            };

            _dataContext.Activities.Add(newActivty);
            _dataContext.SaveChanges();
        }

        public void addUserToActivty(AbstractUserActivty userActivty)
        {
            var newUserActivty = new ActivityUsers
            {
                //UserId = userActivty.UserId,
                //ActivityId = userActivty.ActivityId,
                UserId = _dataContext.Users.Where(a => a.UserId == userActivty.UserId).Select(a => a.UserId).FirstOrDefault(),
                ActivityId = _dataContext.Activities.Where(a => a.ActivityId == userActivty.ActivityId).Select(a => a.ActivityId).FirstOrDefault()
            };

            _dataContext.ActivityUsers.Add(newUserActivty);
            _dataContext.SaveChanges();
        }      
    }
}
