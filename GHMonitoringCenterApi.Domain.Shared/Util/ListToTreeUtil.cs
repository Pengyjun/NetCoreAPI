namespace GHMonitoringCenterApi.Domain.Shared.Util
{
    /// <summary>
    /// list 转树帮助类
    /// </summary>
    public class ListToTreeUtil
    {
        public static List<Guid> allNodes = new List<Guid>();

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

        #region WBS专用 取父级ID
        /// <summary>
        ///  WBS专用 取父级ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parentId"></param>
        /// <param name="printId"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static List<T> GetTree<T>(Guid parentId, string printId, List<T> node) where T : TreeNodeParentId<T>
        {
            List<T> mainNodes = node.Where(x => x.Pid == printId).ToList<T>();
            List<T> otherNodes = node.Where(x => x.Pid != printId).ToList<T>();
            foreach (T dpt in mainNodes)
            {
                if (parentId != Guid.Empty)
                {
                    dpt.ParentId = parentId;
                }
                dpt.Node = GetTree(dpt.Id, dpt.KeyId, otherNodes);
            }
            return mainNodes;
        }
        #endregion

        #region 导入WBS专用
        /// <summary>
        /// 导入WBS专用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parentId"></param>
        /// <param name="printId"></param>
        /// <param name="a"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static List<T> GetTree<T>(int parentId, string printId,List<T> node) where T : TreeNodeParentIds<T>
        {
            List<T> mainNodes = node.Where(x => x.Poid == printId).ToList<T>();
            List<T> otherNodes = node.Where(x => x.Poid != printId).ToList<T>();
            foreach (T dpt in mainNodes)
            {
                dpt.children = GetTree(0,dpt.SId,otherNodes);
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
        /// <returns></returns>
        public static  Tuple<List<T>,List<Guid>> GetTree<T>( string parentId, List<T> node,int length,string grule,int level=0) where T : TreeNode<T>
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
                    mainNodes = node.Where(x => x.Pid == parentId).OrderBy(x => x.Sort).ToList<T>(); ;
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


        #region 项目年初计划wbs专用
        /// <summary>
        /// 项目年初计划wbs专用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="printId"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static List<T> GetProjectPlanTree<T>(string printId, List<T> node) where T : ProjectPlanTreeNode<T>
        {
            List<T> mainNodes = node.Where(x => x.Pid == printId).ToList<T>();
            List<T> otherNodes = node.Where(x => x.Pid != printId).ToList<T>();
            foreach (T dpt in mainNodes)
            {
                dpt.Name = dpt.Name;
                dpt.Children = GetProjectPlanTree(dpt.KeyId, otherNodes);
            }
            return mainNodes;
        }
        #endregion

        #region 调整开累数wbs专用
        /// <summary>
        /// 调整开累数wbs专用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="printId"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static List<ProjectAdjustProductionValueResponseDto> GetTree(string printId, List<ProjectAdjustProductionValueResponseDto> node)  
        {
            List<ProjectAdjustProductionValueResponseDto> mainNodes = node.Where(x => x.PNodeId == printId).ToList<ProjectAdjustProductionValueResponseDto>();
            List<ProjectAdjustProductionValueResponseDto> otherNodes = node.Where(x => x.PNodeId != printId).ToList<ProjectAdjustProductionValueResponseDto>();
            foreach (ProjectAdjustProductionValueResponseDto dpt in mainNodes)
            {
                dpt.Childs = GetTree(dpt.NodeId, otherNodes);
            }
            return mainNodes;
        }
        #endregion
    }

    #region 通用类
   
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

        public Guid Id { get; set; }
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
    #endregion

    #region WBS专用类 取父级ID
    /// <summary>
    /// WBS专用类 取父级ID
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeNodeParentId<T>
    {
        public List<T> Node { get; set; }
        public TreeNodeParentId()
        {
            this.Node = new List<T>();
            this.Pid = "";

        }

        public Guid Id { get; set; }
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
        public Guid? ParentId { get; set; }

    }

    #endregion

    #region 导入WBS专用
    /// <summary>
    /// 导入WBS专用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeNodeParentIds<T>
    {
        public List<T> children { get; set; }
        public TreeNodeParentIds()
        {
            this.children = new List<T>();
            this.SId = "";

        }
        /// <summary>
        /// 父级
        /// </summary>
        public string SId { get; set; }
        /// <summary>
        /// 父级
        /// </summary>
        public string Poid { get; set; }

    }

    #endregion

    #region 项目年初计划
    /// <summary>
    /// 项目年初计划
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProjectPlanTreeNode<T>
    {
        public List<T> Children { get; set; }
        public ProjectPlanTreeNode()
        {
            this.Children = new List<T>();
            this.Pid = "";

        }

        public Guid Id { get; set; }
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
    }
    #endregion


    #region 调整项目开累数使用
    public class ProjectAdjustProductionValueResponseDto
    {
        public Guid ProjectId { get; set; }
        public Guid WbsId { get; set; }
        public string NodeId { get; set; }
        public string PNodeId { get; set; }
        public string ConstructionClassificationName { get; set; }
        public string ConstructionType { get; set; }
        public string ConstructionTypeName { get; set; }
        public string ProductionProperty { get; set; }
        public string ProductionPropertyName { get; set; }
        public string ResourceName { get; set; }

        /// <summary>
        /// 汇率
        /// </summary>
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// 原单价
        /// </summary>
        public decimal SourceUnitPrice { get; set; }
        /// <summary>
        /// 实际单价
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 原工程量
        /// </summary>
        public decimal SourceWorkQuantities { get; set; }
        /// <summary>
        /// 实际工程量
        /// </summary>
        public decimal WorkQuantities { get; set; }
        /// <summary>
        /// 原产值
        /// </summary>
        public decimal SourceProductionValue { get; set; }
        /// <summary>
        /// 实际产值
        /// </summary>
        public decimal ProductionValue { get; set; }
        /// <summary>
        /// 原外包支出
        /// </summary>
        public decimal SourceOutsourcingExpenditure { get; set; }
        /// <summary>
        /// 实际外包支出
        /// </summary>
        public decimal OutsourcingExpenditure { get; set; }

        /// <summary>
        /// 是否是新数据   如果是新增的资源 此值传1  如果是修改的此值传2   如果已经存在的此值是0
        /// </summary>
        public int IsNew { get; set; }

        public List<ProjectAdjustProductionValueResponseDto> Childs { get; set; } = new List<ProjectAdjustProductionValueResponseDto>();



        /// <summary>
        /// 递归计算
        /// </summary>
        public void CalculateSourceProductionValue()
        {
            // 产值计算
            SourceProductionValue = UnitPrice * WorkQuantities;
            //工程量计算

            //外包支出计算



            // 递归计算子节点的原产值
            foreach (var child in Childs)
            {
                child.CalculateSourceProductionValue();
            }
        }
    }
    #endregion
}
