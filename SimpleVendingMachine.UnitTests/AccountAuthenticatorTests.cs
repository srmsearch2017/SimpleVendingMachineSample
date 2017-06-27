using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace SimpleVendingMachine.UnitTests
{
    [TestClass]
    public class AccountAuthenticatorTests
    {
        [TestMethod]
        public void CanCreateAccountAuthenticator()
        {
            //Arrange
            Account account = null;
            AccountManager accountManager = null;

            account = new Account("1");

            //Act
            accountManager = new AccountManager(new List<Account> { account });

            //Assert
            Assert.IsNotNull(accountManager, "Could not create AccountAuthenticator");
        }

        [TestMethod]
        public void CannotCreateAnAccountAuthenticatorWithNullAccounts()
        {
            // Arrange
            AccountManager accountManager = null;

            // Act

            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                accountManager = new AccountManager(null);
            }, 
            "Accounts cannot be null.", 
            "accounts");

            //Assert
            //...
        }


        [TestMethod]
        public void CannotCreateAnInvalidAccountAuthenticator()
        {
            //Arrange
            AccountManager accountManager = null;
            Account account = null;

            account = new Account("1");

            PropertyInfo pi = account.GetType().GetProperty("AccountIdentifier", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            pi.SetValue(account, "");

            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                accountManager = new AccountManager(new List<Account> { account });
            },
            "Accounts cannot contain an Account with a null or empty AccountIdentifier.",
            "accounts");

            //Assert
            //...
        }

        [TestMethod]
        public void CanAuthenticateSingleAccount()
        {
            //Arrange
            const string AccountIdentifier = "12345";

            Account account = null;
            AccountManager accountManager = null;

            account = new Account(AccountIdentifier);

            accountManager = new AccountManager(new List<Account> { account });

            //Act
            bool isValid = accountManager.Authenticate(AccountIdentifier);

            //Assert
            Assert.IsTrue(isValid, "Supplied Account {0} is not valid for Account Authenticator Containing AccountIdentifier {1}.", AccountIdentifier, account.AccountIdentifier);
        }

        [TestMethod]
        public void CanNotAuthenticateIncorrectAccount()
        {
            //Arrange
            const string CorrectAccountIdentifier = "12345";
            const string IncorrectAccountIdentifier = "12346";

            Account account = null;
            AccountManager accountManager = null;

            account = new Account(CorrectAccountIdentifier);
            accountManager = new AccountManager(new List<Account> { account });

            //Act
            bool isValid = accountManager.Authenticate(IncorrectAccountIdentifier);

            //Assert
            Assert.IsFalse(isValid, "Bad Account Identifier {0} returned result {1}.", IncorrectAccountIdentifier, isValid);
        }

        [TestMethod]
        public void CanAuthenticateMultipleAccounts()
        {
            //Arrange
            const string AccountIdentifierOne = "12345";
            const string AccountIdentifierTwo = "12346";

            Account accountOne = null;
            Account accountTwo = null;
            AccountManager accountManager = null;

            accountOne = new Account(AccountIdentifierOne);
            accountTwo = new Account(AccountIdentifierTwo);

            accountManager = new AccountManager(new List<Account> { accountOne, accountTwo });

            //Act
            bool isAccountOneValid = accountManager.Authenticate(AccountIdentifierOne);

            //Assert
            Assert.IsTrue(isAccountOneValid, "Supplied Account {0} is not valid for Account Authenticator Containing AccountIdentifier {1}.", AccountIdentifierOne, accountOne.AccountIdentifier);

            //Act
            bool isAccountTwoValid = accountManager.Authenticate(AccountIdentifierTwo);

            //Assert
            Assert.IsTrue(isAccountTwoValid, "Supplied Account {0} is not valid for Account Authenticator Containing AccountIdentifier {1}.", AccountIdentifierTwo, accountTwo.AccountIdentifier);
        }
    }
}
