using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Utils
{
    public static class DateTimeUtils
    {

        public static long DataTimeToUTC(DateTime time)
        {
            double intResult = 0;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            intResult = (time - startTime).TotalSeconds;
            return (long)intResult;
        }

        /// <summary>
        /// 当日期转Unix时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <remarks>UTC时间毫秒级别时间戳，默认从1970年1月1日开始</remarks>
        /// <returns></returns>
        public static long GetMilliTimeStamp(DateTime time)
        {
            var utcTime = TimeZoneInfo.ConvertTime(time, TimeZoneInfo.Local, TimeZoneInfo.Utc);
            DateTime startTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var result = (utcTime - startTime).TotalMilliseconds;
            return (long)result;
        }

        public static int CalculateAge(DateTime birthDate)
        {
            // 获取当前日期
            DateTime currentDate = DateTime.Today;

            // 计算年龄
            int age = currentDate.Year - birthDate.Year;

            // 如果当前月份小于出生日期月份，或者当前月份相同但当前日期小于出生日期日期，则年龄减1
            if (currentDate.Month < birthDate.Month ||
                currentDate.Month == birthDate.Month && currentDate.Day < birthDate.Day)
            {
                age--;
            }
            return age;
        }

        /// <summary>
        /// Unix时间戳转datetime
        /// </summary>
        /// <param name="unix"></param>
        /// <returns></returns>
        public static DateTime ConvertUnixToDateTime(string unix)
        {
            DateTime startUnixTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Local);
            return startUnixTime.AddSeconds(double.Parse(unix));
        }

        #region  Quarter 季度

        /// <summary>
        /// 获取季度
        /// </summary>
        /// <returns></returns>
        public static int Quarter(this DateTime dateTime)
        {
            return (dateTime.Month - 1) / 3 + 1;
        }
        /// <summary>
        /// 获取指定月份的季度
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int QuarterByMonth(int month)
        {
            if (month < 1 || month > 12)
            {
                throw new ArgumentException("月份不合法");
            }

            return (month - 1) / 3 + 1;
        }

        /// <summary>
        /// 当前季度
        /// </summary>
        /// <returns></returns>
        public static int CurrentQuarter()
        {
            DateTime currentDate = DateTime.Now;
            return (currentDate.Month - 1) / 3 + 1;
        }

        /// <summary>
        /// 季度开始月份
        /// </summary>
        /// <param name="quarter"></param>
        /// <returns></returns>
        public static int StartQuarterMonth(int quarter)
        {
            return (quarter - 1) * 3 + 1;
        }

        /// <summary>
        /// 季度结束月份
        /// </summary>
        /// <param name="quarter"></param>
        /// <returns></returns>
        public static int EndQuarterMonth(int quarter)
        {
            return StartQuarterMonth(quarter) + 2;
        }

        /// <summary>
        /// 日报时间所属月份时间
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime MonthlyDate(DateTime date)
        {
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;
            DateTime result;
            if (day <= 25)
            {
                result = new DateTime(year, month, 1);
            }
            else
            {
                if (month == 12)
                {
                    result = new DateTime(year + 1, 1, 1);
                }
                else
                {
                    result = new DateTime(year, month + 1, 1);
                }
            }
            return result;
        }

        /// <summary>
        /// 指定月份，返回这个月的起始日期和结束日期
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="lastDay">月的最后一天</param>
        /// <returns></returns>
        public static (DateTime firstDate, DateTime lastDate) MonthlyDate(int year, int month, int lastDay = 25)
        {
            int _month;

            int _year;

            if (month == 1)
            {
                _month = 12;
                _year = year - 1;
            }
            else
            {
                _month = month - 1;
                _year = year;
            }

            var firstDate = new DateTime(_year, _month, lastDay + 1);

            var lastDate = new DateTime(year, month, lastDay);

            return (firstDate, lastDate);

        }

        /// <summary>
        /// 当前月份天数
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int MonthlyDayCount(DateTime date)
        {
            var lastDate = date.AddMonths(-1);
            var startDate = new DateTime(lastDate.Year, lastDate.Month, 26);
            var endDate = new DateTime(date.Year, date.Month, 25);
            return (endDate - startDate).Days;
        }

        #endregion
    }
}
