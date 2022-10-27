using Microsoft.Win32;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace ChargeApp
{
    public partial class Charge : Form
    {
        private PowerStatus p = SystemInformation.PowerStatus;
        private BatteryChargeStatus b;
        private int batteryPercent=0;
        private bool isCharging;
        DataService dataService;
       
        public Charge()
        {
            InitializeComponent();
            dataService = new DataService();          
            b = p.BatteryChargeStatus;
        }

        private void Charge_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            label1.Text = "0";
            label2.Text = "";
            label3.Text = "";
        }

        private async void button1_Click(object sender, EventArgs e)
        {       
            List<int> pattern = await dataService.getChargePattern();
            label3.Text = "BadCount: "+pattern[0].ToString() +", OptimalCount: " + pattern[1].ToString() + ", SpotCount: " + pattern[2].ToString();

            List<DischargeModel> cycle = await  dataService.getChargeCycle();
            listView1.Clear();

            foreach (DischargeModel item in cycle)
            {
                listView1.Items.Add(item.ToString());
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (batteryPercent != Convert.ToInt32(p.BatteryLifePercent * 100) || b != p.BatteryChargeStatus)
           // if (batteryPercent!=Convert.ToInt32(p.BatteryLifePercent * 100))
            {
                b = p.BatteryChargeStatus;
                TimeSpan time = TimeSpan.FromSeconds(p.BatteryLifeRemaining);
                if ((b & BatteryChargeStatus.Charging) == BatteryChargeStatus.Charging)
                {
                    label2.Text = "Charging";
                    isCharging = true;
                }
                else { isCharging = false; }
                label1.Text = Convert.ToInt32(p.BatteryLifePercent * 100).ToString() + "%";
                batteryPercent = Convert.ToInt32(p.BatteryLifePercent * 100);
                var result = dataService.addChargeAsync(new ChargeModel(DateTime.Now.ToString(), batteryPercent, isCharging));
              
            }
        }
    }
}