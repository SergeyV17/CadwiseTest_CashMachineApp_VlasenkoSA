namespace CashMachineApp.Models
{
    /// <summary>
    /// Класс банкноты
    /// </summary>
    public struct Banknote
    {
        /*Использовал структуру для того чтобы метод Remove для коллекции Banknotes
        искал совпадения в коллекции по значению, а не по ссылке*/

        public string Name { get; set; } // наименование банкноты
        public int Denomination { get; set; } // номинал
    }
}
