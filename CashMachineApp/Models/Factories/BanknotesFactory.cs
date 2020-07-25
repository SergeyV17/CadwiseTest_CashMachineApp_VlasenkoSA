using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CashMachineApp.Models.Factories
{
    /// <summary>
    /// Класс фабрики банкнот
    /// </summary>
    public static class BanknotesFactory
    {
        private static readonly Random r;
        private static readonly object locker;
        private static readonly int[] denominations; // массив номиналов банкнот

        static BanknotesFactory()
        {
            r = new Random();
            locker = new object();
            denominations = new int[] { 10, 50, 100, 200, 500, 1000, 2000, 5000 };
        }

        /// <summary>
        /// Метод генерации банкнот различного достоинства
        /// </summary>
        /// <param name="maxCountOfBanknotes">максимальное количество банкнот</param>
        /// <returns>коллекция банкнот</returns>
        public static List<Banknote> GetBanknotes(int maxCountOfBanknotes)
        {
            List<Banknote> banknotes = new List<Banknote>();

            Parallel.For(0, maxCountOfBanknotes - maxCountOfBanknotes / 4, (i) =>
            {
                lock (locker)
                {
                    Banknote banknote = GetBanknote(denominations[r.Next(8)]);
                    banknotes.Add(banknote);
                }
            });

            return banknotes;
        }

        /// <summary>
        /// Метод генерации банкноты по номиналу
        /// </summary>
        /// <param name="denomination">номинал</param>
        /// <returns>купюра</returns>
        public static Banknote GetBanknote(int denomination)
        {
            switch (denomination)
            {
                case 10:
                    return new Banknote { Name = "10 rubles", Denomination = 10 };
                case 50:
                    return new Banknote { Name = "50 rubles", Denomination = 50 };
                case 100:
                    return new Banknote { Name = "100 rubles", Denomination = 100 };
                case 200:
                    return new Banknote { Name = "200 rubles", Denomination = 200 };
                case 500:
                    return new Banknote { Name = "500 rubles", Denomination = 500 };
                case 1000:
                    return new Banknote { Name = "1000 rubles", Denomination = 1000 };
                case 2000:
                    return new Banknote { Name = "2000 rubles", Denomination = 2000 };
                case 5000:
                    return new Banknote { Name = "5000 rubles", Denomination = 5000 };
                default:
                    throw new Exception("There is no such denomination of currency");
            }
        }

        /// <summary>
        /// Метод подсчёта и создания коллекции банкнот по заданной сумме (внесения, выдачи)
        /// </summary>
        /// <param name="amountOfCash">сумма средств</param>
        /// <returns>коллекция банкнот</returns>
        public static List<Banknote> GetBanknotesByAmounOfCash(int amountOfCash)
        {
            List<Banknote> banknotes = new List<Banknote>();

            while (amountOfCash > 0)
            {
                if (amountOfCash >= 5000)
                {
                    banknotes.Add(new Banknote { Name = "5000 rubles", Denomination = 5000 });
                    amountOfCash -= 5000;
                }
                else if (amountOfCash >= 2000 && amountOfCash < 5000)
                {
                    banknotes.Add(new Banknote { Name = "2000 rubles", Denomination = 2000 });
                    amountOfCash -= 2000;
                }
                else if (amountOfCash >= 1000 && amountOfCash < 2000)
                {
                    banknotes.Add(new Banknote { Name = "1000 rubles", Denomination = 1000 });
                    amountOfCash -= 1000;
                }
                else if (amountOfCash >= 500 && amountOfCash < 1000)
                {
                    banknotes.Add(new Banknote { Name = "500 rubles", Denomination = 500 });
                    amountOfCash -= 500;
                }
                else if (amountOfCash >= 200 && amountOfCash < 5000)
                {
                    banknotes.Add(new Banknote { Name = "200 rubles", Denomination = 200 });
                    amountOfCash -= 200;
                }
                else if (amountOfCash >= 100 && amountOfCash < 200)
                {
                    banknotes.Add(new Banknote { Name = "100 rubles", Denomination = 100 });
                    amountOfCash -= 100;
                }
                else if (amountOfCash >= 50 && amountOfCash < 100)
                {
                    banknotes.Add(new Banknote { Name = "50 rubles", Denomination = 50 });
                    amountOfCash -= 50;
                }
                else
                {
                    banknotes.Add(new Banknote { Name = "10 rubles", Denomination = 10 });
                    amountOfCash -= 10;
                }
            }

            return banknotes;
        }
    }
}
