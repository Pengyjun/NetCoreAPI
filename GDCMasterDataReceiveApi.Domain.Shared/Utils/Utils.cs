using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO.Compression;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UtilsSharp;

namespace GDCMasterDataReceiveApi.Domain.Shared.Utils
{
    /// <summary>
    ///  通用工具类 
    /// </summary>
    public static class Utils
    {
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
        /// <param name="isSharePtah">是否包含自身路径  默认排除</param>
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
            var platFrom = Environment.OSVersion.Platform.ToString().ToLower();
            if (platFrom.IndexOf("win") >= 0 || platFrom.IndexOf("windows") >= 0)
            {
                return false;
            }
            return true;
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
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
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
            else return string.Empty;
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
        //public static List<EntityFieldRemark> SearchEntityFieldValue<T>(T obj) where T : class, new()
        //{
        //    List<EntityFieldRemark> entityFields = new List<EntityFieldRemark>();
        //    var allProperties = obj.GetType().GetProperties();
        //    foreach (var item in allProperties)
        //    {
        //        entityFields.Add(new EntityFieldRemark
        //        {
        //            Field = item.Name,
        //            Name = item.GetValue(obj) == null ? "" : item.GetValue(obj).ToString()
        //        });
        //    }
        //    return entityFields;
        //}
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

        #region 获取时间戳
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeSpan()
        {
            long timeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            return timeStamp;
        }
        #endregion

