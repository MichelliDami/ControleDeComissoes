using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Portal.API.Controllers;
using Portal.API.ViewModel;
using Portal.Application.Vendedores.DTOs;
using Portal.Application.Vendedores.Services;
using Portal.Domain.Notifications.Portal.Domain.Notifications;
using System.Collections.Generic;
using System.Net;

namespace Portal.WebApi.Controllers
{
    [Route("api/vendedor")]
    public class VendedorController : MainController
    {
        private readonly IVendedorAppService _vendedorAppService;
        private readonly IMapper _mapper;

        public VendedorController(IMapper mapper,
                                  IVendedorAppService vendedorAppService,
                                  INotificador notificador
                                  ) : base(notificador) 
        {
            _vendedorAppService = vendedorAppService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> Criar([FromBody] VendedorViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var dto = _mapper.Map<CadastrarVendedorDto>(viewModel);

            var (validation, data) = await _vendedorAppService.CadastrarAsync(dto);

            if (!validation.IsValid)
            {
                foreach (var erro in validation.Errors)
                    NotificarErro(erro.PropertyName, erro.ErrorMessage);

                return CustomResponse();
            }

            var responseVm = _mapper.Map<VendedorViewModel>(data);

            return CustomResponse(HttpStatusCode.Created, responseVm);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Atualizar(Guid id, [FromBody] VendedorViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var dto = _mapper.Map<AtualizarVendedorDto>(viewModel);

            var (validation, data) = await _vendedorAppService.AtualizarAsync(id, dto);

            if (!validation.IsValid)
            {
                foreach (var erro in validation.Errors)
                    NotificarErro(erro.PropertyName, erro.ErrorMessage);

                return CustomResponse();
            }

            return CustomResponse(HttpStatusCode.OK, data);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Excluir(Guid id)
        {
            var validation = await _vendedorAppService.ExcluirAsync(id);

            if (!validation.IsValid)
            {
                foreach (var erro in validation.Errors)
                    NotificarErro(erro.PropertyName, erro.ErrorMessage);

                return CustomResponse();
            }

            return CustomResponse(HttpStatusCode.NoContent);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> Obter(Guid id)
        {
            var vendedor = await _vendedorAppService.ObterAsync(id);
            if (vendedor is null)
                return NotFound();
            var vm = _mapper.Map<VendedorViewModel>(vendedor);

            return CustomResponse(HttpStatusCode.OK, vm);
        }

        [HttpGet]
        public async Task<ActionResult> Listar()
        {
            var list = await _vendedorAppService.ListarAsync();
            var listVm = _mapper.Map<List<VendedorViewModel>>(list);
            return CustomResponse(HttpStatusCode.OK, listVm); 
        }
    }
}