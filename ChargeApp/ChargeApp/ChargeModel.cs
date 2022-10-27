using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargeApp
{
    internal class ChargeModel
    {
        public ChargeModel(String time, int batteryLevel, bool isCharging)
        {
            Time = time;
            BatteryLevel = batteryLevel;
            IsCharging = isCharging;

        }

        public String Time { get; set; }
        public int BatteryLevel { get; set; }
        public bool IsCharging { get; set; }
    }
}
