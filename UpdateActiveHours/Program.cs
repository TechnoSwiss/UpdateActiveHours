using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace UpdateActiveHours
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var ActiveHourDelta = 12;
                if (Int32.Parse(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion").GetValue("ReleaseId").ToString()) >= 1803 &&
                    Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion").GetValue("ProductName").ToString() != "Windows 10 Home")
                {
                    ActiveHourDelta = 18; // the max active hour delta was raised to 18 hours with release 1803 for non-Home editions of Win10
                }
                if (Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate") != null &&
                    Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate").GetValue("SetActiveHoursMaxRange") != null &&
                    Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate").GetValue("ActiveHoursMaxRange") != null)
                {
                    ActiveHourDelta = Int32.Parse(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate").GetValue("ActiveHoursMaxRange").ToString()); // if this registry key exists a group policy setting has set the max active hour delta, so go with whatever this setting is
                }

                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\WindowsUpdate\UX\Settings", true);
                if (key != null)
                {
                    var ActiveHourStart = Int32.Parse(DateTime.Now.AddHours(-1).Hour.ToString()); // set active time to start at the whole hour before the current hour
                    var ActiveHourEnd = (ActiveHourStart + ActiveHourDelta) > 24 ? (ActiveHourStart + ActiveHourDelta) - 24 : (ActiveHourStart + ActiveHourDelta); // set active time to end at the last whole hour <ActiveHourDelta> after the current hour
                    key.SetValue("ActiveHoursStart", ActiveHourStart); // set active time start to the new value in registry
                    key.SetValue("ActiveHoursEnd", ActiveHourEnd); // set active time end to the new value in the registry
                    if(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\WindowsUpdate\UX\Settings").GetValue("SmartActiveHoursState") != null)
                    {
                        key.SetValue("SmartActiveHoursState", 2); // we're going to be managing the active time with this app, so turn off the "smart" active time setting if it exists
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception in UpdateActiveHours");
            }
        }
    }
}
