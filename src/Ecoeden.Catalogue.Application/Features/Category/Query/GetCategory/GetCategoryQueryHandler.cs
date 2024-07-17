using AutoMapper;
using Ecoeden.Catalogue.Application.Contracts.CQRS;
using Ecoeden.Catalogue.Application.Contracts.Data;
using Ecoeden.Catalogue.Application.Extensions;
using Ecoeden.Catalogue.Domain.Models.Constants;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using Ecoeden.Catalogue.Domain.Models.Enums;

namespace Ecoeden.Catalogue.Application.Features.Category.Query.GetCategory;
public sealed class GetCategoryQueryHandler(ILogger logger, 
    IDocumentRepository<Domain.Entities.Category> categoryRepository,
    IMapper mapper) 
    : IQueryHandler<GetCategoryQuery, Result<CategoryDto>>
{
    private readonly ILogger _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IDocumentRepository<Domain.Entities.Category> _categoryRepository = categoryRepository;

    public async Task<Result<CategoryDto>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().Information("Request - get category by id {id}", request.Id);

        var category = await _categoryRepository.GetByIdAsync(request.Id, MongoDbCollectionNames.Categories);
        if (category is null)
        {
            _logger.Here().Error("No category was found with id {id}", request.Id);
            return Result<CategoryDto>.Faliure(ErrorCodes.NotFound);
        }

        var categoryDto = _mapper.Map<CategoryDto>(category);

        _logger.Here().Information("Category was found by {id}", request.Id);
        _logger.Here().MethodExited();

        return Result<CategoryDto>.Success(categoryDto);
    }
}
