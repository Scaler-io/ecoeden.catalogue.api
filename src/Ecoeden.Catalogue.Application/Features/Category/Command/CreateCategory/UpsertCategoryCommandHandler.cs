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

namespace Ecoeden.Catalogue.Application.Features.Category.Command.CreateCategory;

public sealed class UpsertCategoryCommandHandler(ILogger logger,
    ICacheFactory cacheFactory,
    IDocumentRepository<Domain.Entities.Category> categoryRepository,
    IMapper mapper,
    IOptions<AppConfigOption> appConfigOptions) 
    : ICommandHandler<UpsertCategoryCommand, Result<CategoryDto>>
{
    private readonly ILogger _logger = logger;
    private readonly ICacheService _cacheService = cacheFactory.CreateService(CacheServiceTypes.Distributed);
    private readonly IDocumentRepository<Domain.Entities.Category> _categoryRepository = categoryRepository;
    private readonly IMapper _mapper = mapper;
    private readonly AppConfigOption _appConfig = appConfigOptions.Value;

    public async Task<Result<CategoryDto>> Handle(UpsertCategoryCommand request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();

        Domain.Entities.Category category = new(request.Name, request.ImageFile)
        {
            Id = request.Id
        };

        CategoryDto dto = new();
        var existingCategory = await _categoryRepository.GetByPredicateAsync(category => category.Name.Equals(request.Name, StringComparison.CurrentCultureIgnoreCase),
            MongoDbCollectionNames.Categories);

        if (existingCategory is not null && string.IsNullOrEmpty(request.Id))
        {
            _logger.Here().Error("The category {name} already exists", request.Name);
            return Result<CategoryDto>.Faliure(ErrorCodes.BadRequest, "Category name already exists");
        }

        if (string.IsNullOrEmpty(request.Id))
        {
            category.UpdateCreationData(request.RequestInformation.CurrentUser.Id);
            await _categoryRepository.UpsertAsync(category, MongoDbCollectionNames.Categories);
            dto = _mapper.Map<CategoryDto>(category);
        }
        else
        {
            MapUpdateCategoryEntity(request, category, existingCategory);
            await _categoryRepository.UpsertAsync(existingCategory, MongoDbCollectionNames.Categories);
            dto = _mapper.Map<CategoryDto>(existingCategory);
        }

        await _cacheService.RemoveAsync(_appConfig.CategoryCacheKey, cancellationToken);

        _logger.Here().Information("Category created/updated successfully");
        _logger.Here().MethodExited();

        return Result<CategoryDto>.Success(dto);
    }

    private static void MapUpdateCategoryEntity(UpsertCategoryCommand request, Domain.Entities.Category category, Domain.Entities.Category? existingCategory)
    {
        existingCategory.Name = category.Name;
        existingCategory.ImageFile = category.ImageFile;
        existingCategory.UpdateUpdationData(request.RequestInformation.CurrentUser.Id);
    }
}
