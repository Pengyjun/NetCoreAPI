using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Shared
{

    /// <summary>
    /// 当前用户信息
    /// </summary>
    public class CurrentUser
    {
        /// <summary>
        /// 当前登录角色ID
        /// </summary>
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 用户账户(登录帐号)
        /// </summary>
        public string? Account { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string? Name { get; set; }

        #region 如果用户有多个角色  登录时激活的哪个角色信息
        /// <summary>
        /// 当前登录的角色ID
        /// </summary>
        public Guid CurrentLoginRoleId { get; set; }
        /// <summary>
        /// 当前登录机构ID
        /// </summary>
        public Guid CurrentLoginInstitutionId { get; set; }
        /// <summary>
        /// 当前登录机构名称
        /// </summary>
        public string CurrentLoginInstitutionName { get; set; }
        /// <summary>
        /// 当前登录机构Oid
        /// </summary>
        public string CurrentLoginInstitutionOid { get; set; }
        /// <summary>
        /// 当前登录机构父POID
        /// </summary>
        public string CurrentLoginInstitutionPoid { get; set; }
        /// <summary>
        /// 当前登录用户的部门ID
        /// </summary>
        public Guid? CurrentLoginDepartmentId { get; set; }
        /// <summary>
        /// 获取当前登录的机构分组信息
        /// </summary>
        public string CurrentLoginInstitutionGrule { get; set; }
        /// <summary>
        /// 当前登陆用户是否是超级管理员
        /// </summary>
        public bool CurrentLoginIsAdmin { get; set; }
        /// <summary>
        /// 当前用户登录角色类型
        /// </summary>
        public int CurrentLoginUserType { get; set; }
        /// <summary>
        /// 操作类型  0是全部操作  1只有查询操作   
        /// </summary>
        public int CurrentLoginOperationType { get; set; }
        #endregion

        /// <summary>
        /// 角色信息
        /// </summary>
        public List<RoleInfo>? RoleInfos { get; set; }
    }
}


#region 角色部门信息
/// <summary>
/// 角色信息
/// </summary>
public class RoleInfo
{
    /// <summary>
    /// 角色ID
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// 角色名称
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// 角色类型
    /// </summary>
    public int Type { get; set; }
    /// <summary>
    /// 是否有审批功能  true 有false没有
    /// </summary>
    public bool IsApprove { get; set; }
    /// <summary>
    /// 是否是系统管理员
    /// </summary>
    public bool IsAdmin { get; set; }
    /// <summary>
    /// 当前角色的机构ID
    /// </summary>
    public Guid InstitutionId { get; set; }
    /// <summary>
    /// 当前角色的机构oid 
    /// </summary>
    public string? Oid { get; set; }
    /// <summary>
    /// 当前角色机构的父节点Oid
    /// </summary>
    public string? Poid { get; set; }
    /// <summary>
    /// 当前机构的分组编码
    /// </summary>
    public string? Grule { get; set; }
    /// <summary>
    /// 部门信息集合
    /// </summary>
    public DepartmentInfo? DepartmentInfos { get; set; }

    /// <summary>
    /// 操作类型  0是全部操作  1只有查询操作   
    /// </summary>
    public int OperationType { get; set; }
}

/// <summary>
/// 部门信息
/// </summary>
public class DepartmentInfo
{
    /// <summary>
    /// 当前角色的部门ID
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// 当前角色ID
    /// </summary>
    public Guid RoleId { get; set; }
    /// <summary>
    /// 当前部门名称
    /// </summary>
    public string? Name { get; set; }
}

#endregion
