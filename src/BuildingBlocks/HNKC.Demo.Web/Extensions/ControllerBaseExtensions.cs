<<<<<<<< HEAD:src/BuildingBlocks/HNKC.CrewManagePlatform.Web/Extensions/ControllerBaseExtensions.cs
﻿using HNKC.CrewManagePlatform.Web.ActionResults;
using Microsoft.AspNetCore.Mvc;

namespace HNKC.CrewManagePlatform.Web.Extensions
========
﻿using HNKC.Demo.Web.ActionResults;
using Microsoft.AspNetCore.Mvc;

namespace HNKC.Demo.Web.Extensions
>>>>>>>> 7fd224848dc4910963de00d8c3a15a3418dc1847:src/BuildingBlocks/HNKC.Demo.Web/Extensions/ControllerBaseExtensions.cs
{
    public static class ControllerBaseExtensions
    {
        public static ObjectResult OkMessage(this ControllerBase controller, string message)
        {
            return new OkMessageObjectResult(message);
        }
    }
}
