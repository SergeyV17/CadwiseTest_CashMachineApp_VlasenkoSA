using System;

namespace CashMachineApp.Models.Implementations
{
    /// <summary>
    /// Структура банкноты
    /// </summary>
    public readonly struct Banknote : IComparable<Banknote>
    {
        public string Name { get; }
        public int Denomination { get; }

        public Banknote(string Name, int Denomination)
        {
            this.Name = Name;
            this.Denomination = Denomination;
        }

        /// <summary>
        /// Реализация интерфейса IComparable<T>
        /// </summary>
        /// <param name="other">банкнота</param>
        /// <returns>результат сравнения</returns>
        public int CompareTo(Banknote other)
        {
            return this.Denomination.CompareTo(other.Denomination);
        }
    }
}
