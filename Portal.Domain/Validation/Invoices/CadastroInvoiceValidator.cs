using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Models;
using Portal.Domain.Validation.Documento;

namespace Portal.Domain.Validation.Invoices
{
    public class CadastroInvoiceValidator
    {
        public ValidationResult Validate(Invoice invoice)
        {
            var result = new ValidationResult();

            if (invoice.DataEmissao == default)
                result.Add(nameof(invoice.DataEmissao), "Data de emissão é obrigatória.");

            if (string.IsNullOrWhiteSpace(invoice.ClienteNome))
                result.Add(nameof(invoice.ClienteNome), "Nome do cliente é obrigatório.");
            else if (invoice.ClienteNome.Length > 200)
                result.Add(nameof(invoice.ClienteNome), "Nome do cliente não pode ultrapassar 200 caracteres.");

            if (string.IsNullOrWhiteSpace(invoice.ClienteDocumento))
            {
                result.Add(nameof(invoice.ClienteDocumento), "Documento do cliente é obrigatório.");
            }
            else
            {
                var doc = Utils.ApenasNumeros(invoice.ClienteDocumento);

                if (doc.Length == DocumentoValidator.TamanhoCpf)
                {
                    if (!DocumentoValidator.Validar(doc))
                        result.Add(nameof(invoice.ClienteDocumento), "CPF do cliente inválido.");
                }
                else if (doc.Length == CnpjValidacao.TamanhoCnpj)
                {
                    if (!CnpjValidacao.Validar(doc))
                        result.Add(nameof(invoice.ClienteDocumento), "CNPJ do cliente inválido.");
                }
                else
                {
                    result.Add(nameof(invoice.ClienteDocumento), "Documento do cliente deve ser um CPF (11) ou CNPJ (14).");
                }
            }

            if (invoice.ValorTotal <= 0)
                result.Add(nameof(invoice.ValorTotal), "Valor total deve ser maior que zero.");

            if (invoice.Observacoes is not null && invoice.Observacoes.Length > 500)
                result.Add(nameof(invoice.Observacoes), "Observações não pode ultrapassar 500 caracteres.");

            return result;
        }
    }
}
