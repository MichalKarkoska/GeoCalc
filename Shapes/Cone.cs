using System;

namespace GeoCalc.Shapes
{
    /// <summary>
    /// Trieda reprezentujúca kužeľ.
    /// </summary>
    public class Cone : Shape3D
    {
        public override string Name => "Kužeľ";
        public override string ImageName => "cone";
        public override string[] ParameterNames => new[] { "Polomer (r)", "Výška (v)" };
        public override string[] ParameterDescriptions => new[] 
        { 
            "Polomer podstavy kužeľa",
            "Výška kužeľa"
        };

        public override string GetFormulas()
        {
            return "╔═══════════════════════════════════╗\n" +
                   "║         VZORCE - KUŽEĽ            ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  Objem:    V = (1/3) × π × r² × v ║\n" +
                   "║  Povrch:   S = πr(r + s)          ║\n" +
                   "║  kde s = √(r² + v²)               ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  r = polomer podstavy             ║\n" +
                   "║  v = výška                        ║\n" +
                   "║  s = strana (tvoriacej)           ║\n" +
                   "╚═══════════════════════════════════╝";
        }

        public override double CalculateVolume(double[] parameters)
        {
            double r = parameters[0];
            double v = parameters[1];
            return (1.0 / 3.0) * Math.PI * r * r * v;
        }

        public override double CalculateSurfaceArea(double[] parameters)
        {
            double r = parameters[0];
            double v = parameters[1];
            double s = Math.Sqrt(r * r + v * v); // strana (slant height)
            return Math.PI * r * (r + s);
        }
    }
}
