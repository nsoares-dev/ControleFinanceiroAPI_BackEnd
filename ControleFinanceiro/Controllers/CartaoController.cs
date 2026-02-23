using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.Controllers
{
    [ApiController]
    [Route("API/Cartao")]
    public class CartaoController : Controller
    {
        [HttpGet]

        public IActionResult GetCartoes()
        {
            var cartoes = new[]
            {
                new { Id = 1, Nome = "NUBANK" },
                new { Id = 2, Nome = "INTER" },
                new { Id = 3, Nome = "MERCADOPAGO" },
                new { Id = 4, Nome = "ITAU" }
            };

            return Ok(cartoes);
        }
    }
}
