using System;
using System.Globalization;
using System.Windows.Data;

namespace CashMachineApp.Views.Converters
{
    /// <summary>
    /// Класс конвертирующий булевое значение в строку для отображения статуса банкомата
    /// </summary>
    class BooleanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool currentState = (bool)value;

            return currentState == true ? "ATM is available" : "ATM is unavailable";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
