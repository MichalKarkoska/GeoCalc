namespace GeoCalc.Shapes
{
    /// <summary>
    /// Trieda reprezentujúca obdĺžnik.
    /// </summary>
    public class Rectangle : Shape2D
    {
        public override string Name => "Obdĺžnik";
        public override string ImageName => "rectangle";
        public override string[] ParameterNames => new[] { "Šírka (a)", "Výška (b)" };
        public override string[] ParameterDescriptions => new[] 
        { 
            "Šírka obdĺžnika (vodorovná strana)", 
            "Výška obdĺžnika (zvislá strana)" 
        };

        public override string GetFormulas()
        {
            return "╔═══════════════════════════════════╗\n" +
                   "║       VZORCE - OBDĹŽNIK           ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  Obsah:   S = a × b               ║\n" +
                   "║  Obvod:   O = 2 × (a + b)         ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  a = šírka                        ║\n" +
                   "║  b = výška                        ║\n" +
                   "╚═══════════════════════════════════╝";
        }

        public override double CalculateArea(double[] parameters)
        {
            double a = parameters[0];
            double b = parameters[1];
            return a * b;
        }

        public override double CalculatePerimeter(double[] parameters)
        {
            double a = parameters[0];
            double b = parameters[1];
            return 2 * (a + b);
        }
    }
}
