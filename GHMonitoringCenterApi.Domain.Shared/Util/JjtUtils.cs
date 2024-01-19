using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsSharp;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.IO;
using System.IO.Compression;
using GHMonitoringCenterApi.Domain.Shared.Const;
using System.Net;
using GHMonitoringCenterApi.Domain.Shared.Util;

namespace CDC.MDM.Core.Common.Util
{
    /// <summary>
    /// 交建通消息通知帮助类
    /// </summary>
    public class JjtUtils
    {
        #region 获取交建通请求Token  
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public static string GetJjtToken()
        {

            var corpid = AppsettingsHelper.GetValue("JjtPushMesssage:Corpid");
            var corpsecret = AppsettingsHelper.GetValue("JjtPushMesssage:Corpsecret");
            var getJjtTokenUrl = AppsettingsHelper.GetValue("JjtPushMesssage:GetJjtTokenUrl");
            getJjtTokenUrl = getJjtTokenUrl.Replace("@corpid", corpid).Replace("@corpsecret", corpsecret);
            WebHelper webHelper = new WebHelper();
            var tokenResponse = webHelper.DoGet(getJjtTokenUrl);
            if (tokenResponse.Code == 200)
            {
                return tokenResponse.Result;
            }
            return null;
        }
        #endregion

        #region 交建通推送消息(单发或群发)
        /// <summary>
        /// 交建通推送消息(单发或群发)
        /// </summary>
        /// <param name="singleMessageTemplateRequestDto"></param>
        /// <param name="isCache">是否缓存本次发送的jobid 方便消息撤回使用  </param>
        /// <param name="cacheKey">如果isCache=true  cachekey必须不能为空</param>
        /// <returns></returns>
        public static bool SinglePushMessage(SingleMessageTemplateRequestDto  singleMessageTemplateRequestDto,bool isCache=false,string cacheKey="")
        {
            var isOpen = AppsettingsHelper.GetValue("JjtPushMesssage:IsOpen");
            try
            {
                if (isCache)
                {
                    if (string.IsNullOrWhiteSpace(cacheKey))
                    {
                        return false;
                    }
                }
                if (Convert.ToBoolean(isOpen))
                {
                    var agentid = AppsettingsHelper.GetValue("JjtPushMesssage:Agentid");
                    //var msgtype = AppsettingsHelper.GetValue("JjtPushMesssage:Msgtype");
                    var pushMessageUrl = AppsettingsHelper.GetValue("JjtPushMesssage:PushMessageUrl");
                    //var token = GetJjtToken();
                    //if (string.IsNullOrWhiteSpace(token))
                    //    return false;
                    //pushMessageUrl = pushMessageUrl.Replace("@access_token", token);
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    string messageBody = string.Empty;
                    parameters.Add("agentid", agentid);
                    if (singleMessageTemplateRequestDto.IsAll)
                    {
                        parameters.Add("touser", "@all");
                    }
                    else {
                        if (singleMessageTemplateRequestDto.MessageType == JjtMessageType.Topartys)
                        {
                            parameters.Add("toparty", string.Join("|", singleMessageTemplateRequestDto.Topartys.Select(x => x)));

                        }
                        else if (singleMessageTemplateRequestDto.MessageType == JjtMessageType.CHATID)
                        {
                            parameters.Add("chatid", singleMessageTemplateRequestDto.ChatId);

                        }
                        else {
                            if (singleMessageTemplateRequestDto.UserIds != null && singleMessageTemplateRequestDto.UserIds.Any())
                            {
                                parameters.Add("touser", string.Join("|", singleMessageTemplateRequestDto.UserIds.Select(x => x)));
                            }
                        }
                    }
                    if (singleMessageTemplateRequestDto.MessageType == JjtMessageType.TEXT)
                    {
                        parameters.Add("msgtype", JjtMessageType.TEXT);
                        messageBody = "{ \"content\":"
                         + "\"" + singleMessageTemplateRequestDto.TextContent + "\"}";
                        parameters.Add(JjtMessageType.TEXT, JsonConvert.DeserializeObject(messageBody));
                    } 
                    else if (singleMessageTemplateRequestDto.MessageType == JjtMessageType.TEXTCARD)
                    {
                        parameters.Add("msgtype", JjtMessageType.TEXTCARD);
                        parameters.Add(JjtMessageType.TEXTCARD, JsonConvert.DeserializeObject(singleMessageTemplateRequestDto.TextCardContent.ToJson().ToLower()));
                    }
                    else if (singleMessageTemplateRequestDto.MessageType == JjtMessageType.IMAGE)
                    {
                        parameters.Add("msgtype", JjtMessageType.IMAGE);
                        messageBody = "{ \"media_id\":"
                        + "\"" + singleMessageTemplateRequestDto.Mediaid + "\"}";
                        parameters.Add(JjtMessageType.IMAGE, JsonConvert.DeserializeObject(messageBody));
                    }
                    else if (singleMessageTemplateRequestDto.MessageType == JjtMessageType.CHATID|| singleMessageTemplateRequestDto.MessageType == JjtMessageType.Topartys)
                    {
                        parameters.Add("msgtype", JjtMessageType.IMAGE);
                        messageBody = "{ \"media_id\":"
                        + "\"" + singleMessageTemplateRequestDto.Mediaid + "\"}";
                        parameters.Add(JjtMessageType.IMAGE, JsonConvert.DeserializeObject(messageBody));
                        if (singleMessageTemplateRequestDto.MessageType == JjtMessageType.CHATID && !string.IsNullOrWhiteSpace(singleMessageTemplateRequestDto.TextContent))
                        {
                            parameters.Clear();
                            parameters.Add("chatid", singleMessageTemplateRequestDto.ChatId);
                            parameters.Add("msgtype", "text");
                            messageBody = "{ \"content\":"
                           + "\"" + singleMessageTemplateRequestDto.TextContent + "\"}";
                            parameters.Add(JjtMessageType.TEXT, JsonConvert.DeserializeObject(messageBody));
                        }
                    }
                    //测试群专用
                    if (singleMessageTemplateRequestDto.MessageType == JjtMessageType.Test)
                    {
                        parameters.Clear();
                        parameters.Add("chatid", singleMessageTemplateRequestDto.ChatId);
                        parameters.Add("msgtype", "text");
                        messageBody = "{ \"content\":"
                       + "\"" + singleMessageTemplateRequestDto.TextContent + "\"}";
                        parameters.Add(JjtMessageType.TEXT, JsonConvert.DeserializeObject(messageBody));
                    }

                    WebHelper webHelper = new WebHelper();
                    webHelper.Timeout = 240;
                    var pushResponse = webHelper.DoPostAsync(pushMessageUrl, parameters).GetAwaiter().GetResult();
                    if (pushResponse.Code == 200)
                    {
                        var pushInfo = JObject.Parse(pushResponse.Result);
                        if (pushInfo.ToString().IndexOf("jobid") >= 0)
                        {
                            if (isCache)
                            {
                                //缓存redis数据  方便消息撤回
                                var currentDay=DateTime.Now.ToString("yyyyMMdd");
                                var jobId = pushInfo["jobid"].ToString();
                                var sec = int.Parse(AppsettingsHelper.GetValue("JjtPushMesssage:MessageCahce"));
                                RedisUtil.Instance.SetAsync(currentDay+ cacheKey, jobId, sec);
                            }
                            return true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }
		#endregion

		#region 上传临时素材文件
        /// <summary>
        /// 上传临时素材文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        /// <param name="file"></param>
        /// <returns></returns>
		public static string DoPostFile(string url, string name, string fileName, Stream file)
		{
			string result = "";
			try
			{
				// 设置参数
				HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
				//if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
				//{
				//	ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
				//	//设置协议类型前设置协议版本
				//	request.ProtocolVersion = HttpVersion.Version11;
				//	//这里设置了协议类型。
				//	ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

				//	ServicePointManager.CheckCertificateRevocationList = true;
				//	ServicePointManager.DefaultConnectionLimit = 1000;
				//	ServicePointManager.Expect100Continue = false;
				//}
				//CookieContainer cookieContainer = new CookieContainer();
				//request.CookieContainer = cookieContainer;
				//request.AllowAutoRedirect = true;

				request.Method = "POST";
				string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
				request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
				byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
				byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

				//请求头部信息
				StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"{0}\";filename=\"{1}\"\r\nContent-Type:application/octet-stream\r\n\r\n", name, fileName));
				byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

				byte[] bArr = new byte[file.Length];
				file.Read(bArr, 0, bArr.Length);
				file.Seek(0, SeekOrigin.Begin);
				Stream postStream = request.GetRequestStream();
				postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
				postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
				postStream.Write(bArr, 0, bArr.Length);
				postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
				postStream.Close();

				try
				{
					using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
					{
						int httpStatusCode = (int)response.StatusCode;
						using (Stream responseStream = response.GetResponseStream())
						{
							using (StreamReader sr = new StreamReader(responseStream))
							{
								result = sr.ReadToEnd();
								sr.Close();
								response.Close();
							}
						}
					}
				}
				catch (WebException e)
				{
					using (WebResponse response = e.Response)
					{
						if (response == null)
						{
							return e.Message;
						}

						HttpWebResponse httpResponse = (HttpWebResponse)response;
						using (Stream responseStream = response.GetResponseStream())
						{
							using (StreamReader sr = new StreamReader(responseStream))
							{
								result = sr.ReadToEnd();
								sr.Close();
								response.Close();
								httpResponse.Close();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				result = ex.Message;
			}
			return result;
		}
		#endregion

	}

	/// <summary>
	/// 消息模板（单发或群发）
	/// </summary>
	public class SingleMessageTemplateRequestDto
    {
        /// <summary>
        /// 是否群发  交建通应用对于所有用户可见范围内均可接收
        /// </summary>
        public bool IsAll { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string? MessageType { get; set; }
        /// <summary>
        /// 用户id集合  如果选择是群发消息 此字段可为空 如果指定某个人发送填写这个人的在交建通的人资编码即可
        /// </summary>
        public List<string>? UserIds { get; set; }

        /// <summary>
        /// 推送相关部门
        /// </summary>
        public List<string>? Topartys { get; set; }
        /// <summary>
        /// 推送相关群组
        /// </summary>
        public string ChatId { get; set; }
        /// <summary>
        /// 文本消息内容  如果选择消息类型为问文本类型此内容必填  可以加html代码
        /// </summary>
        public string  TextContent { get; set; }

        /// <summary>
        /// 文本卡片消息内容  如果选择消息类型为问文本卡片类型此内容必填
        /// </summary>
        public TextCardMessageContent TextCardContent { get; set; }

        /// <summary>
        /// 图片消息 如果发送的不是图片消息 此字段可以为空
        /// </summary>
        public string Mediaid { get; set; }
    }

    /// <summary>
    /// 文本卡片消息类型
    /// </summary>
    public class TextCardMessageContent 
    {
        /// <summary>
        /// 标题  不超过128个字节，超过会自动截断  可以加html代码
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容  不超过512个字节，超过会自动截断  可以加html代码
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        ///点击后跳转的链接。选填
        /// </summary>
        public string Url { get; set; }

    }



    

}
