using System.Globalization;

namespace GeoCalc.Helpers
{
    /// <summary>
    /// Pomocná trieda pre validáciu a parsovanie vstupov.
    /// </summary>
    public static class InputValidator
    {
        /// <summary>
        /// Pokúsi sa parsovať textový vstup na číslo.
        /// Podporuje desatinnú čiarku aj bodku.
        /// </summary>
        /// <param name="input">Textový vstup.</param>
        /// <param name="result">Výsledná hodnota ak je parsovanie úspešné.</param>
        /// <returns>True ak sa podarilo parsovať.</returns>
        public static bool TryParseNumber(string input, out double result)
        {
            result = 0;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            // Nahradiť čiarku bodkou pre konzistentné parsovanie
            string normalized = input.Trim().Replace(',', '.');

            // Skúsiť parsovať s invariant culture (bodka ako desatinný oddeľovač)
            return double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
        }

        /// <summary>
        /// Skontroluje či je text prázdny alebo obsahuje iba biele znaky.
        /// </summary>
        public static bool IsEmpty(string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }
    }
}
