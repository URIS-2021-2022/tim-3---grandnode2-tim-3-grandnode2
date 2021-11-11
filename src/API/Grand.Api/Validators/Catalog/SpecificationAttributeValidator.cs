using FluentValidation;
using Grand.Api.DTOs.Catalog;
using Grand.Business.Catalog.Interfaces.Products;
using Grand.Business.Common.Interfaces.Localization;
using Grand.Infrastructure.Validators;
using System.Collections.Generic;
using System.Linq;

namespace Grand.Api.Validators.Catalog
{
    public class SpecificationAttributeValidator : BaseGrandValidator<SpecificationAttributeDto>
    {
        public SpecificationAttributeValidator(
            IEnumerable<IValidatorConsumer<SpecificationAttributeDto>> validators,
            ITranslationService translationService, ISpecificationAttributeService specificationAttributeService)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(translationService.GetResource("Api.Catalog.SpecificationAttribute.Fields.Name.Required"));
            RuleFor(x => x).MustAsync(async (x, y, context) =>
            {
                if (!string.IsNullOrEmpty(x.Id))
                {
                    var sa = await specificationAttributeService.GetSpecificationAttributeById(x.Id);
                    if (sa == null)
                        return false;
                }
                return true;
            }).WithMessage(translationService.GetResource("Api.Catalog.SpecificationAttribute.Fields.Id.NotExists"));
            RuleFor(x => x).Must((x, context) =>
            {
                var item = x.SpecificationAttributeOptions.FirstOrDefault();
                if (string.IsNullOrEmpty(item.Name))
                {
                    return false;
                }
                return true;
            }).WithMessage(translationService.GetResource("Api.Catalog.SpecificationAttributeOptions.Fields.Name.Required"));
        }
    }
}
