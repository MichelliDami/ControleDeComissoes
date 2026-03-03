using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace Portal.Domain.Notifications
    {
        public class Notificacao
        {
            public string Chave { get; }
            public string Mensagem { get; }

            public Notificacao(string chave, string mensagem)
            {
                Chave = chave;
                Mensagem = mensagem;
            }
        }
    }

