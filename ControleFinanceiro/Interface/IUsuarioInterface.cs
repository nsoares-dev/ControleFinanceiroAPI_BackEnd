using ControleFinanceiro.Models.Usuario;

namespace ControleFinanceiro.Interface
{
    public interface IUsuarioInterface
    {
        Task CriarUsuario(UsuarioPost usuario);
        Task<List<UsuarioGet>> ConsultarUsuario(int usuarioId);
        Task<UsuarioLoginResponse> LoginUsuario(string loginOuEmail);
    }
}
