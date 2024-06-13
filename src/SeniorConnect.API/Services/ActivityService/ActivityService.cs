using SeniorConnect.API.Data;
using SeniorConnect.API.Entities;
using SeniorConnect.API.Models.Activity;
using SeniorConnect.API.Services.ActivityService.Interface;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SeniorConnect.API.Services.ActivityService
{
    public class ActivityService : IActivityService
    {
        private readonly DataContext _context;

        public ActivityService(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Activity> GetActivities()
        {
            return _context.Activities.Where(a => a.Date > DateTime.Now).OrderBy(a => a.Date).ToList();
        }

        public Activity GetSingleActivity(int activityId)
        {
            return _context.Activities.Include(a => a.Organizer).FirstOrDefault(a => a.ActivityId == activityId);
        }

        public void SetActivity(AbstractActivity activity)
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

            _context.Activities.Add(newActivty);
            _context.SaveChanges();
        }

        public void AddUserToActivity(AbstractUserActivty userActivity)
        {
            var activityUser = new ActivityUsers
            {
                UserId = userActivity.UserId,
                ActivityId = userActivity.ActivityId
            };

            _context.ActivityUsers.Add(activityUser);
            _context.SaveChanges();
        }

        public IEnumerable<Activity> GetUserToActivity(int userId)
        {
            return _context.ActivityUsers
                        .Where(a => a.UserId == userId && a.Activity.Date > DateTime.Now)
                        .Include(a => a.Activity)
                        .Select(a => a.Activity)
                        .OrderBy(a => a.Date)
                        .ToList();
        }
    }
}
