using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Domain.Shared
{

    /// <summary>
    /// 静态数据
    /// </summary>
    public class CommonData
    {
        public Dictionary<string,string> companyList = new Dictionary<string, string>();
        /// <summary>
        /// 初始化公司静态数据
        /// </summary>
        public Dictionary<string, string> InitCompany() {
            companyList.Add("101341960", "疏浚公司");//疏浚公司
            companyList.Add("101174265", "交建公司");//交建公司
            companyList.Add("101174254", "三公司");//三公司
            companyList.Add("101332050", "四公司");//四公司
            companyList.Add("101305005", "五公司");//五公司
            companyList.Add("101288132", "福建公司");//福建公司
            companyList.Add("101162350", "广航局");//广航局
            return companyList;
        }
    }
}
