using CH.Simple.APP.API.Models.ViewModels;
using CH.Simple.Entities;
using CH.Simple.EntityFrameworkCore;
using CH.Simple.Exceptions;
using CH.Simple.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SqlSugar;
using System.Drawing.Printing;

namespace CH.Simple.APP.API.Controllers
{
    [Route("api/sqlsuger/user")]
    [ApiController]
    public class SqlSugerController : ControllerBase
    {
        private readonly ISqlSugarClient _context;
        public SqlSugerController(ISqlSugarClient db)
        {
            _context = db;
        }
        /// <summary>
        /// 分页获取用户
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="pageIndex">分页下标</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <returns></returns>
        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> GetUserAsync([FromQuery] string? name, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 15)
        {
            int totalCount=0;
            var list = _context.Queryable<User>().ToPageList(pageIndex, pageSize, ref totalCount);

            var pageResult = new PageResult<User>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                List = list
            };
            return Ok(pageResult);
        }

        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        [Route("info")]
        [HttpGet]
        public async Task<IActionResult> GetUserAsync([FromQuery] string id)
        {
            var user = _context.Queryable<User>()
                .InSingle(id);

            if (user == null)
                throw new SimpleException("用户不存在");

            return Ok(user);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns></returns>
        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] VUser model)
        {
            var nowTime = DateTime.Now;
            var result= _context.Insertable<User>(new User
            {
                Id = PKManager.UUID(),
                Name = model.Name,
                Mobile = model.Mobile,
                CreateBy = "system",
                Created = nowTime,
                IsDelete = false,
                Modified = nowTime,
                ModifieBy = "system"
            }).ExecuteCommand();
            return Ok("添加成功！");
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        /// <exception cref="SimpleException"></exception>
        [Route("del")]
        [HttpPost]
        public async Task<IActionResult> DelUserAsync([FromForm] string id)
        {
            _context.Deleteable<User>(new User
            {
                Id = id
            });
            return Ok("删除成功");
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns></returns>
        /// <exception cref="SimpleException"></exception>
        [Route("modify")]
        [HttpPost]
        public async Task<IActionResult> ModifyUserAsync([FromBody] VUser model)
        {
            var user = new User
            {
                Id = model.Id,
                Name = model.Name,
                Mobile = model.Mobile,
                ModifieBy = "system",
                Modified = DateTime.Now
            };
            _context.Updateable(user);
            return Ok("修改成功");
        }
    }
}
