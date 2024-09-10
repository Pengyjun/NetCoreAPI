namespace GDCMasterDataReceiveApi.Application.Contracts.Dto
{
    /// <summary>
    /// 接收主数据数据 基本请求类
    /// </summary>
    public class BaseReceiveDataRequestDto<T> where T : new()
    {
        public IT_DATA<T> IT_DATA { get; set; }
    }

    /// <summary>
    /// 业务请求接收类
    /// </summary>
    public class IT_DATA<T> where T : new()
    {
        public List<T> item { get; set; }
    }
}
