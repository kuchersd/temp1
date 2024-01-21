namespace CSC_Test
{
    public class ShopperProCreditViewModel
    {
        public int ShopperProCreditID { get; set; }
        public string ShopperId { get; set; }
        /// <summary>
        /// The ID of the order when this credit was redeemed in exchange for sheet music.
        /// </summary>
        public string RedemptionOrderId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateExpired { get; set; }
        /// <summary>
        /// The ID of the ReceiptItem when this credit was redeemed in exchange for sheet music.
        /// </summary>
        public Guid? RedemptionReceiptItemId { get; set; }
        /// <summary>
        /// The ID of the ReceiptItem when this credit was originally purchased.
        /// </summary>
        public Guid? PurchaseReceiptItemId { get; set; }
        public ShopperProCreditSourceType ShopperProCreditSourceTypeId { get; set; }
        /// <summary>
        /// Only used for royalties reporting.
        /// </summary>
        public int CreditValue { get; set; }
        public bool IsExpired { get; set; }
        public bool IsAvailable { get; set; }
    }
}
