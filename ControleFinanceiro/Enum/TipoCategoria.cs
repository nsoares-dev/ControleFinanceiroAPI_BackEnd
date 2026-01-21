using System.ComponentModel;

namespace ControleFinanceiro.Enum
{
    public enum TipoCategoria
    {
        [Description("Alimentação")]
        ALIMENTACAO = 1,
        [Description("Contas")]
        CONTAS = 2,
        [Description("Transporte")]
        TRANSPORTE = 3,
        [Description("Entretenimento")]
        ENTRETENIMENTO = 4,
        [Description("Salário")]
        SALARIO = 5,
        [Description("Saúde")]
        SAUDE = 6,
        [Description("Freelance")]
        FREELANCE = 7,
        [Description("Pix")]
        PIX = 8,
        [Description("Eletrônicos")]
        ELETRONICOS = 9,
        [Description("Outros")]
        OUTROS = 10,
    }
}
