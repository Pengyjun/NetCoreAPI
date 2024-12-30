using HNKC.CrewManagePlatform.Models.Dtos.CrewArchives;
using HNKC.CrewManagePlatform.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HNKC.CrewManagePlatform.Models.Dtos.Contract
{
    /// <summary>
    /// 续签
    /// </summary>
    public class CertificateRenewal : IValidatableObject
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
        public List<UploadResponse>? FScans { get; set; }
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
        public List<UploadResponse>? SScans { get; set; }

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
        public List<UploadResponse>? TrainingScans { get; set; }
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
        public List<UploadResponse>? HealthScans { get; set; }
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
        public List<UploadResponse>? SeamanScans { get; set; }
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
        public List<UploadResponse>? PassportScans { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(FCertificate) && FCertificate.Length > 30) yield return new ValidationResult("第一适任证书过长", new string[] { nameof(FCertificate) });
            if (!string.IsNullOrWhiteSpace(SCertificate) && SCertificate.Length > 30) yield return new ValidationResult("第二适任证书过长", new string[] { nameof(SCertificate) });
            if (!string.IsNullOrWhiteSpace(TrainingCertificate) && TrainingCertificate.Length > 30) yield return new ValidationResult("培训合格证过长", new string[] { nameof(TrainingCertificate) });
            if (!string.IsNullOrWhiteSpace(HealthCertificate) && HealthCertificate.Length > 30) yield return new ValidationResult("健康证过长", new string[] { nameof(HealthCertificate) });
            if (!string.IsNullOrWhiteSpace(SeamanCertificate) && SeamanCertificate.Length > 30) yield return new ValidationResult("海员证过长", new string[] { nameof(SeamanCertificate) });
            if (!string.IsNullOrWhiteSpace(PassportCertificate) && PassportCertificate.Length > 30) yield return new ValidationResult("护照过长", new string[] { nameof(PassportCertificate) });
        }
        #endregion
    }
}
