using System;

namespace GeoCalc.Shapes
{
    /// <summary>
    /// Trieda reprezentujúca pravidelný štvroboký ihlan.
    /// </summary>
    public class Pyramid : Shape3D
    {
        public override string Name => "Ihlan";
        public override string ImageName => "pyramid";
        public override string[] ParameterNames => new[] { "Strana podstavy (a)", "Výška (v)" };
        public override string[] ParameterDescriptions => new[] 
        { 
            "Strana štvorcovej podstavy ihlana",
            "Výška ihlana (od podstavy k vrcholu)"
        };

        public override string GetFormulas()
        {
            return "╔═══════════════════════════════════╗\n" +
                   "║    VZORCE - IHLAN (4-BOKÝ)        ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  Objem:    V = (1/3) × a² × v     ║\n" +
                   "║  Povrch:   S = a² + 2a × s        ║\n" +
                   "║  kde s = √((a/2)² + v²)           ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  a = strana podstavy              ║\n" +
                   "║  v = výška ihlana                 ║\n" +
                   "║  s = apotéma steny                ║\n" +
                   "╚═══════════════════════════════════╝";
        }

        public override double CalculateVolume(double[] parameters)
        {
            double a = parameters[0];
            double v = parameters[1];
            return (1.0 / 3.0) * a * a * v;
        }

        public override double CalculateSurfaceArea(double[] parameters)
        {
            double a = parameters[0];
            double v = parameters[1];
            // Apotéma bočnej steny
            double s = Math.Sqrt((a / 2) * (a / 2) + v * v);
            // Podstava + 4 trojuholníkové steny
            return a * a + 2 * a * s;
        }
    }
}
