namespace HNKC.CrewManagePlatform.Models.Dtos
{

    /// <summary>
    /// 基本响应体
    /// </summary>
    public class BaseResponse
    {
        /// <summary>
        /// 业务ID
        /// </summary>
        public Guid? BId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Created { get; set; }
    }
    /// <summary>
    /// 基本响应体
    /// </summary>
    public class DropDownResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public Guid? Country { get; set; }
        /// <summary>
        /// 所在项目
        /// </summary>
        public string? ProjectName { get; set; }
    }
}
