using AutoMapper;
using Ecoeden.Catalogue.Application.Contracts.Cache;
using Ecoeden.Catalogue.Application.Contracts.CQRS;
using Ecoeden.Catalogue.Application.Contracts.Data;
using Ecoeden.Catalogue.Application.Extensions;
using Ecoeden.Catalogue.Application.Factories;
using Ecoeden.Catalogue.Domain.Configurations;
using Ecoeden.Catalogue.Domain.Models.Constants;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using Ecoeden.Catalogue.Domain.Models.Enums;
using Microsoft.Extensions.Options;

namespace Ecoeden.Catalogue.Application.Features.Product.Query.GetAllProducts;
public sealed class GetAllProductsQueryHandler(ILogger logger, 
    IMapper mapper, 
    IDocumentRepository<Domain.Entities.Product> productRepositories,
    ICacheFactory cacheFactory,
    IOptions<AppConfigOption> appConfigOptions) 
    : IQueryHandler<GetAllProductsQuery, Result<IReadOnlyList<ProductDto>>>
{
    private readonly ILogger _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly ICacheService _cacheService = cacheFactory.CreateService(CacheServiceTypes.Distributed);
    private readonly IDocumentRepository<Domain.Entities.Product> _productRepositories = productRepositories;
    private readonly AppConfigOption _appConfig = appConfigOptions.Value;

    public async Task<Result<IReadOnlyList<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().Information("Request - get all product details");

        var cacheResult = await _cacheService.GetAsync<IReadOnlyList<ProductDto>>(_appConfig.ProductCacheKey, cancellationToken);
        if (cacheResult is not null)
        {
            _logger.Here().Information("GetAllProducts - cache hit");
            return Result<IReadOnlyList<ProductDto>>.Success(cacheResult);
        }

        var products = await _productRepositories.GetAllAsync(MongoDbCollectionNames.Products);
        if (products == null || products.Count == 0)
        {
            _logger.Here().Error("No products were found");
            return Result<IReadOnlyList<ProductDto>>.Faliure(ErrorCodes.NotFound);
        }

        var dto = _mapper.Map<IReadOnlyList<ProductDto>>(products);
        await _cacheService.SetAsync(_appConfig.ProductCacheKey, dto, cancellation: cancellationToken);


        _logger.Here().Information("{count} products successfully fetched", products.Count);
        _logger.Here().MethodExited();

        return Result<IReadOnlyList<ProductDto>>.Success(dto);
    }
}
