using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle_Management.Domain.Aggregates.BookingAggregate;
using Vehicle_Management.Domain.IRepositories;
using Vehicle_Management.Domain.SeedWork;
namespace Vehicle_Management.Infrastructure.Repositories
{
    public class ClientRequestsRepository: IClientRequestsRepository
    {
        private readonly Context _context;

        public ClientRequestsRepository(Context context)
        {
            _context = context;
        }

        public async Task<ClientRequest?> GetByIdAsync(Guid id)
        {
            return await _context.ClientRequests.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<ClientRequest> UpdateAsync(ClientRequest ClientRequest)
        {
            _context.ClientRequests.Update(ClientRequest);

            return ClientRequest;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var ClientRequest = await GetByIdAsync(id);
            if (ClientRequest == null)
                return false;

            _context.ClientRequests.Remove(ClientRequest);

            return true;
        }

        public async Task<IEnumerable<ClientRequest>> GetAllAsync()
        {
            return await _context.ClientRequests.ToListAsync();
        }

        public async Task<ClientRequest> AddAsync(ClientRequest ClientRequest)
        {
            await _context.ClientRequests.AddAsync(ClientRequest);
            await _context.SaveChangesAsync();
            return ClientRequest;
        }
    }
}
