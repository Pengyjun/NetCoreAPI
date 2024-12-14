using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.CrewManagePlatform.Utils
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
