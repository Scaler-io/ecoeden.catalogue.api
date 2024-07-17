using AutoMapper;
using Ecoeden.Catalogue.Application.Contracts.Data;
using Ecoeden.Catalogue.Application.Extensions;
using Ecoeden.Catalogue.Domain.Models.Constants;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using Ecoeden.Catalogue.Domain.Models.Enums;
using MediatR;

namespace Ecoeden.Catalogue.Application.Features.Product.Query.GetProductById;
public sealed class GetProductByIdHandler(ILogger logger, 
    IMapper mapper, 
    IDocumentRepository<Domain.Entities.Product> productRepository) 
    : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    private readonly ILogger _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IDocumentRepository<Domain.Entities.Product> _productRepository = productRepository;

    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().Information("Request - get product by its id {id}", request.Id);

        var product = await _productRepository.GetByIdAsync(request.Id, MongoDbCollectionNames.Products);

        if(product is null)
        {
            _logger.Here().Error("No product was found with {id}", request.Id);
            return Result<ProductDto>.Faliure(ErrorCodes.NotFound);
        }

        var dto = _mapper.Map<ProductDto>(product);

        _logger.Here().Information("Product {id} fetched successfully", request.Id);
        _logger.Here().MethodExited();
        return Result<ProductDto>.Success(dto);
    }
}
