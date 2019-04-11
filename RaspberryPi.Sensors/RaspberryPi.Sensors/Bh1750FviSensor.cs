using RaspberryPi.Sensors.Abstractions;
using RaspberryPi.Sensors.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Unosquare.RaspberryIO.Abstractions;

namespace RaspberryPi.Sensors
{
    public enum Bh1750Mode
    {
        Unconfigured = 0,        
        ContinuousStandardResolution = 0x10,        
        ContinuousHighResolution = 0x11,        
        ContinuousLowResolution = 0x13,        
        OneTimeStandardResolution = 0x20,        
        OneTimeHighResolution = 0x21,        
        OneTimeLowResolution = 0x23
    };

    public class Bh1750FviSensor : ILightSensor
    {
        private byte measurementTime;
        private readonly II2CDevice i2cDevice = null;
        private const byte DefaultMeasurementTime = 69;

        public const byte DefaultI2CAddress = 0x23;

        private int GetMaxWaitTime()
        {
            int retValue = 120;
            switch (Mode)
            {
                case Bh1750Mode.ContinuousLowResolution:
                case Bh1750Mode.OneTimeLowResolution:
                    retValue = 24 * MeasurementTime / DefaultMeasurementTime;
                    break;

                case Bh1750Mode.ContinuousHighResolution:
                case Bh1750Mode.ContinuousStandardResolution:
                case Bh1750Mode.OneTimeHighResolution:
                case Bh1750Mode.OneTimeStandardResolution:
                    retValue = 180 * MeasurementTime / DefaultMeasurementTime;
                    break;
            }

            return retValue;
        }


        public Bh1750Mode Mode
        {
            get;
            set;            
        }

        public byte MeasurementTime
        {
            get
            {
                return measurementTime;
            }
            set
            {
                if (value < 32 && value > 254)
                {
                    throw new ArgumentException("Measurement time must be a value between 32 and 254");
                }

                if (i2cDevice == null)
                {
                    throw new DeviceNotConnectedException("Seems that the device is not connected. In order to set the measurement time the device must be connected and initialized");
                }

                //Set MT register. In order to set it we need to send two bytes. First byte is in the form 01000(v7)(v6)(v5) and the second byte
                //is 011(v4)(v3)(v2)(v1)(v0)
                //where (v0), (v1) ... (v7) are the bits 0, 1, 2, ... 7 of the new value (measurement time)
                byte firstByte = (byte)(0b0100_0000 | (value >> 5));
                byte secondByte = (byte)(0b01100000 | (value & 0b00011111));

                i2cDevice.Write(firstByte);
                i2cDevice.Write(secondByte);
                i2cDevice.Write((byte)Mode);
                Thread.Sleep(10);
                measurementTime = value;
                Thread.Sleep(GetMaxWaitTime());

            }
        }               



        public Bh1750FviSensor(II2CBus i2cBus, byte i2cAddress = DefaultI2CAddress, Bh1750Mode mode = Bh1750Mode.ContinuousStandardResolution)
        {

        }
        
        public double GetIluminance()
        {
            double retVal = 0.0;
            return retVal;
        }
    }
}
