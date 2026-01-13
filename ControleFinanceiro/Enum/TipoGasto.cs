using System.ComponentModel;

namespace ControleFinanceiro.Enum
{
    public enum TipoGasto
    {
        [Description("Débito")]
        CARTAODEBITO = 1,
        [Description("Crédito")]
        CARTAOCREDITO = 2,
        [Description("Pix")]
        PIX = 3
    }
}
