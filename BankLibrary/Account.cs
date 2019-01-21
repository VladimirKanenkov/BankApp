using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    public abstract class Account : IAccount
    {
        protected internal event AccountStateHandler Withdrawed;
        protected internal event AccountStateHandler Added;
        protected internal event AccountStateHandler Closed;
        protected internal event AccountStateHandler Opened;
        protected internal event AccountStateHandler Calculated;
        private static int counter = 0;
        protected int _id;
        protected decimal _sum;
        protected int _percentage;
        protected int _days = 0;

        public Account(decimal sum, int percentage)
        {
            _sum = sum;
            _percentage = percentage;
            _id = ++counter;
        }

        public decimal CurrentSum
        {
            get {return _sum; }
        }

        public int Id
        {
            get { return _id; }
        }

        public int Percentage
        {
            get { return _percentage; }
        }

        private void CallEvent(AccountEventArgs e, AccountStateHandler handler)
        {
            if (e != null && handler != null)
                handler(this, e);
        }

        protected virtual void OnOpened(AccountEventArgs e)
        {
            CallEvent(e, Opened);
        }

        protected virtual void OnClosed(AccountEventArgs e)
        {
            CallEvent(e, Closed);
        }

        protected virtual void OnAdded(AccountEventArgs e)
        {
            CallEvent(e, Added);
        }

        protected virtual void OnWithdrawed(AccountEventArgs e)
        {
            CallEvent(e, Withdrawed);
        }

        protected virtual void OnCalculated(AccountEventArgs e)
        {
            CallEvent(e, Calculated);
        }

        public virtual void Put(decimal sum)
        {
            _sum += sum;
            OnAdded(new AccountEventArgs($"На счёт поступило {sum}", sum));
        }

        public virtual decimal Withdraw(decimal sum)
        {
            decimal result = 0;
            if (_sum >= sum)
            {
                _sum -= sum;
                OnWithdrawed(new AccountEventArgs($"Со счёта {Id} снято {sum}, остаток на счету {CurrentSum}", sum));
                result = sum;
            }
            else
                OnWithdrawed(new AccountEventArgs($"На счету {Id} не достаточно средств для снятия!", 0));

            return result;
        }

        protected internal virtual void Open()
        {
            OnOpened(new AccountEventArgs($"Создан новый счёт. Id счёта {Id}", this._id));
        }

        protected internal virtual void Close()
        {
            OnClosed(new AccountEventArgs($"Счёт {this._id} закрыт.", Id));
        }

        protected internal void IncrementDays()
        {
            _days++;
        }

        protected internal virtual void Calculate()
        {
            decimal increment = _sum * _percentage / 100;
            _sum += increment;
            OnCalculated(new AccountEventArgs($"Начислены проценты в размере {increment}",increment));
        }
}
}
