using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Application.Base
{
    public class ServiceResult
    {
        public bool Sucesso { get; set; }
        public List<string> Erros { get; private set; } = [];
        public object Entidade { get; private set; } = null;

        public static ServiceResult BemSucedido()
        {
            return new ServiceResult()
            {
                Sucesso = true,
                Erros = new List<string>(),
                Entidade = null
            };
        }

        public static ServiceResult BemSucedido(object entidade)
        {
            return new ServiceResult()
            {
                Sucesso = true,
                Erros = new List<string>(),
                Entidade = entidade
            };
        }

        public static ServiceResult Falha(string mensagem)
        {
            return new ServiceResult()
            {
                Sucesso = false,
                Erros = new List<string>() { mensagem }
            };
        }

        public static ServiceResult Falha(List<string> erros)
        {
            return new ServiceResult()
            {
                Sucesso = false,
                Erros = erros
            };
        }

        public override string ToString()
        {
            if (Sucesso)
                return string.Empty;

            return $"{string.Join(";", Erros)}";
        }
    }
}
