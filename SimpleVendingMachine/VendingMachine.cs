using System;
using System.Collections.Generic;
using System.Threading;

namespace SimpleVendingMachine
{
    public class VendingMachine
    {
        private int _maxStockLines;
        private int _maxStockInventory;
        private bool _isOutOfService;

        private readonly object _sync = new object();

        private Inventory _inventory;
        private VendingCard _insertedCard;
        private AccountManager _accountManager;
        private AccountService _accountService;

        public int MaxStockLines
        {
            get { return _maxStockLines; }
        }

        public int MaxStockInventory
        {
            get { return _maxStockInventory; }
        } 

        public VendingMachine(AccountService accountService, int maxStockLines = 1, int maxStockInventory = 25)
        {
            _maxStockLines = maxStockLines;
            _maxStockInventory = maxStockInventory;
            _accountService = accountService;

            GetAccounts();

            _inventory = new Inventory(
            MaxStockLines,
            MaxStockInventory,
            new List<StockLine>());
        }

        private void GetAccounts()
        {
            if (_accountManager == null)
            {
                bool lockTaken = false;
                try
                {
                    Monitor.TryEnter(_sync, 3000, ref lockTaken);

                    if (!lockTaken)
                    {
                        throw new Exception("Unable to acquire lock on Synchronization object.");
                    }

                    if (_accountManager == null)
                    {
                        List<Account> accounts =_accountService.GetAllAccounts();
                        _accountManager = new AccountManager(accounts);
                    }
                }
                finally
                {
                    Monitor.Exit(_sync);
                }
            }            
        }
        
        public void InsertCard(VendingCard card)
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(_sync, 3000, ref lockTaken);

                if (!lockTaken)
                {
                    throw new Exception("Unable to acquire lock on Synchronization object.");
                }

                _insertedCard = card;
                DisplayCardBalance();
            }
            finally
            {
                Monitor.Exit(_sync);
            }
        }

        public void EjectCard(VendingCard card)
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(_sync, 3000, ref lockTaken);

                if (!lockTaken)
                {
                    throw new Exception("Unable to acquire lock on Synchronization object.");
                }

                _insertedCard = null;
            }
            finally
            {
                Monitor.Exit(_sync);
            }
        }

        public bool VendProduct(string productIdentifier, string pin)
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
 
                if (_accountManager != null)
                {
                    decimal availableBalance = _accountManager.GetAccountBalance(_insertedCard.AccountIdentifier);

                    if (availableBalance > 0.5M)
                    {
                        if (_inventory.CurrentStockLines.ContainsKey(productIdentifier))
                        {
                            StockLine selectedStockLine = _inventory.CurrentStockLines[productIdentifier];
                            if (selectedStockLine.Product != null)
                            {
                                decimal unitCost = selectedStockLine.Product.Price;
                                if (selectedStockLine.Stock > 0)
                                {
                                    if ((availableBalance >= unitCost) && _insertedCard.DebitAccount(pin, unitCost))
                                    {
                                        result = _inventory.RemoveInventory(productIdentifier, 1);
                                        _isOutOfService = (_inventory.CurrentStockLevel == 0);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                Monitor.Exit(_sync);
            }

            return result;
        }

        public decimal DisplayCardBalance()
        {
            if (_insertedCard != null)
            {
                if (_accountManager != null)
                {
                    return _accountManager.GetAccountBalance(_insertedCard.AccountIdentifier);
                }
            }

            return 0.0m;
        }


        public bool IsOutOfService
        {
            get { return _isOutOfService; }
        }
    }
}
