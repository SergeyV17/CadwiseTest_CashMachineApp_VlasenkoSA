using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CashMachineApp.Models.Implementations.Factories
{
    /// <summary>
    /// Класс фабрики банкнот
    /// </summary>
    public class BanknoteFactory
    {
        private static readonly Random r;
        private static readonly object locker;
        public static Banknote[] BanknoteTypes { get; } // массив номиналов банкнот

        static BanknoteFactory()
        {
            r = new Random();
            locker = new object();

            BanknoteTypes = new Banknote[] 
            {
                new Banknote("5000 rubles", 5000),
                new Banknote("2000 rubles", 2000),
                new Banknote("1000 rubles", 1000),
                new Banknote("500 rubles", 500),
                new Banknote("200 rubles", 200),
                new Banknote("100 rubles", 100),
                new Banknote("50 rubles", 50),
                new Banknote("10 rubles", 10),
            };
        }

        /// <summary>
        /// Метод генерации банкнот различного достоинства
        /// </summary>
        /// <param name="maxCountOfBanknotes">максимальное количество банкнот</param>
        /// <returns>коллекция банкнот</returns>
        public static IList<Banknote> GetBanknotes(int maxCountOfBanknotes)
        {
            // *Для тестирования*
            //IList<Banknote> tempBanknotes = new List<Banknote>
            //{
            //    new Banknote("500 rubles", 500),
            //    new Banknote("100 rubles", 100),
            //    new Banknote("100 rubles", 100),
            //    new Banknote("100 rubles", 100)
            //};

            IList<Banknote> tempBanknotes = new List<Banknote>();

            Parallel.For(0, maxCountOfBanknotes - maxCountOfBanknotes / 4, (i) =>
            {
                lock (locker)
                    tempBanknotes.Add(BanknoteTypes[r.Next(BanknoteTypes.Length)]);
            });

            return tempBanknotes;
        }
    }
}
