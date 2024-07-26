using GDCMasterDataReceiveApi.Domain.IRepository;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.SqlSugarCore
{
    /// <summary>
    /// 基本仓储层  业务层需要对数据库简单操作的情况建议使用仓储 复杂功能还是用db.xx.xxx功能来完成
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseRepository<T> : SimpleClient<T>, IBaseRepository<T> where T : class, new()
    {
        public ISqlSugarClient sqlSugarClient { get; set; }
        public BaseRepository(ISqlSugarClient sqlSugarClient)
        {
            Context = sqlSugarClient;
        }


        #region 扩展仓储方法   如果仓储方法不满足的情况下 可以扩展  一般情况下 不需要扩展自带的方法足够开发
        //自定义扩展方法
        #endregion
    }
}
