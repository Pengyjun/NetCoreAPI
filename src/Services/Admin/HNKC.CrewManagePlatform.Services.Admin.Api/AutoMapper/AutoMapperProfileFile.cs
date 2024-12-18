using AutoMapper;
using HNKC.CrewManagePlatform.Models.Dtos.PullResult;
using HNKC.CrewManagePlatform.Models.Dtos.Salary;
using HNKC.CrewManagePlatform.SqlSugars.Models;

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
            mapperConfigurationExpression.CreateMap<SalaryAsExcelResponse, Salary>();
            mapperConfigurationExpression.CreateMap<HNKC.CrewManagePlatform.SqlSugars.Models.Salary, SalaryAsExcelResponse>();
            mapperConfigurationExpression.CreateMap<ValueDomainDto, HNKC.CrewManagePlatform.SqlSugars.Models.ValueDomain>();

        }

    }
}
