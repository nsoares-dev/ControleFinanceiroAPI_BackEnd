using System.ComponentModel;

namespace ControleFinanceiro.Enum
{
    public enum TipoTransacao
    {
        [Description("Receita")]
        Receita = 1,
        [Description("Despesa")]
        Despesa = 2
    }
}
