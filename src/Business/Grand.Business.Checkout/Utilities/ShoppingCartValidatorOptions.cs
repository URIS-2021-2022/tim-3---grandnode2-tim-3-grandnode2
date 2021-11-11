namespace Grand.Business.Checkout.Services.Orders
{
    public class ShoppingCartValidatorOptions
    {
        private bool getStandardWarnings = true;
        private bool getAttributesWarnings = true;
        private bool getInventoryWarnings = true;
        private bool getGiftVoucherWarnings = true;
        private bool getRequiredProductWarnings = true;
        private bool getRentalWarnings = true;
        private bool getReservationWarnings = true;

        public bool GetStandardWarnings { get => getStandardWarnings; set => getStandardWarnings = value; }
        public bool GetAttributesWarnings { get => getAttributesWarnings; set => getAttributesWarnings = value; }
        public bool GetInventoryWarnings { get => getInventoryWarnings; set => getInventoryWarnings = value; }
        public bool GetGiftVoucherWarnings { get => getGiftVoucherWarnings; set => getGiftVoucherWarnings = value; }
        public bool GetRequiredProductWarnings { get => getRequiredProductWarnings; set => getRequiredProductWarnings = value; }
        public bool GetRentalWarnings { get => getRentalWarnings; set => getRentalWarnings = value; }
        public bool GetReservationWarnings { get => getReservationWarnings; set => getReservationWarnings = value; }
    }
}
