using ControleFinanceiro.Enum;

namespace ControleFinanceiro.Models.Transacao
{
    public class TransacaoPost
    {
        public string NomeTransacao { get; set; }
        public string? Descricao { get; set; }
        public decimal Valor { get; set; }
        public TipoGasto? TipoGastoId { get; set; }
        public TipoTransacao TipoTransacaoId { get; set; }
        public Cartao? CartaoId { get; set; }
        public TipoCategoria TipoCategoriaId { get; set; }
        public DateTime DataTransacao { get; set; }
        public bool Pago { get; set; }
        public bool Fixo { get; set; }
        public int? TotalParcelas { get; set; }
        public DateTime? DataPrimeiroVencimento { get; set; }
    }

    public class TransacaoGet
    {
        public int TransacaoId { get; set; }
        public string NomeTransacao { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public string? TipoGasto { get; set; }
        public string TipoTransacao { get; set; }
        public string? Cartao { get; set; }
        public DateTime DataTransacao { get; set; }
        public bool Pago { get; set; }
        public bool Fixo { get; set; }
        public string TipoCategoria { get; set; }
        public int TotalParcelas { get; set; }
    }

    public class TransacaoDetalheResponse
    {
        public int TransacaoId { get; set; }
        public string NomeTransacao { get; set; }
        public string? Descricao { get; set; }
        public decimal Valor { get; set; }
        public string? TipoGasto { get; set; }
        public string TipoTransacao { get; set; }
        public string TipoCategoria { get; set; }
        public string? Cartao { get; set; }
        public DateTime DataTransacao { get; set; }

        public List<TransacaoParcelasGet> Parcelas { get; set; }

        public class TransacaoParcelasGet
        {
            public int ParcelaId { get; set; }
            public int NumeroParcela { get; set; }
            public decimal ValorParcela { get; set; }
            public DateTime DataVencimento { get; set; }
            public bool Paga { get; set; }
        }
    }
}
