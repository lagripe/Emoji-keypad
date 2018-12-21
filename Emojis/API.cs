using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HidLibrary;
using System.Runtime.InteropServices;
using System.IO;
using System.Management;
using System.Windows.Automation;
using System.Text.RegularExpressions;

namespace Emojis
{
    class API
    {
        
        public static string EnumerateHids()
        {
            StringBuilder details = new StringBuilder();
            HIDImports.HIDD_ATTRIBUTES deviceAttributes;
            Guid guid;
            uint index = 0;

            // get the GUID of the HID class
            HIDImports.HidD_GetHidGuid(out guid);

            // get a handle to all devices that are part of the HID class
            IntPtr hDevInfo = HIDImports.SetupDiGetClassDevs(ref guid, null, IntPtr.Zero, HIDImports.DIGCF_DEVICEINTERFACE);

            // create a new interface data struct and initialize its size
            HIDImports.SP_DEVICE_INTERFACE_DATA diData = new HIDImports.SP_DEVICE_INTERFACE_DATA();
            diData.cbSize = Marshal.SizeOf(diData);

            // get a device interface to a single device (enumerate all devices)
            while (HIDImports.SetupDiEnumDeviceInterfaces(hDevInfo, IntPtr.Zero, ref guid, index, ref diData))
            {
                UInt32 size = 0;

                // get the buffer size for this device detail instance (returned in the size parameter)
                HIDImports.SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, IntPtr.Zero, 0, out size, IntPtr.Zero);

                // create a detail struct and set its size
                HIDImports.SP_DEVICE_INTERFACE_DETAIL_DATA diDetail = new HIDImports.SP_DEVICE_INTERFACE_DETAIL_DATA();

                //On Win x86, cbSize = 5, On x64, cbSize = 8 i
                diDetail.cbSize = (IntPtr.Size == 8) ? (uint)8 : (uint)5;

                // actually get the detail struct
                if (HIDImports.SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, ref diDetail, size, out size, IntPtr.Zero))
                {
                    // open a read/write handle to our device using the DevicePath returned
                    SafeHandle safeHandle = HIDImports.CreateFile(diDetail.DevicePath, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, HIDImports.EFileAttributes.Overlapped, IntPtr.Zero);

                    // create an attributes struct and initialize the size
                    deviceAttributes = new HIDImports.HIDD_ATTRIBUTES();
                    deviceAttributes.Size = Marshal.SizeOf(deviceAttributes);

                    // get the attributes of the current device
                    if (HIDImports.HidD_GetAttributes(safeHandle.DangerousGetHandle(), ref deviceAttributes))
                    {
                        Console.WriteLine("--- HID DEVICE FOUND ---");
                        Console.WriteLine(String.Format("ProductID: 0x{0}", deviceAttributes.ProductID.ToString("X4")));
                        Console.WriteLine(String.Format("VendorID: 0x{0}", deviceAttributes.VendorID.ToString("X4")));
                        Console.WriteLine(String.Format("VersionNumber: 0x{0}", deviceAttributes.VersionNumber.ToString("X4")));
                        Console.WriteLine(String.Format("Size: 0x{0}", deviceAttributes.Size.ToString("X4")));

                    }
                }
                index++;
            }

            return details.ToString();
        }
        public static void RefreshDevices(Bunifu.Framework.UI.BunifuDropdown dropdown,List<String> Devices)
        {
            foreach (string device in Devices)
                dropdown.AddItem(device);
        }

        public static string getChromeUrl(int ID)
        {
            Process chrome = Process.GetProcessById(ID);

            
                // Console.WriteLine(chrome.Id);
                // the chrome process must have a window
                if (chrome.MainWindowHandle == IntPtr.Zero)
                {
                return null;
                }

                // find the automation element
                AutomationElement elm =
                AutomationElement.FromHandle(chrome.MainWindowHandle);

                AutomationElement elmUrlBar = elm.FindFirst(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.NameProperty, "Address and search bar"));
              
                // if it can be found, get the value from the URL bar
                if (elmUrlBar != null)
                {
                Console.WriteLine("getChromeUrl" + DateTime.Now);
                AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();

                    if (patterns.Length > 0)
                    {
                        ValuePattern val =
                        (ValuePattern)elmUrlBar.GetCurrentPattern(patterns[0]);
                        return (val.Current.Value);
                    }
                }
                else
                {
                //Console.WriteLine("getChromeUrl" + DateTime.Now);
                
                elmUrlBar = elm.FindFirst(TreeScope.Descendants,
                   new PropertyCondition(AutomationElement.NameProperty, "Barre d'adresse et de recherche"));
               // Console.WriteLine("getChromeUrl" + DateTime.Now);
                if (elmUrlBar != null)
                    {

                        AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();
                        if (patterns.Length > 0)
                        {
                            ValuePattern val =
                            (ValuePattern)elmUrlBar.GetCurrentPattern(patterns[0]);
                            return val.Current.Value;
                        }
                    }

                }
                
            
            return null;
            
         // there are always multiple chrome processes, so we have to loop through all of them to find the
         // process with a Window Handle and an automation element of name "Address and search bar"
           
        }

        public static string getFireFoxUrl(int ID)
        {
            Process chrome = Process.GetProcessById(ID);


            // Console.WriteLine(chrome.Id);
            // the chrome process must have a window
            if (chrome.MainWindowHandle == IntPtr.Zero)
            {
                return null;
            }
            
            // find the automation element
            AutomationElement elm =
            AutomationElement.FromHandle(chrome.MainWindowHandle);

            AutomationElement elmUrlBar = elm.FindFirst(TreeScope.Descendants,
            new PropertyCondition(AutomationElement.NameProperty, "Search with Google or enter address"));

            // if it can be found, get the value from the URL bar
            if (elmUrlBar != null)
            {
                
                Console.WriteLine("getChromeUrl" + DateTime.Now);
                AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();

                if (patterns.Length > 0)
                {
                    
                    ValuePattern val =
                    (ValuePattern)elmUrlBar.GetCurrentPattern(patterns[1]);
                    return (val.Current.Value);
                }
            }
            else
            {
                //Console.WriteLine("getChromeUrl" + DateTime.Now);

                elmUrlBar = elm.FindFirst(TreeScope.Descendants,
                   new PropertyCondition(AutomationElement.NameProperty, "Saisir un terme à rechercher ou une adresse"));
                // Console.WriteLine("getChromeUrl" + DateTime.Now);
                if (elmUrlBar != null)
                {

                    AutomationPattern[] patterns = elmUrlBar.GetSupportedPatterns();
                    if (patterns.Length > 0)
                    {
                        ValuePattern val =
                        (ValuePattern)elmUrlBar.GetCurrentPattern(patterns[0]);
                        return val.Current.Value;
                    }
                }

            }


            return null;
        }



    }

}
