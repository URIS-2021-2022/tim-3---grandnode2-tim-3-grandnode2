namespace Grand.Business.Checkout.Services.Orders
{
    public class ShoppingCartValidatorOptions
    {
        public bool GetStandardWarnings { get; set; } = true;
        public bool GetAttributesWarnings { get; set; } = true;
        public bool GetInventoryWarnings { get; set; } = true;
        public bool GetGiftVoucherWarnings { get; set; } = true;
        public bool GetRequiredProductWarnings { get; set; } = true;
        public bool GetRentalWarnings { get; set; } = true;
        public bool GetReservationWarnings { get; set; } = true;
    }
}
