namespace GeoCalc.Shapes
{
    /// <summary>
    /// Abstraktná trieda pre 3D tvary.
    /// Rozširuje základnú triedu Shape o špecifické metódy pre 3D geometriu.
    /// </summary>
    public abstract class Shape3D : Shape
    {
        /// <summary>
        /// Vypočíta objem telesa.
        /// </summary>
        public abstract double CalculateVolume(double[] parameters);

        /// <summary>
        /// Vypočíta povrch telesa.
        /// </summary>
        public abstract double CalculateSurfaceArea(double[] parameters);

        /// <summary>
        /// Implementácia Calculate pre 3D tvary s výberom typu výpočtu.
        /// calculationType: 0 = Objem, 1 = Povrch
        /// </summary>
        public override string Calculate(double[] parameters, int calculationType)
        {
            string resultTitle;
            string resultValue;

            if (calculationType == 0)
            {
                // Výpočet objemu
                double volume = CalculateVolume(parameters);
                resultTitle = "🎲 OBJEM";
                resultValue = $"{FormatNumber(volume)} jednotiek³";
            }
            else
            {
                // Výpočet povrchu
                double surfaceArea = CalculateSurfaceArea(parameters);
                resultTitle = "🔲 POVRCH";
                resultValue = $"{FormatNumber(surfaceArea)} jednotiek²";
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
            return Calculate(parameters, 0); // Predvolene vypočítaj objem
        }
    }
}
