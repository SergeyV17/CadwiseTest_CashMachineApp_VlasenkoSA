namespace CashMachineApp.Models.Abstractions
{
    /// <summary>
    /// Интерфейс открытия файла с информацией
    /// </summary>
    interface IInfoFileService
    {
        string FilePath { get; set; } // путь к файлу

        /// <summary>
        /// Метод получающий путь к файлу с информацией
        /// </summary>
        void GetPath(string filePath);
    }
}
