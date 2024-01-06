namespace BudgtService
{
    public class BudgetService
    {
        private IBudgetRepo _budgetRepo;

        public BudgetService(IBudgetRepo budgetRepo)
        {

        }

        public decimal query(DateTime start, DateTime end)
        {
            if (start > end)
            {
                return 0;
            }

            var budgets = _budgetRepo.GetAll();

            if (start == end)
            {
                var yearMonth = start.ToString("yyyyMM");
                var monthlyAmount = budgets.FirstOrDefault(b => b.YearMonth == yearMonth)?.Amount ?? 0;
                return monthlyAmount / DateTime.DaysInMonth(start.Year, start.Month);
            }

            var dateTimeBudgets = budgets.Select(b => new
            {
                Date = DateTime.ParseExact(b.YearMonth, "yyyyMM", null, System.Globalization.DateTimeStyles.None),
                b.Amount
            }).ToList();

            var matchBudgets = dateTimeBudgets
                .Where(b => b.Date >= new DateTime(start.Year, start.Month, 1))
                .Where(b => b.Date <= end);

            if (!matchBudgets.Any())
            {
                return 0;
            }

            var amount = 0m;
            for (var i = start; i < end.AddMonths(1); i.AddMonths(1))
            {
                var budget =
                    matchBudgets.FirstOrDefault(b => b.Date.ToString("yyyyMM") == i.ToString("yyyyMM"))?.Amount ?? 0;

                //start
                if (i.ToString("yyyyMM") == start.ToString("yyyyMM"))
                {
                    if (start.ToString("yyyyMM") == end.ToString("yyyyMM"))
                    {
                        amount += (end.Day - i.Day) /
                            DateTime.DaysInMonth(i.Date.Year, i.Date.Month) * budget;
                    }
                    else
                    {
                        amount += (DateTime.DaysInMonth(i.Date.Year, i.Date.Month) - i.Day) /
                            DateTime.DaysInMonth(i.Date.Year, i.Date.Month) * budget;
                    }
                }
                //end
                else if (i.ToString("yyyyMM") == end.ToString("yyyyMM"))
                {
                    amount += (DateTime.DaysInMonth(i.Date.Year, i.Date.Month) - i.Day) /
                        DateTime.DaysInMonth(i.Date.Year, i.Date.Month) * budget;
                }
                else
                {
                    amount += budget;
                }
            }
            return amount;
        }
    }
}