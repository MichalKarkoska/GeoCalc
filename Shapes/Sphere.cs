using System;

namespace GeoCalc.Shapes
{
    /// <summary>
    /// Trieda reprezentujúca guľu.
    /// </summary>
    public class Sphere : Shape3D
    {
        public override string Name => "Guľa";
        public override string ImageName => "sphere";
        public override string[] ParameterNames => new[] { "Polomer (r)" };
        public override string[] ParameterDescriptions => new[] { "Polomer gule" };

        public override string GetFormulas()
        {
            return "╔═══════════════════════════════════╗\n" +
                   "║          VZORCE - GUĽA            ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  Objem:    V = (4/3) × π × r³     ║\n" +
                   "║  Povrch:   S = 4 × π × r²         ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  r = polomer                      ║\n" +
                   "║  π ≈ 3.14159...                   ║\n" +
                   "╚═══════════════════════════════════╝";
        }

        public override double CalculateVolume(double[] parameters)
        {
            double r = parameters[0];
            return (4.0 / 3.0) * Math.PI * r * r * r;
        }

        public override double CalculateSurfaceArea(double[] parameters)
        {
            double r = parameters[0];
            return 4 * Math.PI * r * r;
        }
    }
}
