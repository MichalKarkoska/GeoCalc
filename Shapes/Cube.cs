namespace GeoCalc.Shapes
{
    /// <summary>
    /// Trieda reprezentujúca kocku.
    /// </summary>
    public class Cube : Shape3D
    {
        public override string Name => "Kocka";
        public override string ImageName => "cube";
        public override string[] ParameterNames => new[] { "Hrana (a)" };
        public override string[] ParameterDescriptions => new[] { "Dĺžka hrany kocky" };

        public override string GetFormulas()
        {
            return "╔═══════════════════════════════════╗\n" +
                   "║         VZORCE - KOCKA            ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  Objem:    V = a³                 ║\n" +
                   "║  Povrch:   S = 6 × a²             ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  a = dĺžka hrany                  ║\n" +
                   "╚═══════════════════════════════════╝";
        }

        public override double CalculateVolume(double[] parameters)
        {
            double a = parameters[0];
            return a * a * a;
        }

        public override double CalculateSurfaceArea(double[] parameters)
        {
            double a = parameters[0];
            return 6 * a * a;
        }
    }
}
