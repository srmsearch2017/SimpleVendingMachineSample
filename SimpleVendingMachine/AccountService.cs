using System.Collections.Generic;

namespace SimpleVendingMachine
{
    public class AccountService
    {
        public List<Account> GetAllAccounts()
        {
            List<Account> results = new List<Account>();

            Account account = new Account("1");

            account.CreditBalance(50.0M);
            results.Add(account);

            return results;
        }
    }
}
