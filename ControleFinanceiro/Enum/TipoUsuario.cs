using System.ComponentModel;

namespace ControleFinanceiro.Enum
{
    public enum TipoUsuario
    {
        [Description("ADMIN")]
        ADMIN = 1,
        [Description("USUARIO_COMUM")]
        USUARIO_COMUM = 2
    }
}
