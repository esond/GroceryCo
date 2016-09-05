using System;
using System.Collections.Generic;
using System.Linq;
using GroceryCo.Kiosk.Features.Cashier;
using GroceryCo.Kiosk.Infrastructure;
using GroceryCo.Model;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace GroceryCo.Kiosk.UnitTests.Features.Cashier
{
    [TestFixture]
    public class PurchaseTester
    {
        #region Test Data Collections

        public static IEnumerable<GroceryItem> TestGroceryItems
        {
            get
            {
                yield return new GroceryItem("apple", 1.52m);
                yield return new GroceryItem("banana", 6.66m);
                yield return new GroceryItem("orange", 1.11m);
            }
        }

        public static IEnumerable<string> ItemNames
        {
            get
            {
                yield return "apple";
                yield return "banana";
                yield return "orange";
            }
        }

        public static IEnumerable<PurchaseItem> OnSalePromomoSampleBasket
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

        public static IEnumerable<PurchaseItem> GroupPromoSampleBasket
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

        public static IEnumerable<PurchaseItem> AdditionalItemPromoSampleBasket
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
                yield return OnSalePromomoSampleBasket;
                yield return GroupPromoSampleBasket;
                yield return AdditionalItemPromoSampleBasket;
            }
        }

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
        public void applying_on_sale_promo_sets_sale_price_for_all_applicable_items_in_purchase(IEnumerable<PurchaseItem> basket)
        {
            decimal salePrice = Math.Round(GetRandomDecimal(0, 3), 2);

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


        #region Group Promo Tests

        [Test]
        [Repeat(5)]
        [TestCaseSource(nameof(SampleBaskets))]
        public void group_promo_sets_sale_price_for_a_set_of_required_items(IEnumerable<PurchaseItem> sampleBasket)
        {
            decimal salePrice = Math.Round(GetRandomDecimal(0, 3), 2);
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

                Assert.That((discountedItemsCount == 0) || (applicableItemsCount%discountedItemsCount < itemsRequired));
            }
        }

        #endregion

        #region AdditionalItem Promo Tests

        [Test]
        public void additional_product_promo_sets_sale_price_for_the_next_item_of_that_type()
        {
            const decimal appleSalePrice = 0.50m;
            const decimal bananaSalePrice = 0.44m;
            const decimal orangeSalePrice = 0.60m;

            int appleItemsRequired = 1;
            int bananaItemsRequired = 2;
            int orangeItemsRequired = 3;

            List<Promotion> promotions = new List<Promotion>();

            promotions.Add(PromotionFactory.CreatePromotion(
                GetTestGroceryItem("apple"), PromotionType.AdditionalProduct, appleItemsRequired, appleSalePrice));
            promotions.Add(PromotionFactory.CreatePromotion(
                GetTestGroceryItem("banana"), PromotionType.AdditionalProduct, bananaItemsRequired, bananaSalePrice));
            promotions.Add(PromotionFactory.CreatePromotion(
                GetTestGroceryItem("orange"), PromotionType.AdditionalProduct, orangeItemsRequired, orangeSalePrice));

            Purchase purchase = new Purchase(AdditionalItemPromoSampleBasket);
            purchase.ApplyManyPromotions(promotions);

            IEnumerable<PurchaseItem> discountedApples =
                purchase.PurchaseItems.Where(pi => pi.GroceryItemName == "apple" && pi.DiscountedPrice == appleSalePrice);
            Assert.AreEqual(1, discountedApples.Count());

            IEnumerable<PurchaseItem> discountedBananas =
                purchase.PurchaseItems.Where(pi => pi.GroceryItemName == "banana" && pi.DiscountedPrice == bananaSalePrice);
            Assert.AreEqual(1, discountedBananas.Count());

            IEnumerable<PurchaseItem> discountedOranges =
                purchase.PurchaseItems.Where(pi => pi.GroceryItemName == "orange" && pi.DiscountedPrice == orangeSalePrice);
            Assert.AreEqual(2, discountedOranges.Count());
        }


        // TODO
        //[Test]
        //public void additional_item_promo_sets_sale_price_for_a_number_of_items_equal_to_multiple_of_required_items_plus_one()
        //{
        //    throw new NotImplementedException();
            
        //}

        #endregion
    }
}
