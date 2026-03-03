using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Models
{
    public abstract  class Entity

    {
        protected Entity()
        {
        }

        public Guid Id { get; set; }
    }

}
