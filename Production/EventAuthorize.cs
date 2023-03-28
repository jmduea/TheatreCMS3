using System.Web.Mvc;
using System.Web.Routing;

namespace TheatreCMS3.Areas.Prod.Models
{
    public class EventAuthorize : AuthorizeAttribute
    {
        // Set Roles property to "EventPlanner" by default
        public EventAuthorize()
        {
            Roles = "EventPlanner";
        }

        // Override the HandleUnauthorizedRequest Method to redirect users to the AccessDenied page
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new { controller = "CalendarEvents", action = "AccessDenied" }));
        }
    }
}