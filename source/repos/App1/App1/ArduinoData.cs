using System;
using System.Collections.Generic;
using System.Text;

namespace App1
{
    class ArduinoData
    {
        public DateTime Date { get; set; }
        public double DHT11Temp { get; set; }
        public double DHT11Humid { get; set; }
        public double DHT22Temp { get; set; }
        public double DHT22Humid { get; set; }
        public int DM460LightIntensity { get; set; }
        public int DM2007LightIntensity { get; set; }
        public double CO { get; set; }
        public double Alcohol { get; set; }
        public double CO2 { get; set; }
        public double Tolueno { get; set; }
        public double NH4 { get; set; }
        public double Acetona { get; set; }
        public bool IsLastReadSuccessful { get; set; }

    }
}
