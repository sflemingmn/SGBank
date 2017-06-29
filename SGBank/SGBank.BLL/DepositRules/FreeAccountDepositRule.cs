using SGBank.Models;
using SGBank.Models.Responses;
using SGBank.Models.Interfaces;

namespace SGBank.BLL.DepositRules
{
    public class FreeAccountDepositRule : IDeposit
    {
        public AccountDepositResponse Deposit(Account account, decimal amount)
        {
            AccountDepositResponse response = new AccountDepositResponse();
            {
                if(account.Type != AccountType.Free)
                {
                    response.Success = false;
                    response.Message = "Error: A non-free account hit the Free Deposit Rule.";
                    return response;
                }

                if(amount > 100)
                {
                    response.Success = false;
                    response.Message = "Free accounts cannot deposit more than $100 at a time.";
                    return response;
                }

                if(amount <= 0)
                {
                    response.Success = false;
                    response.Message = "Depsoit amounts must be greater than zero.";
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
