namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.OtherModels
{
    /// <summary>
    /// 科研项目其他相关models
    /// </summary>
    public  class ScientifiCNoModels
    {
    }
    /// <summary>
    /// 所属二级单位
    /// </summary>
    public class IT_AI
    {
        public List<SecUnit>? Item { get; set; }
    }
    public class SecUnit
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Id {  get; set; }
        /// <summary>
        /// 所属二级单位
        /// </summary>
        public string? Z2NDORG { get; set; }
        /// <summary>
        /// 所属二级单位名称
        /// </summary>
        public string? Z2NDORGN { get; set; }
        /// <summary>
        /// 科研编码
        /// </summary>
        public string? Code { get; set; }
    }
    /// <summary>
    /// 承担单位
    /// </summary>
    public class IT_AG
    {
        public List<CDUnit>? Item { get; set; }
    }
    public class CDUnit
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 承担单位
        /// </summary>
        public string? ZUDTK { get; set; }
        /// <summary>
        /// 承担单位名称
        /// </summary>
        public string? ZUDTKN { get; set; }
        /// <summary>
        /// 内部/外部:1 内部/2 外部
        /// </summary>
        public string? ZIOSIDE { get; set; }
        /// <summary>
        /// 科研编码
        /// </summary>
        public string? Code { get; set; }
    }
    /// <summary>
    /// 曾用名  
    /// </summary>
    public class IT_ONAME
    {
        public List<NameCeng>? Item { get; set; }
    }

    public class NameCeng
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 行项目编号
        /// </summary>
        public string? ZITEM { get; set; }
        /// <summary>
        /// 曾用名
        /// </summary>
        public string? ZOLDNAME { get; set; }
        /// <summary>
        /// 科研编码
        /// </summary>
        public string? Code { get; set; }
    }
    /// <summary>
    ///  参与单位 
    /// </summary>
    public class IT_AH
    {
        public List<CanYUnit>? Item { get; set; }
    }
    public class CanYUnit
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 参与单位
        /// </summary>
        public string? ZPU { get; set; }
        /// <summary>
        /// 参与单位名称
        /// </summary>
        public string? ZPUN { get; set; }
        /// <summary>
        /// 内部/外部 :1 内部/2 外部
        /// </summary>
        public string? ZIOSIDE { get; set; }
        /// <summary>
        /// 科研编码
        /// </summary>
        public string? Code { get; set; }
    }
    /// <summary>
    ///  委托单位
    /// </summary>
    public class IT_AK
    {
        public List<WeiTUnit>? Item { get; set; }
    }
    public class WeiTUnit
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 委托单位
        /// </summary>
        public string? ZAUTHORISE { get; set; }
        /// <summary>
        /// 委托单位名称
        /// </summary>
        public string? ZAUTHORISEN { get; set; }
        /// <summary>
        /// 内部/外部:1 内部/2 外部
        /// </summary>
        public string? ZIOSIDE { get; set; }
        /// <summary>
        /// 科研编码
        /// </summary>
        public string? Code { get; set; }
    }
    /// <summary>
    ///  项目负责人
    /// </summary>
    public class IT_AJ
    {
        public List<PLeader>? Item { get; set; }
    }
    public class PLeader
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 项目负责人
        /// </summary>
        public string ZPRINCIPAL { get; set; }
        /// <summary>
        /// 项目负责人名称
        /// </summary>
        public string? ZPRINCIPALN { get; set; }
        /// <summary>
        /// 科研编码
        /// </summary>
        public string? Code { get; set; }
    }
    /// <summary>
    /// 参与部门
    /// </summary>
    public class IT_DE
    {
        public List<CanYDep>? Item { get; set; }
    }
    public class CanYDep
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// 参与部门编号
        /// </summary>
        public string? ZKZDEPART { get; set; }
        /// <summary>
        /// 参与部门名称
        /// </summary>
        public string? ZKZDEPARTNM { get; set; }
        /// <summary>
        /// 科研编码
        /// </summary>
        public string? Code { get; set; }
    }
}
