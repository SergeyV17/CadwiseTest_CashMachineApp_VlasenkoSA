using System.Collections.Generic;
using CashMachineApp.Models.Implementations;

namespace CashMachineApp.Models.Abstractions
{
    /// <summary>
    /// Интерфейс определяющий реализацию банкомата
    /// </summary>
    interface ICashMachine
    {
        IReadOnlyList<Banknote> Banknotes { get; } // коллекция банкнот
        void AddBanknote(Banknote banknote); // Метод добавления банкноты
        void RemoveBanknote(Banknote banknote); // Метод удаления банкноты

        bool State { get; } // Текущее состояние банкомата

        int[] BanknotesCountOfEachType { get; } // массив количества банкнот каждого номинала

        int MaxCountOfBanknotes { get; } // Максимальное количество хранимых банкнот
        int CurrentCountOfBanknotes { get; } // Текущее количество хранимых банкнот

        bool IsDefaultWithdraw { get; } // Признак выдачи банкнот по умолчанию
        int SelectedWithdrawDenomination { get; } // Выбранный номинал для выдачи банкнот

        void ChangeWithdrawDenomination(int denomination); // Метод задающий номинал для выдачи
    }
}
