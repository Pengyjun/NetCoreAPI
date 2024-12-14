using HNKC.CrewManagePlatform.Web.ActionResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
namespace HNKC.CrewManagePlatform.Web.Extensions


{
    public static class ControllerBaseExtensions
    {
        public static ObjectResult OkMessage(this ControllerBase controller, string message)
        {
            return new OkMessageObjectResult(message);
        }
    }
}
