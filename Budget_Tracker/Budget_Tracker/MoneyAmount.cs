﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Budget_Tracker
{
    public class MoneyAmount
    {
        private double amount;
        private DateTime date;
        private string purpose;

        public MoneyAmount(MoneyAmount m)
        {
            amount = m.amount;
            date = m.date;
            purpose = m.purpose;
        }

        public MoneyAmount(double amount, DateTime date, string purpose)
        {
            this.amount = amount;
            this.date = date;
             this.purpose = purpose;
        }

        public double getAmount()
        {
            return this.amount;
        }

        public DateTime getDate()
        {
            return this.date;
        }

        public string getPurpose()
        {
            return this.purpose;
        }

        public void setAmount(double amount)
        {
            this.amount = amount;
        }

        public void addAmount(double amount)
        {
            this.amount += amount;
        }
    }
}