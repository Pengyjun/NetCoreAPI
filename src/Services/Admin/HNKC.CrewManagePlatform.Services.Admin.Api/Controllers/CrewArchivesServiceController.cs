using HNKC.CrewManagePlatform.Services.Interface.CrewArchives;
using Microsoft.AspNetCore.Mvc;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.Controllers
{
    /// <summary>
    /// 船员档案控制器 restful风格
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CrewArchivesServiceController : BaseController
    {
        private ICrewArchivesService _service;
        /// <summary>
        /// 服务注入
        /// </summary>
        /// <param name="service"></param>
        public CrewArchivesServiceController(ICrewArchivesService service)
        {
            this._service = service;
        }


    }
}
