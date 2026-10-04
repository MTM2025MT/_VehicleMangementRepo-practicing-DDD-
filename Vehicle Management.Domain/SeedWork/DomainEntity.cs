using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Management.Domain.SeedWork
{
    public  interface DomainEntity
    {
         List<INotification> DomainEvents { get; }
        void ClearDomainEvents();

    }
}
