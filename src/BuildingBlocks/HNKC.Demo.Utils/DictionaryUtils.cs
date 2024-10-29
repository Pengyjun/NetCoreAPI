using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HNKC.Demo.Utils
{
    public class DictionaryUtils
    {
        /// <summary>
        /// 转Dictionary
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ConvertToDictionary(object obj)
        {
            var result = new Dictionary<string, string>();

            // 获取对象的所有公共属性
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                // 将属性的名称和值添加到字典中
                if (null != property.GetValue(obj))
                    result.Add(property.Name, property.GetValue(obj).ToString());
            }

            return result;
        }
    }
}
