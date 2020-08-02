using System.Collections.Generic;
using static CashMachineApp.Models.Implementations.Factories.BanknoteFactory;

namespace CashMachineApp.Models.Implementations
{
    class CalculateBanknotes
    {
        /// <summary>
        /// Метод подсчёта и создания коллекции банкнот по заданной сумме для выдачи
        /// </summary>
        /// <param name="withdrawAmount">сумма средств</param>
        /// <param name="banknotesAmount">массив количества банкнот каждого номинала</param>
        /// <param name="banknotesAmountClone">клонированный массив количества банкнот каждого номинала</param>
        /// <returns>коллекция банкнот</returns>
        public static IList<Banknote> WithdrawBanknotesByAmounOfCash(int withdrawAmount , int[] banknotesAmount, out int[] banknotesAmountClone)
        {
            IList<Banknote> tempBanknotes = new List<Banknote>(); // временная коллекция для добавления банкнот на выдачу
            banknotesAmountClone = (int[])banknotesAmount.Clone(); // Клонирование массива, т.к. он передаётся в параметры по ссылке

            // Индекс последней выданной купюры
            int banknoteTypeIndex = 0;

            while (withdrawAmount > 0)
            {
                for (int i = banknoteTypeIndex; i < BanknoteTypes.Length; i++)
                {
                    if (withdrawAmount >= BanknoteTypes[i].Denomination)
                    {
                        banknoteTypeIndex = i;

                        if (banknotesAmountClone[i] > 0)
                        {
                            tempBanknotes.Add(BanknoteTypes[i]);
                            withdrawAmount -= BanknoteTypes[i].Denomination;
                            banknotesAmountClone[i]--;
                            break;
                        }

                        // Если в цикле рассматривается наименьший номинал банкноты и его количество равно 0, то не хватает банкнот для выдачи
                        if (BanknoteTypes[i].Equals(BanknoteTypes[BanknoteTypes.Length - 1]) && banknotesAmountClone[i] == default)
                            return null;
                    }
                }
            }

            return tempBanknotes;
        }

        /// <summary>
        /// Метод подсчёта и создания коллекции банкнот по заданной сумме для внесения
        /// </summary>
        /// <param name="depositAmount">сумма средств</param>
        /// <param name="banknotesAmount">массив количества банкнот каждого номинала</param>
        /// <returns>коллекция банкнот</returns>
        public static IList<Banknote> DepositBanknotesByAmounOfCash(int depositAmount, int[] banknotesAmount)
        {
            IList<Banknote> tempBanknotes = new List<Banknote>(); // временная коллекция для добавления банкнот на выдачу

            // Индекс последней выданной купюры
            int banknoteTypeIndex = 0;

            while (depositAmount > 0)
            {
                for (int i = banknoteTypeIndex; i < BanknoteTypes.Length; i++)
                {
                    if (depositAmount >= BanknoteTypes[i].Denomination)
                    {
                        banknoteTypeIndex = i;

                        tempBanknotes.Add(BanknoteTypes[i]);
                        banknotesAmount[i]++;
                        depositAmount -= BanknoteTypes[i].Denomination;
                        break;
                    }
                }
            }

            return tempBanknotes;
        }
    }
}
