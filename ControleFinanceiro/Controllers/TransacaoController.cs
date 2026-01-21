using ControleFinanceiro.Interface;
using ControleFinanceiro.Models.Transacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ControleFinanceiro.Controllers
{
    [Authorize]
    [ApiController]
    [Route("API/Transacao")]
    public class TransacaoController : Controller
    {
        private readonly ITransacaoInterface _transacaoService;
        public TransacaoController(ITransacaoInterface transacaoService)
        {
            _transacaoService = transacaoService;
        }


        [HttpPost]
        [Route("CriarTransacao")]
        public async Task<IActionResult> CriarTransacao([FromBody] TransacaoPost transacao)
        {
            try
            {
                if (transacao == null)
                    return BadRequest("Dados da despesa não fornecidos.");

                var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                var transacaoId = await _transacaoService.CriarTransacao(usuarioId, transacao);

                return StatusCode(201, new
                {
                    mensagem = "Despesa criada com sucesso.",
                    transacaoId = transacaoId
                });
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

        [HttpGet]
        [Route("ConsultarTransacoes/{transacaoId}")]
        public async Task<IActionResult> ConsultarTransacoes(int? transacaoId)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var transacoes = await _transacaoService.ConsultarTransacoes(usuarioId, transacaoId);

                return Ok(transacoes);
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
        [HttpGet]
        [Route("ConsultarTransacoesDetalhes/{transacaoId}")]
        public async Task<IActionResult> ConsultarTransacoesDetalhes(int transacaoId)
        {
            try
            {
                var usuarioId = int.Parse(
                    User.FindFirst(ClaimTypes.NameIdentifier)!.Value
                );

                var transacao = await _transacaoService
                    .ConsultarTransacaoDetalhes(usuarioId, transacaoId);

                if (transacao == null)
                    return NotFound(new { mensagem = "Transação não encontrada." });

                return Ok(transacao);
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

