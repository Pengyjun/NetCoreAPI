using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Domain.Models
{

    /// <summary>
    /// 从广航局预控系统获取的用户信息
    /// </summary>
    [SugarTable("t_gdcuser", IsDisabledDelete = true)]
    public class GDCUser:BaseEntity<long>
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? Phone { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? Card { get; set; }
        /// <summary>
        /// 域账号
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? DomainAccount { get; set; }
        /// <summary>
        /// 职工号
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? WorkerAccount { get; set; }
    }
}
