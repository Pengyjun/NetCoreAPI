using GDCMasterDataReceiveApi.Application.Contracts.Dto;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.GovernanceData;
using GDCMasterDataReceiveApi.Application.Contracts.Dto.ValueDomain;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;

namespace GDCMasterDataReceiveApi.Application.Contracts.IService.GovernanceData
{


    /// <summary>
    /// 数据治理
    /// </summary>
    [DependencyInjection]
    public interface IGovernanceDataService
    {
        /// <summary>
        /// 治理数据  1是金融机构  2是物资明细编码  3 是往来单位数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<bool> GovernanceDataAsync(int type = 1);

        #region 数据资源
        /// <summary>
        /// 获取数据资源列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<MetaDataDto>>> SearchMetaDataAsync(BaseRequestDto requestDto);
        /// <summary>
        /// 获取数据资源所有表
        /// </summary>
        /// <returns></returns>
        ResponseAjaxResult<List<Tables>> SearchTables();
        /// <summary>
        /// 保存资源列表信息
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveMetaDataAsync(MetaDataRequestDto requestDto);
        /// <summary>
        /// 获取字段类型
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ColumnsInfo>>> GetColumnsTypesAsync();
        #endregion

        #region 数据质量

        #endregion

        #region 数据标准
        /// <summary>
        /// 获取值域分类
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<ValueDomainTypeResponseDto>>> SearchValueDomainTypeAsync();
        /// <summary>
        /// 标准列表
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<DataStardardDto>>> SearchStardardAsync(DataStardardRequestDto requestDto);
        /// <summary>
        /// 保存数据标准
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> SaveStardardAsync(SaveVDomainDto requestDto);
        #endregion
    }
}
