using System.Runtime.Intrinsics.Arm;

namespace GDCMasterDataReceiveApi.Domain.Shared.Utils
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

        /// <summary>
        /// 获取当前机构下的所有子节点数据 （平级关系）
        /// </summary>
        /// <param name="printId"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public List<string> GetAllNodes(string printId, List<InstitutionTree> node)
        {
            GetTree(printId, node);
            var isExist= AllNodes.Where(x => x == printId).Count();
            if (isExist == 0)
            {
                AllNodes.Add(printId);
            }
            return AllNodes;
        }

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
        /// <summary>
        /// 当前节点下的所有节点（树形关系）
        /// </summary>
        public List<InstitutionTree> Nodes { get; set; }
    }
}
