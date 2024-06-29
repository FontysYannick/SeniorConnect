using global::SeniorConnect.API.Models.Activity;
using SeniorConnect.API.Entities;
using System.Collections.Generic;

namespace SeniorConnect.API.Services.ActivityService.Interface
{
    public interface IActivityService
    {
        IEnumerable<Activity> GetActivities();
        Activity GetSingleActivity(int activityId);
        void SetActivity(AbstractActivity activity);
        void AddUserToActivity(AbstractUserActivty userActivity);
        IEnumerable<Activity> GetUserToActivity(int userId);
        bool DeleteActivityUser(int userId, int activityId);
    }
}
