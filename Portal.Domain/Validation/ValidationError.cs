using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Validation
{
    public class ValidationError
    {
        public string Campo { get; }
        public string Mensagem { get; }

        public ValidationError(string campo, string mensagem)
        {
            Campo = campo;
            Mensagem = mensagem;
        }
    }
}
