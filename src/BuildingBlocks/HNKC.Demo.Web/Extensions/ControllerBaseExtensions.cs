using HNKC.Demo.Web.ActionResults;
using Microsoft.AspNetCore.Mvc;

namespace HNKC.Demo.Web.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static ObjectResult OkMessage(this ControllerBase controller, string message)
        {
            return new OkMessageObjectResult(message);
        }
    }
}
