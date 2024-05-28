using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SeniorConnect.Helpers
{
    public class NotificationHelper
    {
        public static void SetNotification(ITempDataDictionary tempData , string title, string type, string? text = null)
        {
            tempData["Notification"] = title;
            tempData["NotificationType"] = type;
            tempData["NotificationText"] = text;
        }

        public static void SetNotificationWithConfirmRedirect(ITempDataDictionary tempData, string title, string type, string redirectAfterConfirmLink, string? text = null)
        {
            tempData["Notification"] = title;
            tempData["NotificationType"] = type;
            tempData["redirectAfterConfirmLink"] = redirectAfterConfirmLink;
            tempData["NotificationText"] = text;
        }
    }
}
