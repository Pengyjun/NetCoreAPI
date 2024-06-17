using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHMonitoringCenterApi.Domain.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public class ShareData
    {
        //Dictionary<string, decimal> data = new Dictionary<string, decimal>();
        List<DataInit> dataInits = new List<DataInit>();
        public List<DataInit> Init() 
        {

            //data.Add("3c5b138b-601a-442a-9519-2508ec1c1eb2", 8.07M);
            //data.Add("a8db9bb0-4667-4320-b03d-b0b7f8728b61", 11.20M);
            //data.Add("11c9c978-9ef3-411d-ba70-0d0eed93e048", 0.39M);
            //data.Add("5a8f39de-8515-456b-8620-c0d01c012e04", 0.03M);
            //data.Add("c0d51e81-03dd-4ef8-bd83-6eb1355879e1", 2.047M);
            //data.Add("65052a94-6ea7-44ba-96b4-cf648de0d28a", 0.31M);
            //data.Add("01ff7a0e-e827-4b46-9032-0a540ce1fba3", 1.32M);
            //data.Add("bd840460-1e3a-45c8-abed-6e66903eb465", 11.07M);

            dataInits.Add(new DataInit()
            {
                CompanyId = "3c5b138b-601a-442a-9519-2508ec1c1eb2",
                Production = 8.29M
            }) ;
            dataInits.Add(new DataInit()
            {
                CompanyId = "a8db9bb0-4667-4320-b03d-b0b7f8728b61",
                //Production =12.05M
                Production =11.69M
            });

            dataInits.Add(new DataInit()
            {
                CompanyId = "11c9c978-9ef3-411d-ba70-0d0eed93e048",
                Production = 0.83M
            });

            dataInits.Add(new DataInit()
            {
                CompanyId = "5a8f39de-8515-456b-8620-c0d01c012e04",
                Production = 0.06M
            });

            dataInits.Add(new DataInit()
            {
                CompanyId = "c0d51e81-03dd-4ef8-bd83-6eb1355879e1",
                Production = 1.66M
            });


            dataInits.Add(new DataInit()
            {
                CompanyId = "65052a94-6ea7-44ba-96b4-cf648de0d28a",
                Production = 0.59M
            });


            dataInits.Add(new DataInit()
            {
                CompanyId = "01ff7a0e-e827-4b46-9032-0a540ce1fba3",
                Production = 1.34M
            });

            dataInits.Add(new DataInit()
            {
                CompanyId = "bd840460-1e3a-45c8-abed-6e66903eb465",
                Production = 11.95M
            });
            return dataInits;
        }
    }

    public class DataInit{

        public string CompanyId { get; set; }
        public decimal Production { get; set; }
    }
}
