using Newtonsoft.Json.Linq;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UtilsSharp;

namespace GHMonitoringCenterApi.Domain.Shared.Util
{

    /// <summary>
    ///  通用工具类 
    /// </summary>
    public static class Utils
    {
        static Object locker = new object();
        #region 去除所有空格
        /// <summary>
        /// 去除所有空格
        /// </summary>
        /// <param name="str"></param>
        public static string TrimAll(this string str)
        {
            return Regex.Replace(str, @"\s", "");
        }
        #endregion

        #region 判断对象是否为空
        /// <summary>
        /// 判断对象是否为空，为空返回true
        /// </summary>
        /// <param name="data">要验证的对象</param>
        public static bool IsNullOrEmpty(object data)
        {
            //如果为null
            if (data == null)
            {
                return true;
            }

            //如果为""
            if (data.GetType() == typeof(String))
            {
                if (string.IsNullOrEmpty(data.ToString().Trim()))
                {
                    return true;
                }
            }

            //如果为DBNull
            if (data.GetType() == typeof(DBNull))
            {
                return true;
            }

            //不为空
            return false;
        }
        #endregion

        #region 判断DataTable是否为空
        /// <summary>
        /// 判断DataTable是否为空
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool IsNullDataSet(DataTable dt)
        {
            if (dt == null)
                return true;
            if (dt.Rows.Count == 0)
                return true;
            return false;
        }
        #endregion

        #region 判断List是否为空
        /// <summary>
        /// 判断List是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsListNullOrEmpty<T>(IList<T> data)
        {
            if (data == null || !data.Any())
            {
                return true;
            }
            return false;
        }
        #endregion

        #region MD5加解密相关
        public static string EncryptMD5(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(Md5Hash(sKey).Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(Md5Hash(sKey).Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string DecryptMD5(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(Md5Hash(sKey).Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(Md5Hash(sKey).Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string Md5Hash(string input)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        #endregion

        #region 验证是否是数字
        /// <summary>
        /// 验证是否是数字
        /// </summary>
        /// <param name="str_number"></param>
        /// <returns></returns>
        public static bool IsNumber(string str_number)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_number, @"^[0-9]*$");
        }
        #endregion

        #region 验证是否是身份证
        /// <summary>
        /// 验证是否是身份证
        /// </summary>
        /// <param name="str_idcard"></param>
        /// <returns></returns>
        public static bool IsIDcard(string str_idcard)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_idcard, @"(^\d{18}$)|(^\d{15}$)");
        }
        #endregion

        #region 验证是否是手机号
        /// <summary>
        /// 验证是否是手机号
        /// </summary>
        /// <param name="str_handset"></param>
        /// <returns></returns>
        public static bool IsPhone(string str_handset)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^[1]+[3,5,7,8,9]+\d{9}");
        }
        #endregion

        #region 验证是否是电话
        /// <summary>
        /// 验证是否是电话
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static bool IsTelephone(string str_telephone)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_telephone, @"^(\d{3,4}-)?\d{6,8}$");
        }
        #endregion

        #region 字符串转GUID
        /// <summary>
        /// 字符串转GUID
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Guid ToGuid(this string str)
        {
            //if (string.IsNullOrWhiteSpace(str) || str.Length != 32)
            //    throw new ArgumentException();
            //var newStr1 = str.Substring(0, 8);
            //var newStr2 = str.Substring(8, 4);
            //var newStr3 = str.Substring(12, 4);
            //var newStr4 = str.Substring(16, 4);
            //var newStr5 = str.Substring(20, 12);
            //return $"{newStr1}-{newStr2}-{newStr3}-{newStr4}-{newStr5}";
            return Guid.Parse(str);
        }
        #endregion

