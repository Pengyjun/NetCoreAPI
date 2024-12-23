﻿using HNKC.CrewManagePlatform.Models.Enums;
using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 紧急联系人
    /// </summary>
    [SugarTable("t_emergencycontacts", IsDisabledDelete = true, TableDescription = "紧急联系人")]
    public class EmergencyContacts : BaseEntity<long>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 100, ColumnDescription = "名称")]
        public string? UserName { get; set; }
        /// <summary>
        /// 关系
        /// </summary>
        [SugarColumn(Length = 5, ColumnDescription = "关系")]
        public FamilyRelationEnum RelationShip { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        [SugarColumn(Length = 11, ColumnDescription = "联系方式")]
        public string? Phone { get; set; }
        /// <summary>
        /// 工作单位
        /// </summary>
        [SugarColumn(Length = 100, ColumnDescription = "工作单位")]
        public string? WorkUnit { get; set; }
        /// <summary>
        /// 关联键
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "关联键")]
        public Guid? EmergencyContactId { get; set; }
    }
}
