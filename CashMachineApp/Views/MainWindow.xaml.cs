using CashMachineApp.Models;
using CashMachineApp.ViewModels;
using System.Windows;

namespace CashMachineApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel(this,
                new MessageCaller(), new InfoFileReader(), new DepositCash(), new WithdrawCash());
        }
    }
}
