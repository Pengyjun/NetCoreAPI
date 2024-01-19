using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Project
{
    /// <summary>
    /// 分包船舶维护信息响应
    /// </summary>
    public class SubShipUserResponseDto
    {
        /// <summary>
        /// 船舶Id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// 船舶Id
        /// </summary>
        public Guid? SubId { get; set; }
        /// <summary>
        /// 船舶名称
        /// </summary>
        public string? Name { get; set; } 

        /// <summary>
        /// 船舶类型GUID
        /// </summary>
        public Guid? TypeId { get; set; } 
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 船级社GUID
        /// </summary>
        public Guid? ClassicId { get; set; }
        /// <summary>
        /// 船级杜名称
        /// </summary>
        public string ClassicName { get; set; }

        /// <summary>
        /// 船舶呼号
        /// </summary> 
        public string? CallSign { get; set; }

        /// <summary>
        /// 船舶MMSI号
        /// </summary>
        public string? Mmsi { get; set; }  

        /// <summary>
        /// 船舶IMO号
        /// </summary>
        public string? Imo { get; set; }

        /// <summary>
        /// 船舶登记号码
        /// </summary>
        public string? RegisterNumber { get; set; }

        /// <summary>
        /// 船舶初始登记号码
        /// </summary>
        public string? FisrtRegisterNumber { get; set; }

        /// <summary>
        /// 船舶识别号
        /// </summary>
        public string? IdentNumber { get; set; }

        /// <summary>
        /// 船舶卫星无线电号
        /// </summary>
        public string? RadioNumber { get; set; }

        /// <summary>
        /// 船检登记号
        /// </summary>
        public string? InspectNumber { get; set; }

        /// <summary>
        /// 船检证书类别
        /// </summary> 
        public string? InspectType { get; set; }

        /// <summary>
        /// 国籍证书类别
        /// </summary>
        public string? NationType { get; set; }

        /// <summary>
        /// 船舶的船籍港
        /// </summary>
        public string? RegistryPort { get; set; }

        /// <summary>
        /// 航行区域
        /// </summary>
        public string? NavigateArea { get; set; }

        /// <summary>
        /// 施工区域
        /// </summary>
        public string? ConstructionArea { get; set; }

        /// <summary>
        /// 所属公司Id
        /// </summary>
          public string? CompanyId { get; set; }
        /// <summary>
        /// 所属公司名称
        /// </summary>
        public string? CompanyName { get; set; }
        /// <summary>
        /// 管理Id
        /// </summary>
       // public bool Belong { get; set; }
        /// <summary>
        /// 管理Id
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 船舶状态GUID
        /// </summary>
        public Guid? StatusId { get; set; }  
        /// <summary>
        /// 船舶状态名称
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 设计单位
        /// </summary>
        public string? Designer { get; set; }

        /// <summary>
        /// 建造单位
        /// </summary>
        public string? Builder { get; set; }

        /// <summary>
        /// 船舶总长
        /// </summary>
      public decimal? LengthOverall { get; set; }  

        /// <summary>
        /// 两柱间长
        /// </summary>
        public decimal? LengthBpp { get; set; }  

        /// <summary>
        /// 船舶型宽
        /// </summary>
        public decimal? Breadth { get; set; }  

        /// <summary>
        /// 船舶型深
        /// </summary>
        public decimal? Depth { get; set; }  

        /// <summary>
        /// 满载吃水
        /// </summary>
        public decimal? LoadedDraft { get; set; }  

        /// <summary>
        /// 空载吃水
        /// </summary>
        public decimal? LightDraft { get; set; }  

        /// <summary>
        /// 满载排水量
        /// </summary>
        public decimal? LoadedDisplacement { get; set; }  

        /// <summary>
        /// 空载排水量
        /// </summary>
        public decimal? LightDisplacement { get; set; }

        /// <summary>
        /// 总吨
        /// </summary>
        public decimal? GrossTonnage { get; set; }  

        /// <summary>
        /// 净吨
        /// </summary>
        public decimal? NetTonnage { get; set; }  

        /// <summary>
        /// 全船总功率
        /// </summary>
        public decimal? TotalPower { get; set; }  

        /// <summary>
        /// 续航力
        /// </summary>
         
        public string? Endurance { get; set; }  
        /// <summary>
        /// 自由航速
        /// </summary>
        public decimal? Speed { get; set; }

        /// <summary>
        /// 建筑物距水面高度
        /// </summary>       
        public string? Height { get; set; }

        /// <summary>
        /// 出厂日期
        /// </summary>
        public string? FinishDate { get; set; }

        /// <summary>
        /// 船舶描述
        /// </summary>       
        public string? Remarks { get; set; }

        /// <summary>
        /// 船舶经营单位
        /// </summary>         
        public string? BusinessUnit { get; set; }

        /// <summary>
        /// 是否是用户添加的船 true是  false  不是
        /// </summary>
        public bool IsUserShip { get; set; }

    }
}
