using CashMachineApp.Interfaces;
using CashMachineApp.Models.Factories;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CashMachineApp.Models
{
    /// <summary>
    /// Класс банкомата
    /// </summary>
    class CashMachine : ICashMachine, INotifyPropertyChanged
    {
        private const int maxCountOfBanknotes = 2000; // максимальное количество хранимых банкнот

        private IList<Banknote> banknotes;    
        public IReadOnlyList<Banknote> Banknotes => (IReadOnlyList<Banknote>)banknotes;// коллекция банкнот 

        public bool CurrentState => banknotes.Count <= 0 || banknotes.Count >= maxCountOfBanknotes ? false : true; // текущее состояние банкомата
        public int MaxCountOfBanknotes => maxCountOfBanknotes;
        public int CurrentCountOfBanknotes => banknotes.Count; // текущее количество хранимых банкнот
        public bool IsDefaultWithdraw => SelectedWithdrawDenomination != default;  // признак выдачи банкнот по умолчанию
        public int SelectedWithdrawDenomination { get; private set; } // выбранный номинал для выдачи банкнот

        /// <summary>
        /// Конструктор банкомата
        /// </summary>
        public CashMachine()
        {
            // заполнение банкомата банкнотами через фабрику
            banknotes = BanknoteFactory.GetBanknotes(maxCountOfBanknotes);
        }

        /// <summary>
        /// Метод добавления банкноты в банкомат
        /// </summary>
        /// <param name="banknote">банкнота для добавления</param>
        public void AddBanknote(Banknote banknote)
        {
            banknotes.Add(banknote);
            OnPropertyChanged(nameof(CurrentCountOfBanknotes));
            OnPropertyChanged(nameof(CurrentState));
        }

        /// <summary>
        /// Метод удаления банкноты из банкомата
        /// </summary>
        /// <param name="banknote">банкнота для удаления</param>
        public void RemoveBanknote(Banknote banknote)
        {
            banknotes.Remove(banknote);
            OnPropertyChanged(nameof(CurrentCountOfBanknotes));
            OnPropertyChanged(nameof(CurrentState));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Метод вызывающий событие PropertyChanged для оповещения UI об изменениях
        /// </summary>
        /// <param name="propertyName">наименование свойства</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
