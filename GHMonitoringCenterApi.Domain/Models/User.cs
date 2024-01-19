using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Models
{

    /// <summary>
    /// 用户表
    /// </summary>
    [SugarTable("t_user",IsDisabledDelete =true)]
    public class User:BaseEntity<Guid>
    {

        /// <summary>
        /// pomId
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid PomId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [SugarColumn(Length = 500)]
        public string? Name { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        [SugarColumn(Length = 20)]
        public string? IdentityCard { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? CompanyId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        [SugarColumn(Length = 36)]
        public Guid? DepartmentId { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [SugarColumn(Length = 20)]

        public string? Phone { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? LoginName { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        [SugarColumn(Length = 20)]
        public string? LoginAccount { get; set; }

        /// <summary>
        /// 集团账号编码，对应集团empcode字段
        /// </summary>
        [SugarColumn(Length = 20)]
        public string? GroupCode { get; set; }

        /// <summary>
        /// 用户工号
        /// </summary>
        [SugarColumn(Length = 20)]
        public string? Number { get; set; }

    }
}
