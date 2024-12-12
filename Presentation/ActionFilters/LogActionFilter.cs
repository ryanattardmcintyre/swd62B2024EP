using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Presentation.ActionFilters
{
    //to do:

    //1. we see how the actionfilter is applied not on just 1 method
    //2. example of filtering images!
    public class LogActionFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Log myLog = new Log();
            myLog.Timestamp = DateTime.Now;
            myLog.User = "Anonymous";
            if (context.HttpContext.User != null)
            {
                     if (context.HttpContext.User.Identity.IsAuthenticated == true)
                     {
                           myLog.User = context.HttpContext.User.Identity.Name;
                     }
            }

            myLog.IpAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString(); //::1 or xx.xx.xx.xx

            myLog.Message = $"Action: {context.HttpContext.Request.Path}, Parameters: {context.HttpContext.Request.QueryString.Value}";

            //in Program.cs this was registered already so it won't be null
            ILogRepository myLogRepository = context.HttpContext.RequestServices.GetService<ILogRepository>();
            myLogRepository.AddLog(myLog);

            base.OnActionExecuting(context);
        }
    }
}
