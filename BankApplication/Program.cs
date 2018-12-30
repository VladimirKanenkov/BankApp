using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BankLibrary;
using static System.Console;

namespace BankApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Bank<Account> bank = new Bank<Account>("Сбербанк");
            bool alive = true;
            while (alive)
            {
                ConsoleColor color = ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("1. Открыть счет \t 2. Вывести средства  \t 3. Добавить на счет");
                Console.WriteLine("4. Закрыть счет \t 5. Пропустить день \t 6. Выйти из программы");
                Console.WriteLine("Введите номер пункта:");
                ForegroundColor = ConsoleColor.White;
                try
                {
                    int command = Convert.ToInt32(ReadLine());
                    switch (command)
                    {
                        case 1:
                            OpenAccount(bank);
                            break;
                        case 2:
                            Withdraw(bank);
                            break;
                        case 3:
                            Put(bank);
                            break;
                        case 4:
                            CloseAccount(bank);
                            break;
                        case 5:
                            break;
                        case 6:
                            alive = false;
                            break;
                    }
                    bank.Calculate();
                }
                catch (Exception e)
                {
                    color = ForegroundColor = ConsoleColor.Red;
                    WriteLine(e.Message);
                    ForegroundColor = color;
                }
            }

            
        }

        private static void OpenAccount(Bank<Account> bank)
        {
            Write("Укажите сумму счёта: ");
            decimal sum = Convert.ToDecimal(ReadLine());
            Write("Выберите тип счёта\n1) До востребования.\n2) Депозитный.\nНомер счёта: ");
            int type = Convert.ToInt32(ReadLine());
            AccountType accountType;
            switch (type)
            {
                case 1:
                    accountType = AccountType.Ordinary;
                    break;
                case 2:
                    accountType = AccountType.Deposite;
                    break;
                default:
                    accountType = AccountType.Deposite;
                    WriteLine("Выбран депозитный счёт по умолчанию.");
                    break;
            }
            bank.Open(accountType,
                sum,
                AddSumHandler,
                WithDrawHandler,
                (o,e)=>WriteLine(e.Message),
                OpenAccountHandler,
                CloseAccountHandler
                );
        }

        private static void Put(Bank<Account> bank)
        {
            Write("Введите сумму, которую хотите положить: ");
            decimal sum = Convert.ToDecimal(ReadLine());
            Write("Введите id: ");
            int id = Convert.ToInt32(ReadLine());
            bank.Put(sum,id);
        }

        private static void Withdraw(Bank<Account> bank)
        {
            Write("Введите сумму для вывода денег: ");
            decimal sum = Convert.ToDecimal(ReadLine());
            Write("Введите id: ");
            int id = Convert.ToInt32(ReadLine());
            bank.Withdraw(sum,id);
        }

        private static void CloseAccount(Bank<Account> bank)
        {
            Write("Введите id: ");
            int id = Convert.ToInt32(ReadLine());
            bank.Close(id);
        }


        private static void OpenAccountHandler(object sender, AccountEventArgs e)
        {
            WriteLine(e.Message);
        }
        private static void AddSumHandler(object sender, AccountEventArgs e)
        {
            WriteLine(e.Message);
        }
        private static void WithDrawHandler(object sender, AccountEventArgs e)
        {
            WriteLine(e.Message);
            if (e.Sum>0)
            {
                WriteLine("Идём тратить денюжкииии...");
            }
        }
        private static void CloseAccountHandler(object sender, AccountEventArgs e)
        {
            WriteLine(e.Message);
        }
    }

}
