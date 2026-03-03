using Microsoft.AspNetCore.Mvc;
using Portal.Application.Invoices.DTOs;
using Portal.Application.Invoices.Services;
using Portal.Domain.Notifications;
using Portal.Domain.Notifications.Portal.Domain.Notifications;
using System.Net;

namespace Portal.API.Controllers
{
    [Route("api/invoices")]
    public class InvoicesController : MainController
    {
        private readonly IInvoiceAppService _invoiceAppService;

        public InvoicesController(
            IInvoiceAppService invoiceAppService,
            INotificador notificador) : base(notificador)
        {
            _invoiceAppService = invoiceAppService;
        }

        [HttpGet]
        public async Task<ActionResult> Listar()
        {
            var list = await _invoiceAppService.ListarAsync();
            return CustomResponse(HttpStatusCode.OK, list);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> Obter(Guid id)
        {
            var invoice = await _invoiceAppService.ObterAsync(id);
            if (invoice is null) return NotFound();

            return CustomResponse(HttpStatusCode.OK, invoice);
        }

        [HttpPost]
        public async Task<ActionResult> Criar([FromBody] CriarInvoiceDto dto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var (validation, data) = await _invoiceAppService.CriarAsync(dto);

            if (!validation.IsValid)
            {
                foreach (var erro in validation.Erros)
                    NotificarErro(erro.Campo, erro.Mensagem);

                return CustomResponse();
            }

            return CustomResponse(HttpStatusCode.Created, data);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Atualizar(Guid id, [FromBody] AtualizarInvoiceDto dto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var (validation, data) = await _invoiceAppService.AtualizarAsync(id, dto);

            if (!validation.IsValid)
            {
                foreach (var erro in validation.Erros)
                    NotificarErro(erro.Campo, erro.Mensagem);

                return CustomResponse();
            }
            return CustomResponse(HttpStatusCode.NoContent);

        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Excluir(Guid id)
        {
            var validation = await _invoiceAppService.ExcluirAsync(id);

            if (!validation.IsValid)
            {
                foreach (var erro in validation.Erros)
                    NotificarErro(erro.Campo, erro.Mensagem);

                return CustomResponse();
            }

            return CustomResponse(HttpStatusCode.NoContent);
        }
    }
}