using System;
using System.IO;
using CashMachineApp.Models.Abstractions;

namespace CashMachineApp.Models.Implementations
{
    /// <summary>
    /// Класс считывающий текст из файла с информацией
    /// </summary>
    public class InfoFileReader : IInfoFileService
    {
        public string FilePath { get; set; } // путь к файлу

        /// <summary>
        /// Метод получающий путь к файлу с информацией
        /// </summary>
        /// <param name="filePath">путь к файлу</param>
        public void GetPath(string filePath)
        {
            FilePath = Path.Combine(Path.GetDirectoryName(Directory.GetParent(Environment.CurrentDirectory).ToString()), filePath);
        }
    }
}
