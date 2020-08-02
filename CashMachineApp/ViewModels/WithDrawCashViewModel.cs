using CashMachineApp.Commands;
using CashMachineApp.Views;
using System.Windows.Input;
using CashMachineApp.Models.Abstractions;

namespace CashMachineApp.ViewModels
{
    /// <summary>
    /// Модель представление для меню выдачи средств
    /// </summary>
    class WithDrawCashViewModel : ViewModelBase
    {
        private readonly IWithdrawCashService withdrawCashService; // сервис взноса средств в банкомат
        private readonly IMessageService messageService; // сервис вызывающий сообщения для пользователя

        readonly ICashMachine cashMachine; // банкомат
        public WithdrawCashWindow WithdrawCashWindow { get; set; } // окно выдачи средств

        /// <summary>
        /// Конструктор модели представления для меню выдачи средств
        /// </summary>
        /// <param name="MessageService">сервис сообщений</param>
        /// <param name="WithdrawCashService">сервис выдачи средств</param>
        /// <param name="CashMachine">банкомат</param>
        public WithDrawCashViewModel(IMessageService MessageService, IWithdrawCashService WithdrawCashService, ICashMachine CashMachine)
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

                                cashMachine.ChangeWithdrawDenomination(0);
                            }
                        }
                        else
                        {
                            bool checkForCorrectWithdraw = withdrawCashService.CheckForCorrectWithdraw(cashMachine.SelectedWithdrawDenomination);

                            // Проверка на корректную выдачу (заданная сумма должна быть кратна выбранному номиналу)
                            if (checkForCorrectWithdraw)
                            {
                                bool notEnoughBanknotes = withdrawCashService.SelectedDenominationWithdraw(cashMachine, cashMachine.SelectedWithdrawDenomination);

                                // Проверка на хранение в банкомате достаточно количества банкнот для выдачи
                                if (notEnoughBanknotes)
                                    messageService.ShowErrorMessage(WithdrawCashWindow, "There are not enough banknotes in the ATM to issue");
                                else
                                {
                                    messageService.ShowInfoMessage(WithdrawCashWindow, "Funds withdrawn successfully");
                                    WithdrawCashWindow.Close();

                                    cashMachine.ChangeWithdrawDenomination(0);
                                }
                            }
                            else
                                messageService.ShowErrorMessage(WithdrawCashWindow, $"The withdraw amount: {WithdrawAmount} cannot be dispensed by: {cashMachine.SelectedWithdrawDenomination} p");
                        }
                    }
                    else
                        messageService.ShowErrorMessage(WithdrawCashWindow, "ATM machine doesn't work with change");
                }));
            }
        }

        /// <summary>
        /// Команда установки номинала для выдачи
        /// </summary>
        private ICommand setDenominationCommand;
        public ICommand SetDenominationCommand
        {
            get
            {
                return setDenominationCommand ??
                (setDenominationCommand = new RelayCommand(obj =>
                {
                    bool parseResult = int.TryParse(obj.ToString(), out int selectedDenominaton);

                    if (!parseResult)
                        throw new System.Exception("Wrong denomination, check WithdrawCashWindow.xaml");

                    cashMachine.ChangeWithdrawDenomination(selectedDenominaton);
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
