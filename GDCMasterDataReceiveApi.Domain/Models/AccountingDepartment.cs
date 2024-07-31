using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 核算部门
    /// </summary>
    [SugarTable("t_accountingdepartment", IsDisabledDelete = true)]
    public class AccountingDepartment : BaseEntity<long>
    {

    }
}
