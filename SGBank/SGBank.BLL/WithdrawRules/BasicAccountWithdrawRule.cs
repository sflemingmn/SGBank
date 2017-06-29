using SGBank.Models;
using SGBank.Models.Responses;
using SGBank.Models.Interfaces;

namespace SGBank.BLL.WithdrawRules
{
    public class BasicAccountWithdrawRule : IWithdraw
    {
        public AccountWithdrawResponse Withdraw(Account account, decimal amount)
        {
            AccountWithdrawResponse response = new AccountWithdrawResponse();
            {
                if (account.Type != AccountType.Basic)
                {
                    response.Success = false;
                    response.Message = "Error: A non-basic account hit the Basic Withdraw Rule. Contact IT.";
                    return response;
                }
                
                if (amount >= 0)
                {
                    response.Success = false;
                    response.Message = "Withdrawal amounts must be negative!";
                    return response;
                }

                if (amount < -500)
                {
                    response.Success = false;
                    response.Message = "Basic accounts cannot withdraw more than $500!";
                    return response;
                }

                if (amount + account.Balance < -100)
                {
                    response.Success = false;
                    response.Message = "This amount will overdraft more than your $100 limit!";
                    return response;
                }

                response.Account = account;
                response.OldBalance = account.Balance;
                response.Amount = amount;
                account.Balance += amount;
                if (account.Balance < 0)
                {
                    account.Balance -= 10;
                }
                response.Success = true;

                return response;
            }
        }
    }
}
