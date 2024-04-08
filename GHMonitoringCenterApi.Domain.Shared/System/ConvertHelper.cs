using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 转换帮助文件
    /// </summary>
    public static  class ConvertHelper
    {
        /// <summary>
        /// 转换成日期Day格式，固定8位数（例：20230401）
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static  int ToDateDay(this DateTime dateTime )
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
        public static bool  TryConvertDateTimeFromDateDay( int dateDay,out DateTime dayTime)
        {
            var dateDayStr = dateDay.ToString();
            if (dateDayStr.Length!=8)
            {
                dayTime = DateTime.MinValue;
                return false;
            }
            var year = dateDayStr.Substring(0, 4);
            var month = dateDayStr.Substring(4, 2);
            var day = dateDayStr.Substring(6, 2);
            return DateTime.TryParse ($"{year}-{month}-{day}", out dayTime);
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
            if (flag==0&& month==2)//润年
            {
                return new DateTime(year, month, 29);
            }

            return new DateTime(year, month, day);
        }
    }
}
