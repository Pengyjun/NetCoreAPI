using GDCMasterDataReceiveApi.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.MainTableOfStatistics
{
    /// <summary>
    /// 统计当前模式所有表（Dm当前指定数据库）请求dto
    /// </summary>
    public class MainTableOfStatisticsRequestDto : IValidatableObject
    {
        /// <summary>
        /// 当前指定dm数据库模式 必填(模式名:SYSDBA)
        /// </summary>
        public string Schema { get; set; }
        /// <summary>
        /// 日期(精确到小时yyyy-MM-dd HH:2024-07-24 08)
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 需要过滤的表/不需要统计的表
        /// </summary>
        public List<string>? ScreenTables { get; set; }
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// 数据库名
        /// </summary>
        public string DataBase { get; set; }
        /// <summary>
        /// 入参校验
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Schema))
            {
                yield return new ValidationResult("模式名不能为空", new string[] { nameof(Schema) });
            }
            if (string.IsNullOrEmpty(Date.ToString()) || DateTime.MinValue == Date)
            {
                yield return new ValidationResult("日期不能为空", new string[] { nameof(Date) });
            }
            if (string.IsNullOrEmpty(Server))
            {
                yield return new ValidationResult("服务器地址不能为空", new string[] { nameof(Server) });
            }
            if (string.IsNullOrEmpty(UserId))
            {
                yield return new ValidationResult("用户不能为空", new string[] { nameof(UserId) });
            }
            if (string.IsNullOrEmpty(Pwd))
            {
                yield return new ValidationResult("密码不能为空", new string[] { nameof(Pwd) });
            }
            if (string.IsNullOrEmpty(DataBase))
            {
                yield return new ValidationResult("数据库名不能为空", new string[] { nameof(DataBase) });
            }
        }
    }
    /// <summary>
    /// 统计当前模式所有表Mysql（当前指定数据库）请求dto
    /// </summary>
    public class MainTableOfStatisticsMysqlRequestDto : IValidatableObject
    {
        /// <summary>
        /// 日期(精确到小时yyyy-MM-dd HH:2024-07-24 08)
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 需要过滤的表/不需要统计的表
        /// </summary>
        public List<string>? ScreenTables { get; set; }
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// 是否需要创建/修改视图 true是
        /// </summary>
        public bool IsCreateView {  get; set; }
        /// <summary>
        /// 查询的视图名称
        /// </summary>
        public string ViewName { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }
        /// <summary>
        /// 数据库名
        /// </summary>
        public string DataBase { get; set; }
        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Date.ToString()) || DateTime.MinValue == Date)
            {
                yield return new ValidationResult("日期不能为空", new string[] { nameof(Date) });
            }
            if (string.IsNullOrEmpty(Server))
            {
                yield return new ValidationResult("服务器地址不能为空", new string[] { nameof(Server) });
            }
            if (string.IsNullOrEmpty(User))
            {
                yield return new ValidationResult("用户不能为空", new string[] { nameof(User) });
            }
            if (string.IsNullOrEmpty(PassWord))
            {
                yield return new ValidationResult("密码不能为空", new string[] { nameof(PassWord) });
            }
            if (string.IsNullOrEmpty(DataBase))
            {
                yield return new ValidationResult("数据库名不能为空", new string[] { nameof(DataBase) });
            }
            if (string.IsNullOrEmpty(ViewName))
            {
                yield return new ValidationResult("视图名称为空", new string[] { nameof(ViewName) });
            }
        }
    }
}
