using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Notifications.Portal.Domain.Notifications;

namespace Portal.Domain.Notifications
{
    public class Notificador : INotificador
    {
        private readonly List<Notificacao> _notificacoes = new();

        public void Handle(Notificacao notificacao)
        {
            _notificacoes.Add(notificacao);
        }

        public bool TemNotificacao() => _notificacoes.Any();

        public IReadOnlyCollection<Notificacao> ObterNotificacoes() => _notificacoes.AsReadOnly();
    }
}
