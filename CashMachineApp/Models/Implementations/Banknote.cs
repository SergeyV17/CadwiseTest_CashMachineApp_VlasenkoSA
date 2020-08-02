namespace CashMachineApp.Models.Implementations
{
    /// <summary>
    /// Структура банкноты
    /// </summary>
    public readonly struct Banknote
    {
        public string Name { get; }
        public int Denomination { get; }

        public Banknote(string Name, int Denomination)
        {
            this.Name = Name;
            this.Denomination = Denomination;
        }
    }
}
