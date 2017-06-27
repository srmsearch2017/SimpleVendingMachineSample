using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleVendingMachine.UnitTests
{
    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public void CanCreateAccount()
        {
            //Arrange
            Account account = null;
            
            //Act
            account = new Account("1");

            //Assert
            Assert.IsNotNull(account, "Account object is null.");
        }

        [TestMethod]
        public void CannotCreateAnAccountWithInvalidAccountIdentifier()
        {
            //Arrange
            Account account = null;

            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                account = new Account("");
            },
            "AccountIdentifier cannnot be null or empty.",
            "accountIdentifier");

            //Assert
            //...
        }

        [TestMethod]
        public void CannotCreateAnAccountWithNullAccountIdentifier()
        {
            //Arrange
            Account account = null;

            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                account = new Account(null);
            },
            "AccountIdentifier cannnot be null or empty.",
            "accountIdentifier");

            //Assert
            //...
        }


        [TestMethod]
        public void CanAssignAccountIdentifier()
        {
            //Arrange
            const string AccountId = "12345";
            Account account = null;

            //Act
            account = new Account(AccountId);

            //Assert
            Assert.IsNotNull(account.AccountIdentifier, "AccountIdentifier is null.");
            Assert.IsTrue(account.AccountIdentifier.Equals(AccountId, StringComparison.InvariantCulture), "Account Identifier {0} does not equal supplied value {1}.", account.AccountIdentifier, AccountId);
        }

        [TestMethod]
        public void CanVerifyInitialBalance()
        {
            //Arrange
            Account account = null;
            decimal expectedValue = 0.0M;

            //Act
            account = new Account("1");

            //Assert
            Assert.IsTrue(account.Balance == expectedValue, "Account Balance {0} does not equal expected initial value {1}.", account.Balance, expectedValue);
        }

        [TestMethod]
        public void CanApplyCreditBalance()
        {
            //Arrange
            Account account = null;
            decimal creditValue = 1.0M;
            account = new Account("1");

            //Act
            account.CreditBalance(creditValue);

            //Assert
            Assert.IsTrue(account.Balance == creditValue, "Account Balance {0} does not equal expected value {1}.", account.Balance, creditValue);
        }

        [TestMethod]
        public void CanApplyDebitBalance()
        {
            //Arrange
            Account account = null;
            decimal valueToApply = 2.0M;
            account = new Account("1");

            //Act
            account.CreditBalance(valueToApply * 2.0M);
            account.DebitBalance(valueToApply);

            //Assert
            Assert.IsTrue(account.Balance == valueToApply, "Account Balance {0} does not equal expected value {1}.", account.Balance, valueToApply);
        }

        [TestMethod]
        public void CannotDebitBalanceBelowZero()
        {
            //Arrange
            Account account = null;
            decimal valueToApply = 2.0M;
            account = new Account("1");

            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                account.DebitBalance(valueToApply);
            },
            "Cannot set Balance to less than zero via DebitBalance method.",
            "amount");

            //Assert
            //...
        }

        [TestMethod]
        public void CannotCreditBalanceBelowZero()
        {
            //Arrange
            Account account = null;
            decimal valueToApply = 2.0M * -1.0M;
            account = new Account("1");

            //Act
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                account.CreditBalance(valueToApply);
            },
            "Cannot set Balance to less than zero via CreditBalance method.",
            "amount");

            //Assert
            //...
        }
    }
}
