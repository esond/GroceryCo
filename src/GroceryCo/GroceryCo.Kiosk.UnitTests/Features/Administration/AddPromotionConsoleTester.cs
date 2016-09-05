using System;
using System.Collections.Generic;
using GroceryCo.Kiosk.Features.Administration;
using GroceryCo.Model;
using NUnit.Framework;

namespace GroceryCo.Kiosk.UnitTests.Features.Administration
{
    [TestFixture]
    public class AddPromotionConsoleTester
    {
        public static IEnumerable<GroceryItem> TestGroceryItems
        {
            get
            {
                yield return new GroceryItem("a", 100.00m);
                yield return new GroceryItem("c", 6.66m);
                yield return new GroceryItem("d", 1.25m);
                yield return new GroceryItem("e", 22m);
                yield return new GroceryItem("f", decimal.MinusOne);
            }
        }

        public static IEnumerable<decimal> SalePrices
        {
            get
            {
                yield return decimal.Zero;
                yield return 1m;
                yield return 0.01m;
                yield return 2.35m;
                yield return decimal.MaxValue;
                yield return decimal.One;
                yield return 6.66m;
            }
        }

        [Test]
        [TestCaseSource(nameof(TestGroceryItems))]
        public void adding_OnSale_promotion_calculates_correct_discount(GroceryItem groceryItem)
        {
            foreach (decimal salePrice in SalePrices)
            {
                Promotion promotion = AddPromotionConsole.CreatePromotion(groceryItem, PromotionType.OnSale, 0, salePrice);

                double expected = (double) (salePrice/groceryItem.Price);

                Assert.AreEqual(expected, promotion.Discount);
            }
        }

        [Test]
        [TestCaseSource(nameof(TestGroceryItems))]
        public void adding_group_promotion_calculates_correct_discount(GroceryItem groceryItem)
        {
            foreach (decimal salePrice in SalePrices)
            {
                Promotion promotion = AddPromotionConsole.CreatePromotion(groceryItem, PromotionType.Group, 666, salePrice);

                double expected = (double)(salePrice / groceryItem.Price);

                Assert.AreEqual(expected, promotion.Discount);
            }
        }

        [Test]
        [TestCaseSource(nameof(TestGroceryItems))]
        public void adding_AdditionalProduct_promotion_calculates_correct_discount(GroceryItem groceryItem)
        {
            foreach (decimal salePrice in SalePrices)
            {
                Promotion promotion = AddPromotionConsole.CreatePromotion(groceryItem, PromotionType.AdditionalProduct, 0, salePrice);

                double expected = (double)(salePrice / groceryItem.Price);

                Assert.AreEqual(expected, promotion.Discount);
            }
        }

        [Test]
        public void adding_promotion_to_item_with_price_of_zero_sets_discount_to_zero()
        {
            GroceryItem foo = new GroceryItem("foo", decimal.Zero);

            Promotion promotion = AddPromotionConsole.CreatePromotion(foo, PromotionType.OnSale, 0, 0m);
            
            Assert.That(Math.Abs(promotion.Discount) < 0.0001);
        }
    }
}
