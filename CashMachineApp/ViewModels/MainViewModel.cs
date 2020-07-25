using CashMachineApp.Commands;
using CashMachineApp.Interfaces;
using CashMachineApp.Models;
using CashMachineApp.Views;
using System;
using System.Windows.Input;

namespace CashMachineApp.ViewModels
{
    /// <summary>
    /// Модель представление для главного окна
    /// </summary>
    class MainViewModel : ViewModelBase
    {
        readonly IMessageService messageService; // сервис вызывающий сообщения для пользователя
        readonly IInfoFileService infoFileService; // сервис работы с информационным файлом

        private readonly MainWindow mainWindow; // главное окно
        public CashMachine CashMachine { get; set; } // банкомат

        public DepositCashViewModel depositCashViewModel { get; private set; } // модель представление для меню внесения средств
        public WithDrawCashViewModel withdrawCashViewModel { get; private set; } // модель представление для меню выдачи средств

        /// <summary>
        /// Конструктор модели представления для главного окна
        /// </summary>
        /// <param name="MainWindow">главное окно</param>
        /// <param name="MessageService">сервис сообщений</param>
        /// <param name="InfoFileService">сервис открытия файла информации</param>
        /// <param name="DepositCashService">сервис внесения средств</param>
        /// <param name="WithdrawCashService">сервис выдачи средств</param>
        public MainViewModel(MainWindow MainWindow, 
            IMessageService MessageService, IInfoFileService InfoFileService, IDepositCashService DepositCashService, IWithdrawCashService WithdrawCashService)
        {
            this.mainWindow = MainWindow;
            this.infoFileService = InfoFileService;
            this.messageService = MessageService;

            CashMachine = new CashMachine();

            depositCashViewModel = new DepositCashViewModel(MessageService, DepositCashService, CashMachine );
            withdrawCashViewModel = new WithDrawCashViewModel(MessageService, WithdrawCashService, CashMachine );
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
                },
                (obj) => CashMachine.CurrentState));
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
                    CashMachine.IsDefaultWithdraw = true;

                    withdrawCashWindow.ShowDialog();
                },
                (obj) => CashMachine.CurrentState));
            }
        }

        #endregion
    }
}
