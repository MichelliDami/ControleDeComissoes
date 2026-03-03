using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Application.Vendedores.DTOs;
using Portal.Domain.Interfaces;
using Portal.Domain.Models;
using Portal.Domain.Validation;
using Portal.Domain.Validation.Vendedores;

namespace Portal.Application.Vendedores.Services
{
    public class VendedorAppService : IVendedorAppService
    {
        private readonly IVendedorRepository _vendedorRepository;
        private readonly IComissaoRepository _comissaoRepository;

        public VendedorAppService(IVendedorRepository vendedorRepository, IComissaoRepository comissaoRepository)
        {
            _vendedorRepository = vendedorRepository;
            _comissaoRepository = comissaoRepository;
        }

        public async Task<ValidationResult> CadastrarAsync(CadastrarVendedorDto dto)
        {
            var vendedor = new Vendedor(
                dto.Nome,
                dto.Cpf,
                dto.Email,
                dto.Telefone,
                dto.PercentualComissao   
            );

            var validation = new CadastroVendedorValidator().Validate(vendedor);

            if (!validation.IsValid)
                return validation;

            if (await _vendedorRepository.ExisteCpfAsync(vendedor.Cpf, vendedor.Id))
                validation.Add(nameof(vendedor.Cpf), "CPF já cadastrado.");

            if (await _vendedorRepository.ExisteEmailAsync(vendedor.Email, vendedor.Id))
                validation.Add(nameof(vendedor.Email), "Email já cadastrado.");

            if (!validation.IsValid)
                return validation;

            await _vendedorRepository.AddAsync(vendedor);

            return validation;
        }

        public async Task<(ValidationResult Validation, VendedorResponseDto? Data)> AtualizarAsync(Guid id, AtualizarVendedorDto dto)
        {
            var validation = new ValidationResult();

            var vendedor = await _vendedorRepository.GetByIdAsync(id);
            if (vendedor is null)
            {
                validation.Add("Id", "Vendedor não encontrado.");
                return (validation, null);
            }

           
            vendedor.AtualizarDados(dto.Nome, dto.Cpf, dto.Email, dto.Telefone,dto.PercentualComissao, dto.Ativo);

          
            validation = new CadastroVendedorValidator().Validate(vendedor);
            if (!validation.IsValid)
                return (validation, null);

            if (await _vendedorRepository.ExisteCpfAsync(vendedor.Cpf, vendedor.Id))
                validation.Add(nameof(vendedor.Cpf), "CPF já cadastrado.");

            if (await _vendedorRepository.ExisteEmailAsync(vendedor.Email, vendedor.Id))
                validation.Add(nameof(vendedor.Email), "Email já cadastrado.");

            if (!validation.IsValid)
                return (validation, null);

            await _vendedorRepository.UpdateAsync(vendedor);

            return (new ValidationResult(), Map(vendedor));
        }

        public async Task<ValidationResult> ExcluirAsync(Guid id)
        {
            var validation = new ValidationResult();

            var vendedor = await _vendedorRepository.GetByIdAsync(id);
            if (vendedor is null)
            {
                validation.Add("Id", "Vendedor não encontrado.");
                return validation;
            }
            var possuiComissoes = await _comissaoRepository.ExisteComissaoParaVendedorAsync(vendedor.Id);

            if (possuiComissoes)
            {
                validation.Add("Vendedor", "Não é permitido excluir vendedor com comissões registradas.");
                return validation;
            }

            await _vendedorRepository.DeleteAsync(vendedor);
            return validation;
        }

        public async Task<VendedorResponseDto?> ObterAsync(Guid id)
        {
            var vendedor = await _vendedorRepository.GetByIdAsync(id);
            return vendedor is null ? null : Map(vendedor);
        }

        public async Task<List<VendedorResponseDto>> ListarAsync()
        {
            var list = await _vendedorRepository.ListAsync();
            return list.Select(Map).ToList();
        }

        private static VendedorResponseDto Map(Vendedor vendedor)
        {
            return new VendedorResponseDto
            {
                Id = vendedor.Id,
                Nome = vendedor.Nome,
                Cpf = vendedor.Cpf,
                Email = vendedor.Email,
                PercentualComissao = vendedor.PercentualComissao,
                Status = vendedor.Ativo
            };
        }
    }
}

