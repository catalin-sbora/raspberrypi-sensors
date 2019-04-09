using RaspberryPi.Sensors.Abstractions;
using RaspberryPi.Sensors.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace RaspberryPi.Sensors
{
    public enum Bmp180Mode
    {
        UltraLowPower = 0,
        Standard,
        HighResolution,
        UltraHighResolution
    };

    public sealed class Bmp180Sensor : ITemperatureSensor, IBarometricPressureSensor, ILightSensor
    {
        private int i2cAddress;
        private Bmp180Mode mode;
        private II2CDevice i2cDevice = null;
        private II2CBus i2cBus = null;

        private enum Register
        {
            AC1 = 0xAA,
            AC2 = 0xAC,
            AC3 = 0xAE,
            AC4 = 0xB0,
            AC5 = 0xB2,
            AC6 = 0xB4,
            B1 = 0xB6,
            B2 = 0xB8,
            MB = 0xBA,
            MC = 0xBC,
            MD = 0xBE,
            ChipId = 0xD0,
            Version = 0xD1,
            SoftReset = 0xE0,
            Control = 0xF4,    
            Data =  0xF6
        }

        private enum Command
        {
            ReadTemperature = 0x2E,
            ReadPressure = 0x34
        }

        public const byte DefaultI2CAddress = 0x77;
        
        public Bmp180Sensor(II2CBus i2cBus, int i2cAddress = Bmp180Sensor.DefaultI2CAddress, Bmp180Mode mode = Bmp180Mode.HighResolution)
        {
            this.i2cAddress = i2cAddress;
            this.mode = Bmp180Mode.HighResolution;
            this.i2cBus = i2cBus;
        }

        public void Connect()
        {           
            i2cDevice = i2cBus.AddDevice(i2cAddress);

        }
        public double GetIluminance()
        {
            throw new NotImplementedException();
        }

        public double GetPressure()
        {
            throw new NotImplementedException();
        }

        public double GetTemperature()
        {
            double retVal = 0.0;
            WriteCommand(Command.ReadTemperature);
            Thread.Sleep(5);
            var rawTempData = ReadUInt16FromRegister(Register.Data);


            return retVal;
        }

        private Int16 ReadInt16FromRegister(Register registerToRead)
        {           
            //sensor stores data with msb first (big-endian) and we need to convert to little endian
            short bigEndianValue = (short)i2cDevice.ReadAddressWord((int)registerToRead);
            short littleEndianValue = (short)((bigEndianValue & 0x00FF) << 8 | ((bigEndianValue & 0xFF00) >> 8));
            return littleEndianValue;
        }

        private UInt16 ReadUInt16FromRegister(Register registerToRead)
        {
            var retVal = ReadInt16FromRegister(registerToRead);
            return (ushort)retVal;
        }

        private void WriteCommand(Command command)
        {
            if (i2cDevice == null)
            {
                throw new DeviceNotConnectedException("Device is not yet connected. Please call the connect method first.");
            }            
            i2cDevice.WriteAddressByte((int)Register.Control, (byte)command);
        }
    }
}
