using System;
using Unosquare.RaspberryIO;
using Unosquare.WiringPi;

namespace RaspberryPi.Sensors.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Initialize WiringPi ...");
            Pi.Init<BootstrapWiringPi>();
            Console.WriteLine("Done");
            try
            {
                Bmp180Sensor bmpSensor = new Bmp180Sensor(Pi.I2C, Bmp180Sensor.DefaultI2CAddress, Bmp180Mode.UltraLowPower);
                Console.Write("Connecting to the BMP 180 sensor ...");
                bmpSensor.Connect();
                Console.WriteLine("Done\n");
                Console.WriteLine("Calibration data: ");
                Console.WriteLine($"AC1     {bmpSensor.CalibrationData.AC1}");
                Console.WriteLine($"AC2     {bmpSensor.CalibrationData.AC2}");
                Console.WriteLine($"AC3     {bmpSensor.CalibrationData.AC3}");
                Console.WriteLine($"AC4     {bmpSensor.CalibrationData.AC4}");
                Console.WriteLine($"AC5     {bmpSensor.CalibrationData.AC5}");
                Console.WriteLine($"AC6     {bmpSensor.CalibrationData.AC6}");
                Console.WriteLine($"B1      {bmpSensor.CalibrationData.B1}");
                Console.WriteLine($"B2      {bmpSensor.CalibrationData.B2}");
                Console.WriteLine($"MB      {bmpSensor.CalibrationData.MB}");
                Console.WriteLine($"MC      {bmpSensor.CalibrationData.MC}");
                Console.WriteLine($"MD      {bmpSensor.CalibrationData.MD}");

                Console.Write("Read Temperature ... ");
                var tempValue = bmpSensor.GetTemperature();
                Console.WriteLine($"Done. {tempValue} deg. Celsius");
                Console.Write("Read Pressure ... ");
                var pressure = bmpSensor.GetPressure();
                Console.WriteLine($"Done. {pressure} Pa");
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed. " + e.Message);
            }
           // Console.Write
        }
    }
}
