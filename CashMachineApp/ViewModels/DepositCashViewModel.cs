using CashMachineApp.Commands;
using CashMachineApp.Views;
using System.Windows.Input;
using CashMachineApp.Models.Abstractions;

namespace CashMachineApp.ViewModels
{
    /// <summary>
    /// Модель представление для меню внесения средств
    /// </summary>
    class DepositCashViewModel : ViewModelBase
    {
        readonly IDepositCashService depositCashService; // сервис внесения средств в банкомат
        readonly IMessageService messageService; // сервис вызывающий сообщения для пользователя

        readonly ICashMachine cashMachine; // банкомат
        internal DepositCashWindow DepositCashWindow { get; set; } // окно меню внесения средств

        /// <summary>
        /// Конструктор модели представления
        /// </summary>
        /// <param name="MessageService">сервис сообщений</param>
        /// <param name="DepositCashService">сервис внесения средств</param>
        /// <param name="CashMachine">банкомат</param>
        public DepositCashViewModel(IMessageService MessageService, IDepositCashService DepositCashService, ICashMachine CashMachine)
        {
            this.messageService = MessageService;
            this.depositCashService = DepositCashService;

            this.cashMachine = CashMachine;
        }

        #region Команды меню внесения средств

        /// <summary>
        /// Команда внесения средств в банкомат
        /// </summary>
        private ICommand depositCashCommand;
        public ICommand DepositCashCommand
        {
            get
            {
                return depositCashCommand ??
                (depositCashCommand = new RelayCommand(obj =>
                {
                    bool checkForTrifles = depositCashService.CheckForTrifles(depositAmount);

                    // Проверка крастности суммы на 10 (проверка на мелочь)
                    if (checkForTrifles)
                    {
                        bool cashMachineIsOverflow = depositCashService.DepositFundsToCashMachine(cashMachine);

                        // Проверка на переполнение банкомата
                        if (cashMachineIsOverflow)
                        {
                            messageService.ShowInfoMessage(DepositCashWindow, "Funds deposited successfully");
                            DepositCashWindow.Close();
                        }
                        else
                            messageService.ShowErrorMessage(DepositCashWindow, "ATM cannot accept the specified amount due to overflow");
                    }
                    else
                        messageService.ShowErrorMessage(DepositCashWindow, "ATM machine doesn't work with change");

                }));
            }
        }

        #endregion

        #region Элементы UI

        private string depositAmount; // Заданная сумма внесения средств
        public string DepositAmount
        {
            get { return depositAmount;}
            set
            {
                depositAmount = value;
                OnPropertyChanged(nameof(DepositAmount));
            }
        }

        #endregion
    }
}
