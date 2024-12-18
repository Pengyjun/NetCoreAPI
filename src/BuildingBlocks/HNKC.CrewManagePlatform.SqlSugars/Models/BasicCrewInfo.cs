using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;
using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 船员基本信息
    /// </summary>
    [SugarTable("t_basiccrewinfo", IsDisabledDelete = true, TableDescription = "船员基本信息")]

    public class BasicCrewInfo
    {
        #region 基本信息
        /// <summary>
        /// 政治面貌
        /// </summary>
        public string? PoliticalStatus { get; set; }
        /// <summary>
        /// 籍贯
        /// </summary>
        public string? NativePlace { get; set; }
        /// <summary>
        /// 民族
        /// </summary>
        public string? Nation { get; set; }
        /// <summary>
        /// 家庭地址
        /// </summary>
        public string? HomeAddress { get; set; }
        /// <summary>
        /// 常住地
        /// </summary>
        public string? BuildAddress { get; set; }
        #endregion

        #region 专业信息
        /// <summary>
        /// 船舶类型
        /// </summary>
        public string? ShipType { get; set; }
        /// <summary>
        /// 船员类型
        /// </summary>
        public string? CrewType { get; set; }
        /// <summary>
        /// 服务簿类型
        /// </summary>
        public string? ServiceBookType { get; set; }
        /// <summary>
        /// 所在船舶
        /// </summary>
        public string? OnBoard { get; set; }
        /// <summary>
        /// 在船职务
        /// </summary>
        public string? PositionOnBoard { get; set; }
        #endregion

        #region 家庭成员&紧急联系人
        /// <summary>
        /// 家庭成员
        /// </summary>
        public List<UserInfos>? HomeUser { get; set; }
        /// <summary>
        /// 紧急联系人
        /// </summary>
        public List<UserInfos>? EmergencyContacts { get; set; }
        #endregion
    }
}
