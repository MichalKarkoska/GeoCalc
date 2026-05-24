using System;

namespace GeoCalc.Shapes
{
    /// <summary>
    /// Trieda reprezentujúca lichobežník.
    /// </summary>
    public class Trapezoid : Shape2D
    {
        public override string Name => "Lichobežník";
        public override string ImageName => "trapezoid";
        public override string[] ParameterNames => new[] { "Základňa a", "Základňa c", "Strana b", "Strana d", "Výška (v)" };
        public override string[] ParameterDescriptions => new[] 
        { 
            "Dolná základňa lichobežníka",
            "Horná základňa lichobežníka",
            "Ľavá bočná strana",
            "Pravá bočná strana",
            "Výška lichobežníka (kolmá vzdialenosť medzi základňami)"
        };

        public override string GetFormulas()
        {
            return "╔═══════════════════════════════════╗\n" +
                   "║     VZORCE - LICHOBEŽNÍK          ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  Obsah:   S = ((a + c) × v) / 2   ║\n" +
                   "║  Obvod:   O = a + b + c + d       ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  a, c = základne                  ║\n" +
                   "║  b, d = bočné strany              ║\n" +
                   "║  v = výška                        ║\n" +
                   "╚═══════════════════════════════════╝";
        }

        public override double CalculateArea(double[] parameters)
        {
            double a = parameters[0]; // dolná základňa
            double c = parameters[1]; // horná základňa
            double v = parameters[4]; // výška
            return ((a + c) * v) / 2;
        }

        public override double CalculatePerimeter(double[] parameters)
        {
            double a = parameters[0];
            double c = parameters[1];
            double b = parameters[2];
            double d = parameters[3];
            return a + b + c + d;
        }
    }
}
