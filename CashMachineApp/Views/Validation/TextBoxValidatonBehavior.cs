using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace CashMachineApp.Views.Validation
{
    /// <summary>
    /// Класс валидации "MinWordTxtBx"
    /// </summary>
    public class TextBoxValidationBehavior : Behavior<TextBox>
    {
        /// <summary>
        /// Метод подписки на события при начале взаимодействия пользователем с TextBox
        /// </summary>
        protected override void OnAttached()
        {
            // Подписка на события обработки ввода в текстбокс
            AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
            AssociatedObject.PreviewTextInput += AssociatedObject_PreviewTextInput;
        }

        /// <summary>
        /// Метод обработки символов при наборе текста в тексбокс
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">аргументы</param>
        void AssociatedObject_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9]+"); // регулярное выражение позволяющее вводить только цифры
            e.Handled = !(regex.IsMatch(e.Text)); // проверка на соответствие регулярному выражению
        }

        /// <summary>
        /// Метод обработки нажатия клавиши на клавиатуре при наборе текста в тексбокс
        /// </summary>
        /// <param name="sender">отправитель</param>
        /// <param name="e">аргументы</param>
        void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) // обработка клавиши "Пробел"
                e.Handled = true;
        }

        /// <summary>
        /// Метод отписки от событий при окончании взаимодействия пользователем с TextBox
        /// </summary>
        protected override void OnDetaching()
        {
            // Отписка от событий обработки ввода в текстбокс
            AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
            AssociatedObject.PreviewTextInput -= AssociatedObject_PreviewTextInput;
        }
    }
}
