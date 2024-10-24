namespace GDCMasterDataReceiveApi.Domain.Shared.Utils
{
    /// <summary>
    /// list 转树帮助类
    /// </summary>
    public class ListToTreeUtil
    {
        public static List<string> allNodes = new List<string>();

        public static object obj = new object();

        #region 适用于直接关系的 没有额外的条件 
        /// <summary>
        /// 适用于直接关系的 没有额外的条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="printId"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static List<T> GetTree<T>(string printId, List<T> node) where T : TreeNode<T>
        {
            List<T> mainNodes = node.Where(x => x.Pid == printId).ToList<T>();
            List<T> otherNodes = node.Where(x => x.Pid != printId).ToList<T>();
            foreach (T dpt in mainNodes)
            {
                dpt.Node = GetTree(dpt.KeyId, otherNodes);
            }
            return mainNodes;
        }
        #endregion

        #region 此方法适用于本项目机构表的非懒加载查询
        /// <summary>
        /// 此方法适用于本项目机构表的非懒加载查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parentId"></param>
        /// <param name="node"></param>
        /// <param name="length"></param>
        /// <param name="grule"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static  Tuple<List<T>,List<string>> GetTree<T>( string parentId, List<T> node,int length,string grule,int level=0) where T : TreeNode<T>
        {
            lock (obj)
            {
                obj = new object();
                if (level == 0)
                { 
                 allNodes.Clear();
                }
                List<T> mainNodes = null;
                if (parentId == "101114066")
                {
                    mainNodes = node.Where(x => x.Pid == parentId).OrderBy(x => x.Sort).ToList<T>(); 
                }
                else
                {
                    mainNodes = node.Where(x => x.Grule.Contains(grule) && x.Grule.Split("-", StringSplitOptions.RemoveEmptyEntries).Length == length)
                        .OrderByDescending(x => x.Sort).ToList<T>();
                }
                if (level == 1)
                {
                    //mainNodes = mainNodes.Where(x => x.KeyId == oid).ToList();
                }
                level += 1;
                List<T> otherNodes = node.Where(x => x.Grule.Split("-", StringSplitOptions.RemoveEmptyEntries).Length >= length).ToList<T>();
                foreach (T dpt in mainNodes)
                {
                    var array = dpt.Grule.Split("-", StringSplitOptions.RemoveEmptyEntries);
                    var len = array.Length + 1;
                    allNodes.Add(dpt.Id);
                    dpt.Node = GetTree(array[len - 2], otherNodes, len, dpt.Grule, level).Item1;
                }
                return Tuple.Create(mainNodes,allNodes);
            }
        }
        #endregion

    }

    /// <summary>
    /// 通用类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeNode<T>
    {
        public List<T> Node { get; set; }
        public TreeNode()
        {
            this.Node = new List<T>();
            this.Pid = "";
           
        }

        public string Id { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string KeyId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 父级
        /// </summary>
        public string Pid { get; set; }
        /// <summary>
        /// 本项目机构表专用字段如果不是机构表此字段可忽略
        /// </summary>
        public string Grule { get; set; }

        public int? Sort { get; set; }
        public string? ParentId { get; set; }
    }
}
