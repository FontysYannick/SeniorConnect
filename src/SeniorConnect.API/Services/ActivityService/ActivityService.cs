using SeniorConnect.API.Data;
using SeniorConnect.API.Models.Activity;
using System.Diagnostics;

namespace SeniorConnect.API.Services.ActivityService
{
    public class ActivityService
    {
        private readonly DataContext _dataContext;

        public ActivityService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void setActivty(AbstractActivity activity)
        {
            var newActivty = new SeniorConnect.API.Entities.Activity
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
    }
}
