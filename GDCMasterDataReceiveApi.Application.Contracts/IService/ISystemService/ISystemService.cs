using GDCMasterDataReceiveApi.Application.Contracts.Dto.System;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Contracts.IService.ISystemService
{


    /// <summary>
    /// 系统接口层
    /// </summary>
    [DependencyInjection]
    public interface ISystemService
    {
        /// <summary>
        /// 获取所有接口方法
        /// </summary>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SystemAllInterfaceResponseDto>>> SearchInterfaceMethodsAsync(string systemIdentity);


        /// <summary>
        /// 获取接口所有返回的字段
        /// </summary>
        /// <param name="interfaceName"></param>
        /// <returns></returns>
        Task<ResponseAjaxResult<List<SystemAllInterfaceResponseDto>>> SearchInterfaceFieldsAsync(string interfaceName);
    }
}
