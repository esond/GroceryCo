using System;
using System.Collections.Generic;
using System.Linq;
using GroceryCo.Kiosk.Features.Cashier;
using GroceryCo.Kiosk.Infrastructure;
using GroceryCo.Model;
using NUnit.Framework;

namespace GroceryCo.Kiosk.UnitTests.Features.Cashier
{
    [TestFixture]
    public class PurchaseTester
    {
        #region Test Data Collections

        public static IEnumerable<string> ItemNames
        {
            get
            {
                yield return "apple";
                yield return "banana";
                yield return "orange";
            }
        }

        public static IEnumerable<GroceryItem> TestGroceryItems
        {
            get
            {
                yield return new GroceryItem("apple", 1.52m);
                yield return new GroceryItem("banana", 6.66m);
                yield return new GroceryItem("orange", 1.11m);
            }
        }

        public static IEnumerable<PurchaseItem> SampleBasket1
        {
            get
            {
                List<PurchaseItem> items = new List<PurchaseItem>();

                items.AddRange(GetTestPurchaseItem("apple", 3));
                items.AddRange(GetTestPurchaseItem("banana", 3));
                items.AddRange(GetTestPurchaseItem("orange", 3));

                return items;
            }
        }

        public static IEnumerable<PurchaseItem> SampleBasket2
        {
            get
            {
                List<PurchaseItem> items = new List<PurchaseItem>();

                items.AddRange(GetTestPurchaseItem("apple", 2));
                items.AddRange(GetTestPurchaseItem("banana", 3));
                items.AddRange(GetTestPurchaseItem("orange", 4));

                return items;
            }
        }

        public static IEnumerable<PurchaseItem> SampleBasket3
        {
            get
            {
                List<PurchaseItem> items = new List<PurchaseItem>();

                items.AddRange(GetTestPurchaseItem("apple", 3));
                items.AddRange(GetTestPurchaseItem("banana", 4));
                items.AddRange(GetTestPurchaseItem("orange", 9));

                return items;
            }
        }

        public static IEnumerable<IEnumerable<PurchaseItem>> SampleBaskets
        {
            get
            {
                yield return new List<PurchaseItem>(SampleBasket1);
                yield return new List<PurchaseItem>(SampleBasket2);
                yield return new List<PurchaseItem>(SampleBasket3);
            }
        }
        
        #endregion

        #region Helpers

        private static GroceryItem GetTestGroceryItem(string name)
        {
            return TestGroceryItems.Single(g => g.Name == name);
        }

        private static IEnumerable<PurchaseItem> GetTestPurchaseItem(string name, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new PurchaseItem(TestGroceryItems.Single(gi => gi.Name == name));
            }
        }

        private static decimal GetRandomDecimal(int min, int max)
        {
            Random random = new Random();

            double next = random.NextDouble();

            return new decimal(min + next*(max - min));
        }

        #endregion

        [Test]
        public void purchase_items_discount_price_is_same_as_regular_price_by_default()
        {
            IEnumerable<GroceryItem> items = TestGroceryItems.ToList();

            Purchase purchase = new Purchase(items.Select(g => new PurchaseItem(g)));

            for (int i = 0; i < items.Count(); i++)
            {
                Assert.AreEqual(items.ElementAt(i).Price, purchase.PurchaseItems.ElementAt(i).DiscountedPrice);
            }
        }

        [Test]
        [TestCaseSource(nameof(SampleBaskets))]
        public void applying_on_sale_promo_sets_sale_price_for_all_applicable_items_in_purchase(
            IEnumerable<PurchaseItem> basket)
        {
            decimal salePrice = Math.Round(GetRandomDecimal(0, 1), 2);

            List<Promotion> promotions = new List<Promotion>();

            foreach (string itemName in ItemNames)
            {
                promotions.Add(PromotionFactory.CreatePromotion(
                    GetTestGroceryItem(itemName), PromotionType.OnSale, 0, salePrice));
            }

            Purchase purchase = new Purchase(basket);
            purchase.ApplyManyPromotions(promotions);

            foreach (string itemName in ItemNames)
            {
                IEnumerable<PurchaseItem> items = purchase.PurchaseItems.Where(pi => pi.GroceryItemName == itemName);

                Assert.That(items.All(a => a.DiscountedPrice == salePrice));
            }
        }

        [Test]
        [TestCaseSource(nameof(SampleBaskets))]
        public void group_promo_sets_sale_price_for_a_set_of_required_items(IEnumerable<PurchaseItem> sampleBasket)
        {
            decimal salePrice = Math.Round(GetRandomDecimal(0, 1), 2);
            int itemsRequired = new Random().Next(5);

            List<Promotion> promotions = new List<Promotion>();

            foreach (string itemName in ItemNames)
            {
                promotions.Add(PromotionFactory.CreatePromotion(
                    GetTestGroceryItem(itemName), PromotionType.Group, itemsRequired, salePrice));
            }

            Purchase purchase = new Purchase(sampleBasket);
            purchase.ApplyManyPromotions(promotions);

            foreach (string itemName in ItemNames)
            {
                int applicableItemsCount = purchase.PurchaseItems.Count(pi => pi.GroceryItemName == itemName);
                int discountedItemsCount = purchase.PurchaseItems.Count(
                    p => (p.GroceryItemName == itemName) && (p.DiscountedPrice == salePrice));
                 
                Assert.That((discountedItemsCount == 0) || (applicableItemsCount % discountedItemsCount < itemsRequired));
            }
        }

        [Test]
        [TestCaseSource(nameof(SampleBaskets))]
        public void additional_product_promo_sets_sale_price_for_the_next_item_of_that_type(
            IEnumerable<PurchaseItem> sampleBasket)
        {
            decimal salePrice = Math.Round(GetRandomDecimal(0, 1), 2);
            int itemsRequired = new Random().Next(3);

            List<Promotion> promotions = new List<Promotion>();

            foreach (string itemName in ItemNames)
            {
                promotions.Add(PromotionFactory.CreatePromotion(
                    GetTestGroceryItem(itemName), PromotionType.AdditionalProduct, itemsRequired, salePrice));
            }

            Purchase purchase = new Purchase(sampleBasket);
            purchase.ApplyManyPromotions(promotions);

            foreach (string itemName in ItemNames)
            {
                double applicableItemsCount = purchase.PurchaseItems.Count(pi => pi.GroceryItemName == itemName);
                double discountedItemsCount = purchase.PurchaseItems.Count(
                    p => (p.GroceryItemName == itemName) && (p.DiscountedPrice == salePrice));

                int expectedDiscountedItemsCount = (int) Math.Floor(applicableItemsCount/(itemsRequired + 1));

                Assert.That(expectedDiscountedItemsCount == (int) discountedItemsCount);
            }
        }
    }
}
