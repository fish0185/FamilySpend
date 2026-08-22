using MediatR;

namespace FamilySpend.App.CreateTransactionCommand;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand>
{
    public async Task Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        // validate category
        // validate balance
        // make transaction
    }
}