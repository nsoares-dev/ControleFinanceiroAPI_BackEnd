using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.Controllers
{
    [ApiController]
    [Route("API/Gasto")]
    public class GastoController : Controller
    {
        [HttpGet]
        public IActionResult GetGasto()
        {
            var gastos = new[]
            {
                new { Id = 1, Nome = "Cartão de Débito" },
                new { Id = 2, Nome = "Cartão de Crédito" },
                new { Id = 3, Nome = "PIX" }
            };

            return Ok(gastos);
        }
    }
}

