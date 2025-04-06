using Autofac;
using Autofac.Extras.DynamicProxy;
using GDCMasterDataReceiveApi.SqlSugarCore;
using GHElectronicFileApi.AopInterceptor;
using System.Reflection;
using GDCMasterDataReceiveApi.Application.Service.ReceiveService;
using GDCMasterDataReceiveApi.Application.Contracts.IService.IReceiveService;

namespace GDCMasterDataReceiveApi.Ioc
{
    /// <summary>
    /// IOC容器初始化
    /// </summary>
    public class AutoFacInit
    {
        public static void Init(ContainerBuilder builder) {
            try
            {
                //自定义特性名称
                var attrName = "DependencyInjectionAttribute";
                //获取bin目录
                var rootPath= AppContext.BaseDirectory;
                //符合条件的接口
                Dictionary<Type,Type> interfaceList = new Dictionary<Type, Type>();
                //符合条件接口的实现
                List<Type> interfaceImplList = new List<Type>();
                //加载dll文件
                var interfaceDll= Assembly.LoadFile($"{rootPath}{Path.DirectorySeparatorChar}GDCMasterDataReceiveApi.Application.Contracts.dll").GetTypes();
                //获取所有接口
                var allInterfaceList = Array.FindAll(interfaceDll, type => type.IsInterface);
                //获取所有类
                var classDll = Assembly.LoadFile($"{rootPath}{Path.DirectorySeparatorChar}GDCMasterDataReceiveApi.Application.dll").GetTypes();
                var allClassList= Array.FindAll(classDll, type =>type.IsPublic&&type.IsClass).ToList();
                foreach (var item in allClassList)
                {
                    //该类实现所实现的所有接口
                    var allInterfaces=item.GetInterfaces();
                    foreach (var interfaceItem in allInterfaces)
                    {
                        var isImplInterface= item.GetInterface(interfaceItem.Name);
                        if (isImplInterface != null)
                        {
                            var customAttributeData = allInterfaces[0].CustomAttributes.Where(x => x.AttributeType.Name == attrName).FirstOrDefault();
                            if (customAttributeData != null&& interfaceList.Where(x=>x.Key== item).Count()==0)
                            {
                                interfaceList.Add(item, customAttributeData.GetType());
                            }
                        }
                       
                    }
                }

                if (interfaceList.Any())
                {
                    foreach (var key in interfaceList)
                    {
                        if (key.Key.Name == "ReceiveService")
                        {
                            builder.RegisterType<ReceiveServiceInterceptor>();
                            builder.RegisterType<ReceiveService>().As<IReceiveService>()
                             .InstancePerLifetimeScope()
                             .InterceptedBy(typeof(ReceiveServiceInterceptor))
                             .EnableInterfaceInterceptors();
                        }
                        else
                        {
                            builder.RegisterTypes(key.Key).AsImplementedInterfaces().InstancePerLifetimeScope();
                        }
                    
                    }
                    //注入工作单元
                    builder.RegisterType<SqlSugarUnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
                    //注入基本仓储
                    builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(Domain.IRepository.IBaseRepository<>));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("依赖注入容器初始化失败");
            }
        }
    }
}
