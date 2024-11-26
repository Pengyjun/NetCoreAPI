namespace GDCMasterDataReceiveApi.Application.Contracts.Dto.GovernanceData
{

    /// <summary>
    /// 数据质量dto
    /// </summary>
    public class DataQualityDto
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public class DataQualityRequestDto
    {
        /// <summary>
        /// 1完整性 2唯一性
        /// </summary>
        public int Type {  get; set; }
    }
}
