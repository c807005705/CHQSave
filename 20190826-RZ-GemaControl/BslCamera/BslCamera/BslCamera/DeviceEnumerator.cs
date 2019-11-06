using PylonC.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BslCamera
{
    internal static class DeviceEnumerator
    {
        /* Data class used for holding device data. */
        internal class Device : IComparable
        {
            public string Name; /* The friendly name of the device. */
            public string FullName; /* The full name string which is unique. */
            public uint Index; /* The index of the device. */
            public string Tooltip; /* The displayed tooltip */
            /// <summary>
            /// 比较
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(object obj)
            {
                Device device = obj as Device;
                if (device == null) return -1;

                int numself = int.Parse(this.Name.Split('(')[0].Split('m')[1]);
                int numother = int.Parse(device.Name.Split('(')[0].Split('m')[1]);

                return numself > numother ? 1 : -1;
            }
        }

        /* Queries the number of available devices and creates a list with device data. */
        internal static List<Device> EnumerateDevices()
        {
            /* Create a list for the device data. */
            List<Device> list = new List<Device>();

            /* Enumerate all camera devices. You must call
            PylonEnumerateDevices() before creating a device. */
            uint count = Pylon.EnumerateDevices();

            /* Get device data from all devices. */
            for (uint i = 0; i < count; ++i)
            {
                /* Create a new data packet. */
                Device device = new Device();
                /* Get the device info handle of the device. */
                PYLON_DEVICE_INFO_HANDLE hDi = Pylon.GetDeviceInfoHandle(i);
                /* Get the name. */
                device.Name = Pylon.DeviceInfoGetPropertyValueByName(hDi, Pylon.cPylonDeviceInfoFriendlyNameKey);
                /* Get the serial number */
                device.FullName = Pylon.DeviceInfoGetPropertyValueByName(hDi, Pylon.cPylonDeviceInfoFullNameKey);
                /* Set the index. */
                device.Index = i;

                /* Create tooltip */
                string tooltip = "";
                uint propertyCount = Pylon.DeviceInfoGetNumProperties(hDi);

                if (propertyCount > 0)
                {
                    for (uint j = 0; j < propertyCount; j++)
                    {
                        tooltip += Pylon.DeviceInfoGetPropertyName(hDi, j) + ": " + Pylon.DeviceInfoGetPropertyValueByIndex(hDi, j);
                        if (j != propertyCount - 1)
                        {
                            tooltip += "\n";
                        }
                    }
                }
                device.Tooltip = tooltip;
                /* Add to the list. */
                list.Add(device);
            }
            return list;
        }
    }
}
