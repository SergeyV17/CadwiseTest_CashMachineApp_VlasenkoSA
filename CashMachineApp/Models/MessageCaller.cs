using CashMachineApp.Interfaces;
using System.IO;
using System.Windows;

namespace CashMachineApp.Models
{
    class MessageCaller : IMessageService
    {
        /// <summary>
        /// Метод вызывающий окно сообщения с информацией об ошибке
        /// </summary>
        /// <param name="window">текущее окно</param>
        /// <param name="message">сообщение</param>
        public void ShowMessageFromFile(Window window, string filePath)
        {
            MessageBox.Show(window,
                File.ReadAllText(filePath),
                window.Title,
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        /// <summary>
        /// Метод вызывающий окно сообщения с информацией об ошибке
        /// </summary>
        /// <param name="window">текущее окно</param>
        /// <param name="message">сообщение</param>
        public void ShowInfoMessage(Window window, string message)
        {
            MessageBox.Show(window,
                message,
                window.Title,
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        /// <summary>
        /// Метод вызывающий окно сообщения с информацией об ошибке
        /// </summary>
        /// <param name="window">текущее окно</param>
        /// <param name="message">сообщение</param>
        public void ShowErrorMessage(Window window, string message)
        {
            MessageBox.Show(window,
                message,
                window.Title,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
