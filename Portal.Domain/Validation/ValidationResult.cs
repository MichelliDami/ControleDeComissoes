using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Validation
{
    public class ValidationResult
    {
        private readonly List<ValidationError> _erros = new();

        public IReadOnlyCollection<ValidationError> Erros => _erros;

        public bool IsValid => !_erros.Any();

        public void Add(string campo, string mensagem)
        {
            _erros.Add(new ValidationError(campo, mensagem));
        }
    }
}
