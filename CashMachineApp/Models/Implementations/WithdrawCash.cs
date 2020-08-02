using System.Collections.Generic;
using System.Linq;
using CashMachineApp.Models.Abstractions;
using CashMachineApp.Models.Implementations.Factories;

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
            if (cashMachine.Balance < WithdrawAmount)
                return true;

            // Имеющиеся в банкомате номиналы банкнот
            IList<Banknote> availableDenominations = GetAvailableDenominations(cashMachine.Banknotes);

            IList<Banknote> tempBanknotes = BanknoteFactory.GetBanknotesByAmounOfCash(cashMachine.Banknotes.ToList(), WithdrawAmount, availableDenominations);

            if (tempBanknotes != null)
            {
                // Выдача средств
                foreach (var banknote in tempBanknotes)
                    cashMachine.RemoveBanknote(banknote);

                return false;
            }

                return true;

            // 500 100 100 100
            // Снять 400
            // Работаем с отфильтрованным списком и где банкноты меньше или равны 400
            // Если сумма всех банкнот больше чем сумма выдачи, то выдаем, иначе не хватает средств

            // 500 500 500
            // 600
            // 
        }

        private IList<Banknote> GetAvailableDenominations(IReadOnlyCollection<Banknote> currentBanknotes)
        {
            IList<Banknote> filteredBanknotes = new List<Banknote>();

            foreach (var banknote in currentBanknotes)
            {
                if (!filteredBanknotes.Contains(banknote))
                    filteredBanknotes.Add(banknote);

                // Если в коллекции пристуствуют все имеющиеся номиналы, прервать цикл
                if (filteredBanknotes.Count == BanknoteFactory.NumbersOfBanknoteTypes)
                    break;
            }

            return filteredBanknotes.OrderByDescending(d => d).ToList();
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
                //cashMachine.IsDefaultWithdraw = true;

                return false;
            }
        }
    }
}
