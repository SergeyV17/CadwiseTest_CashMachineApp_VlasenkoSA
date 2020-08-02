using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Model.Models.Factories
{
    /// <summary>
    /// Класс фабрики банкнот
    /// </summary>
    public class BanknoteFactory
    {
        private static readonly Random r;
        private static readonly object locker;
        private static readonly Banknote[] banknotes; // массив номиналов банкнот

        static BanknoteFactory()
        {
            r = new Random();
            locker = new object();

            banknotes = new Banknote[] 
            {
                new Banknote("10 rubles", 10),
                new Banknote("50 rubles", 50),
                new Banknote("100 rubles", 100),
                new Banknote("200 rubles", 200),
                new Banknote("500 rubles", 500),
                new Banknote("1000 rubles", 1000),
                new Banknote("2000 rubles", 2000),
                new Banknote("5000 rubles", 5000)
            };
        }

        /// <summary>
        /// Метод генерации банкнот различного достоинства
        /// </summary>
        /// <param name="maxCountOfBanknotes">максимальное количество банкнот</param>
        /// <returns>коллекция банкнот</returns>
        public static IList<Banknote> GetBanknotes(int maxCountOfBanknotes)
        {
            IList<Banknote> banknotes = new List<Banknote>();

            Parallel.For(0, maxCountOfBanknotes - maxCountOfBanknotes / 4, (i) =>
            {
                lock (locker)
                {
                    banknotes.Add(banknotes[r.Next(8)]);
                }
            });

            return banknotes;
        }

        /// <summary>
        /// Метод подсчёта и создания коллекции банкнот по заданной сумме (для внесения и выдачи)
        /// </summary>
        /// <param name="amountOfCash">сумма средств</param>
        /// <returns>коллекция банкнот</returns>
        public static IList<Banknote> GetBanknotesByAmounOfCash(int amountOfCash)
        {
            IList<Banknote> banknotes = new List<Banknote>();

            while (amountOfCash > 0)
            {
                if (amountOfCash >= 5000)
                {
                    banknotes.Add(banknotes[7]);
                    amountOfCash -= 5000;
                }
                else if (amountOfCash >= 2000 && amountOfCash < 5000)
                {
                    banknotes.Add(banknotes[6]);
                    amountOfCash -= 2000;
                }
                else if (amountOfCash >= 1000 && amountOfCash < 2000)
                {
                    banknotes.Add(banknotes[5]);
                    amountOfCash -= 1000;
                }
                else if (amountOfCash >= 500 && amountOfCash < 1000)
                {
                    banknotes.Add(banknotes[4]);
                    amountOfCash -= 500;
                }
                else if (amountOfCash >= 200 && amountOfCash < 5000)
                {
                    banknotes.Add(banknotes[3]);
                    amountOfCash -= 200;
                }
                else if (amountOfCash >= 100 && amountOfCash < 200)
                {
                    banknotes.Add(banknotes[2]);
                    amountOfCash -= 100;
                }
                else if (amountOfCash >= 50 && amountOfCash < 100)
                {
                    banknotes.Add(banknotes[1]);
                    amountOfCash -= 50;
                }
                else
                {
                    banknotes.Add(banknotes[0]);
                    amountOfCash -= 10;
                }
            }

            return banknotes;
        }
    }
}
