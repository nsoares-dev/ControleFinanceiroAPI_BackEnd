namespace ControleFinanceiro.Constantes
{
    public class Constantes
    {
        #region Usuário
        public const string CRIARUSUARIO = "CF_CriarUsuario_C";
        public const string CONSULTARUSUARIO = "CF_ConsultarUsuario_R";
        public const string LOGINUSUARIO = "CF_LoginUsuario_R";
        #endregion

        #region Despesa

        public const string CRIARTRANSACAO = "CF_CriarTransacao_C";
        public const string CRIARTRANSACAOPARCELADA = "CF_CriarTransacaoParcelada_C";
        public const string CONSULTARTRANSACOES = "CF_ConsultarTransacoes_R";
        public const string CONSULTARDETALHESTRANSACAO = "CF_ConsultarDetalhesTransacao_R";

        #endregion

        #region Dashboard

        public const string DASHBOARDINICIO = "CF_DashBoardInicio_R";
        public const string DASHBOARDREPORTS_RESUMO = "CF_DashBoardReportsResumo_R";
        public const string DASHBOARDREPORTS_CATEGORIA = "CF_DashBoardReportsCategorias_R";
        public const string DASHBOARDREPORTS_CARTAO = "CF_DashBoardReportsCartao_R";
        public const string DASHBOARDREPORTS_TOPDESPESAS = "CF_DashBoardReportsTopDespesas_R";
        public const string DASHBOARDREPORTS_TENDENCIASWEEK = "CF_DashBoardReportsTendenciasWeek_R";
        public const string DASHBOARDREPORTS_TENDENCIASMensal = "CF_DashBoardReportsTendenciasMensal_R";
        #endregion
    }
}
