using Microsoft.Win32;
using System.Diagnostics;

namespace ChargeApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            
            Application.Run(new Charge());
       

        }


        //static void SystemEvents_PowerModeChanged(object sender, Microsoft.Win32.PowerModeChangedEventArgs e)
        //{
        //    PowerStatus pw = SystemInformation.PowerStatus;
        //    if (e.Mode == Microsoft.Win32.PowerModes.StatusChange)
        //    {
        //        Debug.WriteLine("hi");
        //        Debug.WriteLine(pw.BatteryLifePercent.ToString());
        //    }
        //}
    }
}