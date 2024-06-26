using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using FluentValidation;

namespace Ecoeden.Catalogue.Application.Validators;
internal sealed class CategoryValidators : AbstractValidator<CategoryDto>
{
    public CategoryValidators()
    {
        RuleFor(category => category.Name)
            .NotEmpty()
            .WithErrorCode(ApiError.CatgoryNameError.Code)
            .WithMessage(ApiError.CatgoryNameError.Message);
    }
}
