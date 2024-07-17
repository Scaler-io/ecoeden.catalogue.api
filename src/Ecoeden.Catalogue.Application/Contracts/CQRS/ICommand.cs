using MediatR;

namespace Ecoeden.Catalogue.Application.Contracts.CQRS;

public interface ICommand : ICommand<Unit>
{

}

public interface ICommand<out TResponse> : IRequest<TResponse>
    where TResponse : notnull
{
}
