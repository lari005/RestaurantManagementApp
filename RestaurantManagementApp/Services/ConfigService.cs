using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

namespace RestaurantManagementApp.Services
{
    public static class ConfigService
    {
        
        public static decimal GetReducereMeniu_X() => decimal.Parse(ConfigurationManager.AppSettings["ReducereMeniu_x"]);
        public static decimal GetSumaMinimaDiscount_Y() => decimal.Parse(ConfigurationManager.AppSettings["SumaMinimaDiscount_y"]);
        public static int GetNumarComenzi_Z() => int.Parse(ConfigurationManager.AppSettings["NumarComenzi_z"]);
        public static int GetIntervalTimp_T() => int.Parse(ConfigurationManager.AppSettings["IntervalTimp_t"]);
        public static decimal GetProcentDiscount_W() => decimal.Parse(ConfigurationManager.AppSettings["ProcentDiscount_w"]);
        public static decimal GetLimitaTransport_A() => decimal.Parse(ConfigurationManager.AppSettings["SumaLimitaTransport_a"]);
        public static decimal GetCostTransport_B() => decimal.Parse(ConfigurationManager.AppSettings["CostTransport_b"]);
        public static decimal GetLimitaEpuizare_C() => decimal.Parse(ConfigurationManager.AppSettings["LimitaEpuizareStoc_c"]);
    }
}
