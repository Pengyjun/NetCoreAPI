using HNKCCH.POMS.Web.ActionResults;
using Microsoft.AspNetCore.Mvc;

namespace HNKC.Web.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static ObjectResult OkMessage(this ControllerBase controller, string message)
        {
            return new OkMessageObjectResult(message);
        }
    }
}
