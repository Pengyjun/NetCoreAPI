using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.DomainUser
{

    /// <summary>
    /// 域用户信息
    /// </summary>
    public class DomainUserResponseDto
    {

        /// <summary>
        /// 手机号
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string? Card { get; set; }
        /// <summary>
        /// 职工号
        /// </summary>
        public string? WorkerAccount { get; set; }
        /// <summary>
        /// 域账号
        /// </summary>
        public string? DomainAccount { get; set; }
    }
}
