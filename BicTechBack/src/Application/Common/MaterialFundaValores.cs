namespace BicTechBack.src.Core.Common
{
    /// <summary>
    /// Valores permitidos para el material de funda en productos.
    /// </summary>
    public static class MaterialFundaValores
    {
        public const string Silicona = "Silicona";
        public const string Otro = "Otro";

        public static bool EsValido(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return false;
            return valor.Equals(Silicona, StringComparison.OrdinalIgnoreCase)
                || valor.Equals(Otro, StringComparison.OrdinalIgnoreCase);
        }

        public static string? Normalizar(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return null;
            if (valor.Equals(Silicona, StringComparison.OrdinalIgnoreCase)) return Silicona;
            if (valor.Equals(Otro, StringComparison.OrdinalIgnoreCase)) return Otro;
            return valor.Trim();
        }
    }
}
