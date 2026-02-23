namespace ControleFinanceiro.Models.Dashboard
{
    public class DashboardResponse
    {
        public decimal SaldoTotal { get; set; }
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
        public int Transacoes { get; set; }
        public GraficoReceitaDespesa GraficoReceitasDespesas { get; set; }
        public List<GastoPorCategoria> GastosPorCategoria { get; set; }
    }
    public class GraficoReceitaDespesa
    {
        public decimal Receitas { get; set; }
        public decimal Despesas { get; set; }
    }

    public class GastoPorCategoria
    {
        public string Categoria { get; set; }
        public decimal Valor { get; set; }
    }

    public class DashboardReports
    {
        public ResumoReports Resumo { get; set; }
        public List<TendenciaMensal> TendenciaMensal { get; set; }
        public List<Distribuicao> PorCategoria { get; set; }
        public List<Distribuicao> PorCartao { get; set; }
        public List<TopDespesa> TopDespesas { get; set; }
    }

    public class ResumoReports
    {
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
        public decimal Saldo { get; set; }
        public decimal TaxaEconomia { get; set; }
    }

    public class TendenciaMensal
    {
        public string Mes { get; set; }
        public decimal Receitas { get; set; }
        public decimal Despesas { get; set; }
    }

    public class Distribuicao
    {
        public string Nome { get; set; }
        public decimal Valor { get; set; }
    }

    public class TopDespesa
    {
        public int TransacaoId { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public string Categoria { get; set; }
        public string Cartao { get; set; }
        public DateTime Data { get; set; }
    }
}
