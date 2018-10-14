﻿using System;

namespace MyEconomy.Models
{
    public class Transaction
    {
        public Guid Id { get; private set; }
        public double Amount { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public string AmountString
        {
            get
            {
                return Amount.ToString("N2");
            }
        }

        public Transaction(double amount, DateTime date, string title = "Amount", string description = "")
        {
            Id = Guid.NewGuid();
            Amount = amount;
            Title = title;
            Description = description;
            if (date != null)
                Date = date;
            else
                Date = DateTime.UtcNow;
        }
    }
}
