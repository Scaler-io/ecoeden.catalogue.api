using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Dtos;
using FluentValidation;

namespace Ecoeden.Catalogue.Application.Validators;

public class ProductValidators : AbstractValidator<ProductDto>
{
    [Obsolete]
    public ProductValidators()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithErrorCode(ApiError.ProductNameError.Code)
            .WithMessage(ApiError.ProductNameError.Message);

        RuleFor(p => p.Description)
            .NotEmpty()
            .WithErrorCode(ApiError.ProductDescriptionRequired.Code)
            .WithMessage(ApiError.ProductDescriptionRequired.Message)
            .MinimumLength(10)
            .WithErrorCode(ApiError.ProductDescriptionMinLength.Code)
            .WithMessage(ApiError.ProductDescriptionMinLength.Message);

        RuleFor(p => p.Category)
            .NotEmpty()
            .WithErrorCode(ApiError.ProductCategoryRequired.Code)
            .WithMessage(ApiError.ProductCategoryRequired.Message);

        RuleFor(p => p.Price)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithErrorCode(ApiError.ProductPriceRequired.Code)
            .WithMessage(ApiError.ProductPriceRequired.Message)
            .Must(p => p > 0)
            .WithErrorCode(ApiError.ProductPriceNotZero.Code)
            .WithMessage(ApiError.ProductPriceNotZero.Message);
    }
}
