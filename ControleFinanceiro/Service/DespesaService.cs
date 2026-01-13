using ControleFinanceiro.Helpers;
using ControleFinanceiro.Interface;
using ControleFinanceiro.Models.Despesa;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Transactions;

namespace ControleFinanceiro.Service
{
    public class DespesaService : IDespesaInterface
    {

        private readonly IDbConnection _connection;

        public DespesaService(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> CriarDespesa(int usuarioId, DespesaPost despesa)
        {
            try
            {

                if (_connection.State != ConnectionState.Open)
                    await ((SqlConnection)_connection).OpenAsync();

                using var command = (SqlCommand)_connection.CreateStoredProcedure(Constantes.Constantes.CRIARDESPESA);
                command.Parameters.Add(new SqlParameter("@UsuarioId", usuarioId));
                command.Parameters.Add(new SqlParameter("@NomeDespesa", despesa.NomeDespesa));
                command.Parameters.Add(new SqlParameter("@Descricao", despesa.Descricao ?? ""));
                command.Parameters.Add(new SqlParameter("@Valor", despesa.Valor));
                command.Parameters.Add(new SqlParameter("@TipoGastoId", despesa.TipoGastoId));
                command.Parameters.Add(new SqlParameter("@DataDespesa", despesa.DataDespesa));
                command.Parameters.Add(new SqlParameter("@Pago", despesa.Pago));
                command.Parameters.Add(new SqlParameter("@Fixo", despesa.Fixo));

                var outputId = new SqlParameter("@DespesaId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                command.Parameters.Add(outputId);

                await command.ExecuteNonQueryAsync();

                if (despesa.TipoGastoId == Enum.TipoGasto.CARTAOCREDITO &&
                    despesa.TotalParcelas.HasValue &&
                    despesa.TotalParcelas > 1)
                {
                    using var cmdParcelas = _connection.CreateStoredProcedure(Constantes.Constantes.CRIARDESPESAPARCELADA);
                    cmdParcelas.Parameters.Add(new SqlParameter("@DespesaId", (int)outputId.Value));

                    cmdParcelas.Parameters.Add(new SqlParameter("@ValorTotal", despesa.Valor));

                    cmdParcelas.Parameters.Add(new SqlParameter("@TotalParcelas", despesa.TotalParcelas.Value));

                    cmdParcelas.Parameters.Add(new SqlParameter("@DataPrimeiroVencimento", despesa.DataPrimeiroVencimento ?? (object)DBNull.Value));


                    await cmdParcelas.ExecuteNonQueryAsync();
                }

                return (int)outputId.Value;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
