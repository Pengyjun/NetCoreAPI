using SqlSugar;

namespace GDCMasterDataReceiveApi.Domain.Models
{
    /// <summary>
    /// 发票类型
    /// </summary>
    [SugarTable("t_invoicetype", IsDisabledDelete = true)]
    public class InvoiceType : BaseEntity<long>
    {

    }
}
