using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobot.TestBox
{
    public class CalculateCoefficient
    {

        public double CalculateCoefficient_K1 { get; private set; }
        public double CalculateCoefficient_C1 { get; private set; }
        public double CalculateCoefficient_K2 { get; private set; }
        public double CalculateCoefficient_C2 { get; private set; }

        public CalculateCoefficient(double calculateCoefficient_K1, double calculateCoefficient_C1, double calculateCoefficient_K2, double calculateCoefficient_C2)
        {
            this.CalculateCoefficient_K1 = calculateCoefficient_K1;
            this.CalculateCoefficient_C1 = calculateCoefficient_C1;
            this.CalculateCoefficient_K2 = calculateCoefficient_K2;
            this.CalculateCoefficient_C2 = calculateCoefficient_C2;
        }


    }
}
