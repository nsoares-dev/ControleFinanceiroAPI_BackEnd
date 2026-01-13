using ControleFinanceiro.Interface;
using ControleFinanceiro.Models.Despesa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ControleFinanceiro.Controllers
{
    [Authorize]
    [ApiController]
    [Route("API/Despesa")]
    public class DespesaController : Controller
    {
        private readonly IDespesaInterface _despesaService;
        public DespesaController(IDespesaInterface despesaService)
        {
            _despesaService = despesaService;
        }


        [HttpPost]
        [Route("CriarDespesa")]
        public async Task<IActionResult> CriarDespesa([FromBody] DespesaPost despesa)
        {
            try
            {
                if (despesa == null)
                    return BadRequest("Dados da despesa não fornecidos.");

                var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                var despesaId = await _despesaService.CriarDespesa(usuarioId, despesa);

                return StatusCode(201, new
                {
                    mensagem = "Despesa criada com sucesso.",
                    DespesaId = despesaId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }
    }
}
