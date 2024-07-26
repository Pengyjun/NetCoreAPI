using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.IRepository
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> : ISimpleClient<T> where T : class, new()
    {
    }
}
