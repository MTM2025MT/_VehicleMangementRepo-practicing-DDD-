using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle_Management.Domain.Aggregates.BookingAggregate;
using Vehicle_Management.Domain.SeedWork;

namespace Vehicle_Management.Domain.IRepositories
{
    public interface IClientRequestsRepository
    {
        public Task<ClientRequest?> GetByIdAsync(Guid id);
        public Task<ClientRequest> UpdateAsync(ClientRequest ClientRequest);
        public Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<ClientRequest>> GetAllAsync();
        public Task<ClientRequest> AddAsync(ClientRequest ClientRequest);
    }
}
