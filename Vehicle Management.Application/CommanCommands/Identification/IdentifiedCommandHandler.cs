using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Management.Application.CommanCommands.Identification
{
    public class IdentifiedCommandHandler<TCommand, TResponse>
        : IRequestHandler<
            IdentifiedCommand<TCommand, TResponse>,
            TResponse>
        where TCommand : IRequest<TResponse>
    {
        private readonly IMediator _mediator;
        private readonly IRequestManager _requestManager;

        public IdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager)
        {
            _mediator = mediator;
            _requestManager = requestManager;
        }

        public async Task<TResponse> Handle(
            IdentifiedCommand<TCommand, TResponse> message,
            CancellationToken cancellationToken)
        {
            // 1. Did I process this request before?
            var exists = await _requestManager.ExistAsync(message.Id);

            if (exists)
            {
                return default!;
            }

            // 2. Remember this ID
            await _requestManager
                .CreateRequestForCommandAsync<TCommand>(message.Id);

            // 3. Send the real command
            return await _mediator.Send(
                message.Command,
                cancellationToken);
        }
    }
}
