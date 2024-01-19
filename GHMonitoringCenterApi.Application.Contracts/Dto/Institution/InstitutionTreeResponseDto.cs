using GHMonitoringCenterApi.Domain.Shared.Util;
using Newtonsoft.Json;
using NSwag.Annotations;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Institution
{


    /// <summary>
    /// 机构组织树响应DTO  (懒加载)
    /// </summary>
    public class InstitutionTreeResponseDto:TreeNode<InstitutionTreeResponseDto>
    {
        /// <summary>
        /// PomId
        /// </summary>
         public Guid PomId { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 分组规则码
        /// </summary>
        //[JsonIgnore]
        //public string? Grule { get; set; }
        /// <summary>
        /// 机构编码
        /// </summary>
        public string Oid { get; set; }
        /// <summary>
        /// 上级机构编码
        /// </summary>
        public string Poid { get; set; }

        /// <summary>
        /// 是否存在子节点  true存在 false 不存在
        /// </summary>
        public bool IsNode { get; set; }

        public string? Sort { get; set; }
        /// <summary>
        /// 机构code
        /// </summary>
        public string? Ocode { get; set; }



    }

    ///// <summary>
    ///// 机构组织树响应DTO  (非懒加载)
    ///// </summary>
    //public class InstitutionTreeResponseDto :TreeNode<InstitutionTreeResponseDto>
    //{
    //    /// <summary>
    //    /// 主键
    //    /// </summary>
    //    public Guid Id { get; set; }
    //    /// <summary>
    //    /// 机构名称
    //    /// </summary>
    //    public string? Name { get; set; }
       
    //    /// <summary>
    //    /// 机构编码
    //    /// </summary>
    //    public string? Oid { get; set; }
    //    /// <summary>
    //    /// 上级机构编码
    //    /// </summary>
    //    public string? Poid { get; set; }

    //    public string? Sort { get; set; }
    //}
}
