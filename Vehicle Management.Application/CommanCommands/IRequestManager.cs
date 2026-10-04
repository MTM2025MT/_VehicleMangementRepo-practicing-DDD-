using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Management.Application.CommanCommands
{
    public interface IRequestManager
    {
        Task<bool> ExistAsync(Guid messageId);

        Task CreateRequestForCommandAsync<T>(Guid messageId);
    }
}
