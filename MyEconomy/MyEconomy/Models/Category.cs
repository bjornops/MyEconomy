using PropertyChanged;
using System.Collections.Generic;

namespace MyEconomy.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Category
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Transaction> Transactions { get; } = new List<Transaction>();
        public string SumString
        {
            get
            {
                return CategorySum().ToString("0.00");
            }
        }

        public Category(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
        }

        public void RemoveTransaction(int index)
        {
            if(index >= 0 && index < Transactions.Count)
            {
                Transactions.RemoveAt(index);
            }
        }

        public double CategorySum()
        {
            double sum = 0;
            foreach(Transaction t in Transactions)
            {
                sum += t.Amount;
            }
            return sum;
        }
    }
}
