using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

<<<<<<<< HEAD:src/BuildingBlocks/HNKC.CrewManagePlatform.Web/ActionResults/OkMessageObjectResult.cs
namespace HNKC.CrewManagePlatform.Web.ActionResults
========
namespace HNKC.Demo.Web.ActionResults
>>>>>>>> 7fd224848dc4910963de00d8c3a15a3418dc1847:src/BuildingBlocks/HNKC.Demo.Web/ActionResults/OkMessageObjectResult.cs
{
    public class OkMessageObjectResult : ObjectResult
    {
        public OkMessageObjectResult(object value) : base(value)
        {
            StatusCode = 200;
        }
    }
}
