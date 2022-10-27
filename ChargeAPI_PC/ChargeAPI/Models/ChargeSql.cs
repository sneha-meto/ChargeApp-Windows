using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Data.SqlTypes;

namespace ChargeAPI.Models
{
    public class ChargeSql
    {
        SqlCommand comm;
        SqlConnection conn;

        public ChargeSql()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["mydb"].ConnectionString);
            comm = new SqlCommand();
        }
        public void AddCharge(Charge charge)
        {
            int chargeBit = charge.IsCharging ? 1 : 0;
            Debug.Print(charge.Time);

            comm.CommandText = "insert into ChargePC values('"+ charge.Time+"',"+charge.BatteryLevel+","+chargeBit+")";
            comm.Connection = conn;
            conn.Open();
            comm.ExecuteNonQuery();
            conn.Close();
        }

        public List<Charge> GetCharges()
        {
            List<Charge> chargeList = new List<Charge>();
            comm.CommandText = "select * from ChargePC order by time";
            comm.Connection = conn;
            conn.Open();
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                int battery = Convert.ToInt32(reader["batteryLevel"]);
                String time = reader["time"].ToString();
                bool charging =Boolean.Parse( reader["isCharging"].ToString());

                Charge charge = new Charge(time,battery,charging);
                chargeList.Add(charge);
            }
            conn.Close();
            return chargeList;
        }

        public List<Discharge> GetChargeCycle()
        {
            List<Discharge> discharges = new List<Discharge>();
            List<Charge> chargeList = GetCharges();
          
            DateTime time = new DateTime();
            Charge prev = chargeList.First();
            int level = 0;
            TimeSpan dTime = new TimeSpan(0);
          
            foreach (var charge in chargeList)
            {
                DateTime currentTime = DateTime.Parse(charge.Time);
                if (charge.Equals(chargeList.First())) { time = currentTime; continue; }
                if (currentTime.Hour == time.Hour)
                {
                    if (charge.BatteryLevel < prev.BatteryLevel)
                    { //discharging
                        dTime += currentTime - DateTime.Parse(prev.Time);
                        level += prev.BatteryLevel - charge.BatteryLevel;
                    }
                    else
                    {//charging
                    }
                }
                else
                {//next hr
                    discharges.Add(new Discharge(time,  level, dTime));
                    //reset
                    dTime = new TimeSpan(0);
                    level = 0;
                    time = currentTime;
                }

                if (charge.Equals(chargeList.Last()))
                {
                    discharges.Add(new Discharge(time, level, dTime));
                }
                prev = charge;
            }
            return discharges;
        }

        public int[] getChargePattern()
        {

            int badCount = 0, optimalCount = 0, spotCount = 0;
            List<Charge> chargeList = GetCharges();
            Charge prev = chargeList.First();
            TimeSpan oTime = new TimeSpan(0);
            Charge last = chargeList.Last();

            bool overCharged = false;

            foreach (var charge in chargeList)
            {
                if (charge.IsCharging && charge.BatteryLevel == 100)
                {//overcharging
                    overCharged = true;
                    oTime += DateTime.Parse(charge.Time) - DateTime.Parse(prev.Time); //fix
                }

                else if ((!charge.IsCharging && prev.IsCharging) || (charge.IsCharging && charge.Equals(last)))
                {//end of charging session or last is charging
                    if (overCharged)
                    {
                        oTime += DateTime.Parse(charge.Time) - DateTime.Parse(prev.Time);
                        System.Diagnostics.Debug.WriteLine(oTime);
                        if (oTime.CompareTo(new TimeSpan(0, 30, 0)) == 1)
                        {
                            badCount += 1;
                        }
                        else
                        {
                            optimalCount += 1;
                        }
                        oTime = TimeSpan.Zero;
                        overCharged = false;
                    }
                    else
                    {
                        if (charge.IsCharging && charge.Equals(last)) break;
                        spotCount += 1;
                    }
                }
                prev = charge;
            }
            return new int[3] { badCount, optimalCount, spotCount };
        }
    }
}