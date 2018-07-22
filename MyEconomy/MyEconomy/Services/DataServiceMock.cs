using System;
using System.Collections.Generic;
using MyEconomy.Models;

namespace MyEconomy.Services
{
    public class DataServiceMock : IDataService
    {
        readonly Random _random = new Random();

        public List<Category> GetCategories()
        {
            int numberOfMockCategories = 10;
            List<Category> categories = new List<Category>();

            for (int i = 0; i < numberOfMockCategories; i++)
            {
                categories.Add(new Category(RandomGenerator.RandomTitle(), ""));
            }

            categories = PopulateCategories(categories);
            
            return categories;
        }

        private List<Category> PopulateCategories(List<Category> categories)
        {
            int numberOfTransactions = 4 * categories.Count;

            List<Transaction> transactions = new List<Transaction>();
            for (int i = 0; i < numberOfTransactions; i++)
            {
                transactions.Add(GenerateTransaction());
            }

            transactions.Sort((x, y) => x.Date.CompareTo(y.Date));

            for (int i = 0; i < numberOfTransactions; i++)
            {
                categories[_random.Next(0, categories.Count - 1)].AddTransaction(transactions[i]);
            }

            return categories;
        }

        private Transaction GenerateTransaction()
        {
            double amount = _random.NextDouble() * 1000;
            Transaction transaction = new Transaction(amount, RandomGenerator.RandomDateTime(), RandomGenerator.RandomTitle());

            return transaction;
        }
    }
}
