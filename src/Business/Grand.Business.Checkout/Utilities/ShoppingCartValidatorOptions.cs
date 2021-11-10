namespace Grand.Business.Checkout.Services.Orders
{
    public class ShoppingCartValidatorOptions
    {
        public bool GetAttributesWarnings = true;
        public bool GetInventoryWarnings = true;
        public bool GetGiftVoucherWarnings = true;
        public bool GetRequiredProductWarnings = true;
        public bool GetRentalWarnings = true;
        public bool GetReservationWarnings = true;

        public bool GetStandardWarnings { get; set; } = true;
    }
}
