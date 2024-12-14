using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

<<<<<<<< HEAD:src/BuildingBlocks/HNKC.CrewManagePlatform.Utils/PKManager.cs
namespace HNKC.CrewManagePlatform.Utils
========
namespace HNKC.Demo.Utils
>>>>>>>> 7fd224848dc4910963de00d8c3a15a3418dc1847:src/BuildingBlocks/HNKC.Demo.Utils/PKManager.cs
{
    public class PKManager
    {
        public static bool IsGuid(string guid)
        {
            if (string.IsNullOrEmpty(guid))
                return false;


            bool _result = false;


            try
            {

                var _t = new Guid(guid);
                _result = true;
            }
            catch { }
            return _result;
        }

        public static string UUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
