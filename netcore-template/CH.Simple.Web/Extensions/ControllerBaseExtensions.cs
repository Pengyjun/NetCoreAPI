using CH.Simple.Web.ActionResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CH.Simple.Web.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static ObjectResult OkMessage(this ControllerBase controller, string message)
        {
            return new OkMessageObjectResult(message);
        }
    }
}
