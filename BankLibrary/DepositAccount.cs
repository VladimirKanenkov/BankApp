using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    class DepositAccount:Account
    {
        public DepositAccount(decimal sum, int percentage) : base(sum, percentage) { }
        protected internal override void Open()
        {
            base.OnOpened(new AccountEventArgs("Открыт новый депозитный счёт! Id счёта " + this._id, this._sum));
        }
    }
}
