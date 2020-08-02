using System;
using System.Collections.Generic;
using System.Linq;
using CashMachineApp.Models.Abstractions;
using static CashMachineApp.Models.Implementations.Factories.BanknoteFactory;

namespace CashMachineApp.Models.Implementations
{
    /// <summary>
    /// Класс реализации логики выдачи средств
    /// </summary>
    class WithdrawCash : IWithdrawCashService
    {
        public int WithdrawAmount { get; set; } // сумма выдачи
        public int BanknoteDenomination { get; set; } // номинал банкноты для размена

        /// <summary>
        /// // Метод проверки на корректную выдачу (заданная сумма должна быть кратна выбранному номиналу)
        /// </summary>
        /// <param name="banknoteDenomination">номинал банкноты</param>
        /// <returns>признак успешной проверки</returns>
        public bool CheckForCorrectWithdraw(int banknoteDenomination)
        {
            BanknoteDenomination = banknoteDenomination;

            return WithdrawAmount % BanknoteDenomination == 0 ? true : false;
        }

        /// <summary>
        /// Метод проверяющий кратность заданной суммы на 10 (проверка на мелочь)
        /// </summary>
        /// <param name="textBoxAmountValue">заданная сумма</param>
        /// <returns>признак успешной проверки</returns>
        public bool CheckForTrifles(string textBoxAmountValue)
        {
            WithdrawAmount = int.Parse(textBoxAmountValue);

            return WithdrawAmount % 10 != 0 ? false : true;
        }

        /// <summary>
        /// Выдача средств по умолчанию (без размена на выбранный номинал)
        /// </summary>
        /// <param name="cashMachine">банкомат</param>
        /// <returns>признак успешной операции</returns>
        public bool DefaultWithdraw(ICashMachine cashMachine)    
        {
            IList<Banknote> tempBanknotes = CalculateBanknotes.WithdrawBanknotesByAmounOfCash(
                WithdrawAmount, 
                cashMachine.BanknotesCountOfEachType, 
                out int[] banknotesAmountClone);

            if (tempBanknotes != null)
            {
                // Выдача средств
                foreach (var banknote in tempBanknotes)
                    cashMachine.RemoveBanknote(banknote);

                // Замена массива с количеством банкнот каждого номинала на обработанный массив после операции выдачи
                Array.Copy(banknotesAmountClone, cashMachine.BanknotesCountOfEachType, BanknoteTypes.Length);

                return true;
            }

                return false;
        }

        /// <summary>
        /// Выдача средств с разменом по выбранному номиналу 
        /// </summary>
        /// <param name="cashMachine">банкомат</param>
        /// <param name="banknoteDenomination">номинал</param>
        /// <returns>призак успешной операции</returns>
        public bool SelectedDenominationWithdraw(ICashMachine cashMachine, int banknoteDenomination)
        {
            // Общая сумма наличных имеющихся банкот выбранного номинала
            int amountOfCurrentBanknotes = banknoteDenomination * cashMachine.Banknotes.Count(banknote => banknote.Denomination == banknoteDenomination);

            // Если заданная сумма меньше общей суммы наличных выбранного номинала, произвести выдачу
            if (WithdrawAmount <= amountOfCurrentBanknotes)
            {
                List<Banknote> listOfBanknotesToRemove = new List<Banknote>();

                // Цикл добавления банкнот в временную коллекцию
                foreach (var banknote in cashMachine.Banknotes)
                {
                    // Если сумма выдачи равна 0, выйти из цикла
                    if (WithdrawAmount == 0)
                        break;

                    if (banknote.Denomination == banknoteDenomination)
                    {
                        listOfBanknotesToRemove.Add(banknote);
                        WithdrawAmount -= banknote.Denomination;
                    }
                }

                int index = Array.IndexOf(BanknoteTypes, listOfBanknotesToRemove[0]);

                // Выдача средств
                foreach (var banknote in listOfBanknotesToRemove)
                {
                    cashMachine.RemoveBanknote(banknote);
                    cashMachine.BanknotesCountOfEachType[index]--;
                }

                return true;
            }

            return false;
        }
    }
}
