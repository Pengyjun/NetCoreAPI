using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    /// <summary>
    /// 集合帮助类
    /// </summary>
    public static class EnumerableHelper
    {

        /// <summary>
        /// 分割为二维数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">一维数组</param>
        /// <param name="maxSize">一维数组最大长度</param>
        /// <returns></returns>
        public static IList<T[]> Split<T>([NotNull] this IEnumerable<T> source, int maxSize)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (maxSize <= 0)
            {
                throw new ArgumentException("一维数组最大长度不能小于等于0", nameof(maxSize));
            }
            int sourceLength = source.Count();
            var list = new List<T[]>();
            if (sourceLength == 0)
            {
                return list;
            }
            var length = sourceLength % maxSize == 0 ? sourceLength / maxSize : (sourceLength / maxSize) + 1;
            for (int i = 1; i <= length; i++)
            {
                var size = i < length ? maxSize : sourceLength - ((i - 1) * maxSize);
                var n = new T[size];
                for (int j = 0; j < size; j++)
                {
                    n[j] = source.ElementAt((i - 1) * maxSize + j);
                }
                list.Add(n);
            }
            return list;
        }
    }
}
