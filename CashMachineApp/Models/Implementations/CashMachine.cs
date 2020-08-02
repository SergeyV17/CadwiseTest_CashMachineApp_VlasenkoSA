using System.Collections.Generic;
using CashMachineApp.Models.Abstractions;
using CashMachineApp.Models.Implementations.Factories;
using static CashMachineApp.Models.Implementations.Factories.BanknoteFactory;

namespace CashMachineApp.Models.Implementations
{
    /// <summary>
    /// Класс банкомата
    /// </summary>
    class CashMachine : ICashMachine
    {
        #region Поля

        private const int maxCountOfBanknotes = 100; // максимальное количество хранимых банкнот

        private IList<Banknote> banknotes;

        #endregion

        #region Свойства

        public IReadOnlyList<Banknote> Banknotes => (IReadOnlyList<Banknote>)banknotes;// коллекция банкнот 

        public bool State => banknotes.Count <= 0 || banknotes.Count >= maxCountOfBanknotes ? false : true; // текущее состояние банкомата

        public int[] BanknotesCountOfEachType { get; } // массив количества банкнот каждого номинала
        public int MaxCountOfBanknotes => maxCountOfBanknotes; // максимальное количество хранимых банкнот
        public int CurrentCountOfBanknotes => banknotes.Count; // текущее количество хранимых банкнот

        public bool IsDefaultWithdraw => SelectedWithdrawDenomination == default ? true : false;  // признак выдачи банкнот по умолчанию
        public int SelectedWithdrawDenomination { get; private set; } // выбранный номинал для выдачи банкнот

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор банкомата
        /// </summary>
        public CashMachine()
        {
            banknotes = BanknoteFactory.GetBanknotes(maxCountOfBanknotes);
            BanknotesCountOfEachType = GetCountOfBanknotesOfEachType();
        }

        #endregion

        #region Методы

        /// <summary>
        /// Метод добавления банкноты в банкомат
        /// </summary>
        /// <param name="banknote">банкнота для добавления</param>
        public void AddBanknote(Banknote banknote)
        {
            banknotes.Add(banknote);
        }

        /// <summary>
        /// Метод удаления банкноты из банкомата
        /// </summary>
        /// <param name="banknote">банкнота для удаления</param>
        public void RemoveBanknote(Banknote banknote)
        {
            banknotes.Remove(banknote);
        }

        /// <summary>
        /// Метод задающий номинал для выдачи
        /// </summary>
        /// <param name="denomination"></param>
        public void ChangeWithdrawDenomination(int denomination)
        {
            SelectedWithdrawDenomination = denomination;
        }

        /// <summary>
        /// Метод подсчёта банкнот каждого номинала
        /// </summary>
        /// <returns>массив количества банкнот каждого номинала</returns
        public int[] GetCountOfBanknotesOfEachType()
        {
            int[] CountOfBanknotesOfEachType = new int[BanknoteTypes.Length];

            foreach (var banknote in banknotes)
            {
                for (int i = 0; i < BanknoteTypes.Length; i++)
                {
                    if (banknote.Denomination == BanknoteTypes[i].Denomination)
                        CountOfBanknotesOfEachType[i]++;
                }
            }

            return CountOfBanknotesOfEachType;
        }

        #endregion
    }
}
