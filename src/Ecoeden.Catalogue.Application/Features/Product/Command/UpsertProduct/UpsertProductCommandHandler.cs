using AutoMapper;
using Ecoeden.Catalogue.Application.Contracts.Cache;
using Ecoeden.Catalogue.Application.Contracts.Data;
using Ecoeden.Catalogue.Application.Extensions;
using Ecoeden.Catalogue.Application.Factories;
using Ecoeden.Catalogue.Domain.Models.Constants;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using Ecoeden.Catalogue.Domain.Models.Enums;
using MediatR;

namespace Ecoeden.Catalogue.Application.Features.Product.Command.UpsertProduct;
public sealed class UpsertProductCommandHandler(IMapper mapper,
    ILogger logger,
    ICacheFactory cacheFactory,
    IDocumentRepository<Domain.Entities.Product> productRepository)
    : IRequestHandler<UpsertProductCommand, Result<ProductDto>>
{
    private readonly ILogger _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly ICacheService _cacheService = cacheFactory.CreateService(CacheServiceTypes.Distributed);
    private readonly IDocumentRepository<Domain.Entities.Product> _productRepository = productRepository;
    private const string PRODUCT_CACHE_KEY = "product_cache";

    public async Task<Result<ProductDto>> Handle(UpsertProductCommand request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().Information("Request - create or update product {name}", request.Name);

        Domain.Entities.Product product = new(request.Name, request.Description, request.Category, request.Price)
        {
            Id = request.Id
        };

        var existingProduct = await _productRepository.GetByPredicateAsync(product => product.Name.Equals(request.Name, StringComparison.CurrentCultureIgnoreCase),
            MongoDbCollectionNames.Products);

        if (existingProduct is not null && string.IsNullOrEmpty(request.Id))
        {
            _logger.Here().Error("The product {name} already exists", request.Name);
            return Result<ProductDto>.Faliure(ErrorCodes.BadRequest, "Product name already exists");
        }

        if (string.IsNullOrEmpty(request.Id))
        {
            product.UpdateCreationData(request.RequestInformation.CurrentUser.Id);
        }
        else
        {
            product = existingProduct;
            product.UpdateUpdationData(request.RequestInformation.CurrentUser.Id);
        }
            

        await _productRepository.UpsertAsync(product, MongoDbCollectionNames.Products);

        await _cacheService.RemoveAsync(PRODUCT_CACHE_KEY, cancellationToken);

        var dto = _mapper.Map<ProductDto>(product);

        _logger.Here().Information("Product created/updated successfully");
        _logger.Here().MethodExited();

        return Result<ProductDto>.Success(dto);
    }
}
