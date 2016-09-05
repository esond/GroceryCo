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

        public static IEnumerable<GroceryItem> TestGroceryItems
        {
            get
            {
                yield return new GroceryItem("apple", 1.50m);
                yield return new GroceryItem("banana", 0.50m);
                yield return new GroceryItem("orange", 1.00m);
                yield return new GroceryItem("bread", 6.66m);
                yield return new GroceryItem("eggs", 5.23m);
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

        // TODO: These tests are shockingly big and need to be refactored.
        [Test]
        public void applying_on_sale_promo_sets_sale_price_for_all_applicable_items_in_purchase()
        {
            const decimal appleSalePrice = 0.50m;
            const decimal orangeSalePrice = 0.25m;

            List<Promotion> promotions = new List<Promotion>();

            promotions.Add(PromotionFactory.CreatePromotion(
                GetTestGroceryItem("apple"), PromotionType.OnSale, 0, appleSalePrice));
            promotions.Add(PromotionFactory.CreatePromotion(
                GetTestGroceryItem("orange"), PromotionType.OnSale, 0, orangeSalePrice));

            Purchase purchase = new Purchase(OnSalePromomoSampleBasket);
            purchase.ApplyManyPromotions(promotions);

            IEnumerable<PurchaseItem> apples = purchase.PurchaseItems.Where(pi => pi.GroceryItemName == "apple");
            Assert.That(apples.All(a => a.DiscountedPrice == appleSalePrice));

            IEnumerable<PurchaseItem> oranges = purchase.PurchaseItems.Where(pi => pi.GroceryItemName == "orange");
            Assert.That(oranges.All(a => a.DiscountedPrice == orangeSalePrice));
        }

        #region Group Promo Tests

        [Test]
        public void group_promo_sets_sale_price_for_a_set_of_required_items()
        {
            const decimal appleSalePrice = 0.50m;
            const decimal bananaSalePrice = 0.44m;
            const decimal orangeSalePrice = 0.60m;

            int appleItemsRequired = 1;
            int bananaItemsRequired = 2;
            int orangeItemsRequired = 3;

            List<Promotion> promotions = new List<Promotion>();

            promotions.Add(PromotionFactory.CreatePromotion(
                GetTestGroceryItem("apple"), PromotionType.Group, appleItemsRequired, appleSalePrice));
            promotions.Add(PromotionFactory.CreatePromotion(
                GetTestGroceryItem("banana"), PromotionType.Group, bananaItemsRequired, bananaSalePrice));
            promotions.Add(PromotionFactory.CreatePromotion(
                GetTestGroceryItem("orange"), PromotionType.Group, orangeItemsRequired, orangeSalePrice));

            Purchase purchase = new Purchase(GroupPromoSampleBasket);
            purchase.ApplyManyPromotions(promotions);

            IEnumerable<PurchaseItem> discountedApples =
                purchase.PurchaseItems.Where(pi => pi.GroceryItemName == "apple" && pi.DiscountedPrice == appleSalePrice);
            Assert.AreEqual(2, discountedApples.Count());

            IEnumerable<PurchaseItem> discountedBananas =
                purchase.PurchaseItems.Where(pi => pi.GroceryItemName == "banana" && pi.DiscountedPrice == bananaSalePrice);
            Assert.AreEqual(2, discountedBananas.Count());

            IEnumerable<PurchaseItem> discountedOranges =
                purchase.PurchaseItems.Where(pi => pi.GroceryItemName == "orange" && pi.DiscountedPrice == orangeSalePrice);
            Assert.AreEqual(3, discountedOranges.Count());
        }

        //TODO
        //[Test]
        //public void group_promo_sets_sale_price_for_a_number_of_items_equal_to_multiple_of_required_items()
        //{
        //    throw new NotImplementedException();
        //}

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
