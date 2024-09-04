using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Domain.Shared
{

    /// <summary>
    /// 4A接口接收返回数据
    /// </summary>
    public class MDMResponseResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public string? RETURN_CODE { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? RETURN_DESC { get; set; }


        /// <summary>
        /// 成功
        /// </summary>
        public void Success()
        {
            this.RETURN_CODE = "S0000001";
            this.RETURN_DESC = "处理成功";
        }
        /// <summary>
        /// 失败
        /// </summary>
        public void Fail()
        {
            this.RETURN_CODE = "E0003008";
            this.RETURN_DESC = "未知的程序异常";
        }
        /// <summary>
        /// 用户已存在
        /// </summary>
        public void UserExist()
        {
            this.RETURN_CODE = "E0003001";
            this.RETURN_DESC = "用户已存在";
        }
        /// <summary>
        /// 用户不存在
        /// </summary>
        public void UserNoExist()
        {
            this.RETURN_CODE = "E0003002";
            this.RETURN_DESC = "用户不存在";
        }
        /// <summary>
        /// 修改用户成功
        /// </summary>
        public void   UpdateSuccess()
        {
            this.RETURN_CODE = "E0003008";
            this.RETURN_DESC = "未知的程序异常";
        }
    }

}
