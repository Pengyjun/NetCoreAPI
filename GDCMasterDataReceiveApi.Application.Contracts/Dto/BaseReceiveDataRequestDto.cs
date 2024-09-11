namespace GDCMasterDataReceiveApi.Application.Contracts.Dto
{
    /// <summary>
    /// 接收主数据数据 基本请求类
    /// </summary>
    public class BaseReceiveDataRequestDto<T> 
    {
        /// <summary>
        /// 
        /// </summary>
        public BusinessData<T>? IT_DATA { get; set; }
    }

    /// <summary>
    /// 业务请求接收类
    /// </summary>
    public class BusinessData<T>
    {

        /// <summary>
        /// 
        /// </summary>
        public List<T>? item { get; set; }
    }
}
