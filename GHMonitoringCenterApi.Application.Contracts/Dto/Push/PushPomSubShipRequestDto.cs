using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Application.Contracts.Dto.Push
{
	/// <summary>
	/// 分包船舶推送
	/// </summary>
	public class PushPomSubShipRequestDto
	{
        public string SubShipRequestJson { get; set; }
    }

	public class SubShipRequestDto
	{
		/// <summary>
		/// 船舶GUID
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// 船舶名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 船舶英文名称
		/// </summary>
		public string EnglishName { get; set; }

		/// <summary>
		/// 船舶类型GUID
		/// </summary>
		public string TypeId { get; set; }

		/// <summary>
		/// 船级社GUID
		/// </summary>
		public string ClassicId { get; set; }

		/// <summary>
		/// 船舶呼号
		/// </summary>
		public string CallSign { get; set; }

		/// <summary>
		/// 船舶MMSI号
		/// </summary>
		public string Mmsi { get; set; }

		/// <summary>
		/// 船舶IMO号
		/// </summary>
		public string Imo { get; set; }

		/// <summary>
		/// 船舶登记号码
		/// </summary>
		public string RegisterNumber { get; set; }

		/// <summary>
		/// 船舶初始登记号码
		/// </summary>
		public string FisrtRegisterNumber { get; set; }

		/// <summary>
		/// 船舶识别号
		/// </summary>
		public string IdentNumber { get; set; }

		/// <summary>
		/// 船舶卫星无线电号
		/// </summary>
		public string RadioNumber { get; set; }

		/// <summary>
		/// 船检登记号
		/// </summary>
		public string InspectNumber { get; set; }

		/// <summary>
		/// 船检证书类别
		/// </summary>
		public string InspectType { get; set; }

		/// <summary>
		/// 国籍证书类别
		/// </summary>
		public string NationType { get; set; }

		/// <summary>
		/// 自由船舶/ 外籍船舶"
		/// </summary>
		public bool Belong { get; set; }

		/// <summary>
		/// 船舶的船籍港
		/// </summary>
		public string RegistryPort { get; set; }

		/// <summary>
		/// 航行区域
		/// </summary>
		public string NavigateArea { get; set; }

		/// <summary>
		/// 施工区域
		/// </summary>
		public string ConstructionArea { get; set; }
		/// <summary>
		/// 船舶所属公司Id
		/// </summary>
		public string CompanyId { get; set; }

		/// <summary>
		/// 所属公司名称
		/// </summary>
		public string CompanyName { get; set; }

		/// <summary>
		/// 船舶状态GUID
		/// </summary>
		public string StatusId { get; set; }

		/// <summary>
		/// 设计单位
		/// </summary>
		public string Designer { get; set; }

		/// <summary>
		/// 建造单位
		/// </summary>
		public string Builder { get; set; }
		/// <summary>
		/// 船舶总长
		/// </summary>
		public decimal LengthOverall { get; set; }

		/// <summary>
		/// 两柱间长
		/// </summary>
		public string LengthBpp { get; set; }

		/// <summary>
		/// 船舶型宽
		/// </summary>
		public decimal Breadth { get; set; }

		/// <summary>
		/// 船舶型深
		/// </summary>
		public decimal Depth { get; set; }

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

		///// <summary>
		///// 空载排水量
		///// </summary>
		//[Column(TypeName = "decimal(10, 2)")]
		//public decimal? LightDisplacement { get; set; }

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
		public string Endurance { get; set; }

		///// <summary>
		///// 自由航速
		///// </summary>
		//public string Speed { get; set; }

		/// <summary>
		/// 建筑物距水面高度
		/// </summary>
		public string Height { get; set; }

		/// <summary>
		/// 出厂日期
		/// </summary>
		public string FinishDate { get; set; }

		/// <summary>
		/// 船舶描述
		/// </summary>
		public string Remarks { get; set; }
		/// <summary>
		/// 船舶经营单位
		/// </summary>
		public string ShipOperationUnit { get; set; }
		/// <summary>
		/// 创建人
		/// </summary>
		public string CreatedBy { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreatedAt { get; set; }
		/// <summary>
		/// 船舶性能参数
		/// </summary>
		public List<ShipParameter> ShipParameters { get; set; } = new List<ShipParameter> { };
	}

	public class ShipParameter
	{
		/// <summary>
		/// id
		/// </summary>
		public string Id { get; set; }
		/// <summary>
		/// 船舶Id
		/// </summary>
		public string ShipId { get; set; }
		/// <summary>
		/// 船舶类别   1 : 自有  0 分包
		/// </summary>
		public int ShipCategory { get; set; }
		/// <summary>
		/// 船舶参数名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 船舶参数编码
		/// </summary>
		public string Code { get; set; }
		/// <summary>
		/// 船舶参数类型
		/// </summary>
		public string Type { get; set; }
		/// <summary>
		/// 值
		/// </summary>
		public decimal Value { get; set; }
		/// <summary>
		/// 单位
		/// </summary>
		public string Unit { get; set; }
		/// <summary>
		/// 排序
		/// </summary>
		public int Sequence { get; set; }
		/// <summary>
		/// 备注
		/// </summary>
		public string Remarks { get; set; }
	}
}
