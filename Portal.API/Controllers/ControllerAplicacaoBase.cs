using Microsoft.AspNetCore.Mvc;
using Portal.Application.Base;

namespace Portal.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ControllerAplicacaoBase<Aplic> : ControllerBase where Aplic : IAplicBase
    {
        protected readonly Aplic _aplic;

        public ControllerAplicacaoBase(Aplic aplic)
        {
            _aplic = aplic;
        }

        [HttpGet]
        public async Task<IActionResult> Recuperar()
        {
            return await Executar(async () => await _aplic.ListarAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> RecuperarPorId([FromRoute] Guid id)
        {
            return await Executar(async () => await _aplic.ObterAsync(id));
        }
  protected async Task<IActionResult> Executar(Func<Task<ServiceResult>> acao)
        {
            try
            {
                return RetornoBase(await acao.Invoke());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Erro = "Ocorreu um erro inesperado.",
                    Detalhes = ex.Message
                });
            }
        }

      

        protected IActionResult RetornoBase(ServiceResult resultado, string mensagem = "")
        {
            return resultado.Sucesso ?
                            Ok(new { Result = resultado, Mensagem = mensagem }) :
                            BadRequest(new { Erro = resultado.Erros, Mensagem = mensagem });
        }
    }
}
