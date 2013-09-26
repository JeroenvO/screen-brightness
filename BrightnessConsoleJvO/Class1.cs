using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightnessConsoleJvO
{
    public class Class1
    {
        public byte[] bLevels; //array of valid level values
        private string[] args;
        //constructor
        public Class1(string[] args)
        {
            this.args = args;
            startFunction(args);
        }
        /*
         * Check the arguments and call the functions
         * * */
        private void startFunction(string[] args)
        {
            bLevels = GetBrightnessLevels(); //get the level array for this system
            if (bLevels.Count() == 0) //"WmiMonitorBrightness" is not supported by the system
            {
                Console.WriteLine("Sorry, Your System does not support this brightness control...");
            }
            else
            {
                if (Array.FindIndex(args, item => item.Contains("%")) > -1)
                {  //set brightness to level from args
                    string sPercent = args[Array.FindIndex(args, item => item.Contains("%"))];
                    if (sPercent.Length > 1)
                    {
                        int iPercent = Convert.ToInt16(sPercent.Split('%').ElementAt(0));
                        startup_brightness(iPercent);;
                    }
                }
                if (Array.FindIndex(args, item => item.Contains("+")) > -1)
                { //increase brightess with a number
                    string sIncreasePercent = args[Array.FindIndex(args, item => item.Contains("+"))];
                    if (sIncreasePercent.Length > 1)
                    {
                        int iIncreaesePercent = Convert.ToInt16(sIncreasePercent.Split('+').ElementAt(0));
                        int curBrightness = GetBrightness();
                       // iIncreaesePercent = 10;
                        startup_brightness(curBrightness + iIncreaesePercent);
                    }
                }
                if (Array.FindIndex(args, item => item.Contains("-")) > -1)
                { //decrease brightness with a number
                    string sDecreasePercent = args[Array.FindIndex(args, item => item.Contains("-"))];
                    if (sDecreasePercent.Length > 1)
                    {
                        int iDecreasePercent = Convert.ToInt16(sDecreasePercent.Split('-').ElementAt(0));
                        int curBrightness = GetBrightness();
                        startup_brightness(curBrightness - iDecreasePercent);
                    }
                }
            }
        }
        /*
         * Convert the brightness percentage to a byte and set the brightness using setBrightness
         * */
        private void startup_brightness(int iPercent)
        {
            if (iPercent >= 0 && iPercent <= bLevels[bLevels.Count() - 1])
            {
                byte level = 100;
                foreach (byte item in bLevels)
                {
                    if (item >= iPercent)
                    {
                        level = item;
                        break;
                    }
                }
                SetBrightness(level);
                //check_brightness();
            }
        }
        /*
         * Returns the current brightness setting
         * */
        static int GetBrightness()
        {
            //define scope (namespace)
            System.Management.ManagementScope s = new System.Management.ManagementScope("root\\WMI");

            //define query
            System.Management.SelectQuery q = new System.Management.SelectQuery("WmiMonitorBrightness");

            //output current brightness
            System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher(s, q);

            System.Management.ManagementObjectCollection moc = mos.Get();

            //store result
            byte curBrightness = 0;
            foreach (System.Management.ManagementObject o in moc)
            {
                curBrightness = (byte)o.GetPropertyValue("CurrentBrightness");
                break; //only work on the first object
            }

            moc.Dispose();
            mos.Dispose();

            return (int)curBrightness;
        }
        /*
         * Get the array of allowed brightnesslevels for this system
         * */
        static byte[] GetBrightnessLevels()
        {
            //define scope (namespace)
            System.Management.ManagementScope s = new System.Management.ManagementScope("root\\WMI");

            //define query
            System.Management.SelectQuery q = new System.Management.SelectQuery("WmiMonitorBrightness");

            //output current brightness
            System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher(s, q);
            byte[] BrightnessLevels = new byte[0];

            try
            {
                System.Management.ManagementObjectCollection moc = mos.Get();

                //store result


                foreach (System.Management.ManagementObject o in moc)
                {
                    BrightnessLevels = (byte[])o.GetPropertyValue("Level");
                    break; //only work on the first object
                }

                moc.Dispose();
                mos.Dispose();

            }
            catch (Exception)
            {
                Console.WriteLine("Sorry, Your System does not support this brightness control...");
            }

            return BrightnessLevels;
        }
        /*
         * Set the brightnesslevel to the targetBrightness
         * */
        static void SetBrightness(byte targetBrightness)
        {
            //define scope (namespace)
            System.Management.ManagementScope s = new System.Management.ManagementScope("root\\WMI");

            //define query
            System.Management.SelectQuery q = new System.Management.SelectQuery("WmiMonitorBrightnessMethods");

            //output current brightness
            System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher(s, q);

            System.Management.ManagementObjectCollection moc = mos.Get();

            foreach (System.Management.ManagementObject o in moc)
            {
                o.InvokeMethod("WmiSetBrightness", new Object[] { UInt32.MaxValue, targetBrightness }); //note the reversed order - won't work otherwise!
                break; //only work on the first object
            }

            moc.Dispose();
            mos.Dispose();
        }
    }
}