        #region 压缩指定文件夹  Linux系统有问题
        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="sourceDirectory">y源文件路径</param>
        /// <param name="fileOut">输出</param>
        public static void DirectoryZip(string sourceDirectory, string fileOut)
        {
            string[] allFiles = Directory.GetFiles(sourceDirectory, "", SearchOption.AllDirectories);
            using (FileStream zipFileToOpen = new FileStream(fileOut, FileMode.OpenOrCreate))
            {
                using (ZipArchive archive = new ZipArchive(zipFileToOpen, ZipArchiveMode.Update))
                {
                    foreach (var file in allFiles)
                    {
                        //获取压缩文件相对目录
                        string fileName = file.Replace(sourceDirectory + Path.DirectorySeparatorChar, "");
                        ZipFile(file, fileName, archive);
                    }
                }
            }
        }
        private static void ZipFile(string fileSource, string fileName, ZipArchive archive)
        {
            ZipArchiveEntry readMeEntry = archive.CreateEntry(fileName);
            readMeEntry.LastWriteTime = File.GetLastWriteTime(fileSource);
            using (Stream stream = readMeEntry.Open())
            {
                byte[] bytes = File.ReadAllBytes(fileSource);
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        #endregion

        #region 压缩指定文件夹 Linux版本
        /// <summary>
        /// 压缩指定文件夹 Linux版本
        /// </summary>
        /// <param name="cmd"></param>
        public static string LinuxZipUtil(string cmd)
        {
            string result = string.Empty;
            using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
            {
                proc.StartInfo.FileName = "/bin/bash";
                proc.StartInfo.Arguments = "-c \" " + cmd + " \"";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();
                result += proc.StandardOutput.ReadToEnd();
                result += proc.StandardError.ReadToEnd();
                proc.WaitForExit();
            }
            return result;
        }
        #endregion

        #region 10位转换成年月日小时
        /// <summary>
        /// 10位转换成年月日小时
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime ParseDatetime(string input)
        {
            if (input.Length != 10) { throw new ArgumentException("Input string must be exactly 10 characters long."); }
            int year = int.Parse(input.Substring(0, 4));
            int month = int.Parse(input.Substring(4, 2));
            int day = int.Parse(input.Substring(6, 2));
            int hour = int.Parse(input.Substring(8, 2));
            return new DateTime(year, month, day, hour, 0, 0);
        }
        #endregion

        /// <summary>
        /// 转换成日期Day格式，固定8位数（例：20230401）
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static int ToDateDay(this DateTime dateTime)
        {
            return int.Parse(dateTime.ToString("yyyyMMdd"));
        }

        /// <summary>
        /// 转换成月份Month格式，固定8位数（例：202304）
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static int ToDateMonth(this DateTime dateTime)
        {
            return int.Parse(dateTime.ToString("yyyyMM"));
        }

        /// <summary>
        /// 转换成年份格式，固定4位数（例：2023）
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static int ToDateYear(this DateTime dateTime)
        {
            return dateTime.Year;
        }

        /// <summary>
        /// 转换成日期格式（例：20230406，转换成2023-04-06 00:00:00）
        /// </summary>
        /// <returns></returns>
        public static bool TryConvertDateTimeFromDateDay(int dateDay, out DateTime dayTime)
        {
            var dateDayStr = dateDay.ToString();
            if (dateDayStr.Length != 8)
            {
                dayTime = DateTime.MinValue;
                return false;
            }
            var year = dateDayStr.Substring(0, 4);
            var month = dateDayStr.Substring(4, 2);
            var day = dateDayStr.Substring(6, 2);
            return DateTime.TryParse($"{year}-{month}-{day}", out dayTime);
        }

        /// <summary>
        /// 转换成月份格式时间（例：202304，转换成2023-04-01 00:00:00）
        /// </summary>
        /// <returns></returns>
        public static bool TryParseFromDateMonth(int dateDay, out DateTime monthTime)
        {
            var dateDayStr = dateDay.ToString();
            if (dateDayStr.Length != 6)
            {
                monthTime = DateTime.MinValue;
                return false;
            }
            var year = dateDayStr.Substring(0, 4);
            var month = dateDayStr.Substring(4, 2);
            var day = 1;
            return DateTime.TryParse($"{year}-{month}-{day}", out monthTime);
        }
        /// <summary>
        /// 年月日 时分秒
        /// </summary>
        /// <param name="dateDay"></param>
        /// <param name="dayTime"></param>
        /// <returns></returns>
        public static bool TryConvertDateTimeFromDateDayH(int dateDay, out DateTime dayTime)
        {
            var dateDayStr = dateDay.ToString();
            if (dateDayStr.Length != 14)
            {
                dayTime = DateTime.MinValue;
                return false;
            }
            var year = dateDayStr.Substring(0, 4);
            var month = dateDayStr.Substring(4, 2);
            var day = dateDayStr.Substring(6, 2);
            var hh = dateDayStr.Substring(8, 2);
            var mm = dateDayStr.Substring(10, 2);
            var ss = dateDayStr.Substring(12, 2);
            return DateTime.TryParse($"{year}-{month}-{day} {hh} {mm} {ss}", out dayTime);
        }

        /// <summary>
        /// 转换成年份份格式时间（例：202304，转换成2023-01-01 00:00:00）
        /// </summary>
        /// <returns></returns>
        public static bool TryParseFromDateYear(int dateYear, out DateTime yearTime)
        {
            var dateYearStr = dateYear.ToString();
            if (dateYearStr.Length != 4)
            {
                yearTime = DateTime.MinValue;
                return false;
            }
            var year = dateYear;
            var month = 1;
            var day = 1;
            return DateTime.TryParse($"{year}-{month}-{day}", out yearTime);
        }

        /// <summary>
        /// 转换成农历日期
        /// </summary>
        /// <returns></returns>
        public static DateTime ToChineseDate(this DateTime date)
        {
            var calendar = new ChineseLunisolarCalendar();
            var year = calendar.GetYear(date);
            // 是否有闰月,返回正整数（比如2023年闰2月，返回值为3）
            int flag = calendar.GetLeapMonth(year);
            //有闰月则实际月份减1
            int month = flag > 0 ? calendar.GetMonth(date) - 1 : calendar.GetMonth(date);
            int day = calendar.GetDayOfMonth(date);
            if (flag == 0 && month == 2)//润年
            {
                return new DateTime(year, month, 29);
            }

            return new DateTime(year, month, day);
        }


        #region 获取IP地址
      public  static string GetIP()
        {
            var ip4 = HttpContentAccessFactory.Current.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip4))
            {
                ip4=HttpContentAccessFactory.Current.Request.Headers["X-Forwarded-Proto"].FirstOrDefault();
            }
            if (string.IsNullOrEmpty(ip4))
            {
                ip4 = HttpContentAccessFactory.Current.Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
            return ip4;
        }
        #endregion
    }
}
