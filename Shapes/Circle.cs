namespace GeoCalc.Shapes
{
    /// <summary>
    /// Trieda reprezentujúca kruh.
    /// </summary>
    public class Circle : Shape2D
    {
        public override string Name => "Kruh";
        public override string ImageName => "circle";
        public override string[] ParameterNames => new[] { "Polomer (r)" };
        public override string[] ParameterDescriptions => new[] { "Polomer kruhu - vzdialenosť od stredu k obvodu" };

        public override string GetFormulas()
        {
            return "╔═══════════════════════════════════╗\n" +
                   "║         VZORCE - KRUH             ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  Obsah:   S = π × r²              ║\n" +
                   "║  Obvod:   O = 2 × π × r           ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  r = polomer                      ║\n" +
                   "║  π ≈ 3.14159...                   ║\n" +
                   "╚═══════════════════════════════════╝";
        }

        public override double CalculateArea(double[] parameters)
        {
            double r = parameters[0];
            return System.Math.PI * r * r;
        }

        public override double CalculatePerimeter(double[] parameters)
        {
            double r = parameters[0];
            return 2 * System.Math.PI * r;
        }
    }
}
