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
        public DepositAccount(decimal sum,int percentage) : base(sum, percentage) { }
        protected internal override void Open()
        {
            base.OnOpened(new AccountEventArgs($"Открыт новый депозитный счёт! Id счёта {this._id}",this._sum));
        }

        public override void Put(decimal sum)
        {
            if (_days%30 == 0)
            {
                base.Put(sum);
            }
            else
            {
                base.OnAdded(new AccountEventArgs("На счёт можно положить только после 30 дневного периода!",0));
            }
        }

        public override decimal Withdraw(decimal sum)
        {
            if (_days % 30 == 0)
            {
                return base.Withdraw(sum);
            }
            else
            {
                base.OnWithdrawed(new AccountEventArgs("Со счёта можно снять только после 30 дневного периода!", 0));
                return 0;
            }
        }

        protected internal override void Calculate()
        {
            if (_days % 30 == 0)
            {
               base.Calculate();
            }
        }
    }
}
