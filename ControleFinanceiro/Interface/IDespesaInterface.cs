using ControleFinanceiro.Models.Despesa;

namespace ControleFinanceiro.Interface
{
    public interface IDespesaInterface
    {
        Task<int> CriarDespesa(int usuarioId, DespesaPost despesa);
    }
}
