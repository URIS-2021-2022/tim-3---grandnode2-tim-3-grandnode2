using FluentValidation;
using Grand.Api.DTOs.Catalog;
using Grand.Business.Catalog.Interfaces.Brands;
using Grand.Business.Catalog.Interfaces.Products;
using Grand.Business.Common.Interfaces.Localization;
using Grand.Business.Customers.Interfaces;
using Grand.Domain.Catalog;
using Grand.Domain.Common;
using Grand.Infrastructure.Validators;
using System.Collections.Generic;

namespace Grand.Api.Validators.Catalog
{
    public class ProductValidator : BaseGrandValidator<ProductDto>
    {
        public ProductValidator(
            IEnumerable<IValidatorConsumer<ProductDto>> validators,
            ITranslationService translationService, IProductService productService, IProductLayoutService productLayoutService,
            IBrandService brandService,
            IVendorService vendorService, CommonSettings commonSettings)
            : base(validators)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(translationService.GetResource("Api.Catalog.Product.Fields.Name.Required"));
            
            if (!commonSettings.AllowEditProductEndedAuction)
                RuleFor(x => x.AuctionEnded && x.ProductTypeId == ProductType.Auction).Equal(false).WithMessage(translationService.GetResource("Api.Catalog.Products.Cannoteditauction"));

            RuleFor(x => x.ProductTypeId == ProductType.Auction && !x.AvailableEndDateTimeUtc.HasValue).Equal(false).WithMessage(translationService.GetResource("Api.Catalog.Products.Fields.AvailableEndDateTime.Required"));

            RuleFor(x => x).MustAsync(async (x, y, context) =>
            {
                if (!string.IsNullOrEmpty(x.ParentGroupedProductId))
                {
                    var product = await productService.GetProductById(x.ParentGroupedProductId);
                    if (product == null)
                        return false;
                }
                return true;
            }).WithMessage(translationService.GetResource("Api.Catalog.Product.Fields.ParentGroupedProductId.NotExists"));

            RuleFor(x => x).MustAsync(async (x, y, context) =>
            {
                if (!string.IsNullOrEmpty(x.ProductLayoutId))
                {
                    var layout = await productLayoutService.GetProductLayoutById(x.ProductLayoutId);
                    if (layout == null)
                        return false;
                }
                return true;
            }).WithMessage(translationService.GetResource("Api.Catalog.Product.Fields.ProductLayoutId.NotExists"));

            RuleFor(x => x).MustAsync(async (x, y, context) =>
            {
                if (!string.IsNullOrEmpty(x.VendorId))
                {
                    var vendor = await vendorService.GetVendorById(x.VendorId);
                    if (vendor == null)
                        return false;
                }
                return true;
            }).WithMessage(translationService.GetResource("Api.Catalog.Product.Fields.VendorId.NotExists"));

            RuleFor(x => x).MustAsync(async (x, y, context) =>
            {
                if (!string.IsNullOrEmpty(x.BrandId))
                {
                    var brand = await brandService.GetBrandById(x.BrandId);
                    if (brand == null)
                        return false;
                }
                return true;
            }).WithMessage(translationService.GetResource("Api.Catalog.Product.Fields.BrandId.NotExists"));

            RuleFor(x => x).MustAsync(async (x, y, context) =>
            {
                if (!string.IsNullOrEmpty(x.Id))
                {
                    var product = await productService.GetProductById(x.Id);
                    if (product == null)
                        return false;
                }
                return true;
            }).WithMessage(translationService.GetResource("Api.Catalog.Product.Fields.Id.NotExists"));

        }
    }
}
