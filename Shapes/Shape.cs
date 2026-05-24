using System;

namespace GeoCalc.Shapes
{
    /// <summary>
    /// Abstraktná základná trieda pre všetky geometrické tvary.
    /// Definuje spoločné rozhranie a vlastnosti.
    /// </summary>
    public abstract class Shape
    {
        /// <summary>
        /// Názov tvaru pre zobrazenie v UI.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Názov súboru s obrázkom tvaru (bez cesty).
        /// </summary>
        public abstract string ImageName { get; }

        /// <summary>
        /// Pole názvov parametrov, ktoré tvar vyžaduje.
        /// </summary>
        public abstract string[] ParameterNames { get; }

        /// <summary>
        /// Pole popisov parametrov pre tooltipy.
        /// </summary>
        public abstract string[] ParameterDescriptions { get; }

        /// <summary>
        /// Vráti textový popis použitých vzorcov.
        /// </summary>
        public abstract string GetFormulas();

        /// <summary>
        /// Vykoná výpočty a vráti formátovaný výsledok.
        /// </summary>
        /// <param name="parameters">Pole hodnôt parametrov v rovnakom poradí ako ParameterNames.</param>
        /// <returns>Formátovaný reťazec s výsledkami výpočtov.</returns>
        public abstract string Calculate(double[] parameters);

        /// <summary>
        /// Validuje vstupné parametre.
        /// </summary>
        /// <param name="parameters">Pole hodnôt na validáciu.</param>
        /// <param name="errorMessage">Chybová správa ak validácia zlyhá.</param>
        /// <returns>True ak sú všetky parametre platné.</returns>
        public virtual bool ValidateParameters(double[] parameters, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (parameters == null || parameters.Length != ParameterNames.Length)
            {
                errorMessage = "Nesprávny počet parametrov.";
                return false;
            }

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] <= 0)
                {
                    errorMessage = $"Parameter '{ParameterNames[i]}' musí byť kladné číslo väčšie ako 0.";
                    return false;
                }

                if (double.IsNaN(parameters[i]) || double.IsInfinity(parameters[i]))
                {
                    errorMessage = $"Parameter '{ParameterNames[i]}' obsahuje neplatnú hodnotu.";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Formátuje číslo na zobrazenie s presnosťou na 4 desatinné miesta.
        /// </summary>
        protected string FormatNumber(double value)
        {
            return value.ToString("N4");
        }
    }
}
