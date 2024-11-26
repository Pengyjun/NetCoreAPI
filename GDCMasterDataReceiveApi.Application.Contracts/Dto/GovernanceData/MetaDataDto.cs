using System.ComponentModel.DataAnnotations;

namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.GovernanceData
{
    /// <summary>
    /// 
    /// </summary>
    public class MetaDataDto
    {
        /// <summary>
        /// id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public string? ColumName { get; set; }
        /// <summary>
        /// 列注释
        /// </summary>
        public string? ColumComment { get; set; }
        /// <summary>
        /// 是否主键 false 不是
        /// </summary>
        public bool IsPrimaryKey { get; set; }
        /// <summary>
        /// 是否允许为空 false 否
        /// </summary>
        public bool IsAllowToBeEmpty { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string? DataType { get; set; }
        /// <summary>
        /// 数据长度 自动带出 可更改
        /// </summary>
        //public int DataLength;




        //private int Length;

        public int DataLength
        {
            get { if (DataType == "int") { DataLength = 0; } return 0; }
            set { }
        }


        /// <summary>
        /// 数据小数位
        /// </summary>
        public int DataDecimalPlaces { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string? DefaultValue { get; set; }

      
    }
    /// <summary>
    /// 
    /// </summary>
    public class MetaDataRequestDto
    {
        /// <summary>
        /// 增还是改  1 增 2 改   3是删除
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public string? TableName { get; set; }
        /// <summary>
        /// 数据集
        /// </summary>
        //public List<MetaDataDto>? Mds { get; set; }
        public MetaDataDto Mds { get; set; }
    }
}
