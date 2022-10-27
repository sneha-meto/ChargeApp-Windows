using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargeApp
{
    internal class DischargeModel
    {

        public DischargeModel(DateTime date, int level, TimeSpan dischargeTime)
        {
          
            Date = date;
            Level = level;
            DischargeTime = dischargeTime;
        }

        public override string ToString()
        {
            return $"date: {Date.ToString("dd/MM/yyyy")}, hr: {Date.Hour}, d-Level: {Level}, d-Time: {DischargeTime}";
        }

        public DateTime Date { get; set; }
        public int Level { get; set; }
        public TimeSpan DischargeTime { get; set; }


    }
}
