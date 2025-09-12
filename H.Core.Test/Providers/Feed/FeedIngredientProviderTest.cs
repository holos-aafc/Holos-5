        #region Imports

using System.Linq;
using H.Core.Enumerations;
using H.Core.Providers.Feed;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace H.Core.Test.Providers.Feed {
    [TestClass]
    public class FeedIngredientProviderTest {

        #region Fields

        private FeedIngredientProvider _sut;

        #endregion

        #region Initialization

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _sut = new FeedIngredientProvider();
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        #endregion

        #region Tests

        [TestMethod]
        public void GetFeedDataReturnsCorrectNumberOfRows()
        {
            var result = _sut.GetBeefFeedIngredients();

            Assert.AreEqual(218, result.Count());
        }

        [TestMethod]
        public void GetFeedDataReturnsCorrectlyParseRow()
        {
            var feedData = _sut.GetBeefFeedIngredients();
            var item = feedData.Single(x => x.IngredientType == IngredientType.AlfalfaCubes);

            Assert.AreEqual(100, item.Forage);
            Assert.AreEqual(91, item.DryMatter);
            Assert.AreEqual(18.1, item.CrudeProtein);
            Assert.AreEqual(39.3, item.SP);
            Assert.AreEqual(8.2, item.ADICP);
            Assert.AreEqual(0, item.Sugars);
            Assert.AreEqual(0, item.OA);
            Assert.AreEqual(2.1, item.Fat);
            Assert.AreEqual(12, item.Ash);
            Assert.AreEqual(1.3, item.Starch);
            Assert.AreEqual(45.5, item.NDF);
            Assert.AreEqual(7.6, item.Lignin);
            Assert.AreEqual(56, item.TotalDigestibleNutrient);
            Assert.AreEqual(2, item.ME);
            Assert.AreEqual(1.2, item.NEma);
            Assert.AreEqual(0.6, item.NEga);
            Assert.AreEqual(31, item.RUP);
            Assert.AreEqual(5.1, item.kdPB);
            Assert.AreEqual(30, item.KdCB1);
            Assert.AreEqual(30, item.KdCB2);
            Assert.AreEqual(5.5, item.KdCB3);
            Assert.AreEqual(49.3, item.PBID);
            Assert.AreEqual(75, item.CB1ID);
            Assert.AreEqual(75, item.CB2ID);
            Assert.AreEqual(92, item.Pef);
            Assert.AreEqual(1.16, item.ARG);
            Assert.AreEqual(0.47, item.HIS);
            Assert.AreEqual(1.09, item.ILE);
            Assert.AreEqual(1.67, item.LEU);
            Assert.AreEqual(1.09, item.LYS);
            Assert.AreEqual(0.13, item.MET);
            Assert.AreEqual(0, item.CYS);
            Assert.AreEqual(1.14, item.PHE);
            Assert.AreEqual(0, item.TYR);
            Assert.AreEqual(0.9, item.THR);
            Assert.AreEqual(0.33, item.TRP);
            Assert.AreEqual(1.29, item.VAL);
            Assert.AreEqual(1.49, item.Ca);
            Assert.AreEqual(0.28, item.P);
            Assert.AreEqual(0.28, item.Mg);
            Assert.AreEqual(0.7, item.Cl);
            Assert.AreEqual(2.05, item.K);
            Assert.AreEqual(0.16, item.Na);
            Assert.AreEqual(0.25, item.S);
            Assert.AreEqual(0.77, item.Co);
            Assert.AreEqual(8.54, item.Cu);
            Assert.AreEqual(0, item.I);
            Assert.AreEqual(648.5, item.Fe);
            Assert.AreEqual(44.1, item.Mn);
            Assert.AreEqual(0.8, item.Se);
            Assert.AreEqual(24.3, item.Zn);
            Assert.AreEqual(19.3, item.VitA);
            Assert.AreEqual(1, item.VitD);
            Assert.AreEqual(0, item.VitE);
        }

        [TestMethod]
        public void GetDairyFeedIngredientsReturnsCorrectlyParseRow()
        {
            var feedData = _sut.GetDairyFeedIngredients();
            var item = feedData.Single(x => x.IngredientType == IngredientType.AlfalfaMedicago);
            Assert.AreEqual(56.4, item.TotalDigestibleNutrient);
            Assert.AreEqual(DairyFeedClassType.Forage, item.DairyFeedClass);
            Assert.AreEqual(2.6, item.DE);
            Assert.AreEqual(1.96, item.ME);
            Assert.AreEqual(1.19, item.NEL_ThreeX);
            Assert.AreEqual(1.11, item.NEL_FourX);
            Assert.AreEqual(1.27, item.NEM);
            Assert.AreEqual(0.7, item.NEG);
            Assert.AreEqual(90.3, item.DryMatter);
            Assert.AreEqual(19.2, item.CrudeProtein);
            Assert.AreEqual(3.1, item.NDICP);
            Assert.AreEqual(2.4, item.ADICP);
            Assert.AreEqual(2.5, item.EE);
            Assert.AreEqual(41.6, item.NDF);
            Assert.AreEqual(32.8, item.ADF);
            Assert.AreEqual(7.6, item.Lignin);
            Assert.AreEqual(11, item.Ash);
        }

        [TestMethod]
        public void GetSwineFeedIngredientsReturnsCorrectlyParseRow()
        {
            var feedData = _sut.GetSwineFeedIngredients();
            var item = feedData.Single(x => x.IngredientType == IngredientType.AlfalfaHay);
            var item1 = feedData.Single(x => x.IngredientType == IngredientType.YeastTorula);
            var item2 = feedData.Single(x => x.IngredientType == IngredientType.WheatShorts);

            Assert.AreEqual(77, item2.IleDigestAID);
            Assert.AreEqual("96.7", item1.AAFCO);
            Assert.AreEqual("3.1", item.AAFCO);
            Assert.AreEqual("P324", item.AAFCO2010);
            Assert.AreEqual("1-30-293", item.IFN);
            Assert.AreEqual(90.33, item.DryMatter);
            Assert.AreEqual(19.32, item.CrudeProtein);
            Assert.AreEqual(2.3, item.EE);
            Assert.AreEqual(11, item.Ash);
            Assert.AreEqual(4077, item.GrossEnergy);
            Assert.AreEqual(1830, item.DeSwine);
            Assert.AreEqual(1699, item.ME);
            Assert.AreEqual(878, item.NE);
            Assert.AreEqual(1.02, item.Starch);
            Assert.AreEqual(37, item.NDF);
            Assert.AreEqual(31.01, item.ADF);
            Assert.AreEqual(6.65, item.ADL);
            Assert.AreEqual(1.46, item.Ca);
            Assert.AreEqual(2.48, item.K);
            Assert.AreEqual(0, item.CrudeFiber);
            Assert.AreEqual(0, item.Lactose);
            Assert.AreEqual(0, item.IVP);
        }

        [TestMethod]
        public void GetSwineDataReturnsCorrectNumberOfRows()
        {
            var data = _sut.GetSwineFeedIngredients();
            Assert.AreEqual(123, data.Count());
        }

        [TestMethod]
        public void GetIngredientsForDiet_BeefCowLowEnergyAndProtein_ReturnsNativePrairieHay()
        {
            var result = _sut.GetIngredientsForDiet(AnimalType.BeefCow, DietType.LowEnergyAndProtein);
            Assert.AreEqual(1, result.Count);
            var ingredient = result.First();
            Assert.AreEqual(IngredientType.NativePrairieHay, ((FeedIngredient)ingredient).IngredientType);
            Assert.AreEqual(100, ingredient.PercentageInDiet);
        }

        [TestMethod]
        public void GetIngredientsForDiet_BeefCowMediumEnergyAndProtein_ReturnsExpectedIngredients()
        {
            var result = _sut.GetIngredientsForDiet(AnimalType.BeefCow, DietType.MediumEnergyAndProtein).ToList();
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(IngredientType.AlfalfaHay, ((FeedIngredient)result[0]).IngredientType);
            Assert.AreEqual(22, result[0].PercentageInDiet);
            Assert.AreEqual(IngredientType.MeadowHay, ((FeedIngredient)result[1]).IngredientType);
            Assert.AreEqual(65, result[1].PercentageInDiet);
            Assert.AreEqual(IngredientType.BarleyGrain, ((FeedIngredient)result[2]).IngredientType);
            Assert.AreEqual(3, result[2].PercentageInDiet);
        }

        [TestMethod]
        public void GetIngredientsForDiet_BeefCowHighEnergyAndProtein_ReturnsExpectedIngredients()
        {
            var result = _sut.GetIngredientsForDiet(AnimalType.BeefCow, DietType.HighEnergyAndProtein).ToList();
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(IngredientType.OrchardgrassHay, ((FeedIngredient)result[0]).IngredientType);
            Assert.AreEqual(60, result[0].PercentageInDiet);
            Assert.AreEqual(IngredientType.AlfalfaHay, ((FeedIngredient)result[1]).IngredientType);
            Assert.AreEqual(20, result[1].PercentageInDiet);
            Assert.AreEqual(IngredientType.BarleyGrain, ((FeedIngredient)result[2]).IngredientType);
            Assert.AreEqual(20, result[2].PercentageInDiet);
        }

        [TestMethod]
        public void GetIngredientsForDiet_InvalidAnimalType_ThrowsArgumentOutOfRangeException()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                _sut.GetIngredientsForDiet(AnimalType.Sheep, DietType.LowEnergyAndProtein));
        }

        [TestMethod]
        public void GetIngredientsForDiet_InvalidDietType_ThrowsArgumentOutOfRangeException()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                _sut.GetIngredientsForDiet(AnimalType.BeefCow, (DietType)999));
        }

        [TestMethod]
        public void CopyIngredient_CopiesValuesAndSetsPercentage()
        {
            var original = _sut.GetBeefFeedIngredients().First();
            var percentage = 42.5;
            var copy = _sut.CopyIngredient(original, percentage);

            Assert.AreEqual(original.IngredientType, copy.IngredientType);
            Assert.AreEqual(percentage, copy.PercentageInDiet);
            Assert.AreNotSame(original, copy);
        }

        #endregion
    }
}