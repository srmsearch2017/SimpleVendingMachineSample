using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleVendingMachine.UnitTests
{
    [TestClass]
    public class VendingCardTests
    {
        [TestMethod]
        public void CanCreateVendingCard()
        {
            //Arrange
            VendingCard card = null;
            AccountManager accountManager = null;
            Account account = null;

            account = new Account("1");
            accountManager = new AccountManager(new List<Account> { account });
            
            //Act
            card = new VendingCard("1", accountManager);

            //Assert
            Assert.IsNotNull(card, "VendingCard object is null.");
        }

        [TestMethod]
        public void CannotCreateAVendingCardWithInvalidAccountIdentifier()
        {
            //Arrange
            VendingCard card = null;
            AccountManager accountManager = null;
            Account account = null;

            account = new Account("1");
            accountManager = new AccountManager(new List<Account> { account });

            //Act            
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                card = new VendingCard("", accountManager);
            },
            "AccountIdentifier cannot be null or empty.",
            "accountIdentifier");

            //Assert
            //...
        }

        [TestMethod]       
        public void CannotCreateAVendingCardWithNullAccountIdentifier()
        {
            //Arrange
            VendingCard card = null;
            AccountManager accountManager = null;
            Account account = null;

            account = new Account("1");
            accountManager = new AccountManager(new List<Account> { account });

            //Act            
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                card = new VendingCard(null, accountManager);
            },
            "AccountIdentifier cannot be null or empty.",
            "accountIdentifier");


            //Assert
            //...
        }

        [TestMethod]
        public void CannotCreateAVendingCardWithNullAcountManager()
        {
            //Arrange
            VendingCard card = null;

            //Act            
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                card = new VendingCard("1", null);
            },
            "AccountManager cannot be null.",
            "accountManager");

            //Assert
            //...
        }

        [TestMethod]
        public void CannotSetInvalidPin()
        {
            //Arrange
            VendingCard card = null;
            AccountManager accountManager = null;
            Account account = null;

            account = new Account("1");
            accountManager = new AccountManager(new List<Account> { account });

            card = new VendingCard("1", accountManager);

            //Act            
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                card.SetPin("");
            },
            "Pin cannot be null or empty.",
            "pin");

            //Assert
            //...
        }

        [TestMethod]
        public void CannotSetNullPin()
        {
            //Arrange
            VendingCard card = null;
            AccountManager accountManager = null;
            Account account = null;

            account = new Account("1");
            accountManager = new AccountManager(new List<Account> { account });

            card = new VendingCard("1", accountManager);
            
            //Act            
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                card.SetPin(null);
            },
            "Pin cannot be null or empty.",
            "pin");

            //Assert
            //...
        }

        [TestMethod]
        public void CanSetPin()
        {
            //Arrange
            const string pin = "1234";
            VendingCard card = null;
            AccountManager accountManager = null;
            Account account = null;

            account = new Account("1");
            accountManager = new AccountManager(new List<Account> { account });

            card = new VendingCard("1", accountManager);
            
            //Act
            card.SetPin(pin);

            //Assert
            Assert.IsTrue(card.Pin.Equals( pin, StringComparison.InvariantCulture), "Card Pin {0} does not equal expected value {1}.", card.Pin, pin);
        }

        [TestMethod]
        public void CannotVerifyInvalidPin()
        {
            //Arrange
            VendingCard card = null;
            AccountManager accountManager = null;
            Account account = null;

            account = new Account("1");
            accountManager = new AccountManager(new List<Account> { account });

            card = new VendingCard("1", accountManager);
            
            //Act            
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                card.VerifyPin("");
            },
            "Pin cannot be null or empty.",
            "pin");

            //Assert
            //...
        }

        [TestMethod]
        public void CannotVerifyNullPin()
        {
            //Arrange
            VendingCard card = null;
            AccountManager accountManager = null;
            Account account = null;

            account = new Account("1");
            accountManager = new AccountManager(new List<Account> { account });

            card = new VendingCard("1", accountManager);


            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                card.VerifyPin(null);
            },
            "Pin cannot be null or empty.",
            "pin");

            //Assert
            //...
        }

        [TestMethod]
        public void CanVerifyPin()
        {
            //Arrange
            const string pin = "1234";
            VendingCard card = null;
            AccountManager accountManager = null;
            Account account = null;

            account = new Account("1");
            accountManager = new AccountManager(new List<Account> { account });

            card = new VendingCard("1", accountManager);

            card.SetPin(pin);
            
            //Act
            bool isValid = card.VerifyPin(pin);

            //Assert
            Assert.IsTrue(isValid, "Card Pin Authentication Failed with valid Pin.", pin);
        }

        [TestMethod]
        public void CannotVerifyIncorrectPin()
        {
            //Arrange
            const string pin = "1234";
            const string pin2 = "1235";
            VendingCard card = null;
            AccountManager accountManager = null;
            Account account = null;

            account = new Account("1");
            accountManager = new AccountManager(new List<Account> { account });

            card = new VendingCard("1", accountManager);

            card.SetPin(pin);

            //Act
            bool isValid = card.VerifyPin(pin2);

            //Assert
            Assert.IsFalse(isValid, "Card Pin Authentication Failed with invalid Pin.");
        }
    }
}
