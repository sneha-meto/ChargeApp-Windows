using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChargeAPI.Models
{
    public class Discharge
    {
        public Discharge(DateTime date, int level, TimeSpan dischargeTime)
        {
      
            Date = date;
            Level = level;
            DischargeTime = dischargeTime;
        }

        public DateTime Date { get; set; }
        public int Level { get; set; }
        public TimeSpan DischargeTime { get; set; }

        


    }
}