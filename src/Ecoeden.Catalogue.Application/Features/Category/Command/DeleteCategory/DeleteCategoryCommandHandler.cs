using Ecoeden.Catalogue.Application.Contracts.Cache;
using Ecoeden.Catalogue.Application.Contracts.Data;
using Ecoeden.Catalogue.Application.Extensions;
using Ecoeden.Catalogue.Application.Factories;
using Ecoeden.Catalogue.Domain.Entities;
using Ecoeden.Catalogue.Domain.Models.Constants;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Enums;
using MediatR;

namespace Ecoeden.Catalogue.Application.Features.Category.Command.DeleteCategory;
public sealed class DeleteCategoryCommandHandler(ILogger logger, 
    IDocumentRepository<Domain.Entities.Category> categoryRepository,
    ICacheFactory cacheFactory) 
    : IRequestHandler<DeleteCategoryCommand, Result<bool>>
{
    private readonly ILogger _logger = logger;
    private readonly ICacheService _cacheService = cacheFactory.CreateService(CacheServiceTypes.Distributed);
    private readonly IDocumentRepository<Domain.Entities.Category> _categoryRepository = categoryRepository;
    private const string CATEGORY_CACHE_KEY = "category_cache"; 

    public async Task<Result<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().Information("Request - delete category {id}", request.CategoryId);

        var categoryExists = await _categoryRepository.Exists(category => category.Id == request.CategoryId, MongoDbCollectionNames.Categories);

        if (!categoryExists)
        {
            _logger.Here().Error("No category was found with id {id}", request.CategoryId);
            return Result<bool>.Faliure(ErrorCodes.NotFound);
        }

        await _categoryRepository.DeleteAsync(request.CategoryId, MongoDbCollectionNames.Categories);

        await _cacheService.RemoveAsync(CATEGORY_CACHE_KEY, cancellationToken);

        _logger.Here().Information("Category {id} deleted successfully", request.CategoryId);
        _logger.Here().MethodExited();

        return Result<bool>.Success(true);
    }
}
