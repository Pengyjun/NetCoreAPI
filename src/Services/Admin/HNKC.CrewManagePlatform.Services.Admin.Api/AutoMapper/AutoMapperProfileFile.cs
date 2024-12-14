using AutoMapper;
using HNKC.CrewManagePlatform.Models.Dtos.Salary;
using HNKC.CrewManagePlatform.SqlSugars.Models;
using System;

namespace HNKC.CrewManagePlatform.Services.Admin.Api.AutoMapper
{
    /// <summary>
    /// 映射
    /// </summary>
    public class AutoMapperProfileFile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapperConfigurationExpression"></param>
        public static void AutoMapperProfileInit(IMapperConfigurationExpression mapperConfigurationExpression)
        {
            //多组织行政机构
            mapperConfigurationExpression.CreateMap<SalaryAsExcelResponse, Salary>();
            mapperConfigurationExpression.CreateMap<HNKC.CrewManagePlatform.SqlSugars.Models.Salary, SalaryAsExcelResponse>();
        }
       
    }
}
