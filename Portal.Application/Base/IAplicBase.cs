using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Application.Base
{
    public interface IAplicBase
    {
        Task<ServiceResult> ListarAsync();
        Task<ServiceResult> ObterAsync(Guid id);
    }
}
