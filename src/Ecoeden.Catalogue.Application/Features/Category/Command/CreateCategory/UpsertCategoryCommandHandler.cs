﻿using AutoMapper;
using Ecoeden.Catalogue.Application.Contracts.Cache;
using Ecoeden.Catalogue.Application.Contracts.Data;
using Ecoeden.Catalogue.Application.Extensions;
using Ecoeden.Catalogue.Application.Factories;
using Ecoeden.Catalogue.Domain.Models.Constants;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using Ecoeden.Catalogue.Domain.Models.Enums;
using MediatR;

namespace Ecoeden.Catalogue.Application.Features.Category.Command.CreateCategory;

public sealed class UpsertCategoryCommandHandler(ILogger logger,
    ICacheFactory cacheFactory,
    IDocumentRepository<Domain.Entities.Category> categoryRepository,
    IMapper mapper) 
    : IRequestHandler<UpsertCategoryCommand, Result<CategoryDto>>
{
    private readonly ILogger _logger = logger;
    private readonly ICacheService _cacheService = cacheFactory.CreateService(CacheServiceTypes.Distributed);
    private readonly IDocumentRepository<Domain.Entities.Category> _categoryRepository = categoryRepository;
    private readonly IMapper _mapper = mapper;
    private const string CATEGORY_CACHE_KEY = "category_cache";

    public async Task<Result<CategoryDto>> Handle(UpsertCategoryCommand request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();

        Domain.Entities.Category category = new(request.Name)
        {
            Id = request.Id,
            CreatedBy = request.RequestInformation.CurrentUser.Id,
        };

        var categoryExists = await _categoryRepository.Exists(category => category.Name.ToLower() == request.Name.ToLower(),
            MongoDbCollectionNames.Categories);

        if (categoryExists)
        {
            _logger.Here().Error("The category {name} already exists", request.Name);
            return Result<CategoryDto>.Faliure(ErrorCodes.BadRequest, "Category name already exists");
        }

        await _categoryRepository.UpsertAsync(category, MongoDbCollectionNames.Categories);

        await _cacheService.RemoveAsync(CATEGORY_CACHE_KEY, cancellationToken);

        var dto = _mapper.Map<CategoryDto>(category);

        _logger.Here().Information("Category created/updated successfully");
        _logger.Here().MethodExited();

        return Result<CategoryDto>.Success(dto);
    }
}
