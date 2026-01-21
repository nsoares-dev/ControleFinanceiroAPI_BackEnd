using ControleFinanceiro.Models.Transacao;

namespace ControleFinanceiro.Interface
{
    public interface ITransacaoInterface
    {
        Task<int> CriarTransacao(int usuarioId, TransacaoPost transacao);
        Task<List<TransacaoGet>> ConsultarTransacoes(int usuarioId, int? transacaoId);
        Task<TransacaoDetalheResponse?> ConsultarTransacaoDetalhes(int usuarioId, int transacaoId);
    }
}
