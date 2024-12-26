using System.Runtime.Intrinsics.Arm;

namespace HNKC.CrewManagePlatform.Util
{
    /// <summary>
    /// list 转树帮助类
    /// </summary>
    public class ListToTreeUtil
    {
        /// <summary>
        /// 当前节点下的所有节点（平级关系）
        /// </summary>
        private List<string> AllNodes = new List<string>();

        #region 获取当前机构下的所有子节点数据 （平级关系）
        /// <summary>
        /// 获取当前机构下的所有子节点数据 （平级关系）
        /// </summary>
        /// <param name="printId"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public List<string> GetAllNodes(string printId, List<InstitutionTree> node)
        {
            GetTree(printId, node);
            var isExist = AllNodes.Where(x => x == printId).Count();
            if (isExist == 0)
            {
                AllNodes.Add(printId);
            }
            return AllNodes;
        }
        #endregion

        #region  获取当前机构下的所有子节点数据 （树形关系） 适用于直接关系的 没有额外的条件 
        /// <summary>
        ///获取当前机构下的所有子节点数据 （树形关系） 适用于直接关系的 没有额外的条件  
        /// </summary>
        /// <param name="printId"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public List<InstitutionTree> GetTree(string printId, List<InstitutionTree> node)
        {
            List<InstitutionTree> mainNodes = node.Where(x => x.GPoid == printId).ToList<InstitutionTree>();
            List<InstitutionTree> otherNodes = node.Where(x => x.GPoid != printId).ToList<InstitutionTree>();
            foreach (InstitutionTree dpt in mainNodes)
            {
                AllNodes.Add(dpt.Oid);
                dpt.Nodes = GetTree(dpt.Oid, otherNodes);
            }
            return mainNodes.OrderBy(x => x.Sno).ToList();
        }
        #endregion


        #region 获取菜单
        /// <summary>
        ///获取当前机构下的所有子节点数据 （树形关系） 适用于直接关系的 没有额外的条件  
        /// </summary>
        /// <param name="printId"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static List<UserMenuResponseTree> GetTree(int printId, List<UserMenuResponseTree> node)
        {
            List<UserMenuResponseTree> mainNodes = node.Where(x => x.Parentid == printId).ToList<UserMenuResponseTree>();
            List<UserMenuResponseTree> otherNodes = node.Where(x => x.Parentid != printId).ToList<UserMenuResponseTree>();
            foreach (UserMenuResponseTree dpt in mainNodes)
            {
                dpt.Nodes = GetTree(dpt.Mid, otherNodes);
            }
            return mainNodes.OrderBy(x => x.Sort).ToList();
            #endregion

        }

    }
    /// <summary>
    /// 机构树类
    /// </summary>
    public class InstitutionTree
    {
        public string Oid { get; set; }
        public string POid { get; set; }
        public string GPoid { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Sno { get; set; }
        public string Grule { get; set; }

        public Guid? BusinessId { get; set; }
        /// <summary>
        /// 当前节点下的所有节点（树形关系）
        /// </summary>
        public List<InstitutionTree> Nodes { get; set; }= new List<InstitutionTree>();
    }

    /// <summary>
    /// 菜单类
    /// </summary>
    public class UserMenuResponseTree
    {
        /// <summary>
        /// 菜单编码
        /// </summary>
        public string? MenuCode { get; set; }
        public Guid? BId { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Mid { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public int Parentid { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string? Icon { get; set; }
        /// <summary>
        /// 接口地址
        /// </summary>
        public string? Url { get; set; }
        /// <summary>
        /// 组件ID
        /// </summary>
        public string? ComponentUrl { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }
        public List<UserMenuResponseTree> Nodes { get; set; }
    }
}
