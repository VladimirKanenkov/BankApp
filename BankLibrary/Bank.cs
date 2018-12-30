using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    public class Bank<T> where T:Account
    {
        private T[] accounts;
        public string Name { get; private set; }

        public Bank(string name)
        {
            this.Name = name;
        }

        public void Open(AccountType accountType, decimal sum, AccountStateHandler addSumHandler,
            AccountStateHandler withdraveSumHandler,
            AccountStateHandler calculateHandler, AccountStateHandler openAccountHandler,
            AccountStateHandler closeAccountHandler)
        {
            T newAccount = null;
            switch (accountType)
            {
                case AccountType.Ordinary:
                    newAccount = new DemandAccount(sum, 1) as T;
                    break;
                case AccountType.Deposite:
                    newAccount = new DepositAccount(sum,1) as T;
                    break;
            }

            if (newAccount == null)
                throw new Exception("Ошибка создания счёта!");
            if (accounts == null)
                accounts = new T[] {newAccount};
            else
            {
                T[] tempAccounts = new T[accounts.Length+1];
                for (int i = 0; i < accounts.Length; i++)
                    tempAccounts[i] = accounts[i];
                tempAccounts[accounts.Length] = newAccount;
                accounts = tempAccounts;
            }

            newAccount.Added += addSumHandler;
            newAccount.Withdrawed += withdraveSumHandler;
            newAccount.Opened += openAccountHandler;
            newAccount.Closed += closeAccountHandler;
            newAccount.Calculated += calculateHandler;

            newAccount.Open();
        }
        public int Id
        {
            get => Id;
        }

        public void Put(decimal sum, int id)
        {
            T account = FindAccount(id);
            if (account==null)
                throw new Exception("Счёт не найден!");
            account.Put(sum);
        }
        public void Withdraw(decimal sum, int id)
        {
            T account = FindAccount(id);
            if (account == null)
                throw new Exception("Счёт не найден!");
            account.Withdraw(sum);
        }

        public void Close(int id)
        {
            int index;
            T account = FindAccount(id, out index);
            if (account == null)
                throw new Exception("Счёт не найден!");
            if (accounts.Length <= 1)
                accounts = null;
            else
            {
                T[] tempaccount = new T[accounts.Length-1];
                for (int i = 0,j = 0; i < accounts.Length; i++)
                {
                    if (i!=index)
                    {
                        tempaccount[j++] = accounts[i];
                    }
                }

                accounts = tempaccount;
            }
        }

        public void Calculate()
        {
            if(accounts == null) return;
            for (int i = 0; i < accounts.Length; i++)
            {
                accounts[i].IncrementDays();
                accounts[i].Calculate();
            }

        }


        public T FindAccount(int Id)
        {
            for (int i = 0; i < accounts.Length; i++)
            {
                if (accounts[i].Id == Id)
                {
                    return accounts[i];
                }
            }
            return null;
        }

        public T FindAccount(int Id, out int Index)
        {
            for (int i = 0; i < accounts.Length; i++)
            {
                if (accounts[i].Id == Id)
                {
                    Index = i;
                    return accounts[i];
                }
            }

            Index = -1;
            return null;
        }
    }

    public enum AccountType
    {
        Ordinary,
        Deposite,
    }
}
