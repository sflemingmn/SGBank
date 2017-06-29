using NUnit.Framework;
using SGBank.BLL;
using SGBank.BLL.DepositRules;
using SGBank.BLL.WithdrawRules;
using SGBank.Models;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;

namespace SGBank.Tests
{
    [TestFixture]
    public class FreeAccountTests
    {
        [Test]

        public void CanLoadFreeTestData()
        {
            AccountManager manager = AccountManagerFactory.Create();
            AccountLookupResponse response = manager.LookupAccount("12345");

            Assert.IsNotNull(response.Account);
            Assert.IsTrue(response.Success);
            Assert.AreEqual("12345", response.Account.AccountNumber);
        }

        [TestCase ("12345", "Free Account", 100, AccountType.Free, 250, false)]
        [TestCase ("12345", "Free Account", 100, AccountType.Free, -100, false)]
        [TestCase ("12345", "Free Account", 100, AccountType.Basic, 50, false)]
        [TestCase ("12345", "Free Account", 100, AccountType.Free, 50, true)]

        public void FreeAccountDepositRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, bool expectedResult)

        {
            IDeposit rule = new FreeAccountDepositRule();

            Account account = new Account();
            account.AccountNumber = accountNumber;
            account.Name = name;
            account.Balance = balance;
            account.Type = accountType;

            AccountDepositResponse response = rule.Deposit(account, amount);

            Assert.AreEqual(expectedResult, response.Success);
        }

        [TestCase("12345", "Free Account", 100, AccountType.Free, 50, false)]
        [TestCase("12345", "Free Account", 500, AccountType.Free, -250, false)]
        [TestCase("12345", "Free Account", 100, AccountType.Basic, -50, false)]
        [TestCase("12345", "Free Account", 50, AccountType.Free, -75, false)]
        [TestCase("12345", "Free Account", 100, AccountType.Free, -50, true)]

        public void FreeAccountWithdrawRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, bool expectedResult)

        {
            IWithdraw rule = new FreeAccountWithdrawRule();

            Account account = new Account();
            account.AccountNumber = accountNumber;
            account.Name = name;
            account.Balance = balance;
            account.Type = accountType;

            AccountWithdrawResponse response = rule.Withdraw(account, amount);

            Assert.AreEqual(expectedResult, response.Success);
        }
    }
}