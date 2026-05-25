namespace GeoCalc.Shapes
{
    /// <summary>
    /// Abstraktná trieda pre 2D tvary.
    /// Rozširuje základnú triedu Shape o špecifické metódy pre 2D geometriu.
    /// </summary>
    public abstract class Shape2D : Shape
    {
        /// <summary>
        /// Vypočíta obsah (plochu) tvaru.
        /// </summary>
        public abstract double CalculateArea(double[] parameters);

        /// <summary>
        /// Vypočíta obvod tvaru.
        /// </summary>
        public abstract double CalculatePerimeter(double[] parameters);

        /// <summary>
        /// Implementácia Calculate pre 2D tvary s výberom typu výpočtu.
        /// calculationType: 0 = Obvod, 1 = Obsah
        /// </summary>
        public override string Calculate(double[] parameters, int calculationType)
        {
            string resultTitle;
            string resultValue;
            string resultUnit = "jednotiek";

            if (calculationType == 0)
            {
                // Výpočet obvodu
                double perimeter = CalculatePerimeter(parameters);
                resultTitle = "📏 OBVOD";
                resultValue = $"{FormatNumber(perimeter)} {resultUnit}";
            }
            else
            {
                // Výpočet obsahu (plošného obsahu)
                double area = CalculateArea(parameters);
                resultTitle = "📐 OBSAH (PLOCHA)";
                resultValue = $"{FormatNumber(area)} {resultUnit}²";
            }

            return $"═══════════════════════════════════════════════\n" +
                   $"  ✨ VÝSLEDKY PRE: {Name.ToUpper()}\n" +
                   $"═══════════════════════════════════════════════\n\n" +
                   $"  {resultTitle}\n" +
                   $"  {resultValue}\n\n" +
                   $"═══════════════════════════════════════════════";
        }

        /// <summary>
        /// Súbežná metóda Calculate bez calculationType parametra (pre spätnu kompatibilitu).
        /// </summary>
        public override string Calculate(double[] parameters)
        {
            return Calculate(parameters, 1); // Predvolene vypočítaj obsah
        }
    }
}
