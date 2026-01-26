using ControleFinanceiro.Helpers;
using ControleFinanceiro.Interface;
using ControleFinanceiro.Models.Dashboard;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ControleFinanceiro.Service
{
    public class DashboardService : IDashboardInterface
    {
        private readonly IDbConnection _connection;

        public DashboardService(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<DashboardResponse> ObterDashboard(int usuarioId)
        {
            if (_connection.State != ConnectionState.Open)
                await ((SqlConnection)_connection).OpenAsync();

            using var cmd = (SqlCommand)_connection.CreateStoredProcedure(Constantes.Constantes.DASHBOARDINICIO);
            cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);

            using var reader = await cmd.ExecuteReaderAsync();

            var resumo = new DashboardResponse
            {
                GastosPorCategoria = new List<GastoPorCategoria>()
            };

            if (await reader.ReadAsync())
            {
                resumo.TotalReceitas = reader.GetDecimal("TotalReceitas");
                resumo.TotalDespesas = reader.GetDecimal("TotalDespesas");
                resumo.SaldoTotal = reader.GetDecimal("SaldoTotal");
                resumo.Transacoes = reader.GetInt32("Transacoes");

                resumo.GraficoReceitasDespesas = new GraficoReceitaDespesa
                {
                    Receitas = resumo.TotalReceitas,
                    Despesas = resumo.TotalDespesas
                };
            }

            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    resumo.GastosPorCategoria.Add(new GastoPorCategoria
                    {
                        Categoria = reader.GetString("Categoria"),
                        Valor = reader.GetDecimal("Valor")
                    });
                }
            }

            return resumo;
        }
    }
}
