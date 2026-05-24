namespace GeoCalc.Shapes
{
    /// <summary>
    /// Trieda reprezentujúca štvorec.
    /// </summary>
    public class Square : Shape2D
    {
        public override string Name => "Štvorec";
        public override string ImageName => "square";
        public override string[] ParameterNames => new[] { "Strana (a)" };
        public override string[] ParameterDescriptions => new[] { "Dĺžka strany štvorca" };

        public override string GetFormulas()
        {
            return "╔═══════════════════════════════════╗\n" +
                   "║        VZORCE - ŠTVOREC           ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  Obsah:   S = a²                  ║\n" +
                   "║  Obvod:   O = 4 × a               ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  a = dĺžka strany                 ║\n" +
                   "╚═══════════════════════════════════╝";
        }

        public override double CalculateArea(double[] parameters)
        {
            double a = parameters[0];
            return a * a;
        }

        public override double CalculatePerimeter(double[] parameters)
        {
            double a = parameters[0];
            return 4 * a;
        }
    }
}
