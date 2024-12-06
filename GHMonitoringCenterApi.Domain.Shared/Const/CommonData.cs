
using GHMonitoringCenterApi.Domain.Shared.Util;
using System.Net.Http.Headers;

namespace GHMonitoringCenterApi.Domain.Shared.Const
{
    /// <summary>
    /// 公用数据
    /// </summary>
    public class CommonData
    {
        //状态 中标已签 在建 在建(短暂性停工)  在建(间歇性停工)
        public const string status1 = "75089b9a-b18b-442c-bfc8-fde4024d737f," +
                                                     "cd3c6e83-1b7c-40c2-a415-5a44f13584cc," +
                                                     "19050a4b-fe95-47cf-aafe-531d5894c88b," +
                                                     "2c800da1-184f-408a-b5a4-fd915e8d6b6a";

        //状态 中标已签 在建 在建(短暂性停工)  在建(间歇性停工)
        public const string status = 
                                                     "cd3c6e83-1b7c-40c2-a415-5a44f13584cc," +
                                                     "19050a4b-fe95-47cf-aafe-531d5894c88b," +
                                                     "2c800da1-184f-408a-b5a4-fd915e8d6b6a";
        //状态 停工
        public const string status2 = "5521f60b-08f3-4f22-8573-9bd42f9267e7," +
                                                      "51b3cf5e-5148-422e-a8ad-4480bee435b6," +
                                                      "62986752-9b40-4d02-8887-b014a6ee7a9d," +
                                                      "da35777c-b351-461d-978e-860251120f51," +
                                                      "169c8afe-be81-4cbc-bfe7-61a40da0597b," +
                                                      "2134a0c1-3ab2-4c46-8ba3-a5b1ed3dd3e8," +
                                                      "94617409-8af4-455f-881f-0c9e9c3d994f," +
                                                      "af0e40cf-88f4-43c9-809f-896b147ffb13," +
                                                      "ac7a7f5b-c45b-4e43-b783-81e9c5b92526," +
                                                      "28bc58dc-41ed-4135-8628-a4e6a571032b," +
                                                      "64a241a1-66a2-4cd0-be4e-6f76a0421bb5," +
                                                      "2a30d69d-ad3a-4a51-a1d7-7b363150437d," +
                                                      "fa66f679-c749-4f25-8f1a-5e1728a219ad";
        /// <summary>
        /// 未开工状态  （中标未签、中标已签）
        /// </summary>
        public const string NotWorkStatusIds = "fa66f679-c749-4f25-8f1a-5e1728a219ad,75089b9a-b18b-442c-bfc8-fde4024d737f";
        /// <summary>
        /// 合同项目 = 在建+ 在建短暂性停工 +  在建间歇性停工+长期停工 'ef7bdb95-e802-4bf5-a7ae-9ef5205cd624'
        /// </summary>
        public const string PInStatusIds =
            "cd3c6e83-1b7c-40c2-a415-5a44f13584cc,19050a4b-fe95-47cf-aafe-531d5894c88b,2c800da1-184f-408a-b5a4-fd915e8d6b6a,169c8afe-be81-4cbc-bfe7-61a40da0597b";
        /// <summary>
        /// 合同项目 = 在建+ 在建短暂性停工 +  在建间歇性停工+长期停工 '
        /// </summary>
        public const string StatusIds =
            "cd3c6e83-1b7c-40c2-a415-5a44f13584cc,19050a4b-fe95-47cf-aafe-531d5894c88b,2c800da1-184f-408a-b5a4-fd915e8d6b6a,169c8afe-be81-4cbc-bfe7-61a40da0597b,ef7bdb95-e802-4bf5-a7ae-9ef5205cd624";
        /// <summary>
        /// 合同项目 = 在建+ 在建短暂性停工 +  在建间歇性停工+长期停工+中标未签 +中标已签
        /// </summary>
        public const string BuildIds =
            "cd3c6e83-1b7c-40c2-a415-5a44f13584cc,19050a4b-fe95-47cf-aafe-531d5894c88b,2c800da1-184f-408a-b5a4-fd915e8d6b6a,169c8afe-be81-4cbc-bfe7-61a40da0597b,fa66f679-c749-4f25-8f1a-5e1728a219ad,75089b9a-b18b-442c-bfc8-fde4024d737f";

