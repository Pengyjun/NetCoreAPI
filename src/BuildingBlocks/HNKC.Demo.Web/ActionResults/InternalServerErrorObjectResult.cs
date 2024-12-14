using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

<<<<<<<< HEAD:src/BuildingBlocks/HNKC.CrewManagePlatform.Web/ActionResults/InternalServerErrorObjectResult.cs
namespace HNKC.CrewManagePlatform.Web.ActionResults
========
namespace HNKC.Demo.Web.ActionResults
>>>>>>>> 7fd224848dc4910963de00d8c3a15a3418dc1847:src/BuildingBlocks/HNKC.Demo.Web/ActionResults/InternalServerErrorObjectResult.cs
{
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object error) : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
