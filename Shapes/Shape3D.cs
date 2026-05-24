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
        /// Implementácia Calculate pre 3D tvary.
        /// </summary>
        public override string Calculate(double[] parameters)
        {
            double volume = CalculateVolume(parameters);
            double surfaceArea = CalculateSurfaceArea(parameters);

            return $"═══════════════════════════════════\n" +
                   $"  VÝSLEDKY PRE: {Name.ToUpper()}\n" +
                   $"═══════════════════════════════════\n\n" +
                   $"  📦 Objem:    {FormatNumber(volume)} jednotiek³\n\n" +
                   $"  🎨 Povrch:   {FormatNumber(surfaceArea)} jednotiek²\n\n" +
                   $"═══════════════════════════════════";
        }
    }
}
