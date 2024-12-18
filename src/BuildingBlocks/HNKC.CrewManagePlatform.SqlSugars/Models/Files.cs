﻿using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 文件
    /// </summary>
    [SugarTable("t_files", IsDisabledDelete = true, TableDescription = "文件")]
    public class Files : BaseEntity<long>
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? Name { get; set; }
        /// <summary>
        /// 原始文件名称
        /// </summary>
        [SugarColumn(Length = 100)]
        public string? OriginName { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? SuffixName { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        [SugarColumn(ColumnDataType = "int", DefaultValue = "0")]
        public long? FileSize { get; set; }

        [SugarColumn(Length = 19)]
        public long UserId { get; set; }
    }
}
