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
        internal WithdrawCashWindow WithdrawCashWindow { get; set; } // окно выдачи средств

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

            SelectedDenomination = "default";
        }

        #region Команды меню выдачи средств

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
                            bool enoughBanknotes = withdrawCashService.DefaultWithdraw(cashMachine);
                            ConditionCheckAction(enoughBanknotes);
                        }
                        else
                        {
                            bool checkForCorrectWithdraw = withdrawCashService.CheckForCorrectWithdraw(cashMachine.SelectedWithdrawDenomination);

                            // Проверка на корректную выдачу (заданная сумма должна быть кратна выбранному номиналу)
                            if (checkForCorrectWithdraw)
                            {
                                bool enoughBanknotes = withdrawCashService.SelectedDenominationWithdraw(cashMachine, cashMachine.SelectedWithdrawDenomination);
                                ConditionCheckAction(enoughBanknotes);
                            }
                            else
                                messageService.ShowErrorMessage(WithdrawCashWindow, $"The withdraw amount: {WithdrawAmount} cannot be dispensed by: {cashMachine.SelectedWithdrawDenomination} p");
                        }
                    }
                    else
                        messageService.ShowErrorMessage(WithdrawCashWindow, "ATM machine doesn't work with change");

                    cashMachine.ChangeWithdrawDenomination(0);
                    SelectedDenomination = "default";
                }));
            }
        }

        /// <summary>
        /// Метод обрабатывающий логику проверки на достаточное количество банкнот для выдачи
        /// </summary>
        /// <param name="enoughBanknotes">достаточно ли банкнот в банкномате</param>
        private void ConditionCheckAction(bool enoughBanknotes)
        {
            // Проверка на хранение в банкомате достаточно количества банкнот для выдачи
            if (enoughBanknotes)
            {
                messageService.ShowInfoMessage(WithdrawCashWindow, "Funds withdrawn successfully");
                WithdrawCashWindow.Close();
            }
            else
                messageService.ShowErrorMessage(WithdrawCashWindow, "There are not enough banknotes in the ATM to issue");

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
                    SelectedDenomination = obj as string == "0" ? "default" : obj as string;
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

        private string selectedDenomination; // Выбранный номинал на выдачу средств
        public string SelectedDenomination
        {
            get { return selectedDenomination; }
            set
            {
                selectedDenomination = value;
                OnPropertyChanged(nameof(SelectedDenomination));
            }
        }

        #endregion
    }
}
