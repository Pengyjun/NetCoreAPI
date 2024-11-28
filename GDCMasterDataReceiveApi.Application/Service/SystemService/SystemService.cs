using GDCMasterDataReceiveApi.Application.Contracts.Dto.System;
using GDCMasterDataReceiveApi.Application.Contracts.IService.ISystemService;
using GDCMasterDataReceiveApi.Domain.Models;
using GDCMasterDataReceiveApi.Domain.Shared;
using GDCMasterDataReceiveApi.Domain.Shared.Utils;
using Novell.Directory.Ldap;
using SqlSugar;
using System.Reflection;
using System.Xml;
using UtilsSharp;
namespace GDCMasterDataReceiveApi.Application.Service.SystemService
{


    /// <summary>
    /// 接口实现层
    /// </summary>
    public class SystemService : ISystemService
    {
        #region 依赖注入
        public ISqlSugarClient dbContext { get; set; }
        public SystemService(ISqlSugarClient dbContext)
        {
            this.dbContext = dbContext;
        }
        #endregion
        

        #region 获取所有接口方法
        /// <summary>
        /// 获取所有接口方法
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<SystemAllInterfaceResponseDto>>> SearchInterfaceMethodsAsync(string systemIdentity)
        {
            ResponseAjaxResult<List<SystemAllInterfaceResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<SystemAllInterfaceResponseDto>>();
            List<SystemAllInterfaceResponseDto> systemAllInterfaceResponseDtos = new List<SystemAllInterfaceResponseDto>();
            List<SystemAllInterfaceResponseDto> actionRemark = new List<SystemAllInterfaceResponseDto>();
            //主数据后台接口
            if (systemIdentity == "mdmbackstage")
            {
                //只需要筛选集合的控制器即可
                List<string> controllers = new List<string>();
                controllers.Add("GDCMasterDataReceiveApi.Controller.SearchController");
                //获取bin目录
                var rootPath = AppContext.BaseDirectory;
                var controllerApis = Assembly.LoadFile($"{rootPath}{Path.DirectorySeparatorChar}GDCMasterDataReceiveApi.dll").GetTypes()
                    .Where(x => controllers.Contains(x.FullName)).ToList();

                #region 解析swagger xml  GDCMasterDataReceiveApi.xml
                //解析swagger xml  GDCMasterDataReceiveApi.xml
                var fileContent = string.Empty;
                var stream = FileHelper.ReadFileToStream(rootPath + "GDCMasterDataReceiveApi.xml");
                using (StreamReader readSteam = new StreamReader(stream))
                {
                    fileContent = await readSteam.ReadToEndAsync();
                }
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(fileContent);
                var membersList = xmlDocument.SelectNodes("//member");
                foreach (XmlNode member in membersList)
                {
                    var attValue = member.Attributes["name"];
                    if (attValue != null)
                    {
                        var value = attValue.Value;
                        if (value != null || !string.IsNullOrWhiteSpace(value))
                        {
                            actionRemark.Add(new SystemAllInterfaceResponseDto() { Key = value, Value = member.InnerText.TrimAll() });
                        }
                    }

                }
                #endregion
                foreach (var item in controllerApis)
                {
                    var currentControllerMethods = ((System.Reflection.TypeInfo)((item))).DeclaredMethods;
                    foreach (var method in currentControllerMethods)
                    {
                        var methodRemark = actionRemark.Where(x => x.Key.Contains(method.Name)).FirstOrDefault();
                        systemAllInterfaceResponseDtos.Add(new SystemAllInterfaceResponseDto()
                        {
                            Key = method.Name,
                            Value = methodRemark.Value,
                        });
                    }
                }
            }
            else if (systemIdentity == "financedistribute")
            { 
            
            }
            else if (systemIdentity == "contractdistribute")
            {

            }
            else if (systemIdentity == "mdmdistribute")
            {

            }
            responseAjaxResult.Count = systemAllInterfaceResponseDtos.Count;
            responseAjaxResult.Data = systemAllInterfaceResponseDtos;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion

        
        #region 获取接口所有返回的字段
        /// <summary>
        /// 获取接口所有返回的字段
        /// </summary>
        /// <param name="interfaceName"></param>
        /// <returns></returns>
        public async Task<ResponseAjaxResult<List<SystemAllInterfaceResponseDto>>> SearchInterfaceFieldsAsync(string interfaceName)
        {
            ResponseAjaxResult<List<SystemAllInterfaceResponseDto>> responseAjaxResult = new ResponseAjaxResult<List<SystemAllInterfaceResponseDto>>();
            List<SystemAllInterfaceResponseDto> systemAllInterfaceResponseDtos = new List<SystemAllInterfaceResponseDto>();
            var interfaceMapper=await dbContext.Queryable<InterfaceTableMapper>().Where(x => x.IsDelete == 1&&x.InterfaceName== interfaceName).FirstAsync();
            if (interfaceMapper!=null)
            {
                var sql = $"select COLUMN_NAME AS Key,COMMENTS AS Value from user_col_comments  WHERE owner='GDCMDM' AND  Table_Name='{interfaceMapper.TableName}'";
                var columnNames=await dbContext.Ado.SqlQueryAsync< SystemAllInterfaceResponseDto>(sql);
                foreach (var item in columnNames)
                {

                    if (item.Key == "id" ||
                        item.Key == "createid" ||
                        item.Key == "deleteid" ||
                        item.Key == "timestamp" ||
                        item.Key == "isdelete" )
                    {
                        continue;
                    }
                    systemAllInterfaceResponseDtos.Add(new SystemAllInterfaceResponseDto() 
                    {
                     Key=item.Key,
                     Value=item.Value,
                    });
                }

                responseAjaxResult.Count = columnNames.Count;

            }
           
            responseAjaxResult.Data=systemAllInterfaceResponseDtos;
            responseAjaxResult.Success();
            return responseAjaxResult;
        }
        #endregion
        #region ad域
        public bool AadYu()
        {
            var domain = "10.10.55.3";              //这里也可以是ip
            var port = 389;                      //端口默认389
            var account = "432503199808205690";         //用户姓名全名，即cn属性（Common Name）
            var password = "18117214378";

            //创建实例
            using var connection = new LdapConnection();
            //连接
            connection.Connect(domain, port);
            //绑定用户（认证）
            connection.Bind(account, password);
            //得到用户的账号信息
            var res = connection.WhoAmI();
            var authzId = res.AuthzIdWithoutType;
            Console.WriteLine($"当前账号：{authzId}");
            //获取根信息
            var root = connection.GetRootDseInfo();
            //根据authzId的格式来处理一下，方便后续查询
            if (authzId.Contains("\\"))
            {
                account = authzId.Split('\\', StringSplitOptions.RemoveEmptyEntries).Last();
            }
            else if (authzId.Contains("@"))
            {
                account = authzId.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries).First();
            }
            //过滤，规则可以自行百度下，这里根据account来查询
            var filter = string.Format("(&(|(cn={0})(sAMAccountName={0}))(objectCategory=person)(objectClass=user))", account);
            var result = connection.Search(root.DefaultNamingContext, LdapConnection.ScopeSub, filter, null, false);
            //列出所有属性
            while (result.HasMore())
            {
                try
                {
                    var entry = result.Next();
                    var set = entry.GetAttributeSet();
                    Console.WriteLine($"entry: {entry.Dn}{Environment.NewLine}attr count: {set.Count}");
                    int index = 1;
                    foreach (var attr in set)
                    {
                        if (attr.StringValueArray.Length <= 1)
                        {
                            Console.WriteLine($"attr{index}: {attr.Name} = {attr.StringValue}");
                        }
                        else
                        {
                            Console.WriteLine($"attr{index}: {attr.Name} = [{string.Join(", ", attr.StringValueArray)}]");
                        }
                        index++;
                    }
                }
                catch { }
            }
            return true;
        }
        #endregion

    }
}
