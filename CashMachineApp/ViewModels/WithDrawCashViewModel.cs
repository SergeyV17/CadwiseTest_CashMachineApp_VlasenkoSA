using CashMachineApp.Commands;
using CashMachineApp.Interfaces;
using CashMachineApp.Models;
using CashMachineApp.Views;
using System.Windows.Input;

namespace CashMachineApp.ViewModels
{
    /// <summary>
    /// Модель представление для меню выдачи средств
    /// </summary>
    class WithDrawCashViewModel : ViewModelBase
    {
        private readonly IWithdrawCashService withdrawCashService; // сервис взноса средств в банкомат
        private readonly IMessageService messageService; // сервис вызывающий сообщения для пользователя

        readonly CashMachine cashMachine; // банкомат
        public WithdrawCashWindow WithdrawCashWindow { get; set; } // окно выдачи средств

        private int banknoteDenomination; // выбранный номинал для размена

        /// <summary>
        /// Конструктор модели представления для меню выдачи средств
        /// </summary>
        /// <param name="MessageService">сервис сообщений</param>
        /// <param name="WithdrawCashService">сервис выдачи средств</param>
        /// <param name="CashMachine">банкомат</param>
        public WithDrawCashViewModel(IMessageService MessageService, IWithdrawCashService WithdrawCashService, CashMachine CashMachine)
        {
            this.messageService = MessageService;
            this.withdrawCashService = WithdrawCashService;

            this.cashMachine = CashMachine;
        }

        #region Команды меню взноса средств

        /// <summary>
        /// Команда выдачи средств из банкомата
        /// </summary>
        private ICommand withdrawCashCommand;
        public ICommand WithdrawCashCommand
        {
            get
            {
                return withdrawCashCommand ??
                (withdrawCashCommand = new RelayCommand(obj =>
                {
                    bool checkForTrifles = withdrawCashService.CheckForTrifles(WithdrawAmount);

                    // Проверка крастности суммы на 10 (проверка на мелочь)
                    if (checkForTrifles)
                    {
                        if (cashMachine.IsDefaultWithdraw)
                        {
                            bool notEnoughBanknotes = withdrawCashService.DefaultWithdraw(cashMachine);

                            // Проверка на хранение в банкомате достаточно количества банкнот для выдачи
                            if (notEnoughBanknotes)
                                messageService.ShowErrorMessage(WithdrawCashWindow, "There are not enough banknotes in the ATM to issue");
                            else
                            {
                                messageService.ShowInfoMessage(WithdrawCashWindow, "Funds withdrawn successfully");
                                WithdrawCashWindow.Close();
                            }
                        }
                        else
                        {
                            bool checkForCorrectWithdraw = withdrawCashService.CheckForCorrectWithdraw(banknoteDenomination);

                            // Проверка на корректную выдачу (заданная сумма должна быть кратна выбранному номиналу)
                            if (checkForCorrectWithdraw)
                            {
                                bool notEnoughBanknotes = withdrawCashService.SelectedDenominationWithdraw(cashMachine, banknoteDenomination);

                                // Проверка на хранение в банкомате достаточно количества банкнот для выдачи
                                if (notEnoughBanknotes)
                                    messageService.ShowErrorMessage(WithdrawCashWindow, "There are not enough banknotes in the ATM to issue");
                                else
                                {
                                    messageService.ShowInfoMessage(WithdrawCashWindow, "Funds withdrawn successfully");
                                    WithdrawCashWindow.Close();
                                }
                            }
                            else
                                messageService.ShowErrorMessage(WithdrawCashWindow, $"The withdraw amount: {WithdrawAmount} cannot be dispensed by: {banknoteDenomination} p");
                        }
                    }
                    else
                        messageService.ShowErrorMessage(WithdrawCashWindow, "ATM machine doesn't work with change");
                }));
            }
        }

        /// <summary>
        /// Команда установки номинала для размена
        /// </summary>
        private ICommand setDenominationCommand;
        public ICommand SetDenominationCommand
        {
            get
            {
                return setDenominationCommand ??
                (setDenominationCommand = new RelayCommand(obj =>
                {
                    bool parseResult = int.TryParse(obj.ToString(), out banknoteDenomination);

                    if (!parseResult)
                        throw new System.Exception("Wrong denomination, check WithdrawCashWindow.xaml");

                    cashMachine.IsDefaultWithdraw = false;
                }));
            }
        }

        #endregion

        #region Элементы UI

        private string withdrawAmount; // Заданная сумма выдачи средств
        public string WithdrawAmount
        {
            get { return withdrawAmount; }
            set
            {
                withdrawAmount = value;
                OnPropertyChanged(nameof(WithdrawAmount));
            }
        }

        #endregion
    }
}
