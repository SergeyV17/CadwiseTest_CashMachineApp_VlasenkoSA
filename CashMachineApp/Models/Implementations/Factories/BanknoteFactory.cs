using CashMachineApp.Models.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public static int NumbersOfBanknoteTypes => BanknoteTypes.Length; // количество типов банкнот

        static BanknoteFactory()
        {
            r = new Random();
            locker = new object();

            BanknoteTypes = new Banknote[] 
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
            IList<Banknote> tempBanknotes = new List<Banknote>
            {
                new Banknote("500 rubles", 500),
                new Banknote("100 rubles", 100),
                new Banknote("100 rubles", 100),
                new Banknote("100 rubles", 100)
            };

            //Parallel.For(0, maxCountOfBanknotes - 1, (i) =>
            //{
            //    lock (locker)
            //    {
            //        tempBanknotes.Add(banknotes[r.Next(banknotes.Length)]);
            //    }
            //});

            return tempBanknotes;
        }

        /// <summary>
        /// Метод подсчёта и создания коллекции банкнот по заданной сумме (для внесения и выдачи)
        /// </summary>
        /// <param name="amountOfCash">сумма средств</param>
        /// <returns>коллекция банкнот</returns>
        public static IList<Banknote> GetBanknotesByAmounOfCash(IList<Banknote> banknotes,  int amountOfCash, IList<Banknote> availableDenominations)
        {
            IList<Banknote> prototype = new List<Banknote>(banknotes);

            // Коллекция банкнот для выдачи
            IList<Banknote> tempBanknotes = new List<Banknote>();

            // Индекс последней выданной купюры
            int beginIndex = 0;

            while(amountOfCash > 0)
            {
                for (int i = beginIndex; i < availableDenominations.Count; i++)
                {
                    // Если сумма средств больше чем номинал купюры, тогда будет выдана купюра с наибольшим номиналом
                    // Если меньше, тогда переход к проверке со следующим номиналом купюры
                    if (amountOfCash >= availableDenominations[i].Denomination)
                    {
                        beginIndex = i;

                        // Если в цикле рассматривается самый наименьшый номинал купюры и купюр не хватает для выдачи, прервать выполнение выдачи
                        if (availableDenominations[beginIndex].Denomination == availableDenominations[availableDenominations.Count - 1].Denomination &&
                            !prototype.Contains(availableDenominations[i]))
                            return null;

                        prototype.Remove(availableDenominations[i]);
                        tempBanknotes.Add(availableDenominations[i]);

                        amountOfCash -= availableDenominations[i].Denomination;
                        break;
                    }
                }
            }

            return tempBanknotes;
        }
    }
}
