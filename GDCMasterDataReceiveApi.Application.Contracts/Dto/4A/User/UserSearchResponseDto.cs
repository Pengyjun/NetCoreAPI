namespace GDCMasterDataReceiveApi.Application.Contracts.Dto._4A.User
{
    /// <summary>
    /// 用户响应dto
    /// </summary>
    public class UserSearchResponseDto
    {
        /// <summary>
        /// 人员编码  必填,HR 系统中定义的人员唯  一编码，默认用户名
        /// </summary>
        public string? EMP_CODE { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string? NAME { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string? PHONE { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string? EMAIL { get; set; }
        /// <summary>
        /// 证件编号
        /// </summary>
        public string? CERT_NO { get; set; }
        /// <summary>
        /// 主职所在部门 ID
        /// </summary>
        public string? OFFICE_DEPID { get; set; }
        /// <summary>
        /// 所属部门名称
        /// </summary>
        public string? OFFICE_DEPID_Name { get; set; }
        /// <summary>
        /// 人员状态信息
        /// </summary>
        public string? UserInfoStatus { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string? CompanyName { get; set; }
        /// <summary>
        /// 是否启用   0 禁用  1 启用
        /// </summary>
        public string? Enable { get; set; }
    }
    /// <summary>
    /// 用户机构dto
    /// </summary>
    public class UInstutionDto
    {
        /// <summary>
        /// oid
        /// </summary>
        public string? OID {  get; set; }
        /// <summary>
        /// 上级oid
        /// </summary>
        public string? POID {  get; set; }
        /// <summary>
        /// 规则编码
        /// </summary>
        public string? GRULE {  get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string? NAME {  get; set; }
    }
}
