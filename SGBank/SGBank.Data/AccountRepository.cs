using SGBank.Models;
using SGBank.Models.Interfaces;
using System.Collections.Generic;
using System.IO;
using System;

namespace SGBank.Data
{
    public class AccountRepository : IAccountRepository
    {
        private List<Account> _accountList = new List<Account>();

        private string _filePath;

        public AccountRepository(string filePath)
        {
            _filePath = filePath;

            using (StreamReader sr = new StreamReader(Settings.FilePath))
            {
                sr.ReadLine();
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    Account accountSelect = new Account();
                    string[] columns = line.Split(',');

                    accountSelect.AccountNumber = columns[0];
                    accountSelect.Name = columns[1];
                    accountSelect.Balance = decimal.Parse(columns[2]);

                    switch (columns[3])
                    {
                        case "F":
                            accountSelect.Type = AccountType.Free;
                            break;
                        case "B":
                            accountSelect.Type = AccountType.Basic;
                            break;
                        case "P":
                            accountSelect.Type = AccountType.Premium;
                            break;
                    }
                    _accountList.Add(accountSelect);
                }
            }
        }

        public Account LoadAccount(string accountNumber)
        {
            Account loadAccount = _accountList.Find(load => load.AccountNumber == accountNumber);
            return loadAccount;
        }

        public void SaveAccount(Account account)
        {
            string header = "AccountNumber,Name,Balance,Type";

            using (StreamWriter sw = new StreamWriter(Settings.FilePath))
            {
                sw.WriteLine(header);

                foreach (var type in _accountList)
                {
                    var accountType = type;

                    if (type.AccountNumber == account.AccountNumber)
                    {
                        accountType = account;
                    }

                    string write = $"{accountType.AccountNumber},{accountType.Name},{accountType.Balance},";

                    switch (accountType.Type)
                    {
                        case AccountType.Free:
                            write += "F";
                            break;
                        case AccountType.Basic:
                            write += "B";
                            break;
                        case AccountType.Premium:
                            write += "P";
                            break;
                    }

                    sw.WriteLine(write);
                }
            }

        }
    }
}
