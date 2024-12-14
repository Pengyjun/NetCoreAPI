using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

<<<<<<<< HEAD:src/BuildingBlocks/HNKC.CrewManagePlatform.Utils/EnumUtil.cs
namespace HNKC.CrewManagePlatform.Utils
========
namespace HNKC.Demo.Utils
>>>>>>>> 7fd224848dc4910963de00d8c3a15a3418dc1847:src/BuildingBlocks/HNKC.Demo.Utils/EnumUtil.cs
{
    public static class EnumUtil
    {
        #region FetchDescription
        /// <summary>
        /// 获取枚举值的描述文本
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FetchDescription(this Enum value)
        {
            if (value == null)
            {
                return "";
            }
            var fi = value.GetType().GetField(value.ToString());
            if (fi == null)
            {
                return "";
            }
            DescriptionAttribute[] attributes =
               (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        #endregion FetchDescription


        /// <summary>
        /// 根据Description获取枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description"></param>
        /// <returns></returns>
        public static T GetEnumName<T>(string description)
        {
            Type _type = typeof(T);
            foreach (FieldInfo field in _type.GetFields())
            {
                DescriptionAttribute[] _curDesc = field.GetDescriptAttr();
                if (_curDesc != null && _curDesc.Length > 0)
                {
                    if (_curDesc[0].Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException(string.Format("{0} 未能找到对应的枚举.", description), "Description");
        }

        /// <summary>
        /// 获取字段Description
        /// </summary>
        /// <param name="fieldInfo">FieldInfo</param>
        /// <returns>DescriptionAttribute[] </returns>
        public static DescriptionAttribute[]? GetDescriptAttr(this FieldInfo fieldInfo)
        {
            if (fieldInfo != null)
            {
                return (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            }
            return null;
        }

        public static Dictionary<string, string> EnumToDictionary<T>()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (!typeof(T).IsEnum)
            {
                return dic;
            }
            string desc = string.Empty;
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                var attrs = item.GetType().GetField(item.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    DescriptionAttribute descAttr = attrs[0] as DescriptionAttribute;
                    desc = descAttr.Description;
                }
                dic.Add(item.ToString(), desc);
            }
            return dic;
        }
    }
}
