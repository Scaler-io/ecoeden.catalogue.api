using AutoMapper;
using Ecoeden.Catalogue.Application.Contracts.Data;
using Ecoeden.Catalogue.Application.Extensions;
using Ecoeden.Catalogue.Domain.Models.Constants;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using Ecoeden.Catalogue.Domain.Models.Enums;
using MediatR;

namespace Ecoeden.Catalogue.Application.Features.Product.Query.GetProductByName;
public sealed class GetProductByNameHandler(ILogger logger, 
    IMapper mapper, 
    IDocumentRepository<Domain.Entities.Product> productRepository) 
    : IRequestHandler<GetProductByNameQuery, Result<ProductDto>>
{
    private readonly ILogger _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IDocumentRepository<Domain.Entities.Product> _productRepository = productRepository;

    public async Task<Result<ProductDto>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().Information("Request - get product by its name {name}", request.Name);

        var product = await _productRepository.GetByPredicateAsync(product => product.Name.Equals(request.Name, StringComparison.CurrentCultureIgnoreCase),
            MongoDbCollectionNames.Products);

        if(product is null)
        {
            _logger.Here().Error("No product was found with name {name}", request.Name);
            return Result<ProductDto>.Faliure(ErrorCodes.NotFound);
        }

        var dto = _mapper.Map<ProductDto>(product);

        _logger.Here().Information("Product {name} fetched successfully", request.Name);
        _logger.Here().MethodExited();
        return Result<ProductDto>.Success(dto);
    }
}
