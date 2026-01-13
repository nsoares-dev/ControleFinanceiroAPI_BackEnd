using System.ComponentModel;
using System.Reflection;

namespace ControleFinanceiro.Util
{
    public static class Util
    {
        public static async Task<byte[]?> ConverterParaBase64(IFormFile? arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
                return null;

            using var ms = new MemoryStream();
            await arquivo.CopyToAsync(ms);

            return ms.ToArray();
        }

        public static string GetDescription(this System.Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            if (field == null)
                return value.ToString();

            var attribute = field.GetCustomAttribute<DescriptionAttribute>();

            return attribute?.Description ?? value.ToString();
        }

    }
}

