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
        public List<Banknote> Banknotes { get; set; } // коллекция банкнот

        public bool CurrentState => Banknotes.Count <= 0 || Banknotes.Count >= maxCountOfBanknotes ? false : true; // текущее состояние банкомата

        public int MaxCountOfBanknotes => maxCountOfBanknotes;
        public int CurrentCountOfBanknotes => Banknotes.Count; // текущее количество хранимых банкнот
        public bool IsDefaultWithdraw { get; set; } // признак выдачи банкнот по умолчанию
        public int SelectedWithdrawDenomination { get; set; } // выбранный номинал для выдачи банкнот

        /// <summary>
        /// Конструктор банкомата
        /// </summary>
        public CashMachine()
        {
            // заполнение банкомата через фабрику
            Banknotes = BanknotesFactory.GetBanknotes(maxCountOfBanknotes); 
            IsDefaultWithdraw = true;
        }

        /// <summary>
        /// Метод добавления банкноты в банкомат
        /// </summary>
        /// <param name="banknote">банкнота для добавления</param>
        public void AddBanknote(Banknote banknote)
        {
            Banknotes.Add(banknote);
            OnPropertyChanged(nameof(CurrentCountOfBanknotes));
            OnPropertyChanged(nameof(CurrentState));
        }

        /// <summary>
        /// Метод удаления банкноты из банкомата
        /// </summary>
        /// <param name="banknote">банкнота для удаления</param>
        public void RemoveBanknote(Banknote banknote)
        {
            Banknotes.Remove(banknote);
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
