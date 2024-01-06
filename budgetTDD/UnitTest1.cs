using BudgtService;
using NSubstitute;

namespace budgetTDD
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void EndBiggerThanStart()
        {
            var budgetRepo = NSubstitute.Substitute.For<IBudgetRepo>();
            var budgetService = new BudgetService(budgetRepo);
            budgetRepo.GetAll().Returns(new List<Budget>
            {
                new Budget() { YearMonth = "202301", Amount = 31 * 1 },
                new Budget() { YearMonth = "202302", Amount = 28 * 10 },
                new Budget() { YearMonth = "202303", Amount = 30 * 100 },
            });

            var start = DateTime.Parse("2023-04-01");
            var end = DateTime.Parse("2023-01-01");
            Assert.AreEqual(0, budgetService.query(start, end));
        }
    }
}