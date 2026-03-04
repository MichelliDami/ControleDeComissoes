using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Portal.Application.Utils.Documento;
using Portal.Domain.Models;

namespace Portal.Application.Invoices.Validation
{
    public class CadastroInvoiceValidator : AbstractValidator<Invoice>
    {
        public CadastroInvoiceValidator()
        {
            RuleFor(i => i.DataEmissao)
                .NotEmpty()
                .WithMessage("Data de emissão é obrigatória.");

            RuleFor(i => i.ClienteNome)
                .NotEmpty().WithMessage("Nome do cliente é obrigatório.")
                .MaximumLength(200).WithMessage("Nome do cliente não pode ultrapassar 200 caracteres.");

            RuleFor(i => i.ClienteDocumento)
                .NotEmpty().WithMessage("Documento do cliente é obrigatório.")
                .Must(doc => DocumentoValido(doc))
                .WithMessage("Documento do cliente deve ser um CPF (11) ou CNPJ (14) válido.");

            RuleFor(i => i.ValorTotal)
                .GreaterThan(0)
                .WithMessage("Valor total deve ser maior que zero.");

            When(i => i.Observacoes is not null, () =>
            {
                RuleFor(i => i.Observacoes!)
                    .MaximumLength(500)
                    .WithMessage("Observações não pode ultrapassar 500 caracteres.");
            });
        }

        private static bool DocumentoValido(string? documento)
        {
            if (string.IsNullOrWhiteSpace(documento))
                return false;

            var doc = Utils.ApenasNumeros(documento);

            if (doc.Length == DocumentoValidator.TamanhoCpf)
                return DocumentoValidator.Validar(doc);

            if (doc.Length == CnpjValidacao.TamanhoCnpj)
                return CnpjValidacao.Validar(doc);

            return false;
        }
    }
}
