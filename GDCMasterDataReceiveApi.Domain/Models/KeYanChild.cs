using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    ///科研相关其他信息
    /// <summary>
    /// 二级单位
    /// </summary>
    [SugarTable("t_kysecunit", IsDisabledDelete = true)]
    public class KySecUnit
    {
        /// <summary>
        /// 科研编码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string Code { get; set; }
        /// <summary>
        /// 所属二级单位
        /// </summary>
        [SugarColumn(Length =100, ColumnName = "SecUnit")]
        public string Z2NDORG { get; set; }
        /// <summary>
        /// 所属二级单位名称
        /// </summary>
        [SugarColumn(Length = 500, ColumnName = "SecUnitName")]
        public string? Z2NDORGN { get; set; }
    }
    /// <summary>
    /// 承担单位
    /// </summary>
    [SugarTable("t_kycdunit", IsDisabledDelete = true)]
    public class KyCDUnit
    {
        /// <summary>
        /// 科研编码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string Code { get; set; }
        /// <summary>
        /// 承担单位
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "CDUnit")]
        public string ZUDTK { get; set; }
        /// <summary>
        /// 承担单位名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "CDUnitName")]
        public string? ZUDTKN { get; set; }
        /// <summary>
        /// 内部/外部:1 内部/2 外部
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "Zioside")]
        public string ZIOSIDE { get; set; }
    }
    /// <summary>
    /// 曾用名
    /// </summary>
    [SugarTable("t_kynameceng", IsDisabledDelete = true)]
    public class KyNameCeng
    {
        /// <summary>
        /// 科研编码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string Code { get; set; }
        /// <summary>
        /// 行项目编号
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "PNo")]
        public string ZITEM { get; set; }
        /// <summary>
        /// 曾用名
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "Name")]
        public string ZOLDNAME { get; set; }
    }
    /// <summary>
    /// 参与单位
    /// </summary>
    [SugarTable("t_kycanyunit", IsDisabledDelete = true)]
    public class KyCanYUnit
    {
        /// <summary>
        /// 科研编码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string Code { get; set; }
        /// <summary>
        /// 参与单位
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "CyUnit")]
        public string ZPU { get; set; }
        /// <summary>
        /// 参与单位名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "CyUnitName")]
        public string? ZPUN { get; set; }
        /// <summary>
        /// 内部/外部 :1 内部/2 外部
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "Zioside")]
        public string ZIOSIDE { get; set; }
    }
    /// <summary>
    /// 委托单位
    /// </summary>
    [SugarTable("t_kyweitunit", IsDisabledDelete = true)]
    public class KyWeiTUnit
    {
        /// <summary>
        /// 科研编码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string Code { get; set; }
        /// <summary>
        /// 委托单位
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "WeiTUnit")]
        public string ZAUTHORISE { get; set; }
        /// <summary>
        /// 委托单位名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "WeiTUnitName")]
        public string? ZAUTHORISEN { get; set; }
        /// <summary>
        /// 内部/外部:1 内部/2 外部
        /// </summary>
        [SugarColumn(Length = 50, ColumnName = "Zioside")]
        public string ZIOSIDE { get; set; }
    }
    /// <summary>
    /// 项目负责人
    /// </summary>
    [SugarTable("t_kypleader", IsDisabledDelete = true)]
    public class KyPLeader
    {
        /// <summary>
        /// 科研编码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string Code { get; set; }
        /// <summary>
        /// 项目负责人
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "Pleader")]
        public string ZPRINCIPAL { get; set; }
        /// <summary>
        /// 项目负责人名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "PleaderName")]
        public string? ZPRINCIPALN { get; set; }
    }
    /// <summary>
    /// 参与部门
    /// </summary>
    [SugarTable("t_kycanydep", IsDisabledDelete = true)]
    public class KyCanYDep
    {
        /// <summary>
        /// 参与部门编号
        /// </summary>
        [SugarColumn(Length = 100, ColumnName = "CanYDep")]
        public string ZKZDEPART { get; set; }
        /// <summary>
        /// 参与部门名称
        /// </summary>
        [SugarColumn(Length = 200, ColumnName = "CanYDepName")]
        public string? ZKZDEPARTNM { get; set; }
        /// <summary>
        /// 科研编码
        /// </summary>
        [SugarColumn(Length = 100)]
        public string Code { get; set; }
    }
}
