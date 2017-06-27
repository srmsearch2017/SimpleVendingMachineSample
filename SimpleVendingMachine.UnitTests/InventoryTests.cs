using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleVendingMachine.UnitTests
{
    [TestClass]
    public class InventoryTests
    {
        [TestMethod]
        public void CanCreateInventory()
        {
            //Arrange
            Inventory inventory = null;
            const int MaxStockLines = 1;
            const int MaxStockLevel = 1;

            List<StockLine> stockLines = new List<StockLine>();

            //Act
            inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);

            //Assert
            Assert.IsNotNull(inventory, "Inventory object is null.");
        }

        [TestMethod]
        public void CanCreateInventoryAndSetMaxStockLines()
        {
            //Arrange
            Inventory inventory = null;
            const int MaxStockLines = 1;
            const int MaxStockLevel = 2;

            List<StockLine> stockLines = new List<StockLine>();

            //Act
            inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);

            //Assert
            Assert.IsTrue(inventory.MaxStockLines == MaxStockLines, string.Format("MaxStockLines ({0}) is not equal to expected value {1}.", inventory.MaxStockLines, MaxStockLines));
        }

        [TestMethod]
        public void CanCreateInventoryAndSetMaxStockLevel()
        {
            //Arrange
            Inventory inventory = null;
            const int MaxStockLines = 1;
            const int MaxStockLevel = 2;

            List<StockLine> stockLines = new List<StockLine>();

            //Act
            inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);

            //Assert
            Assert.IsTrue(inventory.MaxStockLevel == MaxStockLevel, string.Format("MaxStockLevel ({0}) is not equal to expected value {1}.", inventory.MaxStockLevel, MaxStockLevel));
        }

        [TestMethod]
        public void CannotCreateInventoryWithMaxStockLevelLessThanZero()
        {
            //Arrange
            Inventory inventory = null;
            const int MaxStockLines = 1;
            const int MaxStockLevel = -1;

            List<StockLine> stockLines = new List<StockLine>();

            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);
            },
            "MaxStockLevel cannot be less than zero.",
            "maxStockLevel");

            //Assert
            //...

        }

        [TestMethod]
        public void CannotCreateInventoryWithMaxStockLevelOfZero()
        {
            //Arrange
            Inventory inventory = null;
            const int MaxStockLines = 1;
            const int MaxStockLevel = 0;

            List<StockLine> stockLines = new List<StockLine>();

            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);
            },
            "MaxStockLevel cannot be less than zero.",
            "maxStockLevel");

            //Assert
            //...
        }

        [TestMethod]
        public void CannotCreateInventoryWithMaxStockLinesLessThanZero()
        {
            //Arrange
            Inventory inventory = null;

            const int MaxStockLines = -1;
            const int MaxStockLevel = 1;

            List<StockLine> stockLines = new List<StockLine>();

            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);
            },
            "MaxStockLines cannot be less than zero.",
            "maxStockLines");

            //Assert
            //...

        }

        [TestMethod]
        public void CannotCreateInventoryWithMaxStockLinesOfZero()
        {
            //Arrange
            Inventory inventory = null;
            const int MaxStockLines = 0;
            const int MaxStockLevel = 1;

            List<StockLine> stockLines = new List<StockLine>();

            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);
            },
            "MaxStockLines cannot be less than zero.",
            "maxStockLines");

            //Assert
            //...
        }

        [TestMethod]
        public void CannotCreateInventoryTooManyProductLines()
        {
            //Arrange
            Inventory inventory = null;
            Product productOne = null;
            Product productTwo = null;
            StockLine stockLineOne = null;
            StockLine stockLineTwo = null;

            const int MaxStockLines = 1;
            const int MaxStockLevel = 4;

            productOne = new Product("1") { ProductName = "Product1", Price = 1.0M };
            productTwo = new Product("2") { ProductName = "Product2", Price = 2.0M };
            stockLineOne = new StockLine(productOne) { Stock = 1 };
            stockLineTwo = new StockLine(productTwo) { Stock = 2 };

            List<StockLine> stockLines = new List<StockLine> { stockLineOne, stockLineTwo };

            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);
            },
            string.Format("Stock contains too many product lines ({0}), MaxStockLines is {1}.", stockLines.Count, MaxStockLines),
            "stock");

            //Assert
            //...
        }

        [TestMethod]
        public void CannotCreateInventoryWithNullStock()
        {
            //Arrange
            Inventory inventory = null;
            const int MaxStockLines = 1;
            const int MaxStockLevel = 1;

            List<StockLine> stockLines = null;

            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);
            },
            "Stock cannot be null.",
            "stock");

            //Assert
            //...
        }

        [TestMethod]
        public void CanCreateInventoryAndAddInitialStock()
        {
            //Arrange
            Inventory inventory = null;
            Product productOne = null;
            StockLine stockLineOne = null;

            const string productIdentfier = "1";
            const int MaxStockLines = 1;
            const int MaxStockLevel = 4;

            productOne = new Product(productIdentfier) { ProductName = "Product1", Price = 1.0M };
            stockLineOne = new StockLine(productOne) { Stock = MaxStockLevel };

            List<StockLine> stockLines = new List<StockLine> { stockLineOne };

            inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);

            //Act
            int result = inventory.GetProductStockLevel(productIdentfier);

            //Assert
            Assert.IsTrue(result == MaxStockLevel, string.Format("Inventory for product {0} was {1} not expected value {2}.", productIdentfier, result, MaxStockLevel));
        }

        [TestMethod]
        public void CanGetProductStockLevelWithValidProductIdentifier()
        {
            //Arrange
            Inventory inventory = null;
            Product productOne = null;
            StockLine stockLineOne = null;

            const string productIdentfier = "1";
            const int MaxStockLines = 1;
            const int MaxStockLevel = 2;

            productOne = new Product(productIdentfier) { ProductName = "Product1", Price = 1.0M };
            stockLineOne = new StockLine(productOne) { Stock = MaxStockLevel };
            List<StockLine> stockLines = new List<StockLine> { stockLineOne };

            inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);

            //Act
            int result = inventory.GetProductStockLevel(productIdentfier);

            //Assert
            Assert.IsTrue(result == MaxStockLevel, string.Format("Inventory for product {0} was {1} not expected value {2}.", productIdentfier, result, MaxStockLevel));

        }

        [TestMethod]
        public void CannotGetProductStockLevelWithInvalidProductIdentifier()
        {
            //Arrange
            Inventory inventory = null;
            Product productOne = null;
            StockLine stockLineOne = null;

            const string productIdentfier = "1";
            const string invalidProductIdentfier = "2";
            const int MaxStockLines = 1;
            const int MaxStockLevel = 2;

            productOne = new Product(productIdentfier) { ProductName = "Product1", Price = 1.0M };
            stockLineOne = new StockLine(productOne) { Stock = MaxStockLevel };

            List<StockLine> stockLines = new List<StockLine> { stockLineOne };
            inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);

            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                int result = inventory.GetProductStockLevel(invalidProductIdentfier);
            },
            string.Format("Product with ProductIdentifier {0} does not exist in stockLines.", invalidProductIdentfier),
            "productIdentifier");

            //Assert
            //...

        }

        [TestMethod]
        public void CannotGetProductStockLevelWithValidProductIdentifierWithEmptyInventory()
        {
            //Arrange
            Inventory inventory = null;

            const string productIdentfier = "1";
            const int MaxStockLines = 1;
            const int MaxStockLevel = 2;

            List<StockLine> stockLines = new List<StockLine>();
            inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);

            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                inventory.GetProductStockLevel(productIdentfier); ;
            },
            string.Format("Product with ProductIdentifier {0} does not exist in stockLines.", productIdentfier),
            "productIdentifier");

            //Assert
            //...

        }

        [TestMethod]
        public void CannotGetProductStockLevelWitNullProductIdentifier()
        {
            //Arrange
            Inventory inventory = null;

            const int MaxStockLines = 1;
            const int MaxStockLevel = 2;

            List<StockLine> stockLines = new List<StockLine>();
            inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);

            const string productIdentifier = null;

            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                inventory.GetProductStockLevel(productIdentifier); ;
            },
            "ProductIdentifier cannot be null or empty.",
            "productIdentifier");

            //Assert
            //...
        }

        [TestMethod]
        public void CannotGetProductStockLevelWitEmptyProductIdentifier()
        {
            Inventory inventory = null;

            const int MaxStockLines = 1;
            const int MaxStockLevel = 2;

            List<StockLine> stockLines = new List<StockLine>();
            inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);

            const string productIdentifier = "";

            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                inventory.GetProductStockLevel(productIdentifier);
            },
            "ProductIdentifier cannot be null or empty.",
            "productIdentifier");
        }

        [TestMethod]
        public void CanGetProductStockLinesForPopulatedInventory()
        {
            //Arrange
            Inventory inventory = null;
            Product productOne = null;
            StockLine stockLineOne = null;
            Product productTwo = null;
            StockLine stockLineTwo = null;

            const string productIdentfierOne = "1";
            const int productOneStockCount = 1;
            const string productIdentfierTwo = "2";
            const int productTwoStockCount = 1;

            const int ExpectedStockCount = 2;
            const int MaxStockLines = 2;
            const int MaxStockLevel = 2;

            productOne = new Product(productIdentfierOne) { ProductName = "Product1", Price = 1.0M };
            stockLineOne = new StockLine(productOne) { Stock = productOneStockCount };

            productTwo = new Product(productIdentfierTwo) { ProductName = "Product2", Price = 2.0M };
            stockLineTwo = new StockLine(productTwo) { Stock = productTwoStockCount };

            List<StockLine> stockLines = new List<StockLine> { stockLineOne, stockLineTwo };
            inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);

            //Act
            Dictionary<string, StockLine> results = inventory.CurrentStockLines;

            //Assert
            Assert.IsNotNull(results, "Inventory returned null CurrentStockLines when stockLines were supplied.");

            Assert.IsTrue(results.Count == ExpectedStockCount, string.Format("Inventory returned incorrect CurrentStockLines, got {0} entries in results, expected {1}.", results.Count, ExpectedStockCount));

            List<string> keys = new List<string>(results.Keys);

            Assert.IsTrue((results.ContainsKey(productIdentfierOne) && (keys[0] == productIdentfierOne)), "Key at Index 0 is {0}, expected {1}.", keys[0], productIdentfierOne);
            Assert.IsNotNull(results[productIdentfierOne].Product, "Product at Index 0 is null.");
            Assert.IsNotNull(results[productIdentfierOne].Product.ProductIdentifier, "Product at Index 0 has a null ProductIdentifier.");
            Assert.IsTrue(results[productIdentfierOne].Product.ProductIdentifier.Equals(productIdentfierOne, StringComparison.InvariantCulture), string.Format("Incorrect Product with ProductIdentifier {0} at Index 0, expected {1}.", results[productIdentfierOne].Product.ProductIdentifier, productIdentfierOne));
            Assert.IsTrue(results[productIdentfierOne].Stock == productOneStockCount, string.Format("Inventory at Index 0 has Stock Count {0}, expected {1}.", results[productIdentfierOne].Stock, productOneStockCount));

            Assert.IsTrue((results.ContainsKey(productIdentfierTwo) && (keys[1] == productIdentfierTwo)), "Key at Index 1 is {0}, expected {1}.", keys[1], productIdentfierTwo);
            Assert.IsNotNull(results[productIdentfierTwo].Product, "Product at Index 1 is null.");
            Assert.IsNotNull(results[productIdentfierTwo].Product.ProductIdentifier, "Product at Index 1 has a null ProductIdentifier.");
            Assert.IsTrue(results[productIdentfierTwo].Product.ProductIdentifier.Equals(productIdentfierTwo, StringComparison.InvariantCulture), string.Format("Incorrect Product with ProductIdentifier {0} at Index 1, expected {1}.", results[productIdentfierTwo].Product.ProductIdentifier, productIdentfierTwo));
            Assert.IsTrue(results[productIdentfierTwo].Stock == productTwoStockCount, string.Format("Inventory at Index 0 has Stock Count {0}, expected {1}.", results[productIdentfierTwo].Stock, productTwoStockCount));

        }

        [TestMethod]
        public void CanGetProductStockLinesForUnpopulatedInventory()
        {
            //Arrange
            Inventory inventory = null;

            const int MaxStockLines = 1;
            const int MaxStockLevel = 2;
            const int ExpectedStockCount = 0;

            List<StockLine> stockLines = new List<StockLine>();
            inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);

            //Act
            Dictionary<string, StockLine> results = inventory.CurrentStockLines;

            //Assert
            Assert.IsNotNull(results, "Inventory returned null CurrentStockLines when stockLines were supplied.");

            Assert.IsTrue(results.Count == ExpectedStockCount, string.Format("Inventory has CurrentStockLines Count of {0}, expected {1}.", results.Count, ExpectedStockCount));
        }

        [TestMethod]
        public void CanGetCurrentStockLevelForPopulatedInventory()
        {
            //Arrange
            Inventory inventory = null;
            Product productOne = null;
            StockLine stockLineOne = null;
            Product productTwo = null;
            StockLine stockLineTwo = null;

            const string productIdentfierOne = "1";
            const int productOneStockCount = 1;
            const string productIdentfierTwo = "2";
            const int productTwoStockCount = 1;

            const int ExpectedStockLevel = 2;
            const int MaxStockLines = 2;
            const int MaxStockLevel = 2;

            productOne = new Product(productIdentfierOne) { ProductName = "Product1", Price = 1.0M };
            stockLineOne = new StockLine(productOne) { Stock = productOneStockCount };

            productTwo = new Product(productIdentfierTwo) { ProductName = "Product2", Price = 2.0M };
            stockLineTwo = new StockLine(productTwo) { Stock = productTwoStockCount };

            List<StockLine> stockLines = new List<StockLine> { stockLineOne, stockLineTwo };
            inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);

            //Act
            int result = inventory.CurrentStockLevel;

            //Assert
            Assert.IsTrue(result == ExpectedStockLevel, string.Format("Inventory has CurrentStockLevel {0}, expected {1}.", result, ExpectedStockLevel));

        }


        [TestMethod]
        public void CanRemoveInventoryFromPopulatedInventory()
        {
            //Arrange
            Inventory inventory = null;
            Product productOne = null;
            StockLine stockLineOne = null;
            Product productTwo = null;
            StockLine stockLineTwo = null;

            const string productIdentfierOne = "1";
            const int productOneStockCount = 1;
            const int expectedProductOneStockCount = 0;
            const string productIdentfierTwo = "2";
            const int productTwoStockCount = 1;
            const int expectedProductTwoStockCount = 1;

            const int ExpectedStockLinesCount = 2;
            const int MaxStockLines = 2;
            const int MaxStockLevel = 2;

            productOne = new Product(productIdentfierOne) { ProductName = "Product1", Price = 1.0M };
            stockLineOne = new StockLine(productOne) { Stock = productOneStockCount };

            productTwo = new Product(productIdentfierTwo) { ProductName = "Product2", Price = 2.0M };
            stockLineTwo = new StockLine(productTwo) { Stock = productTwoStockCount };

            List<StockLine> stockLines = new List<StockLine> { stockLineOne, stockLineTwo };
            inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);

            //Act
            inventory.RemoveInventory(productIdentfierOne, productOneStockCount);
            Dictionary<string, StockLine> results = inventory.CurrentStockLines;

            //Assert
            Assert.IsNotNull(results, "Inventory returned null CurrentStockLines when stockLines should exist.");

            Assert.IsTrue(results.Count == ExpectedStockLinesCount, string.Format("Inventory returned incorrect CurrentStockLines, got {0} entries in results, expected {1}.", results.Count, ExpectedStockLinesCount));

            List<string> keys = new List<string>(results.Keys);

            Assert.IsTrue((results.ContainsKey(productIdentfierOne) && (keys[0] == productIdentfierOne)), "Key at Index 0 is {0}, expected {1}.", keys[0], productIdentfierOne);
            Assert.IsNotNull(results[productIdentfierOne].Product, "Product at Index 0 is null.");
            Assert.IsNotNull(results[productIdentfierOne].Product.ProductIdentifier, "Product at Index 0 has a null ProductIdentifier.");
            Assert.IsTrue(results[productIdentfierOne].Product.ProductIdentifier.Equals(productIdentfierOne, StringComparison.InvariantCulture), string.Format("Incorrect Product with ProductIdentifier {0} at Index 0, expected {1}.", results[productIdentfierOne].Product.ProductIdentifier, productIdentfierOne));
            Assert.IsTrue(results[productIdentfierOne].Stock == expectedProductOneStockCount, string.Format("Inventory at Index 0 has Stock Count {0}, expected {1}.", results[productIdentfierOne].Stock, expectedProductOneStockCount));

            Assert.IsTrue((results.ContainsKey(productIdentfierTwo) && (keys[1] == productIdentfierTwo)), "Key at Index 1 is {0}, expected {1}.", keys[1], productIdentfierTwo);
            Assert.IsNotNull(results[productIdentfierTwo].Product, "Product at Index 1 is null.");
            Assert.IsNotNull(results[productIdentfierTwo].Product.ProductIdentifier, "Product at Index 1 has a null ProductIdentifier.");
            Assert.IsTrue(results[productIdentfierTwo].Product.ProductIdentifier.Equals(productIdentfierTwo, StringComparison.InvariantCulture), string.Format("Incorrect Product with ProductIdentifier {0} at Index 1, expected {1}.", results[productIdentfierTwo].Product.ProductIdentifier, productIdentfierTwo));
            Assert.IsTrue(results[productIdentfierTwo].Stock == expectedProductTwoStockCount, string.Format("Inventory at Index 0 has Stock Count {0}, expected {1}.", results[productIdentfierTwo].Stock, expectedProductTwoStockCount));

        }

        [TestMethod]
        public void CannotRemoveInventoryWithNullProductIdentifier()
        {
            //Arrange
            Inventory inventory = null;

            const int MaxStockLines = 2;
            const int MaxStockLevel = 2;

            const string productIdentifier = null;

            List<StockLine> stockLines = new List<StockLine>();
            inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);

            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                inventory.RemoveInventory(productIdentifier, 1);
            },
            "ProductIdentifier cannot be null or empty.",
            "productIdentifier");
        }


        [TestMethod]
        public void CannotRemoveInventoryWithNegativeCount()
        {
            //Arrange
            Inventory inventory = null;

            const int MaxStockLines = 2;
            const int MaxStockLevel = 2;
            const int count = -1;
            const string productIdentifier = "1";

            List<StockLine> stockLines = new List<StockLine>();
            inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);

            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                inventory.RemoveInventory(productIdentifier, count);
            },
            "Count must be greater than zero.",
            "count");
        }
        
        [TestMethod]
        public void CannotRemoveInventoryWithInvalidProductIdentifier()
        {
            //Arrange
            Inventory inventory = null;

            const int MaxStockLines = 2;
            const int MaxStockLevel = 2;

            const string productIdentifier = "";

            List<StockLine> stockLines = new List<StockLine>();
            inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);

            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                inventory.RemoveInventory(productIdentifier, 1);
            },
            "ProductIdentifier cannot be null or empty.",
            "productIdentifier");
        }

        [TestMethod]
        public void CannotRemoveInventoryWithIncorrectProductIdentifier()
        {
            //Arrange
            Inventory inventory = null;

            const int MaxStockLines = 2;
            const int MaxStockLevel = 2;
            const string productIdentifier = "1";

            List<StockLine> stockLines = new List<StockLine>();
            inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);

            //Act
            ExceptionAssert.Throws<ApplicationException>(() =>
            {
                inventory.RemoveInventory(productIdentifier, 1);
            },
            string.Format("Product with ProductIdentifier {0} does not exist in stockLines.", productIdentifier),
            "productIdentifier");
        }

        [TestMethod]
        public void CannotRemoveInventoryWithQuantityGreaterThanCurrentStockLevel()
        {
            //Arrange
            Inventory inventory = null;

            const int MaxStockLines = 1;
            const int MaxStockLevel = 2;
            const string productIdentifier = "1";
            const int count = 2;

            const string productIdentfierOne = "1";
            const int productOneStockCount = 1;
            
            Product productOne = null;
            StockLine stockLineOne = null;

            productOne = new Product(productIdentfierOne) { ProductName = "Product1", Price = 1.0M };
            stockLineOne = new StockLine(productOne) { Stock = productOneStockCount };

            List<StockLine> stockLines = new List<StockLine> { stockLineOne };
            inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);

            //Act
            ExceptionAssert.Throws<Exception>(() =>
            {
                inventory.RemoveInventory(productIdentifier, count);
            },
            string.Format("Cannot Remove {0} of Product with ProductIdentifier {1}, insufficient stock.", count, productIdentifier));
        }




        [TestMethod]
        public void CanAddInventoryItemsToPopulatedInventory()
        {

        }

        [TestMethod]
        public void CanAddInventoryItemsToEmptyInventory()
        {
            //Arrange
            Inventory inventory = null;
            Product productOne = null;
            StockLine stockLineOne = null;
            Product productTwo = null;
            StockLine stockLineTwo = null;

            const string productIdentfierOne = "1";
            const int productOneStockCount = 1;
            const int expectedProductOneStockCount = 0;
            const string productIdentfierTwo = "2";
            const int productTwoStockCount = 1;
            const int expectedProductTwoStockCount = 1;

            const int ExpectedStockLinesCount = 2;
            const int MaxStockLines = 2;
            const int MaxStockLevel = 2;

            productOne = new Product(productIdentfierOne) { ProductName = "Product1", Price = 1.0M };
            stockLineOne = new StockLine(productOne) { Stock = productOneStockCount };

            productTwo = new Product(productIdentfierTwo) { ProductName = "Product2", Price = 2.0M };
            stockLineTwo = new StockLine(productTwo) { Stock = productTwoStockCount };

            List<StockLine> stockLines = new List<StockLine> { stockLineOne, stockLineTwo };
            inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);

            //Act
            inventory.RemoveInventory(productIdentfierOne, productOneStockCount);
            Dictionary<string, StockLine> results = inventory.CurrentStockLines;

            //Assert
            Assert.IsNotNull(results, "Inventory returned null CurrentStockLines when stockLines should exist.");

            Assert.IsTrue(results.Count == ExpectedStockLinesCount, string.Format("Inventory returned incorrect CurrentStockLines, got {0} entries in results, expected {1}.", results.Count, ExpectedStockLinesCount));

            List<string> keys = new List<string>(results.Keys);

            Assert.IsTrue((results.ContainsKey(productIdentfierOne) && (keys[0] == productIdentfierOne)), "Key at Index 0 is {0}, expected {1}.", keys[0], productIdentfierOne);
            Assert.IsNotNull(results[productIdentfierOne].Product, "Product at Index 0 is null.");
            Assert.IsNotNull(results[productIdentfierOne].Product.ProductIdentifier, "Product at Index 0 has a null ProductIdentifier.");
            Assert.IsTrue(results[productIdentfierOne].Product.ProductIdentifier.Equals(productIdentfierOne, StringComparison.InvariantCulture), string.Format("Incorrect Product with ProductIdentifier {0} at Index 0, expected {1}.", results[productIdentfierOne].Product.ProductIdentifier, productIdentfierOne));
            Assert.IsTrue(results[productIdentfierOne].Stock == expectedProductOneStockCount, string.Format("Inventory at Index 0 has Stock Count {0}, expected {1}.", results[productIdentfierOne].Stock, expectedProductOneStockCount));

            Assert.IsTrue((results.ContainsKey(productIdentfierTwo) && (keys[1] == productIdentfierTwo)), "Key at Index 1 is {0}, expected {1}.", keys[1], productIdentfierTwo);
            Assert.IsNotNull(results[productIdentfierTwo].Product, "Product at Index 1 is null.");
            Assert.IsNotNull(results[productIdentfierTwo].Product.ProductIdentifier, "Product at Index 1 has a null ProductIdentifier.");
            Assert.IsTrue(results[productIdentfierTwo].Product.ProductIdentifier.Equals(productIdentfierTwo, StringComparison.InvariantCulture), string.Format("Incorrect Product with ProductIdentifier {0} at Index 1, expected {1}.", results[productIdentfierTwo].Product.ProductIdentifier, productIdentfierTwo));
            Assert.IsTrue(results[productIdentfierTwo].Stock == expectedProductTwoStockCount, string.Format("Inventory at Index 0 has Stock Count {0}, expected {1}.", results[productIdentfierTwo].Stock, expectedProductTwoStockCount));

        }

        [TestMethod]
        public void CannotAddInventoryWithNullInventory()
        {
            //Arrange
            Inventory inventory = null;

            const int MaxStockLines = 1;
            const int MaxStockLevel = 4;

            inventory = new Inventory(MaxStockLines, MaxStockLevel, new List<StockLine>());

            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                inventory.AddInventory(null);
            },
            "Inventory cannot be null.",
            "inventory");
        }


        [TestMethod]
        public void CannotAddInventoryWithNullProduct()
        {
            //Arrange
            Inventory inventory = null;
            Product productOne = null;
            StockLine stockLineOne = null;

            const int MaxStockLines = 1;
            const int MaxStockLevel = 4;

            productOne = new Product("1");

            stockLineOne = new StockLine(productOne) { Stock = 1 };

            PropertyInfo pi = stockLineOne.GetType().GetProperty("Product", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            pi.SetValue(stockLineOne, null);

            inventory = new Inventory(MaxStockLines, MaxStockLevel, new List<StockLine>());

            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                inventory.AddInventory(stockLineOne);
            },
            "Product cannot be null.",
            "inventory");
        }


        [TestMethod]
        public void CannotAddInventoryWithNullProductIdentifier()
        {
            //Arrange
            Inventory inventory = null;
            Product productOne = null;
            StockLine stockLineOne = null;

            const int MaxStockLines = 1;
            const int MaxStockLevel = 4;

            productOne = new Product("1") { ProductName = "Product1", Price = 1.0M };

            PropertyInfo pi = productOne.GetType().GetProperty("ProductIdentifier", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            pi.SetValue(productOne, null);

            stockLineOne = new StockLine(productOne) { Stock = 1 };

            inventory = new Inventory(MaxStockLines, MaxStockLevel, new List<StockLine>());

            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                inventory.AddInventory(stockLineOne);
            },
            "ProductIdentifier cannot be null or empty.",
            "inventory");
        }


        [TestMethod]
        public void CannotAddInventoryWithNegativeCount()
        {
            //Arrange
            Inventory inventory = null;
            Product productOne = null;
            StockLine stockLineOne = null;

            const int MaxStockLines = 1;
            const int MaxStockLevel = 4;

            productOne = new Product("1") { ProductName = "Product1", Price = 1.0M };

            stockLineOne = new StockLine(productOne) { Stock = 1 };

            FieldInfo fi = stockLineOne.GetType().GetField("_stock", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            fi.SetValue(stockLineOne, -1);

            inventory = new Inventory(MaxStockLines, MaxStockLevel, new List<StockLine>());
            
            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                inventory.AddInventory(stockLineOne);
            },
            "Stock must be greater than zero.",
            "inventory");
        }

        [TestMethod]
        public void CanAddInventoryToExistingStockLine()
        {
            //Arrange
            Inventory inventory = null;
            Product productOne = null;
            StockLine stockLineOne = null;
            StockLine stockLineToAdd = null;

            const string productIdentfierOne = "1";
            const int productOneStockCount = 1;

            const int expectedProductOneStockCount = 2;

            const int MaxStockLines = 2;
            const int MaxStockLevel = 2;

            productOne = new Product(productIdentfierOne) { ProductName = "Product1", Price = 1.0M };
            stockLineOne = new StockLine(productOne) { Stock = productOneStockCount };

            List<StockLine> stockLines = new List<StockLine> { stockLineOne };
            inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);

            stockLineToAdd = new StockLine(productOne) { Stock = productOneStockCount };

            //Act
            inventory.AddInventory(stockLineToAdd);
            Dictionary<string, StockLine> results = inventory.CurrentStockLines;

            int expectedStockCount = (productOneStockCount * 2);
            //Assert
            Assert.IsTrue(results.ContainsKey(productIdentfierOne), string.Format("Inventory for does not contain Product with ProductIdentifier {0}.", productIdentfierOne));
            Assert.IsTrue(results[productIdentfierOne].Stock == expectedProductOneStockCount, string.Format("Inventory for ProductIdentifier {0} is {1}, expected {2}.",productIdentfierOne, results[productIdentfierOne].Stock, expectedProductOneStockCount));
            Assert.IsTrue(inventory.CurrentStockLevel == expectedStockCount, string.Format("Inventory CurrentStockCount is {0}, expected {1}.", inventory.CurrentStockLevel, expectedStockCount));
        }

        [TestMethod]
        public void CannotAddInventoryWithQuantityGreaterThanMaxStockLevel()
        {
            //Arrange
            Inventory inventory = null;
            Product productOne = null;
            StockLine stockLineOne = null;
            StockLine stockLineToAdd = null;

            const string productIdentfierOne = "1";
            const int productOneStockCount = 1;
            const int productOneStockCountToAdd = 2;

            const int MaxStockLines = 1;
            const int MaxStockLevel = 2;

            productOne = new Product(productIdentfierOne) { ProductName = "Product1", Price = 1.0M };
            stockLineOne = new StockLine(productOne) { Stock = productOneStockCount };

            List<StockLine> stockLines = new List<StockLine> { stockLineOne };
            inventory = new Inventory(MaxStockLines, MaxStockLevel, stockLines);

            stockLineToAdd = new StockLine(productOne) { Stock = productOneStockCountToAdd };

            //Act
            ExceptionAssert.Throws<ApplicationException>(() =>
            {
                inventory.AddInventory(stockLineToAdd);
            }, 
            string.Format("Cannot add ({0}) Products with ProductIdentfier{1}, this exceeds MaxStockLevel of {2}.", stockLineToAdd.Stock, stockLineToAdd.Product.ProductIdentifier, MaxStockLevel));

            //Assert
            //...
        }
    }
}
