namespace GeoCalc.Shapes
{
    /// <summary>
    /// Trieda reprezentujúca kváder.
    /// </summary>
    public class Cuboid : Shape3D
    {
        public override string Name => "Kváder";
        public override string ImageName => "cuboid";
        public override string[] ParameterNames => new[] { "Dĺžka (a)", "Šírka (b)", "Výška (c)" };
        public override string[] ParameterDescriptions => new[] 
        { 
            "Dĺžka kvádra",
            "Šírka kvádra",
            "Výška kvádra"
        };

        public override string GetFormulas()
        {
            return "╔═══════════════════════════════════╗\n" +
                   "║         VZORCE - KVÁDER           ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  Objem:    V = a × b × c          ║\n" +
                   "║  Povrch:   S = 2(ab + bc + ac)    ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  a = dĺžka                        ║\n" +
                   "║  b = šírka                        ║\n" +
                   "║  c = výška                        ║\n" +
                   "╚═══════════════════════════════════╝";
        }

        public override double CalculateVolume(double[] parameters)
        {
            double a = parameters[0];
            double b = parameters[1];
            double c = parameters[2];
            return a * b * c;
        }

        public override double CalculateSurfaceArea(double[] parameters)
        {
            double a = parameters[0];
            double b = parameters[1];
            double c = parameters[2];
            return 2 * (a * b + b * c + a * c);
        }
    }
}
