using System.Collections.Generic;
using CashMachineApp.Models.Abstractions;
using CashMachineApp.Models.Implementations.Factories;

namespace CashMachineApp.Models.Implementations
{
    /// <summary>
    /// Класс банкомата
    /// </summary>
    class CashMachine : ICashMachine
    {
        #region Поля

        private const int maxCountOfBanknotes = 2000; // максимальное количество хранимых банкнот

        private IList<Banknote> banknotes;

        #endregion

        #region Свойства

        public IReadOnlyList<Banknote> Banknotes => (IReadOnlyList<Banknote>)banknotes;// коллекция банкнот 

        public bool State => banknotes.Count <= 0 || banknotes.Count >= maxCountOfBanknotes ? false : true; // текущее состояние банкомата
        public int Balance => GetCashMachineBalance(); // баланс суммы наличных в банкомате

        //public int [] BanknotesCountOfEachType { get; } // массив количества банкнот каждого номинала

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
            //BanknotesCountOfEachType = GetBanknotesOfEachType();
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
        /// Метод расчета суммы средств в банкомате
        /// </summary>
        public int GetCashMachineBalance()
        {
            int balance = 0;

            foreach (var banknote in banknotes)
                balance += banknote.Denomination;

            return balance;
        }

        /// <summary>
        /// Метод задающий номинал для выдачи
        /// </summary>
        /// <param name="denomination"></param>
        public void ChangeWithdrawDenomination(int denomination)
        {
            SelectedWithdrawDenomination = denomination;
        }

        ///// <summary>
        ///// Метод подсчёта банкнот каждого номинала
        ///// </summary>
        ///// <returns>массив количества банкнот каждого номинала</returns>
        //private int[] GetBanknotesOfEachType()
        //{
        //    int[] banknotesOfEachType = new int[BanknoteFactory.NumbersOfBanknoteTypes];

        //    for (int i = 0; i < BanknoteFactory.NumbersOfBanknoteTypes; i++)
        //        banknotesOfEachType[i] = this.banknotes.Where(b => b.Denomination == BanknoteFactory.BanknoteTypes[i].Denomination).Count();

        //    return banknotesOfEachType;
        //}

        #endregion
    }
}
