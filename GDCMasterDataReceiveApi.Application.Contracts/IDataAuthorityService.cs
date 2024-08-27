using GDCMasterDataReceiveApi.Application.Contracts.Dto.DataAuthority;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;

namespace GDCMasterDataReceiveApi.Application.Contracts
{
    /// <summary>
    /// 数据可查看字段接口
    /// </summary>
    [DependencyInjection]
    public interface IDataAuthorityService
    {
        /// <summary>
        /// 获取用户可查看的字段
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="rId"></param>
        /// <param name="instutionId"></param>
        /// <param name="depId"></param>
        /// <param name="pjectId"></param>
        /// <returns></returns>
        Task<DataAuthorityDto> GetDataAuthorityAsync(long uId, string rId, long instutionId, long? depId, long? pjectId);
        /// <summary>
        /// 新增或修改可授权字段（列表选择字段点击确认后使用）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="colums">,拼接的字段串</param>
        /// <param name="uId">当前操作人id 不可为空</param>
        /// <param name="rId">当前操作人角色id</param>
        /// <param name="instutionId">当前操作人机构id 不可为空</param>
        /// <param name="depId">当前操作人项目部id</param>
        /// <param name="pjectId">当前操作人项目id</param>
        /// <returns></returns>
        Task<ResponseAjaxResult<bool>> InsertOrModifyDataAuthoryAsync(long id, string? colums, long uId, string rId, long instutionId, long? depId, long? pjectId);
    }
}
