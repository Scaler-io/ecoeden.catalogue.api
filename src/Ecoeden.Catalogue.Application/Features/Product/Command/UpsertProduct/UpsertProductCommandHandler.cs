using AutoMapper;
using Contracts.Events;
using Ecoeden.Catalogue.Application.Contracts.Cache;
using Ecoeden.Catalogue.Application.Contracts.CQRS;
using Ecoeden.Catalogue.Application.Contracts.Data;
using Ecoeden.Catalogue.Application.Contracts.EventBus;
using Ecoeden.Catalogue.Application.Extensions;
using Ecoeden.Catalogue.Application.Factories;
using Ecoeden.Catalogue.Domain.Configurations;
using Ecoeden.Catalogue.Domain.Events;
using Ecoeden.Catalogue.Domain.Models.Constants;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using Ecoeden.Catalogue.Domain.Models.Enums;
using Microsoft.Extensions.Options;

namespace Ecoeden.Catalogue.Application.Features.Product.Command.UpsertProduct;
public sealed class UpsertProductCommandHandler(IMapper mapper,
    ILogger logger,
    ICacheFactory cacheFactory,
    IDocumentRepository<Domain.Entities.Product> productRepository,
    IOptions<AppConfigOption> appConfigOptions,
    IPublishServiceFactory publishServiceFactory)  
    : ICommandHandler<UpsertProductCommand, Result<ProductDto>>
{
    private readonly ILogger _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly AppConfigOption _appConfig = appConfigOptions.Value;
    private readonly ICacheService _cacheService = cacheFactory.CreateService(CacheServiceTypes.Distributed);
    private readonly IDocumentRepository<Domain.Entities.Product> _productRepository = productRepository;
    private readonly IPublishServiceFactory _publishServiceFactory = publishServiceFactory;

    public async Task<Result<ProductDto>> Handle(UpsertProductCommand request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().Information("Request - create or update product {name}", request.Name);

        Domain.Entities.Product product = new(request.Name, request.Description, request.Category, request.Price, request.ImageFile)
        {
            Id = request.Id
        };

        ProductDto dto = new();
        var existingProduct = await _productRepository.GetByPredicateAsync(product => product.Name.Equals(request.Name, StringComparison.CurrentCultureIgnoreCase),
            MongoDbCollectionNames.Products);

        if (existingProduct is not null && string.IsNullOrEmpty(request.Id))
        {
            _logger.Here().Error("The product {name} already exists", request.Name);
            return Result<ProductDto>.Faliure(ErrorCodes.BadRequest, "Product name already exists");
        }

        if (string.IsNullOrEmpty(request.Id)) 
            dto = await UpsertProductJob<ProductCreated>(dto, product, request);
        else 
            dto = await UpsertProductJob<ProductUpdated>(dto, existingProduct, request);

        await _cacheService.RemoveAsync(_appConfig.ProductCacheKey, cancellationToken);

        _logger.Here().Information("Product created/updated successfully");
        _logger.Here().MethodExited();

        return Result<ProductDto>.Success(dto);
    }

    private async Task<ProductDto> UpsertProductJob<TEvent>(ProductDto dto, Domain.Entities.Product product, UpsertProductCommand request, bool isUpdate = false) where TEvent : GenericEvent
    {
        ArgumentNullException.ThrowIfNull(dto);

        var service = _publishServiceFactory.CreatePublishService<Domain.Entities.Product, TEvent>();
        if (!isUpdate) product.UpdateCreationData(request.RequestInformation.CurrentUser.Id);
        else MapUpdateProductEntity(request, product);
        await _productRepository.UpsertAsync(product, MongoDbCollectionNames.Products);
        await service.PublishAsync(product, request.RequestInformation.CorrelationId, new Dictionary<string, string>
        {
            { "applicationName", _appConfig.ApplicationIdentifier }
        });
        return _mapper.Map<ProductDto>(product);
    }

    private static void MapUpdateProductEntity(UpsertProductCommand request, Domain.Entities.Product? existingProduct)
    {
        existingProduct.Name = request.Name;
        existingProduct.Description = request.Description;
        existingProduct.Price = request.Price;
        existingProduct.ImageFile = request.ImageFile;
        existingProduct.UpdateUpdationData(request.RequestInformation.CurrentUser.Id);
    }
}
