using System;

namespace GeoCalc.Shapes
{
    /// <summary>
    /// Trieda reprezentujúca valec.
    /// </summary>
    public class Cylinder : Shape3D
    {
        public override string Name => "Valec";
        public override string ImageName => "cylinder";
        public override string[] ParameterNames => new[] { "Polomer (r)", "Výška (v)" };
        public override string[] ParameterDescriptions => new[] 
        { 
            "Polomer podstavy valca",
            "Výška valca"
        };

        public override string GetFormulas()
        {
            return "╔═══════════════════════════════════╗\n" +
                   "║         VZORCE - VALEC            ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  Objem:    V = π × r² × v         ║\n" +
                   "║  Povrch:   S = 2πr(r + v)         ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  r = polomer podstavy             ║\n" +
                   "║  v = výška                        ║\n" +
                   "║  π ≈ 3.14159...                   ║\n" +
                   "╚═══════════════════════════════════╝";
        }

        public override double CalculateVolume(double[] parameters)
        {
            double r = parameters[0];
            double v = parameters[1];
            return Math.PI * r * r * v;
        }

        public override double CalculateSurfaceArea(double[] parameters)
        {
            double r = parameters[0];
            double v = parameters[1];
            return 2 * Math.PI * r * (r + v);
        }
    }
}
