using System;

namespace SimpleVendingMachine
{
    public class Product
    {
        public Product(string productIdentifier)
        {
            if(string.IsNullOrWhiteSpace(productIdentifier))
            {
                throw new ArgumentException("ProductIdentifier cannnot be null.", "productIdentifier");
            }

            ProductIdentifier = productIdentifier;
        }

        public string ProductIdentifier
        {
            get;
            private set;
        }

        private string _productName;
        public string ProductName
        {
            get { return _productName; }
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Supplied value cannot be null or empty.", "value");
                }

                _productName = value;
            }
        }

        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set
            {
                if (value < 0.0M)
                {
                    throw new ArgumentException("Supplied value cannot be less than zero.", "value");
                }

                _price = value;
            }
        }
    }
}
