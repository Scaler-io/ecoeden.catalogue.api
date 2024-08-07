﻿using AutoMapper;
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

namespace Ecoeden.Catalogue.Application.Features.Category.Query.GetAllCategories;
public sealed class GetAllCategoriesQueryHandler(ILogger logger, 
    IDocumentRepository<Domain.Entities.Category> categoryRepository,
    ICacheFactory cacheFactory,
    IMapper mapper,
    IOptions<AppConfigOption> appConfigOptions
    ): IQueryHandler<GetAllCategoriesQuery, Result<IReadOnlyList<CategoryDto>>>
{
    private readonly ILogger _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly ICacheService _cacheService = cacheFactory.CreateService(CacheServiceTypes.Distributed);
    private readonly IDocumentRepository<Domain.Entities.Category> _categoryRepository = categoryRepository;
    private readonly AppConfigOption _appConfig = appConfigOptions.Value;  

    public async Task<Result<IReadOnlyList<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().Information("Request - get all categories");

        var cacheResult = await _cacheService.GetAsync<IReadOnlyList<CategoryDto>>(_appConfig.CategoryCacheKey, cancellationToken);
        if (cacheResult is not null && cacheResult.Count != 0)
        {
            _logger.Here().Information("GetAllCategories - cache hit");
            return Result<IReadOnlyList<CategoryDto>>.Success(cacheResult);
        }

        var categories = await _categoryRepository.GetAllAsync(MongoDbCollectionNames.Categories);

        if(categories is null || categories.Count == 0)
        {
            _logger.Here().Error("No categories were found");
            return Result<IReadOnlyList<CategoryDto>>.Faliure(ErrorCodes.NotFound);
        }

        IReadOnlyList<CategoryDto> dto = _mapper.Map<IReadOnlyList<CategoryDto>>(categories);
        await _cacheService.SetAsync(_appConfig.CategoryCacheKey, dto, cancellation: cancellationToken);

        _logger.Here().Information("{count} categories found");
        _logger.Here().MethodExited();

        return Result<IReadOnlyList<CategoryDto>>.Success(dto);
    }
}
