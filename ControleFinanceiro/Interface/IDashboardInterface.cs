using ControleFinanceiro.Models.Dashboard;

namespace ControleFinanceiro.Interface
{
    public interface IDashboardInterface
    {
        Task<DashboardResponse> ObterDashboard(int usuarioId);
        Task<DashboardReports> DashboardReports(int usuarioId, string period);
    }
}
