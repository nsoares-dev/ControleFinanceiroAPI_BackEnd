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

        #region Métodos Públicos

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

        public async Task<DashboardReports> DashboardReports(int usuarioId, string period)
        {
            var (inicio, fim) = Helpers.Helpers.CalcularPeriodo(period);

            var resumo = await GetResumo(usuarioId, inicio, fim);
            var categoria = await GetCategoria(usuarioId, inicio, fim);
            var cartao = await GetCartoes(usuarioId, inicio, fim);
            var topDespesas = await GetTopDespesas(usuarioId, inicio, fim);

            var tendencia = period == "ano" ? await GetTendenciaMensal(usuarioId, inicio.Year) 
                : await GetTendenciaDiaria(usuarioId, inicio, fim);

            return new DashboardReports
            {
                Resumo = resumo,
                PorCategoria = categoria,
                PorCartao = cartao,
                TopDespesas = topDespesas,
                TendenciaMensal = tendencia
            };
        }

        #endregion 

        #region Métodos Privados

        private async Task<ResumoReports> GetResumo(int usuarioId, DateTime inicio, DateTime fim)
        {
            if (_connection.State != ConnectionState.Open)
                await ((SqlConnection)_connection).OpenAsync();

            using var cmd = (SqlCommand)_connection.CreateStoredProcedure(Constantes.Constantes.DASHBOARDREPORTS_RESUMO);
            cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
            cmd.Parameters.AddWithValue("@DataInicio", inicio);
            cmd.Parameters.AddWithValue("@DataFim", fim);

            using var reader = await cmd.ExecuteReaderAsync();
            await reader.ReadAsync();

            var totalReceitas = reader.GetDecimal("TotalReceitas");
            var totalDespesas = reader.GetDecimal("TotalDespesas");


            return new ResumoReports
            {
                TotalReceitas = totalReceitas,
                TotalDespesas = totalDespesas,
                Saldo = totalReceitas - totalDespesas,
                TaxaEconomia = totalReceitas == 0
                    ? 0
                    : Math.Round(((totalReceitas - totalDespesas) / totalReceitas) * 100, 2)
            };
        }
        private async Task<List<Distribuicao>> GetCategoria(int usuarioId, DateTime inicio, DateTime fim)
        {
            if (_connection.State != ConnectionState.Open)
                await ((SqlConnection)_connection).OpenAsync();

            var lista = new List<Distribuicao>();

            using var cmd = (SqlCommand)_connection.CreateStoredProcedure(Constantes.Constantes.DASHBOARDREPORTS_CATEGORIA);

            cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
            cmd.Parameters.AddWithValue("@DataInicio", inicio);
            cmd.Parameters.AddWithValue("@DataFim", fim);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new Distribuicao
                {
                    Nome = reader.GetString("Nome"),
                    Valor = reader.GetDecimal("Valor")
                });
            }

            return lista;
        }
        private async Task<List<Distribuicao>> GetCartoes(int usuarioId, DateTime inicio, DateTime fim)
        {
            if (_connection.State != ConnectionState.Open)
                await ((SqlConnection)_connection).OpenAsync();

            var cartoes = new List<Distribuicao>();

            using var cmd = (SqlCommand)_connection.CreateStoredProcedure(Constantes.Constantes.DASHBOARDREPORTS_CARTAO);

            cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
            cmd.Parameters.AddWithValue("@DataInicio", inicio);
            cmd.Parameters.AddWithValue("@DataFim", fim);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                cartoes.Add(new Distribuicao
                {
                    Nome = reader.GetString(0),
                    Valor = reader.GetDecimal(1)
                });
            }

            return cartoes;
        }

        private async Task<List<TopDespesa>> GetTopDespesas(int usuarioId, DateTime inicio, DateTime fim)
        {
            if (_connection.State != ConnectionState.Open)
                await ((SqlConnection)_connection).OpenAsync();

            var despesas = new List<TopDespesa>();

            using var cmd = (SqlCommand)_connection.CreateStoredProcedure(Constantes.Constantes.DASHBOARDREPORTS_TOPDESPESAS);

            cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
            cmd.Parameters.AddWithValue("@DataInicio", inicio);
            cmd.Parameters.AddWithValue("@DataFim", fim);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                despesas.Add(new TopDespesa
                {
                    TransacaoId = reader.GetInt32(0),
                    Descricao = reader.GetString(1),
                    Valor = reader.GetDecimal(2),
                    Categoria = reader.GetString(3),
                    Cartao = reader.GetString(4),
                    Data = reader.GetDateTime(5)
                });
            }

            return despesas;
        }

        private async Task<List<TendenciaMensal>> GetTendenciaMensal(int usuarioId, int ano)
        {
            if (_connection.State != ConnectionState.Open)
                await ((SqlConnection)_connection).OpenAsync();

            var tendencias = new List<TendenciaMensal>();

            using var cmd = (SqlCommand)_connection.CreateStoredProcedure(Constantes.Constantes.DASHBOARDREPORTS_TENDENCIASMensal);

            cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
            cmd.Parameters.AddWithValue("@Ano", ano);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                tendencias.Add(new TendenciaMensal
                {
                    Mes = reader.GetString(0),
                    Receitas = reader.GetDecimal(1),
                    Despesas = reader.GetDecimal(2),
                });
            }

            return tendencias;
        }

        private async Task<List<TendenciaMensal>> GetTendenciaDiaria(int usuarioId, DateTime inicio, DateTime fim)
        {
            if (_connection.State != ConnectionState.Open)
                await ((SqlConnection)_connection).OpenAsync();

            var tendencias = new List<TendenciaMensal>();

            using var cmd = (SqlCommand)_connection.CreateStoredProcedure(Constantes.Constantes.DASHBOARDREPORTS_TENDENCIASWEEK);

            cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
            cmd.Parameters.AddWithValue("@DataInicio", inicio);
            cmd.Parameters.AddWithValue("@DataFim", fim);


            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                tendencias.Add(new TendenciaMensal
                {
                    Mes = reader.GetDateTime(0).ToString("dd/MM"),
                    Receitas = reader.GetDecimal(1),
                    Despesas = reader.GetDecimal(2),
                });
            }

            return tendencias;
        }

        #endregion
    }
}
