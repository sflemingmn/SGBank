using NUnit.Framework;
using SGBank.BLL.DepositRules;
using SGBank.BLL.WithdrawRules;
using SGBank.Models;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;

namespace SGBank.Tests
{
    [TestFixture]
    public class BasicAccountTests
    {

        [TestCase("33333", "Basic Account", 100, AccountType.Free, 250, false)]
        [TestCase("33333", "Basic Account", 100, AccountType.Basic, -100, false)]
        [TestCase("333333", "Basic Account", 100, AccountType.Basic, 250, true)]

        public void BasicAccountDepositRuleTest(
                    string accountNumber, 
                    string name, 
                    decimal balance, 
                    AccountType accountType, 
                    decimal amount, 
                    bool expectedResult)
        {
            IDeposit rule = new NoLimitDepositRule();

            Account account = new Account();
            account.AccountNumber = accountNumber;
            account.Name = name;
            account.Balance = balance;
            account.Type = accountType;

            AccountDepositResponse response = rule.Deposit(account, amount);

            Assert.AreEqual(expectedResult, response.Success);
        }

        [TestCase("33333", "Basic Account", 1500, AccountType.Basic, -1000, 1500, false)]
        [TestCase("33333", "Basic Account", 100, AccountType.Free, -100, 100, false)]
        [TestCase("33333", "Basic Account", 100, AccountType.Basic, 100, 100, false)]
        [TestCase("33333", "Basic Account", 150, AccountType.Basic, -50, 100, true)]
        [TestCase("33333", "Basic Account", 100, AccountType.Basic, -150, -60, true)]

        public void BasicAccountWithdrawTest(
                    string accountNumber, 
                    string name, 
                    decimal balance, 
                    AccountType accountType, 
                    decimal amount, 
                    decimal newBalance, 
                    bool expectedResult)
        {
            IWithdraw rule = new BasicAccountWithdrawRule();

            Account account = new Account();
            account.AccountNumber = accountNumber;
            account.Name = name;
            account.Balance = balance;
            account.Type = accountType;

            AccountWithdrawResponse response = rule.Withdraw(account, amount);

            Assert.AreEqual(expectedResult, response.Success);
            Assert.AreEqual(newBalance, account.Balance);
        }
    }
}
