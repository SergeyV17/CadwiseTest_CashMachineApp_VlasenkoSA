using CashMachineApp.Interfaces;
using CashMachineApp.Models.Factories;
using System.Collections.Generic;
using System.Linq;

namespace CashMachineApp.Models
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
        public bool DefaultWithdraw(CashMachine cashMachine)    
        {
            // Временный список банкнот
            List<Banknote> tempListOfBanknotes = BanknotesFactory.GetBanknotesByAmounOfCash(WithdrawAmount);

            // Проверка есть ли столько купюр в банкомате
            if (cashMachine.CurrentCountOfBanknotes - tempListOfBanknotes.Count > 0)
            {
                // Выдача средств
                foreach (var banknote in tempListOfBanknotes)
                    cashMachine.RemoveBanknote(banknote);

                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Выдача средств с разменом по выбранному номиналу 
        /// </summary>
        /// <param name="cashMachine">банкомат</param>
        /// <param name="banknoteDenomination">номинал</param>
        /// <returns>призак успешной операции</returns>
        public bool SelectedDenominationWithdraw(CashMachine cashMachine, int banknoteDenomination)
        {
            // Общая сумма наличных имеющихся банкот выбранного номинала
            int amountOfCurrentBanknotes = banknoteDenomination * cashMachine.Banknotes.Count(banknote => banknote.Denomination == banknoteDenomination);

            // Если общая сумма наличных меньше чем заданная сумма выдачи, то выдать заданную сумму невозможно
            if (amountOfCurrentBanknotes < WithdrawAmount)
                return true;
            else
            {
                List<Banknote> listOfBanknotesToRemove = new List<Banknote>();

                // Цикл добавления банкнот в временную коллекцию
                foreach (var banknote in cashMachine.Banknotes)
                {
                    // Если сумма выдачи стала равна 0, выйти из цикла
                    if (WithdrawAmount == 0)
                        break;

                    if (banknote.Denomination == banknoteDenomination)
                    {
                        listOfBanknotesToRemove.Add(banknote);
                        WithdrawAmount -= banknote.Denomination;
                    }
                }

                // Выдача средств
                foreach (var banknote in listOfBanknotesToRemove)
                    cashMachine.RemoveBanknote(banknote);

                // Возврат банкомата к режиму выдачи средств по умолчанию
                cashMachine.IsDefaultWithdraw = true;

                return false;
            }
        }
    }
}
