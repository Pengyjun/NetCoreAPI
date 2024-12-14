using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

<<<<<<<< HEAD:src/BuildingBlocks/HNKC.CrewManagePlatform.Utils/DictionaryUtils.cs
namespace HNKC.CrewManagePlatform.Utils
========
namespace HNKC.Demo.Utils
>>>>>>>> 7fd224848dc4910963de00d8c3a15a3418dc1847:src/BuildingBlocks/HNKC.Demo.Utils/DictionaryUtils.cs
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
