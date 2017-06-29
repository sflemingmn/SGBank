using SGBank.Models;
using SGBank.Models.Responses;
using SGBank.Models.Interfaces;

namespace SGBank.BLL.WithdrawRules
{
    public class FreeAccountWithdrawRule : IWithdraw
    {
        public AccountWithdrawResponse Withdraw(Account account, decimal amount)
        {
            AccountWithdrawResponse response = new AccountWithdrawResponse();
            {
                if (account.Type != AccountType.Free)
                {
                    response.Success = false;
                    response.Message = "Error: A non-free account hit the Free Withdraw Rule. Contact IT.";
                    return response;
                }

                if (amount >= 0)
                {
                    response.Success = false;
                    response.Message = "Withdrawal amounts must be negative!";
                    return response;
                }
                    if (amount < -100)
                    {
                        response.Success = false;
                        response.Message = "Free accounts cannot withdraw more than $100!";
                        return response;
                    }

                if (amount + account.Balance < 0)
                {
                    response.Success = false;
                    response.Message = "Free accounts cannot overdraft.";
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
