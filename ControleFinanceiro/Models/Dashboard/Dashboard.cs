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
}
