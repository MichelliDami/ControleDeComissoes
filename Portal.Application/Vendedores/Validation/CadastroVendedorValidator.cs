using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Portal.Application.Utils.Documento;
using Portal.Domain.Models;

namespace Portal.Application.Vendedores.Validation
{
    public class CadastroVendedorValidator : AbstractValidator<Vendedor>
    {
        public CadastroVendedorValidator()
        {
            RuleFor(v => v.Nome)
                .NotEmpty()
                .WithMessage("Nome é obrigatório.")
                .MaximumLength(200)
                .WithMessage("Nome não pode ultrapassar 200 caracteres.");

            RuleFor(v => v.Cpf)
                .NotEmpty()
                .WithMessage("CPF é obrigatório.")
                .Must(CpfValido)
                .WithMessage("CPF inválido.");

            RuleFor(v => v.Email)
                .NotEmpty()
                .WithMessage("Email é obrigatório.")
                .EmailAddress()
                .WithMessage("Email inválido.");

            RuleFor(v => v.PercentualComissao)
                .GreaterThan(0)
                .WithMessage("Percentual de comissão deve ser maior que zero.")
                .LessThanOrEqualTo(15)
                .WithMessage("Percentual não pode ser maior que 15%.");
        }

        private static bool CpfValido(string? cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            var doc = UtilsNumero.ApenasNumeros(cpf);
            return DocumentoValidator.Validar(doc);
        }
    }
}
