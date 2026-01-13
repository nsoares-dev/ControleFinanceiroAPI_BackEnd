using ControleFinanceiro.Enum;

namespace ControleFinanceiro.Models.Despesa
{
    public class DespesaPost
    {
        public string NomeDespesa { get; set; }
        public string? Descricao { get; set; }
        public decimal Valor { get; set; }
        public TipoGasto TipoGastoId { get; set; }
        public DateTime DataDespesa { get; set; }
        public bool Pago { get; set; }
        public bool Fixo { get; set; }
        public int? TotalParcelas { get; set; }
        public DateTime? DataPrimeiroVencimento { get; set; }
    }
}
