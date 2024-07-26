using AutoMapper;

namespace GDCMasterDataReceiveApi.Application.Contracts.AutoMapper
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
            //mapperConfigurationExpression.CreateMap<PomCurrencyResponseDto, Currency>()
            //      .ForMember(x => x.PomId, y => y.MapFrom(u => u.Id));

        }
    }
}
