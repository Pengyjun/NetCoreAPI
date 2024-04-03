using CH.Simple.APP.API.Models.ViewModels;
using CH.Simple.Entities;
using CH.Simple.EntityFrameworkCore;
using CH.Simple.Exceptions;
using CH.Simple.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using CH.Simple.Web.Extensions;
using CH.Simple.Utils;

namespace CH.Simple.APP.API.Controllers
{
    [Route("api/efcore/user")]
    [ApiController]
    public class EFCoreController : ControllerBase
    {
        private readonly SimpleContext _context;
        public EFCoreController(SimpleContext context)
        {
            _context = context;
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
            var queryable = _context.Users.AsNoTracking()
                .Where(x => true);

            if (!string.IsNullOrEmpty(name))
                queryable = queryable.Where(x => x.Name.Contains(name));

            var list = await queryable.ToPageResultAsync(pageIndex, pageSize);
            return Ok(list);
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
            var user = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

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
            _context.Users.Add(new User
            {
                Id = PKManager.UUID(),
                Name = model.Name,
                Mobile = model.Mobile,
                CreateBy = "system",
                Created = nowTime,
                IsDelete = false,
                Modified = nowTime,
                ModifieBy = "system"
            });
            _context.SaveChanges();
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
            var user = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                throw new SimpleException("用户不存在");

            _context.Users.Remove(user);
            _context.SaveChanges();
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
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (user == null)
                throw new SimpleException("用户不存在");

            user.Name = model.Name;
            user.Mobile = model.Mobile;
            user.Modified = DateTime.Now;
            user.ModifieBy = "system";
            _context.SaveChanges();
            return Ok("修改成功");
        }

    }
}
