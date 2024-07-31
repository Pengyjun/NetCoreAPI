using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 中交项目行业分类产业分类、业务板块、十二大业务类型、江河湖海对照关系
    /// </summary>
    [SugarTable("t_projectclassification", IsDisabledDelete = true)]
    public class ProjectClassification : BaseEntity<long>
    {

    }
}