        #region 转驼峰命名
        /// <summary>
        /// 转驼峰命名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToPascal(this string str)
        {
            string[] split = str.Split(new char[] { '/', ' ', '_', '.' });
            string newStr = "";
            foreach (var item in split)
            {
                char[] chars = item.ToCharArray();
                chars[0] = char.ToUpper(chars[0]);
                for (int i = 1; i < chars.Length; i++)
                {
                    chars[i] = char.ToLower(chars[i]);
                }
                newStr += new string(chars);
            }
            return newStr;
        }
        #endregion

        #region 获取当前项目根节点
        /// <summary>
        /// 获取当前项目根节点
        /// </summary>
        /// <param name="isRoot">是否包含根目录</param>
        /// <param name="isSharePtah">是否包含 此路径 GHMonitoringCenterApi.Domain.Shared 默认排除</param>
        /// <returns></returns>
        public static string GetRootPath(bool isRoot = true, bool isSharePtah = false)
        {
            var rootPath = Environment.CurrentDirectory;
            if (!isRoot)
            {
                var splitAfterPath = string.Empty;
                var strArray = rootPath.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
                strArray[strArray.Length - 1] = Path.DirectorySeparatorChar.ToString();
                splitAfterPath = string.Join(Path.DirectorySeparatorChar.ToString(), strArray.Select(x => x)).TrimAll();
                if (isSharePtah)
                {
                    return Path.TrimEndingDirectorySeparator(splitAfterPath).Replace("\\", "/");
                }
                else
                {
                    return Path.TrimEndingDirectorySeparator(splitAfterPath).Replace("\\", "/");
                }
            }
            return rootPath.Replace("\\", "/");
        }
        #endregion

        #region 判断当前系统是否是Linux
        public static bool IsLinxuSystem()
        {
             return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            //var platFrom = Environment.OSVersion.Platform.ToString().ToLower();
            //if (platFrom.IndexOf("win") >= 0 || platFrom.IndexOf("windows") >= 0)
            //{
            //    return false;
            //}
            //return true;
        }
        #endregion

        #region 获取枚举描述
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (string.IsNullOrWhiteSpace(name))
                return value.ToString();

            var field = type.GetField(name);
            var des = field?.GetCustomAttribute<DescriptionAttribute>();
            if (des == null)
                return value.ToString();

