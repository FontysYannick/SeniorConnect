using SeniorConnect.API.Data;
using SeniorConnect.API.Entities;
using SeniorConnect.API.Models.Activity;
using SeniorConnect.API.Services.ActivityService.Interface;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;

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
            var newActivity = new Activity
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

            _context.Activities.Add(newActivity);
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

        public bool DeleteActivity(int activityId)
        {
            var activity = _context.Activities.FirstOrDefault(a => a.ActivityId == activityId);
            if (activity != null)
            {
                _context.Activities.Remove(activity);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteActivityUser(int userId, int activityId)
        {
            var activityUser =
                _context.ActivityUsers.FirstOrDefault(a => a.UserId == userId && a.ActivityId == activityId);

            if (activityUser == null)
            {
                return false;
            }
            
            _context.ActivityUsers.Remove(activityUser);
            _context.SaveChanges();
            return true;
        }
    }
}
