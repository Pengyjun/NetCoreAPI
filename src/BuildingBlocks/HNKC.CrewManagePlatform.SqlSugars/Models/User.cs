using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 用户表
    /// </summary>
    [SugarTable("t_user", IsDisabledDelete = true,TableDescription = "用户表")]
    public class User:BaseEntity<long>
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [SugarColumn(Length = 128,ColumnDescription = "姓名")]
        public string? Name { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "工号")]
        public string? WorkNumber { get; set; }

        /// <summary>
        /// 所属船舶（部门）OID
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "所属船舶（部门）OID")]
        public string?  Oid { get; set; }

        /// <summary>
        /// 职务ID
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "职务ID")]
        public string? JobId { get; set; }

        /// <summary>
        /// 用工形式
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "用工形式")]
        public string? EmploymentId { get; set; }


        /// <summary>
        /// 劳务公司
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "劳务公司")]
        public string? ServiceCompanyId{ get; set; }


        /// <summary>
        /// 手机号
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "手机号")]
        public string? Phone { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "密码")]
        public string? Password { get; set; }


        /// <summary>
        /// 船舶类型
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "船舶类型")]
        public string? ShipTypeId { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "身份证号")]
        public string? CardId { get; set; }

        /// <summary>
        /// 第一适任证书（有职务）
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "第一适任证书（有职务）")]
        public string? FirstCertificateId { get; set; }
        /// <summary>
        /// 第二适任证书
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "第二适任证书")]
        public string? SecondCertificateId { get; set; }


        /// <summary>
        /// 培训合格证编号
        /// </summary>
        [SugarColumn(Length = 32, ColumnDescription = "培训合格证编号")]
        public string? CertificateNumberId { get; set; }
    }
}
