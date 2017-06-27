using System;

namespace SimpleVendingMachine
{
    public class StockLine
    {
        public StockLine(Product product)
        {
            if(product == null)
            {
                throw new ArgumentException("Product cannot be null", "product");
            }

            Product = product;
        }

        public Product Product
        {
            get;
            private set;
        }

        private int _stock;
        public int Stock
        {
            get { return _stock; }
            set
            {
                if(value < 0)
                {
                    throw new ArgumentException("Supplied value cannot be less than zero.", "value");
                }
                _stock = value;
            }
        }
    }
}
