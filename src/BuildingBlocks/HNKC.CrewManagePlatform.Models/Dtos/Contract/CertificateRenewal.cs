using HNKC.CrewManagePlatform.Models.Enums;

namespace HNKC.CrewManagePlatform.Models.Dtos.Contract
{
    /// <summary>
    /// 续签
    /// </summary>
    public class CertificateRenewal
    {
        /// <summary>
        /// 
        /// </summary>
        public string? BId { get; set; }
        /// <summary>
        /// 证书类型
        /// </summary>
        public CertificatesEnum Certificates { get; set; }
        /// <summary>
        /// 第一适任证
        /// 证书编号
        /// </summary>
        public string? FCertificate { get; set; }
        /// <summary>
        /// 适任航区
        /// </summary>
        public string? FNavigationArea { get; set; }
        /// <summary>
        /// 适任职务
        /// </summary>
        public string? FPosition { get; set; }
        /// <summary>
        /// 签发日期
        /// </summary>
        public DateTime? FSignTime { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        public DateTime? FEffectiveTime { get; set; }
        /// <summary>
        /// 第一适任证扫描件
        /// </summary>
        public Guid? FScans { get; set; }
        /// <summary>
        /// 第二适任证
        /// 证书编号
        /// </summary>
        public string? SCertificate { get; set; }
        /// <summary>
        /// 适任航区
        /// </summary>
        public string? SNavigationArea { get; set; }
        /// <summary>
        /// 适任职务
        /// </summary>
        public string? SPosition { get; set; }
        /// <summary>
        /// 签发日期
        /// </summary>
        public DateTime? SSignTime { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        public DateTime? SEffectiveTime { get; set; }
        /// <summary>
        /// 第二适任证扫描件
        /// </summary>
        public Guid? SScans { get; set; }

        #region 培训合格证
        /// <summary>
        /// 培训合格证
        /// 证书编号
        /// </summary>
        public string? TrainingCertificate { get; set; }
        /// <summary>
        /// 签发日期
        /// </summary>
        public DateTime? TrainingSignTime { get; set; }
        /// <summary>
        /// Z01有效日期
        /// </summary>
        public DateTime? Z01EffectiveTime { get; set; }
        /// <summary>
        /// Z07有效日期
        /// </summary>
        public DateTime? Z07EffectiveTime { get; set; }
        /// <summary>
        /// Z08有效日期
        /// </summary>
        public DateTime? Z08EffectiveTime { get; set; }
        /// <summary>
        /// Z04有效日期
        /// </summary>
        public DateTime? Z04EffectiveTime { get; set; }
        /// <summary>
        /// Z05有效日期
        /// </summary>
        public DateTime? Z05EffectiveTime { get; set; }
        /// <summary>
        /// Z02有效日期
        /// </summary>
        public DateTime? Z02EffectiveTime { get; set; }
        /// <summary>
        /// Z06有效日期
        /// </summary>
        public DateTime? Z06EffectiveTime { get; set; }
        /// <summary>
        /// Z09有效日期
        /// </summary>
        public DateTime? Z09EffectiveTime { get; set; }
        /// <summary>
        /// 扫描件 
        /// </summary>
        public Guid? TrainingScans { get; set; }
        #endregion

        #region 健康证
        /// <summary>
        /// 健康证
        /// 证书编号
        /// </summary>
        public string? HealthCertificate { get; set; }
        /// <summary>
        /// 签发日期
        /// </summary>
        public DateTime? HealthSignTime { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        public DateTime? HealthEffectiveTime { get; set; }
        /// <summary>
        /// 扫描件
        /// </summary>
        public Guid? HealthScans { get; set; }
        #endregion

        #region 海员证
        /// <summary>
        /// 海员证
        /// 证书编号
        /// </summary>
        public string? SeamanCertificate { get; set; }
        /// <summary>
        /// 签发日期
        /// </summary>
        public DateTime? SeamanSignTime { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        public DateTime? SeamanEffectiveTime { get; set; }
        /// <summary>
        /// 扫描件  
        /// </summary>
        public Guid? SeamanScans { get; set; }
        #endregion

        #region 护照
        /// <summary>
        /// 护照
        /// 证书编号
        /// </summary>
        public string? PassportCertificate { get; set; }
        /// <summary>
        /// 签发日期
        /// </summary>
        public DateTime? PassportSignTime { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        public DateTime? PassportEffectiveTime { get; set; }
        /// <summary>
        /// 扫描件 
        /// </summary>
        public Guid? PassportScans { get; set; }
        #endregion
    }
}
