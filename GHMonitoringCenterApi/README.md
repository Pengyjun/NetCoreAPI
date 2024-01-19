# 框架基本要求
1.定义请求入参出参方式一律使用DTO的方式【强制】

2.关于DTO命名规范请求参数类要以Request或RequestDto结尾,响应类要以Response或ResponseDto结尾
（建议使用RequestDto或ResponseDto结尾）【强制】

3.需要登录才能访问的接口必须加上Authorize,如果可以匿名访问的可以加上AllowAnonymous特性或者不添加也可以【强制】

4.关于模型验证建议采用类模型验证(就是请求类继承IValidatableObject接口实现Validate方法)来进行对参数的验证【强制】

5.项目自带的工具类库（DatatLakeTimePullApi.Domain.Share.Utils）以及UtilsSharp的工具类,如果没有所需要的工具类
可自行扩展像加密帮助类  http帮助类  应用程序缓存帮助类  读取配置文件帮助类 md5帮助类等都在UtilsSharp命名空间下
helper结尾的类

6.关于此项目获取token是使用CAS服务来获取的,获取和解析Token是调用远程服务来解决的

7.返回接口描述信息以及状态吗在ResponseMessage和HttpStatusCode这两个类中定义,ResponseMessage定义接口信息描述，
HttpStatusCode状态吗描述【强制】

8.实体对象映射采用AutoMapper在DatatLakeTimePullApi.Application.Contracts.AutoMapper里面手动配置（也可以才用自动配置）

9.框架采用Autofac Ioc容器来管理类的实例,Ioc容器在DatatLakeTimePullApi.Ioc文件夹里面手动配置Ioc容器(可以自动扫描配置)

10.业务接口命名IXxxService 业务接口实现类命名XxxService【强制】

11.接口都要以异步方法编写,异步方法命名都要以Async结尾

12.增删改的请求方法一律使用POST查询方法用GET【强制】

13.变量命名：数组变量名建议使用Array结尾,集合已List结尾,单个对象变量名以Single结尾.首字母小写,控制器参数变量名查询类的可以以filter结尾,或已全类名作为变量名
首字母小写

14.所有控制器必须要继承BaseController控制器【强制】

15.项目ORM框架采用Sqlsugar,使用Code Firsr模式【强制】

16.表的命名规范一律采用类名加小写前缀加T_ 例如一个用户类(User)生成表名称是t_user【强制】

17.所有表的主键没有特殊说明一律使用GUID且不能用GUID.NEWGUID要用GHMonitoringCenterApi.Domain.Shared.Util下的GuidUtil
工具类来生成【强制】

18.此框架集成了审计日志 如果不需要可以在配置文件中禁用他

19.如果是简单的功能使用仓储模式（BaseRepository）来完成 复杂功能使用db上下文的方式来完成

20.本框架集成了工作单元模式 如果需要用到事物的 只需要在action方法上打上UnitOfWork特性即可开启事务(注意不能有try cache)

21. 所有表没有特殊说明要继承BaseEntity实体

22.项目表 项目区域表 项目省份表  项目类型表 机构表等是从pom系统拉取的,这些表中的关联条件用pomId不需要用Id进行关联

# GIT 使用简单要求
master 生产环境代码 不能再此分支做任何没有测试的代码
test   测试环境
dev    开发环境  新功能开发好合并test分支就行发布测试完全没有问题合并到master主分支代码


###########目录结构描述
├── Readme.md											 //自述文件
├── GHMonitoringCenterApi								 //应用程序 
	   ├── Controllers									 //控制器层
	   ├── Filters										 //过滤器
	   ├── Ioc											 //Ioc依赖注入
	   ├── Logs											 //日志
	   ├── appsettings.json								 //应用程序配置文件
	   ├── Dockerfile								     //Dockerfile文件
	   ├── serilog.json								     //日志配置文件文件
├──GHMonitoringCenterApi.Application						 //业务逻辑层
│     ├── Service										 //业务逻辑的具体实现
├──GHMonitoringCenterApi.Application.Contracts			 //业务实现的抽象层以及数据传输
	   ├── AutoMapper									 //配置实体映射
	   ├── Dto											 //配置请求参数和响应参数的DTO	
	   ├── IService										 //业务抽象层
├── GHMonitoringCenterApi.Domain                          // 领域层定义一些基本类型
	   ├── Models										 //数据库对应的实体
├── GHMonitoringCenterApi.Domain.Share				     //领域共享层    定义一些常量   枚举  公共类
	   ├── Const											 //常量自定义
	   ├── Enums											 //枚举自定义
	   ├── Util											 //工具类自定义
├── GHMonitoringCenterApi.SqlSugarCore					 //数据访问层  定义和数据库进行交互
	  

