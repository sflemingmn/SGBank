using SGBank.Models;
using SGBank.Models.Responses;
using SGBank.Models.Interfaces;

namespace SGBank.BLL.DepositRules
{
    public class NoLimitDepositRule : IDeposit
    {
        public AccountDepositResponse Deposit(Account account, decimal amount)
        {
            AccountDepositResponse response = new AccountDepositResponse();
            {
                if(account.Type == AccountType.Free)
                {
                    response.Success = false;
                    response.Message = "Error: Only basic and premium accounts can deposit with no limit. Contact IT.";
                    return response;
                }

                if(amount <= 0)
                {
                    response.Success = false;
                    response.Message = "Deposit amounts must be positive!";
                    return response;
                }

                response.Account = account;
                response.OldBalance = account.Balance;
                response.Amount = amount;
                account.Balance += amount;
                response.Success = true;

                return response;
            }
        }
    }
}
