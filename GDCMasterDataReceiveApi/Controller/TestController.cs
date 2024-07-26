using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GDCMasterDataReceiveApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseController
    {
        public IBaseService baseService { get; set; }
        public TestController(IBaseService baseService) { 
        this.baseService = baseService;
        }

        [HttpGet]
        public GlobalCurrentUser Test()
        {
           var a= SnowflakeAlgorithmUtil.GenerateSnowflakeId();
            return null;
        }
    }
}
