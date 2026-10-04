using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle_Management.Domain;
using Vehicle_Management.Domain.IRepositories;
using Vehicle_Management.Domain.SeedWork;

namespace Vehicle_Management.Application.CommanCommands
{
    public class RequestManager : IRequestManager
    {
        private readonly IClientRequestsRepository ClientRequestsRepository;

        public RequestManager(IClientRequestsRepository clientRequestsRepository)
        {
            ClientRequestsRepository = clientRequestsRepository;
        }

        public async Task<bool> ExistAsync(Guid messageId)
        {
            var result = await ClientRequestsRepository.GetByIdAsync(messageId);
            if (result == null)
            {
                return false;
            }
            return true;
        }

        public async Task CreateRequestForCommandAsync<T>(Guid messageId)
        {
            var request = new  ClientRequest
            {
                Id = messageId,
                Name = typeof(T).Name,
                Time = DateTime.UtcNow
            };

            await ClientRequestsRepository.AddAsync(request);

           
        }
    }
}
