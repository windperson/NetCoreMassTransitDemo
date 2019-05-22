using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourierPatternDemo.AccountData
{
    public class AccountRepo
    {
        private static readonly List<Account> _accounts = new List<Account>()
        {
            new Account
            {
                AccountId = "player1",
                Balance = 100,
            },
            new Account
            {
                AccountId = "player2",
                Balance = 50,
            }
        };

        public List<Account> Data => _accounts;
    }

    public class Account
    {
        public string AccountId { get; set; }
        public decimal Balance { get; set; }
    }
}
