using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Models;
using Portal.Domain.Validation.Documento;

namespace Portal.Domain.Validation.Vendedores
{
    public class CadastroVendedorValidator
    {
        public ValidationResult Validate(Vendedor vendedor)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(vendedor.Nome))
                result.Add(nameof(vendedor.Nome), "Nome é obrigatório.");
            else if (vendedor.Nome.Length > 200)
            {
                result.Add(nameof(vendedor.Nome), "Nome não pode ultrapassar 200 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(vendedor.Cpf))
                result.Add(nameof(vendedor.Cpf), "CPF é obrigatório.");
            else if (!DocumentoValidator.Validar(vendedor.Cpf))
                result.Add(nameof(vendedor.Cpf), "CPF inválido.");


            if (string.IsNullOrWhiteSpace(vendedor.Email))
                result.Add(nameof(vendedor.Email), "Email é obrigatório.");

            if (vendedor.PercentualComissao > 15)
                result.Add(nameof(vendedor.PercentualComissao),"Percentual não pode ser maior que 15%.");

            if (vendedor.PercentualComissao <= 0)
                result.Add(nameof(vendedor.PercentualComissao), "Percentual de comissão deve ser maior que zero.");

            return result;
        }
    }
}
