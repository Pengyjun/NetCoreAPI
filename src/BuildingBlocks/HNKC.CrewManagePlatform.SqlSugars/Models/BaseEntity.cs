using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// CodeFirst基本实体全部都要继承此类
    /// </summary>
    /// <typeparam name="T">主键数据类型</typeparam>
    public class BaseEntity<T> where T : struct
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public T Id { get; set; }
        /// <summary>
        /// 业务主键
        /// </summary>
        [SugarColumn(Length = 36, ColumnDescription = "业务主键")]
        public Guid BusinessId { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", IsOnlyIgnoreUpdate = true)]
        public DateTime? Created { get; set; }
        /// <summary>
        /// 更新人Id
        /// </summary>
        [SugarColumn(Length = 64)]
        public string? ModifiedBy { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime? Modified { get; set; }
        /// <summary>
        /// 是否删除  0删除  1有效
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "1")]
        public int IsDelete { get; set; } = 1;
    }
}
