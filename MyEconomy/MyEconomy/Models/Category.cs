using MyEconomy.Services;
using PropertyChanged;
using System;
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
                return CategorySum().ToString("N2");
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

        public void AddRecurringTransaction(Transaction transaction, DateTime startDate, DateTime endDate, string reccurence)
        {
            DateTime sampleDate = startDate;
            while (sampleDate < endDate)
            {
                Transactions.Add(new Transaction(transaction.Amount, sampleDate, transaction.Title, transaction.Description));
                sampleDate = RecurrenceService.GetSubsequentDate(sampleDate, RecurrenceService.TranslateAbbreviationToInterval(reccurence));
            }
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

        public double CategorySum(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new ArgumentException("End date must be later than start date.");
            double sum = 0;
            foreach (Transaction t in Transactions)
            {
                if(t.Date >= startDate && t.Date <= endDate)
                    sum += t.Amount;
            }
            return sum;
        }
    }
}
