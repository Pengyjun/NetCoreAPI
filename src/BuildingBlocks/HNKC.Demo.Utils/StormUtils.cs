using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.Demo.Utils
{
    public class StormUtils
    {
        /// <summary>
        /// 是否在扇面内
        /// </summary>
        /// <param name="centerLat"></param>
        /// <param name="centerLon"></param>
        /// <param name="targetLat"></param>
        /// <param name="targetLon"></param>
        /// <param name="radius"></param>
        /// <param name="sectorStartAngle"></param>
        /// <param name="sectorEndAngle"></param>
        /// <returns></returns>
        public static bool IsInSector(double centerLat, double centerLon, double targetLat, double targetLon, double radius, double sectorStartAngle, double sectorEndAngle)
        {
            // 将经纬度转换为弧度
            double centerLatRad = centerLat * Math.PI / 180.0;
            double centerLonRad = centerLon * Math.PI / 180.0;
            double targetLatRad = targetLat * Math.PI / 180.0;
            double targetLonRad = targetLon * Math.PI / 180.0;

            // 计算经纬度之间的距离
            double distance = Math.Acos(Math.Sin(centerLatRad) * Math.Sin(targetLatRad) + Math.Cos(centerLatRad) * Math.Cos(targetLatRad) * Math.Cos(centerLonRad - targetLonRad)) * 6371.0;

            // 判断目标点位是否在半径范围内
            if (distance <= radius)
            {
                // 计算方位角
                double bearing = Math.Atan2(Math.Sin(targetLonRad - centerLonRad) * Math.Cos(targetLatRad), Math.Cos(centerLatRad) * Math.Sin(targetLatRad) - Math.Sin(centerLatRad) * Math.Cos(targetLatRad) * Math.Cos(targetLonRad - centerLonRad));
                bearing = (bearing + 2 * Math.PI) % (2 * Math.PI);

                // 判断方位角是否在扇面的角度范围内
                if (bearing >= sectorStartAngle * Math.PI / 180.0 && bearing <= sectorEndAngle * Math.PI / 180.0)
                {
                    return true; // 在扇面内
                }
            }

            return false; // 不在扇面内
        }
    }
}
