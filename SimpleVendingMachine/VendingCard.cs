using System;
using System.Threading;

namespace SimpleVendingMachine
{
    public class VendingCard
    {
        private readonly AccountManager _accountManager;

        private readonly object _sync = new object();

        public VendingCard(string accountIdentifier, AccountManager accountManager)
        {
            if(string.IsNullOrWhiteSpace(accountIdentifier))
            {
                throw new ArgumentException("AccountIdentifier cannot be null or empty.", "accountIdentifier");
            }

            if ((accountManager == null))
            {
                throw new ArgumentException("AccountManager cannot be null.", "accountManager");
            }

            _accountManager = accountManager;

            AccountIdentifier = accountIdentifier;
        }

        public string AccountIdentifier
        {
            get;
            private set;
        }

        public string Pin
        {
            get;
            private set;
        }

        public bool SetPin(string pin)
        {
            bool result = false;

            if (string.IsNullOrWhiteSpace(pin))
            {
                throw new ArgumentException("Pin cannot be null or empty.", "pin");
            }

            Pin = pin;

            result = true;

            return result;
        }

        public bool VerifyPin(string pin)
        {
            if (string.IsNullOrWhiteSpace(pin))
            {
                throw new ArgumentException("Pin cannot be null or empty.", "pin");
            }

            return Pin.Equals(pin, StringComparison.InvariantCulture);
        }

        private bool AuthenticateAccount()
        {
            bool lockTaken = false;

            try
            {
                Monitor.TryEnter(_sync, 3000, ref lockTaken);

                if (!lockTaken)
                {
                    throw new Exception("Unable to acquire lock on Synchronization object.");
                }

                return _accountManager.Authenticate(AccountIdentifier);
            }
            finally
            {
                Monitor.Exit(_sync);
            }
        }

        public bool CreditAccount(string pin, decimal amount)
        {
            bool result = false;

            if (AuthenticateAccount() && VerifyPin(pin))
            {
                bool lockTaken = false;
                try
                {
                    Monitor.TryEnter(_sync, 3000, ref lockTaken);

                    if (!lockTaken)
                    {
                        throw new Exception("Unable to acquire lock on Synchronization object.");
                    }

                    result = _accountManager.CreditAccount(AccountIdentifier, amount);
                }
                finally
                {
                    Monitor.Exit(_sync);
                }
            }

            return result;
        }

        public bool DebitAccount(string pin, decimal amount)
        {
            bool result = false;

            if (AuthenticateAccount() && VerifyPin(pin))
            {
                bool lockTaken = false;

                try
                {
                    Monitor.TryEnter(_sync, 3000, ref lockTaken);

                    if (!lockTaken)
                    {
                        throw new Exception("Unable to acquire lock on Synchronization object.");
                    }

                    result = _accountManager.DebitAccount(AccountIdentifier, amount);
                }
                finally
                {
                    Monitor.Exit(_sync);
                }

            }

            return result;
        }
    }
}
