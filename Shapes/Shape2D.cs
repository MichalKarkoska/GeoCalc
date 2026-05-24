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
        /// Implementácia Calculate pre 2D tvary.
        /// </summary>
        public override string Calculate(double[] parameters)
        {
            double area = CalculateArea(parameters);
            double perimeter = CalculatePerimeter(parameters);

            return $"═══════════════════════════════════\n" +
                   $"  VÝSLEDKY PRE: {Name.ToUpper()}\n" +
                   $"═══════════════════════════════════\n\n" +
                   $"  📐 Obsah (plocha):  {FormatNumber(area)} jednotiek²\n\n" +
                   $"  📏 Obvod:           {FormatNumber(perimeter)} jednotiek\n\n" +
                   $"═══════════════════════════════════";
        }
    }
}
