using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.Controllers
{
    [ApiController]
    [Route("API/Categoria")]
    public class CategoriaController : Controller
    {
        [HttpGet]
        public IActionResult GetCategorias()
        {
            var categorias = new[]
            {
                new { Id = 1, Nome = "Alimentação" },
                new { Id = 2, Nome = "Contas" },
                new { Id = 3, Nome = "Transporte" },
                new { Id = 5, Nome = "Salário" },
                new { Id = 6, Nome = "Saúde" },
                new { Id = 7, Nome = "Freelance" },
                new { Id = 8, Nome = "PIX" },
                new { Id = 9, Nome = "Eletrônicos" },
                new { Id = 10, Nome = "Outros" }
            };

            return Ok(categorias);
        }
    }
}
