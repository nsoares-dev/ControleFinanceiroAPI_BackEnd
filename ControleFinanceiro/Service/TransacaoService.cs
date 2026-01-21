using ControleFinanceiro.Enum;
using ControleFinanceiro.Helpers;
using ControleFinanceiro.Interface;
using ControleFinanceiro.Models.Transacao;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Transactions;
using static ControleFinanceiro.Models.Transacao.TransacaoDetalheResponse;

namespace ControleFinanceiro.Service
{
    public class TransacaoService : ITransacaoInterface
    {

        private readonly IDbConnection _connection;

        public TransacaoService(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> CriarTransacao(int usuarioId, TransacaoPost transacao)
        {
            try
            {

                if (_connection.State != ConnectionState.Open)
                    await ((SqlConnection)_connection).OpenAsync();

                using var command = (SqlCommand)_connection.CreateStoredProcedure(Constantes.Constantes.CRIARTRANSACAO);
                command.Parameters.Add(new SqlParameter("@UsuarioId", usuarioId));
                command.Parameters.Add(new SqlParameter("@NomeTransacao", transacao.NomeTransacao));
                command.Parameters.Add(new SqlParameter("@Descricao", transacao.Descricao ?? ""));
                command.Parameters.Add(new SqlParameter("@Valor", transacao.Valor));
                command.Parameters.Add(new SqlParameter("@TipoGastoId", transacao.TipoGastoId));
                command.Parameters.Add(new SqlParameter("@CartaoId", transacao.CartaoId));
                command.Parameters.Add(new SqlParameter("@TipoCategoriaId", transacao.TipoCategoriaId));
                command.Parameters.Add(new SqlParameter("@TipoTransacaoId", transacao.TipoTransacaoId));
                command.Parameters.Add(new SqlParameter("@DataTransacao", transacao.DataTransacao));
                command.Parameters.Add(new SqlParameter("@Pago", transacao.Pago));
                command.Parameters.Add(new SqlParameter("@Fixo", transacao.Fixo));

                var outputId = new SqlParameter("@TransacaoId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                command.Parameters.Add(outputId);

                await command.ExecuteNonQueryAsync();

                if (transacao.TipoGastoId == Enum.TipoGasto.CARTAOCREDITO &&
                    transacao.TotalParcelas.HasValue &&
                    transacao.TipoTransacaoId == Enum.TipoTransacao.Despesa &&
                    transacao.TotalParcelas > 1)
                {
                    using var cmdParcelas = _connection.CreateStoredProcedure(Constantes.Constantes.CRIARTRANSACAOPARCELADA);
                    cmdParcelas.Parameters.Add(new SqlParameter("@TransacaoId", (int)outputId.Value));

                    cmdParcelas.Parameters.Add(new SqlParameter("@ValorTotal", transacao.Valor));

                    cmdParcelas.Parameters.Add(new SqlParameter("@TotalParcelas", transacao.TotalParcelas.Value));

                    cmdParcelas.Parameters.Add(new SqlParameter("@DataPrimeiroVencimento", transacao.DataPrimeiroVencimento ?? (object)DBNull.Value));


                    await cmdParcelas.ExecuteNonQueryAsync();
                }

                return (int)outputId.Value;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<TransacaoGet>> ConsultarTransacoes(int usuarioId, int? transacaoId)
        {

            if (_connection.State != ConnectionState.Open)
                await ((SqlConnection)_connection).OpenAsync();

            var transacoes = new List<TransacaoGet>();

            using var command = (SqlCommand)_connection.CreateStoredProcedure(Constantes.Constantes.CONSULTARTRANSACOES);
            command.Parameters.Add(new SqlParameter("@UsuarioId", usuarioId));

            if (transacaoId > 0)
                command.Parameters.Add(new SqlParameter("@TransacaoId", transacaoId));

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                transacoes.Add(new TransacaoGet
                {
                    TransacaoId = reader.GetInt32(reader.GetOrdinal("TransacaoId")),
                    NomeTransacao = reader.IsDBNull(reader.GetOrdinal("NomeTransacao")) ? null : reader.GetString(reader.GetOrdinal("NomeTransacao")),
                    Descricao = reader.IsDBNull(reader.GetOrdinal("Descricao")) ? null : reader.GetString(reader.GetOrdinal("Descricao")),
                    Valor = reader.GetDecimal(reader.GetOrdinal("Valor")),
                    TipoGasto = reader.IsDBNull(reader.GetOrdinal("TipoGasto")) ? null : reader.GetString(reader.GetOrdinal("TipoGasto")),
                    TipoTransacao = reader.IsDBNull(reader.GetOrdinal("TipoTransacao")) ? null : reader.GetString(reader.GetOrdinal("TipoTransacao")),
                    Cartao = reader.IsDBNull(reader.GetOrdinal("Cartao")) ? null : reader.GetString(reader.GetOrdinal("Cartao")),
                    DataTransacao = reader.GetDateTime(reader.GetOrdinal("DataTransacao")),
                    Pago = reader.IsDBNull(reader.GetOrdinal("Pago")) ? false : reader.GetBoolean(reader.GetOrdinal("Pago")),
                    Fixo = reader.IsDBNull(reader.GetOrdinal("Fixo")) ? false : reader.GetBoolean(reader.GetOrdinal("Fixo")),
                    TipoCategoria = reader.IsDBNull(reader.GetOrdinal("TipoCategoria")) ? null : reader.GetString(reader.GetOrdinal("TipoCategoria")),
                    TotalParcelas = reader.GetInt32(reader.GetOrdinal("QtdParcelas"))
                });
            }

            return transacoes;

        }
        public async Task<TransacaoDetalheResponse?> ConsultarTransacaoDetalhes( int usuarioId, int transacaoId)
        {
            TransacaoDetalheResponse? transacao = null;

            if (_connection.State != ConnectionState.Open)
                await ((SqlConnection)_connection).OpenAsync();

            using var command = (SqlCommand)_connection
                .CreateStoredProcedure(Constantes.Constantes.CONSULTARDETALHESTRANSACAO);

            command.Parameters.Add(new SqlParameter("@TransacaoId", transacaoId));
            //command.Parameters.Add(new SqlParameter("@UsuarioId", usuarioId));

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                if (transacao == null)
                {
                    transacao = new TransacaoDetalheResponse
                    {
                        TransacaoId = reader.GetInt32(reader.GetOrdinal("TransacaoId")),
                        NomeTransacao = reader.IsDBNull(reader.GetOrdinal("NomeTransacao")) ? null : reader.GetString(reader.GetOrdinal("NomeTransacao")),
                        Descricao = reader.IsDBNull(reader.GetOrdinal("Descricao")) ? null : reader.GetString(reader.GetOrdinal("Descricao")),
                        Valor = reader.GetDecimal(reader.GetOrdinal("Valor")),
                        TipoGasto = reader.IsDBNull(reader.GetOrdinal("TipoGasto")) ? null : reader.GetString(reader.GetOrdinal("TipoGasto")),
                        TipoTransacao = reader.IsDBNull(reader.GetOrdinal("TipoTransacao")) ? null : reader.GetString(reader.GetOrdinal("TipoTransacao")),
                        TipoCategoria = reader.IsDBNull(reader.GetOrdinal("TipoCategoria")) ? null : reader.GetString(reader.GetOrdinal("TipoCategoria")),
                        Cartao = reader.IsDBNull(reader.GetOrdinal("Cartao")) ? null : reader.GetString(reader.GetOrdinal("Cartao")),
                        DataTransacao = reader.GetDateTime(reader.GetOrdinal("DataTransacao")),
                        Parcelas = new List<TransacaoParcelasGet>()
                    };
                }

                var parcelaIdOrdinal = reader.GetOrdinal("ParcelaId");

                // Se tiver parcelas
                if (!reader.IsDBNull(parcelaIdOrdinal))
                {
                    transacao.Parcelas.Add(new TransacaoParcelasGet
                    {
                        ParcelaId = reader.GetInt32(parcelaIdOrdinal),

                        NumeroParcela = reader.GetInt32(reader.GetOrdinal("NumeroParcela")),

                        ValorParcela = reader.GetDecimal(reader.GetOrdinal("ValorParcela")),

                        DataVencimento = reader.GetDateTime(reader.GetOrdinal("DataVencimento")),

                        Paga = reader.IsDBNull(reader.GetOrdinal("Paga"))
                            ? false
                            : reader.GetBoolean(reader.GetOrdinal("Paga"))
                    });
                }
            }

            return transacao;
        }

    }
}
