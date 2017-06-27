using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleVendingMachine.UnitTests
{
    [TestClass]
    public class VendingMachineTests
    {
        [TestMethod]
        public void CanCreateVendingMachine()
        {
            //Arrange
            VendingMachine vendingMachine = null;

            AccountService accountService = new AccountService();
            //Act
            vendingMachine = new VendingMachine(accountService);

            //Assert
            Assert.IsNotNull(vendingMachine, "VendingMachine object is null.");
        }
    }
}
