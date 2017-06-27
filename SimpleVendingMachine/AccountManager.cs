using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SimpleVendingMachine
{
    public class AccountManager
    {
        private readonly Dictionary<string, Account> _accounts;

        private readonly object _sync = new object();

        public AccountManager(List<Account> accounts)
        {
            if(accounts == null)
            {
                throw new ArgumentException("Accounts cannot be null.", "accounts");
            }

            if(accounts.Any(x => string.IsNullOrWhiteSpace(x.AccountIdentifier)))
            {
                throw new ArgumentException("Accounts cannot contain an Account with a null or empty AccountIdentifier.", "accounts");
            }

            _accounts = new Dictionary<string, Account>();

            accounts.Distinct().ToList().ForEach(acc =>
            {
                _accounts.Add(acc.AccountIdentifier, acc);
            });
        }

        public bool Authenticate(string accountIdentifier)
        {
            if(accountIdentifier == null)
            {
                throw new ArgumentException("AccountIdentifier cannot be null.", "accountIdentifier");
            }

            bool lockTaken = false;
            bool result = false;

            try
            {
                Monitor.TryEnter(_sync, 3000, ref lockTaken);

                if (!lockTaken)
                {
                    throw new Exception("Unable to acquire lock on Synchronization object.");
                }

                result = _accounts.ContainsKey(accountIdentifier);
            }
            finally
            {
                Monitor.Exit(_sync);
            }

            return result;
        }

        public decimal GetAccountBalance(string accountIdentifier)
        {
            decimal result = 0.0M;

            bool lockTaken = false;

            try
            {
                Monitor.TryEnter(_sync, 3000, ref lockTaken);

                if (!lockTaken)
                {
                    throw new Exception("Unable to acquire lock on Synchronization object.");
                }

                if (_accounts.ContainsKey(accountIdentifier))
                {
                    result = _accounts[accountIdentifier].Balance;
                }
            }
            finally
            {
                Monitor.Exit(_sync);
            }

            return result;
        }

        public bool CreditAccount(string accountIdentifier, decimal amount)
        {
            bool lockTaken = false;
            bool result = false;

            try
            {
                Monitor.TryEnter(_sync, 3000, ref lockTaken);

                if (!lockTaken)
                {
                    throw new Exception("Unable to acquire lock on Synchronization object.");
                }

                if (_accounts.ContainsKey(accountIdentifier))
                {
                    _accounts[accountIdentifier].CreditBalance(amount);
                }
            }
            finally
            {
                Monitor.Exit(_sync);
            }                

            return result;
        }

        public bool DebitAccount(string accountIdentifier, decimal amount)
        {
            bool result = false;
            bool lockTaken = false;
            
            try
            {
                Monitor.TryEnter(_sync, 3000, ref lockTaken);

                if (!lockTaken)
                {
                    throw new Exception("Unable to acquire lock on Synchronization object.");
                }

                if (_accounts.ContainsKey(accountIdentifier))
                {
                    _accounts[accountIdentifier].DebitBalance(amount);
                }
            }
            finally
            {
                Monitor.Exit(_sync);
            }

            return result;
        }
    }
}
