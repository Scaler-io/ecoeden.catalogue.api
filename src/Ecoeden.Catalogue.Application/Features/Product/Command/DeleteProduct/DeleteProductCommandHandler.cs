using Contracts.Events;
using Ecoeden.Catalogue.Application.Contracts.Cache;
using Ecoeden.Catalogue.Application.Contracts.CQRS;
using Ecoeden.Catalogue.Application.Contracts.Data;
using Ecoeden.Catalogue.Application.Contracts.EventBus;
using Ecoeden.Catalogue.Application.Extensions;
using Ecoeden.Catalogue.Application.Factories;
using Ecoeden.Catalogue.Domain.Configurations;
using Ecoeden.Catalogue.Domain.Models.Constants;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Enums;
using Microsoft.Extensions.Options;

namespace Ecoeden.Catalogue.Application.Features.Product.Command.DeleteProduct;
public sealed class DeleteProductCommandHandler(ILogger logger,
    ICacheFactory cacheFactory,
    IDocumentRepository<Domain.Entities.Product> productRepository,
    IOptions<AppConfigOption> appConfigOption,
    IPublishServiceFactory publishServiceFactory)
    : ICommandHandler<DeleteProductCommand, Result<bool>>
{

    private readonly ILogger _logger = logger;
    private readonly ICacheService _cacheService = cacheFactory.CreateService(Domain.Models.Enums.CacheServiceTypes.Distributed);
    private readonly IDocumentRepository<Domain.Entities.Product> _productRepository = productRepository;
    private readonly AppConfigOption _appConfig = appConfigOption.Value;
    private readonly IPublishServiceFactory _publishServiceFactory = publishServiceFactory;


    public async Task<Result<bool>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().Information("Request - delet product {id}", command.ProductId);

        var existingProduct = await _productRepository.GetByPredicateAsync(p => p.Id == command.ProductId, MongoDbCollectionNames.Products);

        if(existingProduct is null)
        {
            _logger.Here().Error("{ErroCode} No product was found with id {id}", ErrorCodes.NotFound, command.ProductId);
            return Result<bool>.Faliure(ErrorCodes.NotFound);
        }

        await _productRepository.DeleteAsync(command.ProductId, MongoDbCollectionNames.Products);
        await _cacheService.RemoveAsync(_appConfig.ProductCacheKey, cancellationToken);
        var publishService = _publishServiceFactory.CreatePublishService<Domain.Entities.Product, ProductDeleted>();
        await publishService.PublishAsync(existingProduct, command.CorrelationId);

        _logger.Here().Information("Product {id} has been deleted successfully", command.ProductId);
        _logger.Here().MethodExited();

        return Result<bool>.Success(true);
    }
}
