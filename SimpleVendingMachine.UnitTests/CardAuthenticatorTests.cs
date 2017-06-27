using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleVendingMachine.UnitTests
{
    [TestClass]
    public class CardAuthenticatorTests
    {
        [TestMethod]
        public void CanCreateCardAuthenticator()
        {
            //Arrange
            const string AccounIdentifier = "12345";
            const string PinNo = "12345";

            Account account = null;
            AccountManager accountManager = null;
            VendingCard card = null;
            CardAuthenticator cardAuthenticator = null;

            account = new Account("1");
            accountManager = new AccountManager(new List<Account> { account });
            cardAuthenticator = new CardAuthenticator();

            card = new VendingCard(AccounIdentifier, accountManager);
            card.SetPin(PinNo);

            //Act
            bool isValid = cardAuthenticator.Authenticate(card, PinNo);

            //Assert
            Assert.IsTrue(isValid, "Supplied Pin {0} is not valid for Card with Pin {1}.", PinNo, card.Pin);
        }

        [TestMethod]
        public void CanAuthenticateSingleCard()
        {
            //Arrange
            const string AccounIdentifier = "12345";
            const string PinNo = "12345";

            Account account = null;
            AccountManager accountManager = null;
            VendingCard card = null;
            CardAuthenticator cardAuthenticator = null;

            account = new Account("1");
            accountManager = new AccountManager(new List<Account> { account });

            card = new VendingCard(AccounIdentifier, accountManager);
            card.SetPin(PinNo);

            cardAuthenticator = new CardAuthenticator();

            //Act
            bool isValid = cardAuthenticator.Authenticate(card, PinNo);

            //Assert
            Assert.IsTrue(isValid, "Supplied Pin {0} is not valid for Card with Pin {1}.", PinNo, card.Pin);
        }

        [TestMethod]
        public void CannotAuthenticateNullCard()
        {
            //Arrange
            const string PinNo = "12345";

            CardAuthenticator cardAuthenticator = new CardAuthenticator();

            //Act            
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                cardAuthenticator.Authenticate(null, PinNo);
            },
            "Card cannot be null.",
            "card");

            //Assert
            //...
        }

        [TestMethod]
        public void CannotAuthenticateNullPin()
        {
            //Arrange
            const string AccounIdentifier = "12345";

            Account account = null;
            AccountManager accountManager = null;
            VendingCard card = null;
            CardAuthenticator cardAuthenticator = null;

            account = new Account("1");
            accountManager = new AccountManager(new List<Account> { account });
            card = new VendingCard(AccounIdentifier, accountManager);

            cardAuthenticator = new CardAuthenticator();

            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                cardAuthenticator.Authenticate(card, null);
            },
            "Pin cannot be null.",
            "pin");


            //Assert            
            //...
        }
    }
}
