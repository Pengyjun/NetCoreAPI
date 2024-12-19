using System.ComponentModel;

namespace HNKC.CrewManagePlatform.Models.Enums
{
    /// <summary>
    /// 家庭关系
    /// </summary>
    public enum FamilyRelationEnum
    {
        /// <summary>
        /// 父母
        /// </summary>
        [Description("父母")]
        DadAndMom = 0,
        /// <summary>
        /// 亲人
        /// </summary>
        [Description("亲人")]
        Relative = 1,
        /// <summary>
        /// 朋友
        /// </summary>
        [Description("朋友")]
        Friend = 2,
        /// <summary>
        /// 同学
        /// </summary>
        [Description("同学")]
        Student = 3
    }
}
