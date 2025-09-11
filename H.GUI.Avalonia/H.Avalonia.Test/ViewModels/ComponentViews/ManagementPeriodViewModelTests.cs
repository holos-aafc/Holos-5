using H.Avalonia.ViewModels.ComponentViews;

namespace H.Avalonia.Test.ViewModels.ComponentViews
{
    [TestClass]
    public class ManagementPeriodViewModelTests
    {
        private ManagementPeriodViewModel _viewModel;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new ManagementPeriodViewModel();
            _viewModel.StartDate = new DateTime(2000, 01, 01);
            _viewModel.EndDate = new DateTime(2010, 01, 01);
            _viewModel.PeriodName = "Test Period";
            _viewModel.NumberOfDays = 365;
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }


        [TestMethod]
        public void TestConstructor()
        {
            Assert.IsNotNull(_viewModel);
        }

        [TestMethod]
        public void TestValidatePeriodName()
        {
            Assert.IsFalse(_viewModel.HasErrors);

            _viewModel.PeriodName = "";
            
            Assert.IsTrue(_viewModel.HasErrors);
            var errors = _viewModel.GetErrors(nameof(_viewModel.PeriodName)) as IEnumerable<string>;
            Assert.IsNotNull(errors);
            Assert.AreEqual("Name cannot be empty.", errors.ToList()[0]);
        }

        [TestMethod]
        public void TestValidateStartDate()
        {
            Assert.IsFalse(_viewModel.HasErrors);

            _viewModel.StartDate = new DateTime(2020, 01, 01);
            Assert.IsTrue(_viewModel.HasErrors);

            var errors = _viewModel.GetErrors(nameof(_viewModel.StartDate)) as IEnumerable<string>;
            Assert.IsNotNull(errors);
            Assert.AreEqual("Must be a valid date before the End Date.", errors.ToList()[0]);
        }

        [TestMethod]
        public void TestValidateEndDate()
        {
            Assert.IsFalse(_viewModel.HasErrors);

            _viewModel.EndDate = new DateTime(1998, 02, 08);
            Assert.IsTrue(_viewModel.HasErrors);

            var errors = _viewModel.GetErrors(nameof(_viewModel.EndDate)) as IEnumerable<string>;
            Assert.IsNotNull(errors);
            Assert.AreEqual("Must be a valid date later than the Start Date.", errors.ToList()[0]);
        }

        [TestMethod]
        public void TestValidateNumberOfDays()
        {
            Assert.IsFalse(_viewModel.HasErrors);

            _viewModel.NumberOfDays = -1;
            Assert.IsTrue(_viewModel.HasErrors);

            var errors = _viewModel.GetErrors(nameof(_viewModel.NumberOfDays)) as IEnumerable<string>;
            Assert.IsNotNull (errors);
            Assert.AreEqual("Must be greater than 0.", errors.ToList()[0]);
        }
    }
}