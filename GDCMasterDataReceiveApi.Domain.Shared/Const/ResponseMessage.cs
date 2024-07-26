using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDCMasterDataReceiveApi.Domain.Shared.Const
{
    /// <summary>
	/// 响应信息常量设置
	/// </summary>
	public class ResponseMessage
    {
        public const string SYSTEM_ERROR = "系统错误";
        public const string OPERATION_SUCCESS = "响应成功";
        public const string OPERATION_FAIL = "响应失败";
        public const string OPERATION_Token_FAIL = "Token不合法";
        public const string OPERATION_PARAMETER_ERROR = "请求参数输入错误请检查";
        public const string OPERATION_DATA_NOTEXIST = "数据不存在";
        public const string OPERATION_UPDATE_FAIL = "更新失败";
        public const string OPERATION_UPDATE_SUCCESS = "更新成功";
        public const string OPERATION_SAVE_SUCCESS = "保存成功";
        public const string OPERATION_Withdraw_SUCCESS = "撤回成功";
        public const string OPERATION_Withdraw_Fail = "撤回失败";
        public const string OPERATION_SAVE_FAIL = "保存失败";
        public const string OPERATION_DELETE_FAIL = "删除失败";
        public const string OPERATION_NOT_DATA = "无数据";
        public const string OPERATION_DELETE_SUCCESS = "删除成功";
        public const string OPERATION_INSERT_FAIL = "新增失败";
        public const string OPERATION_INSERT_SUCCESS = "新增成功";
        public const string OPERATION_UPLOAD_SUCCESS = "上传成功";
        public const string OPERATION_UPLOAD_FAIL = "上传失败";
        public const string OPERATION_AUTHORIZATION_SUCCESS = "授权成功";
        public const string OPERATION_UPLOADFILETYPE_FAIL = "上传文件类型不允许";
        public const string OPERATION_UPLOADFILESIZE_FAIL = "上传文件大小不允许";
        public const string OPERATION_HTTPCONTENTTYPE_FAIL = "ContentType必须是multipart/from-data格式";
        public const string OPERATION_NOLOGIN_FAIL = "您还没有登录请先登录";
        public const string OPERATION_NOPERMISSION_FAIL = "对不起您没有权限操作";
        public const string OPERATION_ROLE_NOTEXIST = "角色不存在";
        public const string OPERATION_COMPANY_IDENTICAL = "该公司下已存在相同项目名称";
        public const string OPERATION_SAMETYPE_IDENTICAL = "机构同种类型只能有一个";
        public const string OPERATION_APPROVALROLE_FAIL = "该机构下没有审批用户";
        public const string OPERATION_ROLE_EMPLOYED = " 已有人任职,同种职位只能有一个人任职";
        public const string OPERATION_FILE_FIRST = "请先上传文件";
        public const string OPERATION_IDENTICAL_ROLE = "该机构下已存在相同角色";
        public const string OPERATION_DEPARTMENT_USER = "该部门下已存在相同用户";
        public const string OPERATION_INSTITUTION_FAIL = "无法为该机构添加角色";
        public const string OPERATION_ASSIGNMENT_FAIL = "用户分配机构上限为10个";
        public const string OPERATION_SYSTEMROLE_EXIST = "不能为系统角色再次添加其他角色";
        public const string OPERATION_IMPORTEXCEL_SUCCESS = "导入成功";
        public const string OPERATION_IMPORTEXCEL_FAIL = "导入出错";
        public const string OPERATION_NOTSHIFT_STRUCTURE = "该结构只能移动到二级分类下";
        public const string DATA_NOTEXIST_PROJECT = "项目不存在";
        public const string DATA_NOTEXIST_DAYREPORT = "项目日报不存在";
        public const string DATA_NOTEXIST_SAFEDAYREPORT = "安监日报不存在";
        public const string DATA_NOTEXIST_SHIPDAYREPORT = "船舶日报不存在";
        public const string DATA_NOTEXIST_BUILDDAYREPORT = "施工日志不存在";
        public const string DATA_NOTEXIST_OWNERSHIP = "自有船舶不存在";
        public const string DATA_NOTEXIST_JOB = "任务不存在";
        public const string NOTALLOW_CHANGE_DAYREPORT = "项目日报不可更改";
        public const string NOTALLOW_CHANGE_SAFEDAYREPORT = "安监日报不可更改";
        public const string NOTALLOW_CHANGE_SHIPDAYREPORT = "船舶日报不可更改";
        public const string NOTALLOW_CHANGE_SHIPDYNAMICDAYREPORT = "船舶动态日报不可更改";
        public const string NOTALLOW_CHANGE_JOB = "任务不可更改";
        public const string DATA_NOTMATCH_BIZ = "数据与业务不匹配";
        public const string NOT_FIND_FILE = "找不到文件";
        public const string NOT_MAXIMUM_FILE = "最多上传{}个文件";
        public const string NOT_ACTIVATEUSER_SUCCESS = "激活登录用户成功";
        public const string NOT_ACTIVATEUSER_FILE = "激活登录用户失败";
        public const string NOT_ROLEUSER_FILE = "当前角色下面已存在{0}用户";
        public const string JJT_SendMSG_Fail = "交建通消息发送失败";
        public const string OPERATION_DEALINGUNIT_FAIL = "该条船舶已有分包商";
        public const string OPERATION_CATEGORY_FAIL = "该类别已添加";
        public const string OPERATION_RECALL_SUCCESS = "撤回消息成功";
        public const string OPERATION_RECALL_FAIL = "撤回消息失败";
        public const string DEVICE_SAVER_FAIL = "该设备已在其他项目上进场";
        public const string DEVICE_IMPORT_FAIL = "Excel表格有误";
        public const string DEVICE_TIME_FAIL = "起始时间需大于2023年8月";
        public const string EQUIPMENT_COMPANYNAME_FAIL = "公司名称有误";
        public const string EQUIPMENT_PROJECTNAME_FAIL = "项目名称有误";
        public const string EQUIPMENT_CATEGORYNAME_FAIL = "设备大类有误";
        public const string EQUIPMENT_DEVUICECLASSNAME_FAIL = "设备中类有误";
        public const string EQUIPMENT_SUBCATEGORIESNAME_FAIL = "设备小类有误";
        public const string EQUIPMENT_SHIPTYPENAME_FAIL = "船舶类型有误";
        public const string EQUIPMENT_NAVIGATIONAREANAME_FAIL = "船舶航区有误";
        public const string EQUIPMENT_REPORTINGMONTH_FAIL = "填报月份有误";
        public const string DATA_NOAUTHORITYOPERATE_FAIL = "无权限操作";
        public const string DATA_EXCELERROR_FAIL = "Excel文件有错误请检查";
        public const string DATA_NOTTIMINGTASK_FAIL = "不是定时任务发起禁止调用";

        public const string PROJECT_NOTLEADER_FAIL = "找不到项目经理人员";

        public const string PROJECT_APPROVE_SUCCESS = "审核通过";
        public const string PROJECT_Rejected_SUCCESS = "已驳回";

        public const string OPERATION_LOGINOUT_SUCCESS = "您已在其他终端退出，请重新登录";
    }
}
