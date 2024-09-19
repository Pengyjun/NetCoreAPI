using GDCMasterDataReceiveApi.Application.Contracts.Dto.System;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISystemService;
using GDCMasterDataReceiveApi.Domain.Shared;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Application.Service.SystemService
{


    /// <summary>
    /// 接口实现层
    /// </summary>
    public class SystemService : ISystemService
    {
      

        #region 获取所有接口方法Region
        /// <summary>
        /// 获取所有接口方法
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<SystemAllInterfaceResponseDto>>> SearchInterfaceMethodsAsync()
        {
            ResponseAjaxResult<List<SystemAllInterfaceResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<SystemAllInterfaceResponseDto>>();
            List<SystemAllInterfaceResponseDto> systemAllInterfaceResponseDtos = new List<SystemAllInterfaceResponseDto>();
            //只需要筛选集合的控制器即可
            List<string>  controllers=new List<string>();
            controllers.Add("GDCMasterDataReceiveApi.Controller.SearchController");
            //获取bin目录
            var rootPath = AppContext.BaseDirectory;
            var controllerApis = Assembly.LoadFile($"{rootPath}{Path.DirectorySeparatorChar}GDCMasterDataReceiveApi.dll").GetTypes()
                .Where(x=> controllers.Contains(x.FullName)).ToList();
            foreach (var item in controllerApis)
            {
                var currentControllerMethods = ((System.Reflection.TypeInfo)((item))).DeclaredMethods;
                foreach (var method in currentControllerMethods)
                {
                    systemAllInterfaceResponseDtos.Add(new SystemAllInterfaceResponseDto() {
                      Key = method.Name,
                       Value= method.Name,
                    });
                }
            }
            responseAjaxResult.Count = systemAllInterfaceResponseDtos.Count;
            responseAjaxResult.Data = systemAllInterfaceResponseDtos;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion


        #region 获取接口所有返回的字段
        /// <summary>
        /// 获取接口所有返回的字段
        /// </summary>
        /// <param name="interfaceName"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<SystemAllInterfaceResponseDto>>> SearchInterfaceFieldsAsync(string interfaceName)
        {
            ResponseAjaxResult<List<SystemAllInterfaceResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<SystemAllInterfaceResponseDto>>();
            //只需要筛选集合的控制器即可
            List<string> controllers = new List<string>();
            controllers.Add("GDCMasterDataReceiveApi.Controller.SearchController");
            //获取bin目录
            var rootPath = AppContext.BaseDirectory;
            var controllerApis = Assembly.LoadFile($"{rootPath}{Path.DirectorySeparatorChar}GDCMasterDataReceiveApi.dll").GetTypes()
                .Where(x => controllers.Contains(x.FullName)).ToList();

            foreach (var item in controllerApis)
            {
                var currentControllerMethods = ((System.Reflection.TypeInfo)((item))).DeclaredMethods;
                foreach (var method in currentControllerMethods)
                {

                    if (method.Name == interfaceName)
                    {
                        //method.GetBaseDefinition
                    }
                }
            }

            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

    }
}
