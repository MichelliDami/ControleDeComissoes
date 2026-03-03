using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Portal.API.ViewModel;
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
        private readonly IMapper _mapper;

        public InvoicesController(
            IInvoiceAppService invoiceAppService,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _invoiceAppService = invoiceAppService;
            _mapper = mapper;
        }
       

        [HttpPost]
        public async Task<ActionResult> Criar([FromBody] InvoiceViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var dto = _mapper.Map<CriarInvoiceDto>(viewModel);

            var (validation, data) = await _invoiceAppService.CriarAsync(dto);

            if (!validation.IsValid)
            {
                foreach (var erro in validation.Errors)
                    NotificarErro(erro.PropertyName, erro.ErrorMessage);

                return CustomResponse();
            }

            var response = _mapper.Map<InvoiceViewModel>(data);

            return CustomResponse(HttpStatusCode.Created, response);
        }


        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Atualizar(Guid id, [FromBody] InvoiceViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var dto = _mapper.Map<AtualizarInvoiceDto>(viewModel);

            var (validation, data) = await _invoiceAppService.AtualizarAsync(id, dto);

            if (!validation.IsValid)
            {
                foreach (var erro in validation.Errors)
                    NotificarErro(erro.PropertyName, erro.ErrorMessage);

                return CustomResponse();
            }

            var response = _mapper.Map<InvoiceViewModel>(data);
            return CustomResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        public async Task<ActionResult> Listar()
        {
            var list = await _invoiceAppService.ListarAsync();
            var response = _mapper.Map<IEnumerable<InvoiceViewModel>>(list);

            return CustomResponse(HttpStatusCode.OK, response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> Obter(Guid id)
        {
            var invoice = await _invoiceAppService.ObterAsync(id);
            if (invoice is null) return NotFound();

            var response = _mapper.Map<InvoiceViewModel>(invoice);

            return CustomResponse(HttpStatusCode.OK, response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Excluir(Guid id)
        {
            var validation = await _invoiceAppService.ExcluirAsync(id);

            if (!validation.IsValid)
            {
                foreach (var erro in validation.Errors)
                {
                    NotificarErro(erro.PropertyName, erro.ErrorMessage);
                }

                return CustomResponse();
            }

            return CustomResponse(HttpStatusCode.NoContent);
        }





    }
}