using CashMachineApp.ViewModels;
using System.Windows;
using CashMachineApp.Models.Implementations;


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