            return des.Description;
        }
        #endregion

        #region 根据经纬度获取位置
        /// <summary>
        /// 根据经纬度获取位置
        /// </summary>
        /// <param name="lat">经度</param>
        /// <param name="lon">纬度</param>
        /// <returns></returns>
        public static async Task<PositionInfo> GetPositionInfoAsync(string lon, string lat)
        {
            var location = $"{lon},{lat}";
            var url = AppsettingsHelper.GetValue("GaoDeMap:GeoCodeUrl");
            var key = AppsettingsHelper.GetValue("GaoDeMap:Key");
            url = url.Replace("@key", key).Replace("@location", location);
            WebHelper webHelper = new WebHelper();
            var result = await webHelper.DoGetAsync(url);
            if (result.Code == 200 && !string.IsNullOrWhiteSpace(result.Result))
            {
                var obj = JObject.Parse(result.Result);
                var province = obj["regeocode"]["addressComponent"]["province"].ToString();
                var city = obj["regeocode"]["addressComponent"]["city"].ToString();
                var district = obj["regeocode"]["addressComponent"]["district"].ToString();
                var adcode = obj["regeocode"]["addressComponent"]["adcode"].ToString();
                PositionInfo positionInfo = new PositionInfo()
                {
                    City = city,
                    District = district,
                    AdCode = adcode,
                    Province = province,
                };
                return positionInfo;
            }
            return new PositionInfo() { };
        }
        #endregion

        #region 根据地址获取位置
        /// <summary>
        /// 根据地址获取位置
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetAddress(string address)
        {
            string province = string.Empty;
            string city = string.Empty;
            string country = string.Empty;
            string community = string.Empty;
            Dictionary<string, string> addres = new Dictionary<string, string>();
            int index = 0;

            StringBuilder stringBuilder = new StringBuilder();          //创建StringBuilder类对象
            string str = address;
            char[] array = str.ToCharArray();                //把字符串转化成字符数组
            IEnumerator enumerator = array.GetEnumerator();         //得到枚举器
            while (enumerator.MoveNext())                         //开始枚举
            {
                if ((char)enumerator.Current != ' ')         //向StringBuilder类对象添加非空格字符
                    stringBuilder.Append(enumerator.Current.ToString());
            }
            string area = stringBuilder.ToString();
            if (area.Contains("省"))
            {
                index = area.IndexOf("省");
                province = area.Substring(0, index + 1);
                addres.Add("province", province);
            }
            if (area.Contains("自治区"))
            {
                index = area.IndexOf("自治区");
                province = area.Substring(0, index + 3);
                addres.Add("province", province);
            }
            if (area.Contains("市") || area.Contains("自治州") || area.Contains("盟") || area.Contains("地区"))
            {
                if (!area.Contains("省"))
                {
                    if (area.Contains("自治区"))
                    {
                        area = area.Substring(index + 3);
                    }
                    else
                    {
                        area = area.Substring(index);
                    }
                }
                else
                {
                    area = area.Substring(index + 1);
                }
                if (area.Contains("自治州"))
                {
                    index = area.IndexOf("自治州");
                    city = area.Substring(0, index + 3);
                    addres.Add("city", city);
                    area = area.Substring(index + 3);
                    if (area.Contains("市"))
                    {
                        index = area.IndexOf("市");
                        country = area.Substring(0, index + 1);
                        addres.Add("country", country);
                    }
                }
                else if (area.Contains("盟"))
                {
                    index = area.IndexOf("盟");
                    city = area.Substring(0, index + 1);
                    addres.Add("city", city);
                    area = area.Substring(index + 1);
                    if (area.Contains("市"))
                    {
                        index = area.IndexOf("市");
                        country = area.Substring(0, index + 1);
                        addres.Add("country", country);
                    }
                }
                else if (area.Contains("地区"))
                {
                    index = area.IndexOf("地区");
                    city = area.Substring(0, index + 2);
                    addres.Add("city", city);
                    area = area.Substring(index + 2);
                }
                else
                {
                    index = area.IndexOf("市");
                    city = area.Substring(0, index + 1);
                    addres.Add("city", city);
                }

                //地址里出现2个市的时候
                int firstIndex = area.IndexOf("市");
                int firstQuIndex = area.IndexOf("区");
                int secondIndex = area.IndexOf("市", firstIndex + 1);
                if (secondIndex > 0 && firstQuIndex == -1)
                {
                    area = area.Substring(firstIndex + 1, area.Length - (firstIndex + 1));
                    country = area.Substring(0, area.IndexOf("市") + 1);
                    if (addres.ContainsKey("country") == false)
                    {
                        addres.Add("country", country);
                    }
                }
            }
            if (area.Contains("县") || area.Contains("区"))
            {
                index = area.IndexOf("市");
                area = area.Substring(index + 1);
                index = area.IndexOf("县");
                if (index == -1)
                {
                    index = area.IndexOf("区");
                    country = area.Substring(0, index + 1);
                    addres.Add("country", country);
                    index = area.IndexOf("区");
                    community = area.Substring(index + 1);
                    addres.Add("community", community);
                }
                else
                {
                    country = area.Substring(0, index + 1);
                    addres.Add("country", country);
                    index = area.LastIndexOf("县");
                    community = area.Substring(index + 1);
                    addres.Add("community", community);
                }
            }
            else
            {
                //当地址没有县区时 直接截取市后面的所有数据
                int firstIndex = area.IndexOf("市");
                if (firstIndex > 0)
                {
                    community = area.Substring(firstIndex + 1, area.Length - (firstIndex + 1));
                    addres.Add("community", community);
                }
                else
                {
                    addres.Add("community", area);
                }
            }
            return addres;
        }
        #endregion

        #region 根据分隔符分割数组
        /// <summary>
        /// 根据分隔符分割数组
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] SplitStr(this string str, string separator)
        {
            return str.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        }

        #endregion

        #region 获取一个类的所有属性
        /// <summary>
        /// 获取一个类的所有属性
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetClassFiledAttribute<T>() where T : class, new()
        {
            Dictionary<string, string> dir = new Dictionary<string, string>();
            T t = new T();
            PropertyInfo[] fieldInfos = t.GetType().GetProperties();
            string fieldComment = string.Empty;
            foreach (var item in fieldInfos)
            {

                var customAttributes = item.CustomAttributes.ToList();
                if (customAttributes.Any())
                {
                    if (customAttributes.Count == 1)
                    {
                        var name = customAttributes[0].NamedArguments;
                        if (name[0].MemberInfo.ToString().IndexOf("String", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            fieldComment = name[0].TypedValue.ToString();
                            dir.Add(item.Name, fieldComment);
                        }
                    }


                }

            }
            return dir;
        }
        #endregion

        #region 获取实体字段名称与值
        /// <summary>
        /// 获取实体字段名称与值 
        /// </summary>
        /// <returns></returns>
        public static List<EntityFieldRemark> SearchEntityFieldValue<T>(T obj) where T : class, new()
        {
            List<EntityFieldRemark> entityFields = new List<EntityFieldRemark>();
            var allProperties = obj.GetType().GetProperties();
            foreach (var item in allProperties)
            {
                entityFields.Add(new EntityFieldRemark
                {
                    Field = item.Name,
                    Name = item.GetValue(obj) == null ? "" : item.GetValue(obj).ToString()
                });
            }
            return entityFields;
        }
        #endregion

        #region 过滤SQL字符
        /// <summary>
        /// 过滤SQL字符。
        /// </summary>
        /// <param name="str">要过滤SQL字符的字符串。</param>
        /// <returns>已过滤掉SQL字符的字符串。</returns>
        public static string ReplaceSQLChar(string str)
        {
            if (str == String.Empty || str == null)
                return String.Empty;
            str = str.Replace("'", "‘");
            str = str.Replace(";", "；");
            str = str.Replace(",", ",");
            str = str.Replace("?", "?");
            str = str.Replace("<", "＜");
            str = str.Replace(">", "＞");
            str = str.Replace("(", "(");
            str = str.Replace(")", ")");
            str = str.Replace("@", "＠");
            str = str.Replace("=", "＝");
            str = str.Replace("+", "＋");
            str = str.Replace("*", "＊");
            str = str.Replace("&", "＆");
            str = str.Replace("#", "＃");
            str = str.Replace("%", "％");
            str = str.Replace("$", "￥");
            return str;
        }
        #endregion

        /// <summary>
        /// 生成项目清单编码
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>

        public static string ListEncoding(Guid projectId)
        {
            lock (locker)
            {
                string str = projectId.ToString();
                str = str.Replace("-", "");
                var currentTime = DateTime.Now.ToString("yyyymmdd");
                string listEncoding = "p" + str + currentTime;
                return listEncoding;
            }

        }

        #region 流转字节
        /// <summary>
        /// 流转字节
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
        #endregion

        #region 日期差计算
        /// <summary>
        /// 日期差计算
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int CalculateMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month;

            if (endDate.Day < startDate.Day)
            {
                monthsApart--;
            }

            return monthsApart;
        }
        #endregion

        #region 获取枚举的注释属性
        /// <summary>
        /// 枚举获取对应的注释属性
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDescription(this Enum value)
        {
            return EnumExtension.GetEnumDescription(value);
        }
        #endregion

        #region 根据选择月份 获取日期段 上月26 至 本月25
        /// <summary>
        /// 根据选择月份 获取日期段 上月26 至 本月25
        /// </summary>
        /// <param name="monthTime"></param>
        /// <param name="thisMonth">0：本月  1：上月</param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public static void GetDatePeriod(DateTime monthTime, int thisMonth, out DateTime startTime, out DateTime endTime)
        {
            var now = DateTime.Now;
            if (string.IsNullOrWhiteSpace(monthTime.ToString()) || monthTime == DateTime.MinValue)
            {
                //处理跨年的情况
                if (now.Month == 1)
                {
                    if (thisMonth == 0)
                    {
                        startTime = new DateTime(now.AddYears(-1).Year, now.AddMonths(-1).Month, 26);
                    }
                    else
                    {
                        startTime = new DateTime(now.AddYears(-1).Year, now.AddMonths(-2).Month, 26);
                    }
                }
                else
                {
                    if (thisMonth == 0)
                    {
                        startTime = new DateTime(now.Year, now.AddMonths(-1).Month, 26);
                    }
                    else
                    {
                        startTime = new DateTime(now.Year, now.AddMonths(-2).Month, 26);
                    }
                }
                if (thisMonth == 0)
                {
                    endTime = new DateTime(now.Year, now.Month, 25);
                }
                else
                {
                    endTime = new DateTime(now.Year, now.AddMonths(-1).Month, 25);
                }

            }
            else
            {
                //处理跨年的情况
                if (monthTime.Month == 1)
                {
                    startTime = new DateTime(monthTime.AddYears(-1).Year, monthTime.AddMonths(-1).Month, 26);
                }
                else
                {
                    startTime = new DateTime(monthTime.Year, monthTime.AddMonths(-1).Month, 26);
                }
                endTime = new DateTime(monthTime.Year, monthTime.Month, 25);
            }
        }
        #endregion



        #region 传入一个时间获取周期
        /// <summary>
        /// 传入一个时间获取周期
        /// </summary>
        /// <param name="time"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public static void GetDateRange(DateTime time, out int startTime, out int endTime)
        {
            if (time.Day > 26 && time.Day <= 31)
            {
                startTime = int.Parse(time.ToString("yyyyMM26"));
                endTime = int.Parse(time.AddMonths(1).ToString("yyyyMM25"));
            }
            else
            {
                startTime = int.Parse(time.AddMonths(-1).ToString("yyyyMM26"));
                endTime = int.Parse(time.ToString("yyyyMM25"));
            }

        }
        #endregion



        /// <summary>
        /// 判断文本末尾是否是括号
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool EndsWithParenthesis(string text)
        {
            // 检查是否以 '(' 或 ')' 结尾
            return text.EndsWith("(") || text.EndsWith(")") || text.EndsWith("（") || text.EndsWith("）");
        }



        /// <summary>
        /// 根据时间int类型 获取月份数据
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static int GetMonth(int day)
        {
            DateTime now;
            ConvertHelper.TryConvertDateTimeFromDateDay(day, out now);
            var startTime = string.Empty;
            if (now.Day >= 26)
            {
                startTime = now.AddMonths(1).ToString("yyyy-MM-26 00:00:00");
                //startTime = DateTime.Now.ToString("yyyy-MM-26 00:00:00");
            }
            else
            {
                startTime = now.ToString("yyyy-MM-26 00:00:00");
                //startTime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-26 00:00:00");
            }
            return Convert.ToDateTime(startTime).Month;
            //return   Convert.ToDateTime(startTime).AddMonths(1).Month;
        }
        /// <summary>
        /// 根据时间int类型 获取年份数据
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static int GetYear(int day)
        {
            DateTime now;
            ConvertHelper.TryConvertDateTimeFromDateDay(day, out now);
            var startTime = string.Empty;
            if (now.Day >= 26)
            {
                startTime = now.AddMonths(1).ToString("yyyy-MM-26 00:00:00");
            }
            else
            {
                startTime = now.ToString("yyyy-MM-26 00:00:00");
            }
            return Convert.ToDateTime(startTime).Year;
        }
    }
}
