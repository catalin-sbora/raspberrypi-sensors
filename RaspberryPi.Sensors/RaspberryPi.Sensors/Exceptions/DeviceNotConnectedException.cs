using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.Sensors.Exceptions
{
    public class DeviceNotConnectedException: Exception
    {
        public DeviceNotConnectedException(string message) : base(message)
        {
        }
        public DeviceNotConnectedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
