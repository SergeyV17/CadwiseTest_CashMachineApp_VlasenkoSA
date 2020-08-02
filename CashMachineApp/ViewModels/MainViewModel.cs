using CashMachineApp.Commands;
using CashMachineApp.Views;
using System;
using System.Windows.Input;
using CashMachineApp.Models.Abstractions;
using CashMachineApp.Models.Implementations;

namespace CashMachineApp.ViewModels
{
    /// <summary>
    /// Модель представление для главного окна
    /// </summary>
    class MainViewModel : ViewModelBase
    {
        private readonly IMessageService messageService; // сервис вызывающий сообщения для пользователя
        private readonly IInfoFileService infoFileService; // сервис работы с информационным файлом

        private readonly MainWindow mainWindow; // главное окно

        private readonly ICashMachine cashMachine; // банкомат

        private readonly DepositCashViewModel depositCashViewModel; // модель представление для меню внесения средств
        private readonly WithDrawCashViewModel withdrawCashViewModel; // модель представление для меню выдачи средств

        /// <summary>
        /// Конструктор модели представления для главного окна
        /// </summary>
        /// <param name="mainWindow">главное окно</param>
        /// <param name="MessageService">сервис сообщений</param>
        /// <param name="InfoFileService">сервис открытия файла информации</param>
        /// <param name="DepositCashService">сервис внесения средств</param>
        /// <param name="WithdrawCashService">сервис выдачи средств</param>
        public MainViewModel(MainWindow mainWindow, 
            IMessageService MessageService, IInfoFileService InfoFileService, IDepositCashService DepositCashService, IWithdrawCashService WithdrawCashService)
        {
            this.mainWindow = mainWindow;
            this.infoFileService = InfoFileService;
            this.messageService = MessageService;

            cashMachine = new CashMachine();

            depositCashViewModel = new DepositCashViewModel(MessageService, DepositCashService, cashMachine );
            withdrawCashViewModel = new WithDrawCashViewModel(MessageService, WithdrawCashService, cashMachine );
        }   

        #region Команды меню программы

        /// <summary>
        /// Команда выхода из приложения
        /// </summary>
        private ICommand exitCommand;
        public ICommand ExitCommand
        {
            get
            {
                return exitCommand ??
                (exitCommand = new RelayCommand(obj => { mainWindow.Close(); }));
            }
        }

        /// <summary>
        /// Команда открытия файла информации
        /// </summary>
        private ICommand showInfoCommand;
        public ICommand ShowInfoCommand
        {
            get
            {
                return showInfoCommand ??
                (showInfoCommand = new RelayCommand(obj =>
                {
                    try
                    {
                        infoFileService.GetPath(@"Views\Resources\Text\Info.txt");
                        messageService.ShowMessageFromFile(mainWindow, infoFileService.FilePath);
                    }
                    catch (Exception ex)
                    {
                        messageService.ShowErrorMessage(mainWindow, ex.Message);
                    }
                }));
            }
        }

        #endregion

        #region Команды главного меню банкомата

        /// <summary>
        /// Команда вызова окна внесения средств в банкомат
        /// </summary>
        private ICommand openDepositCashMenuCommand;
        public ICommand OpenDepositCashMenuCommand
        {
            get
            {
                return openDepositCashMenuCommand ??
                (openDepositCashMenuCommand = new RelayCommand(obj =>
                {
                    var depositCashWindow = new DepositCashWindow() { Owner = mainWindow , DataContext = depositCashViewModel};
                    depositCashViewModel.DepositCashWindow = depositCashWindow;

                    depositCashWindow.ShowDialog();
                    StatusPropertyChanged();
                },
                (obj) => cashMachine.State));
            }
        }

        /// <summary>
        /// Команда вызова окна выдачи средств из банкомата
        /// </summary>
        private ICommand openWithdrawCashMenuCommand;
        public ICommand OpenWithdrawCashMenuCommand
        {
            get
            {
                return openWithdrawCashMenuCommand ??
                (openWithdrawCashMenuCommand = new RelayCommand(obj =>
                {
                    var withdrawCashWindow = new WithdrawCashWindow() { Owner = mainWindow, DataContext = withdrawCashViewModel };

                    withdrawCashViewModel.WithdrawCashWindow = withdrawCashWindow;

                    withdrawCashWindow.ShowDialog();
                    StatusPropertyChanged();
                },
                (obj) => cashMachine.State));
            }
        }

        #endregion

        #region Статус банкомата

        public bool CurrentState => cashMachine.State;
        public int MaxCountOfBanknotes => cashMachine.MaxCountOfBanknotes;
        public int CurrentCountOfBanknotes => cashMachine.CurrentCountOfBanknotes;

        /// <summary>
        /// Метод обновляющий UI при внесении или снятии средств
        /// </summary>
        private void StatusPropertyChanged()
        {
            OnPropertyChanged(nameof(CurrentState));
            OnPropertyChanged(nameof(CurrentCountOfBanknotes));
        }

        #endregion
    }
}
