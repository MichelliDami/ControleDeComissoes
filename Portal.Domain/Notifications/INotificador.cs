using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Notifications
{
    using System.Collections.Generic;

    namespace Portal.Domain.Notifications
    {
        public interface INotificador
        {
            void Handle(Notificacao notificacao);
            bool TemNotificacao();
            IReadOnlyCollection<Notificacao> ObterNotificacoes();
        }
    }
}
