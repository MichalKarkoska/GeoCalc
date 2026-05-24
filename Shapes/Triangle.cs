using System;

namespace GeoCalc.Shapes
{
    /// <summary>
    /// Trieda reprezentujúca trojuholník.
    /// Používa Heronov vzorec pre výpočet obsahu.
    /// </summary>
    public class Triangle : Shape2D
    {
        public override string Name => "Trojuholník";
        public override string ImageName => "triangle";
        public override string[] ParameterNames => new[] { "Strana a", "Strana b", "Strana c" };
        public override string[] ParameterDescriptions => new[] 
        { 
            "Prvá strana trojuholníka",
            "Druhá strana trojuholníka",
            "Tretia strana trojuholníka"
        };

        public override string GetFormulas()
        {
            return "╔═══════════════════════════════════╗\n" +
                   "║      VZORCE - TROJUHOLNÍK         ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  Obvod:   O = a + b + c           ║\n" +
                   "║                                   ║\n" +
                   "║  Heronov vzorec pre obsah:        ║\n" +
                   "║  s = (a + b + c) / 2              ║\n" +
                   "║  S = √(s(s-a)(s-b)(s-c))          ║\n" +
                   "╠═══════════════════════════════════╣\n" +
                   "║  a, b, c = strany trojuholníka    ║\n" +
                   "║  s = polomer obvodu               ║\n" +
                   "╚═══════════════════════════════════╝";
        }

        public override bool ValidateParameters(double[] parameters, out string errorMessage)
        {
            // Najprv základná validácia
            if (!base.ValidateParameters(parameters, out errorMessage))
                return false;

            // Kontrola trojuholníkovej nerovnosti
            double a = parameters[0];
            double b = parameters[1];
            double c = parameters[2];

            if (a + b <= c || a + c <= b || b + c <= a)
            {
                errorMessage = "Zadané strany nemôžu tvoriť trojuholník!\n" +
                              "Súčet dvoch strán musí byť väčší ako tretia strana.";
                return false;
            }

            return true;
        }

        public override double CalculateArea(double[] parameters)
        {
            double a = parameters[0];
            double b = parameters[1];
            double c = parameters[2];
            
            // Heronov vzorec
            double s = (a + b + c) / 2;
            return Math.Sqrt(s * (s - a) * (s - b) * (s - c));
        }

        public override double CalculatePerimeter(double[] parameters)
        {
            return parameters[0] + parameters[1] + parameters[2];
        }
    }
}
