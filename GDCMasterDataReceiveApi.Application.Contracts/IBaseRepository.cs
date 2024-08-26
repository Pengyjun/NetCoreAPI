using GDCMasterDataReceiveApi.Domain.Shared.Annotation;
using SqlSugar;

namespace GDCMasterDataReceiveApi.Application.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> : ISimpleClient<T> where T : class, new()
    { }
}
