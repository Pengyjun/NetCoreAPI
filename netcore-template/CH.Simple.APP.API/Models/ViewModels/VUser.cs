using System.ComponentModel.DataAnnotations;

namespace CH.Simple.APP.API.Models.ViewModels
{
    public class VUser
    {
        /// <summary>
        /// 新增可为空,修改不为空
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [Required(ErrorMessage = "姓名不能为空")]
        public string Name { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Required(ErrorMessage = "手机号码不能为空")]
        public string Mobile { get; set; }
    }
}
