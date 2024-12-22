using SqlSugar;

namespace HNKC.CrewManagePlatform.SqlSugars.Models
{
    /// <summary>
    /// 适任及证书
    /// </summary>
    [SugarTable("t_certificateofcompetency", IsDisabledDelete = true, TableDescription = "适任及证书")]
    public class CertificateOfCompetency : BaseEntity<long>
    {
        /// <summary>
        /// 第一适任证
        /// 证书编号
        /// </summary>
        [SugarColumn(Length = 50, ColumnDescription = "第一适任证证书编号")]
        public string? FCertificate { get; set; }
        /// <summary>
        /// 适任航区
        /// </summary>
        [SugarColumn(Length = 200, ColumnDescription = "第一适任证适任航区")]
        public string? FNavigationArea { get; set; }
        /// <summary>
        /// 适任职务
        /// </summary>
        [SugarColumn(Length = 200, ColumnDescription = "第一适任证适任职务")]
        public string? FPosition { get; set; }
        /// <summary>
        /// 签发日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "第一适任证签发日期")]
        public DateTime? FSignTime { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "第一适任证有效日期")]
        public DateTime? FEffectiveTime { get; set; }
        /// <summary>
        /// 第一适任证扫描件
        /// </summary>
        [SugarColumn(ColumnDataType = "text", ColumnDescription = "第一适任证扫描件")]
        public Guid? FScans { get; set; }
        /// <summary>
        /// 第二适任证
        /// 证书编号
        /// </summary>
        [SugarColumn(Length = 50, ColumnDescription = "第二适任证证书编号")]
        public string? SCertificate { get; set; }
        /// <summary>
        /// 适任航区
        /// </summary>
        [SugarColumn(Length = 200, ColumnDescription = "第二适任证适任航区")]
        public string? SNavigationArea { get; set; }
        /// <summary>
        /// 适任职务
        /// </summary>
        [SugarColumn(Length = 100, ColumnDescription = "第二适任证适任职务")]
        public string? SPosition { get; set; }
        /// <summary>
        /// 签发日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "第二适任证签发日期")]
        public DateTime? SSignTime { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "第二适任证有效日期")]
        public DateTime? SEffectiveTime { get; set; }
        /// <summary>
        /// 第二适任证扫描件
        /// </summary>
        [SugarColumn(ColumnDataType = "text", ColumnDescription = "第二适任证扫描件")]
        public Guid? SScans { get; set; }

        #region 培训合格证
        /// <summary>
        /// 培训合格证
        /// 证书编号
        /// </summary>
        [SugarColumn(Length = 50, ColumnDescription = "培训合格证证书编号")]
        public string? TrainingCertificate { get; set; }
        /// <summary>
        /// 签发日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "培训合格证签发日期")]
        public DateTime? TrainingSignTime { get; set; }
        /// <summary>
        /// Z01有效日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "Z01有效日期")]
        public DateTime? Z01EffectiveTime { get; set; }
        /// <summary>
        /// Z07有效日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "Z07有效日期")]
        public DateTime? Z07EffectiveTime { get; set; }
        /// <summary>
        /// Z08有效日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "Z08有效日期")]
        public DateTime? Z08EffectiveTime { get; set; }
        /// <summary>
        /// Z04有效日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "Z04有效日期")]
        public DateTime? Z04EffectiveTime { get; set; }
        /// <summary>
        /// Z05有效日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "Z05有效日期")]
        public DateTime? Z05EffectiveTime { get; set; }
        /// <summary>
        /// Z02有效日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "Z02有效日期")]
        public DateTime? Z02EffectiveTime { get; set; }
        /// <summary>
        /// Z06有效日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "Z06有效日期")]
        public DateTime? Z06EffectiveTime { get; set; }
        /// <summary>
        /// Z09有效日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "Z09有效日期")]
        public DateTime? Z09EffectiveTime { get; set; }
        /// <summary>
        /// 扫描件 
        /// </summary>
        [SugarColumn(ColumnDataType = "text", ColumnDescription = "培训合格证扫描件")]
        public Guid? TrainingScans { get; set; }
        #endregion

        #region 健康证
        /// <summary>
        /// 健康证
        /// 证书编号
        /// </summary>
        [SugarColumn(Length = 50, ColumnDescription = "健康证证书编号")]
        public string? HealthCertificate { get; set; }
        /// <summary>
        /// 签发日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "健康证签发日期")]
        public DateTime? HealthSignTime { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "健康证有效日期")]
        public DateTime? HealthEffectiveTime { get; set; }
        /// <summary>
        /// 扫描件
        /// </summary>
        [SugarColumn(ColumnDataType = "text", ColumnDescription = "健康证扫描件")]
        public Guid? HealthScans { get; set; }
        #endregion

        #region 海员证
        /// <summary>
        /// 海员证
        /// 证书编号
        /// </summary>
        [SugarColumn(Length = 50, ColumnDescription = "海员证")]
        public string? SeamanCertificate { get; set; }
        /// <summary>
        /// 签发日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "海员证签发日期")]
        public DateTime? SeamanSignTime { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "海员证有效日期")]
        public DateTime? SeamanEffectiveTime { get; set; }
        /// <summary>
        /// 扫描件  
        /// </summary>
        [SugarColumn(ColumnDataType = "text", ColumnDescription = "海员证扫描件")]
        public Guid? SeamanScans { get; set; }
        #endregion

        #region 护照
        /// <summary>
        /// 护照
        /// 证书编号
        /// </summary>
        [SugarColumn(Length = 50, ColumnDescription = "护照证书编号")]
        public string? PassportCertificate { get; set; }
        /// <summary>
        /// 签发日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "护照签发日期")]
        public DateTime? PassportSignTime { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime", ColumnDescription = "护照有效日期")]
        public DateTime? PassportEffectiveTime { get; set; }
        /// <summary>
        /// 扫描件 
        /// </summary>
        [SugarColumn(ColumnDataType = "text", ColumnDescription = "护照扫描件")]
        public Guid? PassportScans { get; set; }
        #endregion
    }
}
