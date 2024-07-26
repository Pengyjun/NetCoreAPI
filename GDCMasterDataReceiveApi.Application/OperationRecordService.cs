using GDCMasterDataReceiveApi.Application.Contracts;
using GDCMasterDataReceiveApi.Domain.Shared;
using Microsoft.Extensions.Logging;
using SqlSugar;
using UtilsSharp;

namespace GDCMasterDataReceiveApi.Application
{


    /// <summary>
    /// 日志记录实现层
    /// </summary>
    public class OperationRecordService :CurrentUser,IOperationRecordService
    {
        #region 依赖注入

        public ILogger<OperationRecordService> logger { get; set; }
        public ISqlSugarClient dbContent { get; set; }
   
        public OperationRecordService(ILogger<OperationRecordService> logger, ISqlSugarClient dbContent)
        {
            this.logger = logger;
            this.dbContent = dbContent;
        }
        #endregion

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task WirteLogAsync(LogInfo logRequestDto)
        {
            try
            {
                var url = Path.Combine($"{AppsettingsHelper.GetValue("Log:Url")}/logapi/OperateLogs/InsLogs");
                WebHelper webHelper = new WebHelper();
                Dictionary<string, object> parames = new Dictionary<string, object>();
                parames.Add("operationType", logRequestDto.OperationType);
                parames.Add("systemLogSource", 2);
                parames.Add("businessModule", logRequestDto.BusinessModule);
                parames.Add("businessRemark", logRequestDto.BusinessRemark);
                parames.Add("operationObject", logRequestDto.OperationObject);
                parames.Add("clientIp", IpHelper.GetClientIp());
                parames.Add("deviceinformation", HttpContentAccessFactory.Current.Request.Headers["User-Agent"].ToString());
                parames.Add("operationId", GlobalCurrentUser.Id);
                parames.Add("operationName", GlobalCurrentUser.Name);
                parames.Add("operationTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                parames.Add("diffLogsDtos", logRequestDto.logDiffDtos);
                parames.Add("DataId", logRequestDto.DataId);
                parames.Add("GroupIdentity", logRequestDto.GroupIdentity);
                //不等待 
                webHelper.DoPostAsync(url, parames);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Http记录日志出现错误;");
            }

        }
    }
}
