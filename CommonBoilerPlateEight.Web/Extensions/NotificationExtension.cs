using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace CommonBoilerPlateEight.Web.Extensions;

public static class NotificationExtension
{

    public static void NotifySuccess(this Controller controller, string message)
    {
        var msg = new
        {
            message = message,
            status = NotificationType.success.ToString()
        };

        controller.TempData["Message"] = JsonSerializer.Serialize(msg);
    }
    public static void NotifyError(this Controller controller, string message)
    {
        var msg = new
        {
            message = message,
            status = NotificationType.error.ToString()
        };

        controller.TempData["Message"] = JsonSerializer.Serialize(msg);
    }

    public static void NotifyInfo(this Controller controller, string message)
    {
        var msg = new
        {
            message = message,
            status = NotificationType.info.ToString()
        };

        controller.TempData["Message"] = JsonSerializer.Serialize(msg);
    }

    public static void NotifyModelStateErrors(this Controller controller)
    {
        var errors = string.Join("<br>", controller.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());
        controller.NotifyInfo(errors);
    }
}

public enum NotificationType
{
    error,
    success,
    info
}
