using AutoMapper;
using Ecoeden.Catalogue.Application.Contracts.Data;
using Ecoeden.Catalogue.Application.Extensions;
using Ecoeden.Catalogue.Domain.Models.Constants;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using Ecoeden.Catalogue.Domain.Models.Enums;
using MediatR;

namespace Ecoeden.Catalogue.Application.Features.Product.Query.GetProductsByCategory;
public sealed class GetProductsByCategoryQueryHandler(ILogger logger,
    IMapper mapper,
    IDocumentRepository<Domain.Entities.Product> productRepository) 
    : IRequestHandler<GetProductsByCategoryQuery, Result<IReadOnlyList<ProductDto>>>
{
    private readonly ILogger _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IDocumentRepository<Domain.Entities.Product> _productRepository = productRepository;

    public async Task<Result<IReadOnlyList<ProductDto>>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().Information("Request - get products by category {category}", request.Category);

        var products = await _productRepository.GetListByPredicateAsync(product => product.Category.Equals(request.Category, StringComparison.CurrentCultureIgnoreCase),
            MongoDbCollectionNames.Products);

        if (products is null || products.Count == 0)
        {
            _logger.Here().Error("No products were found in {category}", request.Category);
            return Result<IReadOnlyList<ProductDto>>.Faliure(ErrorCodes.NotFound);
        }

        var dto = _mapper.Map<IReadOnlyList<ProductDto>>(products);

        _logger.Here().Information("{count} products fetched successfully", products.Count);
        _logger.Here().MethodExited();
        return Result<IReadOnlyList<ProductDto>>.Success(dto);
    }
}
