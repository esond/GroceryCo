using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GroceryCo.Model;
using GroceryCo.Repository;

namespace GroceryCo.Kiosk.Features.Cashier
{
    public class CashierConsole
    {
        private readonly IRepository _repository;

        public CashierConsole(IRepository repository)
        {
            _repository = repository;
        }

        public void Run()
        {
            bool goBack = false;

            while (!goBack)
            {
                Console.WriteLine("Select an action...");
                Console.Write("[S]tart a new checkout | Go [b]ack:");

                string action = Console.ReadLine()?.ToLowerInvariant();

                switch (action)
                {
                    case "s":
                        StartCheckout();
                        break;

                    case "b":
                        goBack = true;
                        break;

                    default:
                        Console.WriteLine("Invalid selection.");
                        break;
                }
            }
        }

        private void StartCheckout()
        {
            IEnumerable<GroceryItem> allItems = _repository.GetAll<GroceryItem>().ToList();
            IEnumerable<Promotion> promotions = _repository.GetAll<Promotion>().ToList();

            List<PurchaseItem> purchasedItems = new List<PurchaseItem>();
            List<Promotion> effectivePromotions = new List<Promotion>();

            IEnumerable<string> basketContents = GetBasketContents();

            foreach (string basketItem in basketContents)
            {
                GroceryItem purchasedGroceryItem = allItems.Single(i => i.Name == basketItem);
                purchasedItems.Add(new PurchaseItem(purchasedGroceryItem));

                Promotion promotion = promotions.SingleOrDefault(p => p.GroceryItemName == purchasedGroceryItem.Name);

                if ((promotion != null) && !effectivePromotions.Exists(p => p.Id == promotion.Id))
                    effectivePromotions.Add(promotion);
            }

            Cashier.DoCheckout(purchasedItems, effectivePromotions);
        }

        private IEnumerable<string> GetBasketContents()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            dialog.FilterIndex = 1;

            if (dialog.ShowDialog() != DialogResult.OK)
                return Enumerable.Empty<string>();

            string basketContents;

            using (Stream stream = dialog.OpenFile())
            using (StreamReader reader = new StreamReader(stream))
            {
                basketContents = reader.ReadToEnd();
            }

            return basketContents.Split(',');
        }
    }
}
