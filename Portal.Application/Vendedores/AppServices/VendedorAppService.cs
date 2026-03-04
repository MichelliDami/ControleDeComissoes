using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Application.Vendedores.DTOs;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;
using FluentValidation.Results;
using Portal.Application.Base;
using FluentValidation;
using Portal.Application.Invoices.Views;
using Portal.Application.Vendedores.Views;

namespace Portal.Application.Vendedores.Services
{
    public class VendedorAppService : IVendedorAppService
    {
        private readonly IVendedorRepository _vendedorRepository;
        private readonly IComissaoRepository _comissaoRepository; 
        private readonly IValidator<Vendedor> _validator;

        public VendedorAppService(IVendedorRepository vendedorRepository, 
                                  IComissaoRepository comissaoRepository,
                                  IValidator<Vendedor> validator)
        {
            _validator = validator;
            _vendedorRepository = vendedorRepository;
            _comissaoRepository = comissaoRepository;
        }

        public async Task<ServiceResult> CadastrarAsync(CadastrarVendedorDto dto)
        {
            var vendedor = new Vendedor(
                dto.Nome,
                dto.Cpf,
                dto.Email,
                dto.Telefone,
                dto.PercentualComissao
            );

            var validation = await _validator.ValidateAsync(vendedor);

            if (!validation.IsValid)
                return ServiceResult.Falha(string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage)));

            if (await _vendedorRepository.ExisteCpfAsync(vendedor.Cpf, vendedor.Id))
                return ServiceResult.Falha(" CPF já cadastrado.");

            if (await _vendedorRepository.ExisteEmailAsync(vendedor.Email, vendedor.Id))
                return ServiceResult.Falha("Email já cadastrado.");

            await _vendedorRepository.AddAsync(vendedor);

            return ServiceResult.BemSucedido(VendedorView.Map(vendedor));
        }

        public async Task<ServiceResult> AtualizarAsync (Guid id, AtualizarVendedorDto dto)
        {
           

            var vendedor = await _vendedorRepository.GetByIdAsync(id);
            if (vendedor is null)
            {
                return ServiceResult.Falha("Vendedor não encontrado.");
              
            }

            vendedor.AtualizarDados(dto.Nome, dto.Cpf, dto.Email, dto.Telefone, dto.PercentualComissao, dto.Ativo);

            var validation = await _validator.ValidateAsync(vendedor);

            if (!validation.IsValid)
                return ServiceResult.Falha(string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage)));

            if (await _vendedorRepository.ExisteCpfAsync(vendedor.Cpf, vendedor.Id))
                return ServiceResult.Falha("CPF já cadastrado.");

            if (await _vendedorRepository.ExisteEmailAsync(vendedor.Email, vendedor.Id))
                return ServiceResult.Falha("Email já cadastrado.");

            await _vendedorRepository.UpdateAsync(vendedor);

            return ServiceResult.BemSucedido(VendedorView.Map(vendedor));
        }

        public async Task<ServiceResult> ExcluirAsync(Guid id)
        {
            var validation = new ValidationResult();

            var vendedor = await _vendedorRepository.GetByIdAsync(id);
            if (vendedor is null)
            {
                return ServiceResult.Falha("Vendedor não encontrado.");
              
            }

            var possuiComissoes = await _comissaoRepository.ExisteComissaoParaVendedorAsync(vendedor.Id);
            if (possuiComissoes)
            {
                return ServiceResult.Falha("Não é permitido excluir vendedor com comissões registradas.");
               
            }
            await _vendedorRepository.DeleteAsync(vendedor);
            return ServiceResult.BemSucedido();
        }

        public async Task<ServiceResult> ObterAsync(Guid id)
        {
            var vendedor = await _vendedorRepository.GetByIdAsync(id);
            if (vendedor is null)
                return ServiceResult.Falha("Vendedor não encontrada.");

            return ServiceResult.BemSucedido(VendedorView.Map(vendedor));
        }

        public async Task<ServiceResult> ListarAsync()
        {
            var list = await _vendedorRepository.ListAsync();
            return ServiceResult.BemSucedido(list.Select(VendedorView.Map).ToList());
        }

       
    }
}

