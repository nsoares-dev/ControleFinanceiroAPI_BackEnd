using ControleFinanceiro.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ControleFinanceiro.Controllers
{
    [Authorize]
    [ApiController]
    [Route("API/Dashboard")]
    public class DashboardController : Controller
    {
        private readonly IDashboardInterface _dashboardService;

        public DashboardController(IDashboardInterface dashboardService)
        {
            _dashboardService = dashboardService;
        }


        [HttpGet]
        [Route("DashboardInicio")]
        public async Task<IActionResult> ObterDashboardInicio()
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                var dash = await _dashboardService.ObterDashboard(usuarioId);

                return Ok(dash);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensagem = "Erro interno do servidor.",
                    detalhe = ex.Message
                });
            }

        }
    }
}
