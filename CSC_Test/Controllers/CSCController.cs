using Microsoft.AspNetCore.Mvc;

namespace CSC_Test.Controllers
{
    [ApiController]
    [Route("api/pro-credit")]
    public class CSCController : ControllerBase
    {
        private readonly ILogger<CSCController> _logger;

        public CSCController(ILogger<CSCController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("getbyshopperid")]
        public IEnumerable<ShopperProCreditViewModel> Get(string shopperId)
        {
            return shopperProCredits.Where(s => s.ShopperId.Equals(shopperId));
        }

        [HttpGet]
        [Route("getbyid")]
        public ShopperProCreditViewModel GetById(int shopperProCreditId)
        {
            return shopperProCredits.First(s => s.ShopperProCreditID == shopperProCreditId);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add([FromBody] AddShopperProCreditViewModel viewModel)
        {
            for (int i = 0; i < viewModel.QuantityToBeAdded; i++)
            {
                shopperProCredits = shopperProCredits.Append(
                new ShopperProCreditViewModel
                {
                    ShopperId = viewModel.ShopperId,
                    DateCreated = DateTime.Now,
                    DateExpired = DateTime.Now.AddYears(1),
                    CreditValue = viewModel.CreditValue,
                    ShopperProCreditSourceTypeId = ShopperProCreditSourceType.CustomerService,
                    ShopperProCreditID = shopperProCredits.OrderByDescending(s => s.ShopperProCreditID).First().ShopperProCreditID + 1,
                }).ToList();
            }

            return Ok();
        }

        [HttpPost]
        [Route("renew-pro-credit")]
        public IActionResult Renew(RenewOrRemoveShopperProCredits shopperProCreditIds)
        {

            var proCredits = shopperProCredits.Where(s => shopperProCreditIds.ShopperProCreditsIds.Contains(s.ShopperProCreditID));

            List<ShopperProCreditViewModel> newProCredits = new List<ShopperProCreditViewModel>();

            foreach (var proCredit in proCredits)
            {
                var newProCredit = new ShopperProCreditViewModel
                {
                    ShopperProCreditID = shopperProCredits.OrderByDescending(s => s.ShopperProCreditID).First().ShopperProCreditID + 1,
                    ShopperId = proCredit.ShopperId,
                    DateCreated = proCredit.DateCreated,
                    DateExpired = DateTime.Now.AddYears(1),
                    CreditValue = proCredit.CreditValue,
                    ShopperProCreditSourceTypeId = proCredit.ShopperProCreditSourceTypeId,
                    IsAvailable = proCredit.IsAvailable,
                    IsExpired = proCredit.IsExpired,
                    PurchaseReceiptItemId = proCredit.PurchaseReceiptItemId,
                    RedemptionOrderId = proCredit.RedemptionOrderId,
                    RedemptionReceiptItemId = proCredit.RedemptionReceiptItemId,
                };

                shopperProCredits = shopperProCredits.Append(newProCredit).ToList();
            }

           
            return Ok();
        }

        [HttpDelete]
        [Route("remove")]
        public IActionResult Remove(RenewOrRemoveShopperProCredits shopperProCreditIds)
        {
            var proCredits = shopperProCredits.Where(s => shopperProCreditIds.ShopperProCreditsIds.Contains(s.ShopperProCreditID));

            foreach (var proCredit in proCredits)
            {
                shopperProCredits.Remove(proCredit);
            }

            return Ok();
        }

        private static List<ShopperProCreditViewModel> shopperProCredits = new List<ShopperProCreditViewModel>()
        {
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 1,
                ShopperId = "FFFFDDSS",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "55",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 2,
                ShopperId = "dasdasdas",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "56",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 3,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = null,
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 4,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.CustomerService,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "77",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 5,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "77",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 6,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "77",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 7,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "77",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 8,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "77",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 9,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "77",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 10,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-3),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "77",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 11,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "77",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 12,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "77",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 13,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "77",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 14,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "77",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 15,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "77",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 16,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "77",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 17,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "77",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 18,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "77",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 19,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "77",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 20,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "77",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 21,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "77",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 22,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "77",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 23,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "77",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 24,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "77",
                RedemptionReceiptItemId = Guid.NewGuid(),
            },
            new ShopperProCreditViewModel
            {
                ShopperProCreditID = 25,
                ShopperId = "3",
                DateCreated = DateTime.Now.AddYears(-2),
                DateExpired = DateTime.Now.AddYears(-1),
                CreditValue = 5,
                ShopperProCreditSourceTypeId = ShopperProCreditSourceType.ProPremiumMonthlyInitial,
                IsAvailable = true,
                IsExpired = true,
                PurchaseReceiptItemId = Guid.NewGuid(),
                RedemptionOrderId = "77",
                RedemptionReceiptItemId = Guid.NewGuid(),
            }
        };
    }
}
