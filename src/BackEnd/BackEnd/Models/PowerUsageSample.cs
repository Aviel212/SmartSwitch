using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace BackEnd.Models
{
    /// <summary>
    /// A current and voltage sample of a given device at a given time
    /// </summary>
    public class PowerUsageSample
    {
        [Key]
        public DateTime SampleDate { get; set; }
        public double Current { get; set; } // [V]
        public double Voltage { get; set; } // [A]

        public PowerUsageSample() { }

        public PowerUsageSample(DateTime dt, double v, double c)
        {
            Current = c;
            SampleDate = dt;
            Voltage = v;
        }

        public PowerUsageSample(double v, double c) : this(DateTime.Now, v, c) { }

        public double GetWattage()
        {
            return Current * Voltage; // [W]
        }
    }
}