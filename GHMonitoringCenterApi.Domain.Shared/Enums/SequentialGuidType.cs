using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Shared.Enums
{
    public enum SequentialGuidType
    {
        /// <summary>
        /// <para>dddddddd-dddd-Mddd-Ndrr-rrrrrrrrrrrr</para>
        /// <para>用于MySql和PostgreSql</para>
        /// <para>当使用<see cref="Guid.ToString()"/>方法进行格式化时连续</para>
        /// <para>顺序体现在第8个字节</para>
        /// </summary>
        AsString,
        /// <summary>
        /// <para>dddddddd-dddd-Mddd-Ndrr-rrrrrrrrrrrr</para>
        /// <para>用于Oracle</para>
        /// <para>当使用<see cref="Guid.ToByteArray()"/>方法进行格式化时连续</para>
        /// <para>顺序体现在第8个字节，连续递增</para>
        /// </summary>
        AsBinary,
        /// <summary>
        /// <para>rrrrrrrr-rrrr-Mxdr-Nddd-dddddddddddd</para>
        /// <para>用于SqlServer</para>
        /// <para>连续性体现于GUID的第4块（Data4）</para>
        /// <para>顺序比较Block5 > Block4 > Block3 > Block2 > Block1</para>
        /// </summary>
        AsEnd,
    }
}
