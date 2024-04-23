using CH.Simple.Web.Modles;
using Microsoft.AspNetCore.Mvc;

namespace CH.Simple.Web.Controllers
{
    public abstract class HnkcControllerBase : ControllerBase
    {
        /// <summary>
        /// msg
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [NonAction]
        protected virtual ObjectResult OkMsg(string msg)
        {
            return new OkMsgObjectResult(msg);
        }
    }
}