        /// <summary>
        /// 必填项目日报的状态 = 在建+ 在建短暂性停工 +  在建间歇性停工+中标已签
        /// </summary>
        public const string DayReportIds =
            "cd3c6e83-1b7c-40c2-a415-5a44f13584cc,19050a4b-fe95-47cf-aafe-531d5894c88b,2c800da1-184f-408a-b5a4-fd915e8d6b6a,75089b9a-b18b-442c-bfc8-fde4024d737f";
        /// <summary>
        /// 合同项目 排除长期停工
        /// </summary>
        public const string PSInStatusIds = "cd3c6e83-1b7c-40c2-a415-5a44f13584cc,19050a4b-fe95-47cf-aafe-531d5894c88b,2c800da1-184f-408a-b5a4-fd915e8d6b6a";
        /// <summary>
        /// 在建 = 在建  
        /// </summary>
        public const string PConstruc = "cd3c6e83-1b7c-40c2-a415-5a44f13584cc";
        /// <summary>
        /// 停缓建 = 暂停长期停工+在建短暂性停工 +  在建间歇性停工
        /// </summary>
        public const string PSuspend = "169c8afe-be81-4cbc-bfe7-61a40da0597b,19050a4b-fe95-47cf-aafe-531d5894c88b,2c800da1-184f-408a-b5a4-fd915e8d6b6a";
        /// <summary>
        /// 耙吸船 绞吸船 抓斗船
        /// </summary>
        public const string ShipType = "06b7a5ce-e105-46c8-8b1d-24c8ba7f9dbf,f1718922-c213-4409-a59f-fdaf3d6c5e23,6959792d-27a4-4f2b-8fa4-a44222f08cb2";
        /// <summary>
        /// 耙吸船 绞吸船 抓斗船
        /// </summary>
        public static readonly Guid?[] ShipTypes = new Guid?[] { "06b7a5ce-e105-46c8-8b1d-24c8ba7f9dbf".ToGuid(), "f1718922-c213-4409-a59f-fdaf3d6c5e23".ToGuid(), "6959792d-27a4-4f2b-8fa4-a44222f08cb2".ToGuid() };
        /// <summary>
        /// 项目非施工类
        /// </summary>
        public const string PNonConstruType = "048120ae-1e9f-46d8-a38f-5d5e9e49ecba";
        /// <summary>
        /// 在建状态集合（ 中标已签, 在建, 在建(短暂性停工),  在建(间歇性停工)）
        /// </summary>
        public static readonly Guid?[] ConstructionStatus = new Guid?[] { "75089b9a-b18b-442c-bfc8-fde4024d737f".ToGuid(), "cd3c6e83-1b7c-40c2-a415-5a44f13584cc".ToGuid(), "19050a4b-fe95-47cf-aafe-531d5894c88b".ToGuid(), "2c800da1-184f-408a-b5a4-fd915e8d6b6a".ToGuid() };

        /// <summary>
        /// 项目类型-其他非施工类
        /// </summary>
        public static readonly Guid NoConstrutionProjectType = "048120ae-1e9f-46d8-a38f-5d5e9e49ecba".ToGuid();

        /// <summary>
        /// 人民币币种Id
        /// </summary>
        public static readonly Guid RMBCurrencyId = "2a0e99b4-f989-4967-b5f1-5519091d4280".ToGuid();

        /// <summary>
        /// 美元币种Id
        /// </summary>
        public static readonly Guid USACurrencyId = "edea1eb4-936f-465a-8ffa-1d1669bc3776".ToGuid();

        /// <summary>
        /// 欧元币种Id
        /// </summary>
        public static readonly Guid EuroCurrencyId = "43e52b6c-f41f-48c8-822c-103732f81cc1".ToGuid();


        //船舶日报导出 非关联的船舶项目名称为这个
        public static readonly string ShipProjectName = "非关联项目";

        /// <summary>
        /// 陈翠 账号
        /// </summary>
        public static readonly string ChenCuiGroupCode = "2016146340";

        /// <summary>
        /// 完工 交工  竣工  终止
        /// </summary>

        public static string NoStatus = "28bc58dc-41ed-4135-8628-a4e6a571032b,2a30d69d-ad3a-4a51-a1d7-7b363150437d,62986752-9b40-4d02-8887-b014a6ee7a9d,51b3cf5e-5148-422e-a8ad-4480bee435b6";



        //交建公司重点项目
        public static string jjCompanyProjectids = "08db3b35-fb38-4beb-8f31-7a1654ce60d2,08db3b35-fb38-4be9-8815-483744dec8e4,08dc31b3-f7f2-440f-8fe0-7ce613f79537,08db8665-81e2-4fdf-8099-e57bdde04e90";



        /// <summary>
        /// 只有交建公司/系统管理员/能看到这个项目  其他公司  管理员都看不到
        /// </summary>

        public Dictionary<string, string> res = new Dictionary<string, string>()
        {
            {"08dcdec4-4d90-4802-80fe-1293e55fbdff","a8db9bb0-4667-4320-b03d-b0b7f8728b61" },//项目ID,公司ID
        };




    }
}
