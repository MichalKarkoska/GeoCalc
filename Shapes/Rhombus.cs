namespace GeoCalc.Shapes
{
    /// <summary>
    /// Trieda reprezentujúca kosoštvorec.
    /// </summary>
    public class Rhombus : Shape2D
    {
        public override string Name => "Kosoštvorec";
        public override string ImageName => "rhombus";
        public override string[] ParameterNames => new[] { "Uhlopriečka d₁", "Uhlopriečka d₂", "Strana (a)" };
        public override string[] ParameterDescriptions => new[] 
        { 
            "Prvá uhlopriečka kosoštvorca",
            "Druhá uhlopriečka kosoštvorca",
            "Dĺžka strany kosoštvorca"
        };

        public override string GetFormulas()
        {
            return "╔═══════════════════════════════════╗\n" +
                   "║      VZORCE - KOSOŠTVOREC         ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  Obsah:   S = (d₁ × d₂) / 2       ║\n" +
                   "║  Obvod:   O = 4 × a               ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  d₁, d₂ = uhlopriečky             ║\n" +
                   "║  a = dĺžka strany                 ║\n" +
                   "╚═══════════════════════════════════╝";
        }

        public override double CalculateArea(double[] parameters)
        {
            double d1 = parameters[0];
            double d2 = parameters[1];
            return (d1 * d2) / 2;
        }

        public override double CalculatePerimeter(double[] parameters)
        {
            double a = parameters[2];
            return 4 * a;
        }
    }
}
