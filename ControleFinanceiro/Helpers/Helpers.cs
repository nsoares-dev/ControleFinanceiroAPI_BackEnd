using System.Data;
using System.Data.Common;

namespace ControleFinanceiro.Helpers
{
    public static class Helpers
    {
        public static DbCommand CreateStoredProcedure(this IDbConnection connection, string procedure)
        {
            var cmd = (DbCommand)connection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procedure;

            return cmd;
        }

        public static (DateTime inicio, DateTime fim) CalcularPeriodo(string period)
        {
            var hoje = DateTime.Today;
            DateTime inicio;

            switch (period?.ToLower())
            {
                case "semana":
                    inicio = hoje.AddDays(-6);
                    break;

                case "mes":
                    inicio = hoje.AddDays(-29);
                    break;

                case "ano":
                    inicio = hoje.AddDays(-364);
                    break;

                default:
                    throw new ArgumentException("Período inválido");
            }

            var fim = hoje.AddDays(1).AddTicks(-1);

            return (inicio, fim);
        }
    }
}
