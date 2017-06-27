using System;
using System.Threading;

namespace SimpleVendingMachine
{
    public class Account
    {
        public readonly object _sync = new object();

        private decimal balance;

        public Account(string accountIdentifier)
        {
            if(string.IsNullOrWhiteSpace(accountIdentifier))
            {
                throw new ArgumentException("AccountIdentifier cannnot be null or empty.", "accountIdentifier");
            }

            AccountIdentifier = accountIdentifier;
        }

        public string AccountIdentifier
        {
            get;
            private set;
        }

        public decimal Balance
        {
            get { return balance; }
            private set
            {
                if(value < 0.0M)
                {
                    throw new ArgumentException("Supplied value cannot be less than zero.", "value");
                }

                bool lockTaken = false;
                
                try
                {
                    Monitor.TryEnter(_sync, 3000, ref lockTaken);
                    
                    if (!lockTaken)
                    {
                        throw new Exception("Unable to acquire lock on Synchronization object.");
                    }

                    balance = value;
                }
                finally
                {
                    Monitor.Exit(_sync);
                }
            }
        }

        public bool CreditBalance(decimal amount)
        {
            bool result = false;
            try
            {
                Balance += amount;
                result = true;
            }
            catch(ArgumentException argEx)
            {
                string argExMessage = string.Format("{0}{1}{2}{3}", "Supplied value cannot be less than zero.", Environment.NewLine, "Parameter name: ", "value");
                if (argEx.ParamName == "value" && argEx.Message == argExMessage)
                {
                    throw new ArgumentException("Cannot set Balance to less than zero via CreditBalance method.", "amount");
                }

                throw argEx;
            }

            return result;
        }

        public bool DebitBalance(decimal amount)
        {
            bool result = false;
            try
            { 
                Balance -= amount;
                result = true; 
            }
            catch(ArgumentException argEx)
            {
                string argExMessage = string.Format("{0}{1}{2}{3}", "Supplied value cannot be less than zero.", Environment.NewLine, "Parameter name: ", "value");
                if (argEx.ParamName == "value" && argEx.Message == argExMessage)
                {
                    throw new ArgumentException("Cannot set Balance to less than zero via DebitBalance method.", "amount");
                }

                throw argEx;
            }

            return result;
        }
    }
}
