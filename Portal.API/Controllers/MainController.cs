using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Portal.Domain.Notifications;
using Portal.Domain.Notifications.Portal.Domain.Notifications;

namespace Portal.API.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private readonly INotificador _notificador;

        protected MainController(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }

        protected ActionResult CustomResponse(HttpStatusCode statusCode = HttpStatusCode.OK, object? result = null)
        {
            if (OperacaoValida())
            {
                return new ObjectResult(result)
                {
                    StatusCode = Convert.ToInt32(statusCode),
                };
            }

            return BadRequest(new
            {
                success = false,
                errors = _notificador.ObterNotificacoes().Select(n => new
                {
                    field = n.Chave,
                    message = n.Mensagem
                })
            });
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotificarErroModelInvalida(modelState);
            return CustomResponse();
        }

        protected void NotificarErroModelInvalida(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);

            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;

                NotificarErro("ModelState", errorMsg);
            }
        }

        protected void NotificarErro(string chave, string mensagem)
        {
            _notificador.Handle(new Notificacao(chave, mensagem));
        }
    }
}