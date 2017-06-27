using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SimpleVendingMachine
{
    public class Inventory
    {
        private readonly object _sync = new object();

        private readonly Dictionary<string, StockLine> _stockLines;

        private readonly int MaxAllowedStock;
        private readonly int MaxAllowedStockLines;
        private int _currentStockLevel;

        public Inventory(int maxStockLines, int maxStockLevel, List<StockLine> stock)
        {
            if (maxStockLines <= 0)
            {
                throw new ArgumentException("MaxStockLines cannot be less than zero.", "maxStockLines");
            }

            if (maxStockLevel <= 0)
            {
                throw new ArgumentException("MaxStockLevel cannot be less than zero.", "maxStockLevel");
            }

            if (stock == null)
            {
                throw new ArgumentException("Stock cannot be null.", "stock");
            }

            MaxAllowedStock = maxStockLevel;
            MaxAllowedStockLines = maxStockLines;

            _stockLines = new Dictionary<string, StockLine>();
            var uniqueProductCount = stock.GroupBy(x => x.Product.ProductIdentifier).Count();

            if (uniqueProductCount > MaxAllowedStockLines)
            {
                throw new ArgumentException(string.Format("Stock contains too many product lines ({0}), MaxStockLines is {1}.", uniqueProductCount, MaxStockLines), "stock");
            }

            foreach (StockLine stockItems in stock)
            {
                AddInventory(stockItems);
            }
        }

        public int MaxStockLevel
        {
            get { return MaxAllowedStock; }
        }

        public int CurrentStockLevel
        {
            get { return _currentStockLevel; }
        }

        public int MaxStockLines
        {
            get { return MaxAllowedStockLines; }
        }

        public bool RemoveInventory(string productIdentifier, int count)
        {
            bool result = false;
            if (string.IsNullOrWhiteSpace(productIdentifier))
            {
                throw new ArgumentException("ProductIdentifier cannot be null or empty.", "productIdentifier");
            }

            if (count <= 0)
            {
                throw new ArgumentException("Count must be greater than zero.", "count");
            }

            bool lockTaken = false;

            try
            {
                Monitor.TryEnter(_sync, 3000, ref lockTaken);

                if (!lockTaken)
                {
                    throw new Exception("Unable to acquire lock on Synchronization object.");
                }

                if (!_stockLines.ContainsKey(productIdentifier))
                {
                    throw new ApplicationException(string.Format("Product with ProductIdentifier {0} does not exist in stockLines.", productIdentifier));
                }

                if ((_stockLines[productIdentifier].Stock - count) < 0)
                {
                    throw new Exception(string.Format("Cannot Remove {0} of Product with ProductIdentifier {1}, insufficient stock.", count, productIdentifier));
                }

                _stockLines[productIdentifier].Stock -= count;
                _currentStockLevel -= count;
                result = true;
            }
            finally
            {
                Monitor.Exit(_sync);
            }

            return result;
        }

        public bool AddInventory(StockLine inventory)
        {
            if (inventory == null)
            {
                throw new ArgumentException("Inventory cannot be null.", "inventory");
            }

            if (inventory.Product == null)
            {
                throw new ArgumentException("Product cannot be null.", "inventory");
            }

            if (string.IsNullOrWhiteSpace(inventory.Product.ProductIdentifier))
            {
                throw new ArgumentException("ProductIdentifier cannot be null or empty.", "inventory");
            }

            if (inventory.Stock <= 0)
            {
                throw new ArgumentException("Stock must be greater than zero.", "inventory");
            }

            bool result = false;
            bool lockTaken = false;

            try
            {
                Monitor.TryEnter(_sync, 3000, ref lockTaken);

                if (!lockTaken)
                {
                    throw new Exception("Unable to acquire lock on Synchronization object.");
                }

                if (!_stockLines.ContainsKey(inventory.Product.ProductIdentifier))
                {
                    _stockLines.Add(inventory.Product.ProductIdentifier, new StockLine(inventory.Product));
                }

                StockLine stockEntry = _stockLines[inventory.Product.ProductIdentifier];

                if((_currentStockLevel + inventory.Stock) > MaxStockLevel)
                {
                    throw new ApplicationException(string.Format("Cannot add ({0}) Products with ProductIdentfier{1}, this exceeds MaxStockLevel of {2}.", inventory.Stock, inventory.Product.ProductIdentifier, MaxStockLevel));
                }

                _stockLines[inventory.Product.ProductIdentifier].Stock += inventory.Stock;

                _currentStockLevel += inventory.Stock;

                result = true;
            }
            finally
            {
                Monitor.Exit(_sync);
            }

            return result;
        }

        public Dictionary<string, StockLine> CurrentStockLines
        {
            get
            {
                Dictionary<string, StockLine> result = null;
                bool lockTaken = false;

                try
                {
                    Monitor.TryEnter(_sync, 3000, ref lockTaken);

                    if (!lockTaken)
                    {
                        throw new Exception("Unable to acquire lock on Synchronization object.");
                    }

                    result = new Dictionary<string, StockLine>(_stockLines);
                }
                finally
                {
                    Monitor.Exit(_sync);
                }

                return result;
            }
        }

        public int GetProductStockLevel(string productIdentifier)
        {
            if (string.IsNullOrWhiteSpace(productIdentifier))
            {
                throw new ArgumentException("ProductIdentifier cannot be null or empty.", "productIdentifier");
            }

            int result = 0;
            Dictionary<string, StockLine> currentStockLines = CurrentStockLines;

            if (currentStockLines.ContainsKey(productIdentifier))
            {
                result = currentStockLines[productIdentifier].Stock;
            }
            else
            {
                throw new ArgumentException(string.Format("Product with ProductIdentifier {0} does not exist in stockLines.", productIdentifier), "productIdentifier");
            }

            return result;
        }
    }
}
