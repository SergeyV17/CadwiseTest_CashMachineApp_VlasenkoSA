using CashMachineApp.Models;
using System.Collections.Generic;

namespace CashMachineApp.Interfaces
{
    /// <summary>
    /// Интерфейс определяющий реализацию банкомата
    /// </summary>
    interface ICashMachine
    {
        List<Banknote> Banknotes { get; } // Коллекция банкнот

        void AddBanknote(Banknote banknote); // Метод добавления банкноты

        void RemoveBanknote(Banknote banknote); // Метод удаления банкноты

        bool CurrentState { get; } // Текущее состояние банкомата

        int MaxCountOfBanknotes { get; } // Максимальное количество хранимых банкнот

        int CurrentCountOfBanknotes { get; } // Текущее количество хранимых банкнот

        bool IsDefaultWithdraw { get; set; } // Признак выдачи банкнот по умолчанию

        int SelectedWithdrawDenomination { get; set; } // Выбранный номинал для выдачи банкнот
    }
}
