#region Imports

using System.Collections.ObjectModel;
using H.Core.Calculators.Carbon;
using H.Core.Enumerations;
using H.Core.Models;
using H.Core.Models.LandManagement.Fields;
using H.Core.Providers;
using H.Core.Providers.Animals;
using H.Core.Providers.Climate;
using H.Core.Providers.Evapotranspiration;
using H.Core.Providers.Precipitation;
using H.Core.Providers.Soil;

#nullable disable

#endregion

namespace H.Core.Test.Calculators
{
    [TestClass]
    public class ICBMSoilCarbonCalculatorTest : UnitTestBase
    {
        #region Fields

        private ICBMSoilCarbonCalculator _sut;

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
            var iCBMSoilCarbonCalculator = new ICBMSoilCarbonCalculator(base._climateProvider, base._n2OEmissionFactorCalculator);
            
            

            _sut = iCBMSoilCarbonCalculator;
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        #endregion

        #region Tests

        #region CalculateManureCarbonInput

        [TestMethod]
        public void CalculateManureCarbonInputReturnsZeroWhenThereAreNoManureApplicationViewItems()
        {
            var cropViewItem = new CropViewItem()
            {
                ManureApplicationViewItems = new ObservableCollection<ManureApplicationViewItem>()
                {

                }
            };

            var result = _sut.CalculateManureCarbonInputPerHectare(cropViewItem);

            Assert.AreEqual(0, result);
        }


        [TestMethod]
        public void CalculateManureCarbonInputReturnsNonZeroWhenThereAreManureApplicationViewItems()
        {
            var manureComposition = new DefaultManureCompositionData()
            {
                ManureStateType = ManureStateType.Composted,
                CarbonFraction = 25,
                AnimalType = AnimalType.Beef
            };

            var cropViewItem = new CropViewItem()
            {
                Year = DateTime.Now.Year,
                ManureApplicationViewItems = new ObservableCollection<ManureApplicationViewItem>()
                {
                    new ManureApplicationViewItem()
                    {
                        ManureStateType = ManureStateType.Composted,
                        ManureAnimalSourceType = ManureAnimalSourceTypes.BeefManure,
                        ManureLocationSourceType = ManureLocationSourceType.Livestock,
                        AnimalType = AnimalType.Beef,
                        DateOfApplication = DateTime.Now,
                        DefaultManureCompositionData = manureComposition,
                        AmountOfManureAppliedPerHectare = 100,
                    }
                }
            };

            var farm = new Farm()
            {
                DefaultManureCompositionData = new ObservableCollection<DefaultManureCompositionData>()
                {
                   manureComposition,
                }
            };

            var result = _sut.CalculateManureCarbonInputPerHectare(cropViewItem);

            Assert.AreEqual(25, result);
        }

        #endregion

        /// <summary>
        /// Equation 2.2.2-27
        /// </summary>
        [TestMethod]
        public void CalculateAmountOfNitrogenAppliedFromManure()
        {
            var result = _sut.CalculateAmountOfNitrogenAppliedFromManure(0.234, 0.121);
            Assert.AreEqual(0.028314, result, 0.000001);
        }

        /// <summary>
        /// Equation 2.2.2-28
        /// </summary>
        [TestMethod]
        public void CalculateAmountOfPhosphorusAppliedFromManure()
        {
            var result = _sut.CalculateAmountOfPhosphorusAppliedFromManure(1.234, 123.231);
            Assert.AreEqual(152.067054, result, 0.00001);
        }

        /// <summary>
        /// Equation 2.2.2-29
        /// </summary>
        [TestMethod]
        public void CalculateMoistureOfManure()
        {
            var result = _sut.CalculateMoistureOfManure(1.234, 123.231);
            Assert.AreEqual(0.0152067054, result);
        }

        /// <summary>
        /// Equation 2.2.3-1
        /// </summary>
        [TestMethod]
        public void CalculateAverageAboveGroundResidueCarbonInput()
        {
            var result = _sut.CalculateAverageAboveGroundResidueCarbonInput(0.12, 0.442);
            Assert.AreEqual(0.562, result);
        }

        /// <summary>
        /// Equation 2.2.3-2
        /// </summary>
        [TestMethod]
        public void CalculateAverageBelowGroundResidueCarbonInput()
        {
            var result = _sut.CalculateAverageBelowGroundResidueCarbonInput(0.234, 3.4234);
            Assert.AreEqual(3.6574, result);
        }

        /// <summary>
        /// Equation 2.2.3-3
        /// </summary>
        [TestMethod]
        public void CalculateAverageManureCarbonInput()
        {
            var result = _sut.CalculateAverageManureCarbonInput(1243.35434532);
            Assert.AreEqual(1243.35434532, result);
        }

        /// <summary>
        /// Equation 2.2.3-4
        /// </summary>
        [TestMethod]
        public void CalculateAboveGroundSteadyState()
        {
            var result = _sut.CalculateYoungPoolSteadyStateAboveGround(1, 2, 1);
            Assert.AreEqual(0.15651764274966565181808062346542, result, 0.00000001);
        }

        /// <summary>
        /// Equation 2.2.3-5
        /// </summary>
        [TestMethod]
        public void CalculateBelowGroundSteadyState()
        {
            var result = _sut.CalculateYoungPoolSteadyStateBelowGround(1, 3, 3);
            Assert.AreEqual(1.2342503594618505957689934913731e-4, result, 0.00000001);
        }

        /// <summary>
        /// Equation 2.2.3-6
        /// </summary>
        [TestMethod]
        public void CalculateYoungPoolSteadyStateManure()
        {
            var result = _sut.CalculateYoungPoolSteadyStateManure(1, 2, 2);
            Assert.AreEqual(0.01865736036377404793890488238391, result);
        }

        /// <summary>
        /// Equation 2.2.3-7
        /// </summary>
        [TestMethod]
        public void CalculateOldPoolSteadyState()
        {
            var youngPoolDecompostionRate = 1.0;
            var oldPoolDecompositionRate = 2.0;
            var climateParamter = 3.0;
            var aboveGroundHumification = 1.0;
            var belowGroundHumification = 2.0;
            var aboveGroundCarbonInput = 1.0;
            var belowGroundCarbonInput = 2.0;
            var manureSteadyState = 1.0;
            var manureCarbonInput = 2.0;
            var manureHumification = 3.0;

            var youngPoolSteadyStateAboveGround = 1.0; //_sut.CalculateYoungPoolSteadyStateAboveGround(aboveGroundCarbonInput, youngPoolDecompostionRate, climateParamter);
            var youngPoolSteadyStateBelowGround = 2.0;//_sut.CalculateYoungPoolSteadyStateBelowGround(belowGroundCarbonInput, youngPoolDecompostionRate, climateParamter);

            var result = _sut.CalculateOldPoolSteadyState(youngPoolDecompostionRate,
                                                          oldPoolDecompositionRate,
                                                          climateParamter,
                                                          aboveGroundHumification,
                                                          belowGroundHumification,
                                                          aboveGroundCarbonInput,
                                                          belowGroundCarbonInput,
                                                          youngPoolSteadyStateAboveGround,
                                                          youngPoolSteadyStateBelowGround,
                                                          manureSteadyState,
                                                          manureCarbonInput,
                                                          manureHumification);
            Assert.AreEqual(0.90109159037376883669811488366329, result, 0.00000000001);
        }

        /// <summary>
        /// Equation 2.2.4-1
        /// </summary>
        [TestMethod]
        public void CalculateYoungPoolAboveGroundCarbonAtInterval()
        {
            var result = _sut.CalculateYoungPoolAboveGroundCarbonAtInterval(0.232, 3.432, 0.532, 5.767);
            Assert.AreEqual(0.17042012737096903330645093846963, result, 0.0000001);
        }

        /// <summary>
        /// Equation 2.2.4-2
        /// </summary>
        [TestMethod]
        public void CalculateYoungPoolBelowGroundCarbonAtInterval()
        {
            var result = _sut.CalculateYoungPoolBelowGroundCarbonAtInterval(2.546, 2.7543, 0.435, 32.3);
            Assert.AreEqual(4.1903069151643476599675250028938e-6, result, 0.0000001);
        }

        /// <summary>
        /// Equation 2.2.4-3
        /// </summary>
        [TestMethod]
        public void CalculateYoungPoolManureCarbonAtInterval()
        {
            var result = _sut.CalculateYoungPoolManureCarbonAtInterval(0.32, 0.2342, 45.53, 0.65);
            Assert.AreEqual(7.7792634024786830648485547714321e-14, result, 0.0000001);
        }
        /// <summary>
        /// Equation 2.2.4-4
        /// </summary>
        [TestMethod]
        public void CalculateOldPoolSoilCarbonAtInterval()
        {
            var result = _sut.CalculateOldPoolSoilCarbonAtInterval(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);
            Assert.AreEqual(6.1853337707258386287097434158479e-15, result, 0.0000000001);
        }
        /// <summary>
        /// Equation 2.2.4-5
        /// </summary>
        [TestMethod]
        public void CalculateSoilCarbonAtInterval()
        {
            var result = _sut.CalculateSoilCarbonAtInterval(0.32, 21.03, 42.34, 0.321);
            Assert.AreEqual(64.011, result, 0.001);
        }

        /// <summary>
        /// Equation 2.2.4-6
        /// </summary>
        [TestMethod]
        public void CalculateChangeInSoilCarbonAtInterval()
        {
            var result = _sut.CalculateChangeInSoilCarbonAtInterval(0.446, 0.853);
            Assert.AreEqual(-0.407, result);
        }

        /// <summary>
        /// Equation 2.2.4-7
        /// </summary>
        [TestMethod]
        public void CalculateChangeInSoilOrganicCarbonForFieldAtInterval()
        {
            var result = _sut.CalculateChangeInSoilOrganicCarbonForFieldAtInterval(123.34, 0.5679);
            Assert.AreEqual(70.044786, result);
        }

        /// <summary>
        /// Equation 2.2.4-8
        /// </summary>
        [TestMethod]
        public void CalculateChangeInSoilOrganicCarbonForFarmAtInterval()
        {
            var result = _sut.CalculateChangeInSoilOrganicCarbonForFarmAtInterval(
                new System.Collections.Generic.List<double>
                {
                    0.24,
                    12.42,
                    0.56
                });
            Assert.AreEqual(13.22, result);
        }

        /// <summary>
        /// Equation 2.2.5-1
        /// </summary>
        [TestMethod]
        public void CalculateCarbonDioxideEquivalentsForSoil()
        {
            var result = _sut.CalculateCarbonDioxideEquivalentsForSoil(47.654564);
            Assert.AreEqual(174.73340133333333333333333333333, result, 0.00000001);
        }

        /// <summary>
        /// Equation 2.2.5-2
        /// </summary>
        [TestMethod]
        public void CalculateChangeInCarbonDioxideEquivalentsForSoil()
        {
            var result = _sut.CalculateChangeInCarbonDioxideEquivalentsForSoil(0.332542);
            Assert.AreEqual(1.2193206666666666666666666666667, result);
        }

        /// <summary>
        /// Equation 2.2.5-3
        /// </summary>
        [TestMethod]
        public void CalculateCarbonDioxideChangeForSoilsByMonth()
        {
            var result = _sut.CalculateCarbonDioxideChangeForSoilsByMonth(0.0032);
            Assert.AreEqual(2.6666666666666666666666666666667e-4, result);
        }

        #region CalculateInputsFromSupplementalHayFedToGrazingAnimals

        [TestMethod]
        public void CalculateInputsFromSupplementalHayFedToGrazingAnimals()
        {
            var farm = new Farm()
            {
                Defaults = new Defaults()
                {
                    DefaultSupplementalFeedingLossPercentage = 20,
                },
            };

            var currentYearViewItem = new CropViewItem()
            {
                CarbonConcentration = 0.45,

                // This is a supplemental feeding to grazing animals and the extra carbon left over once animals are finished must be accounted for in total above ground inputs
                HayImportViewItems = new ObservableCollection<HayImportViewItem>()
                {
                    new HayImportViewItem()
                    {
                        NumberOfBales = 10,
                        BaleWeight = 500,
                        MoistureContentAsPercentage = 12,
                    }
                }
            };

            var result = _sut.CalculateInputsFromSupplementalHayFedToGrazingAnimals(
                previousYearViewItem: null,
                currentYearViewItem: currentYearViewItem,
                nextYearViewItems: null,
                farm: farm);

            // = [(10 * 500) * (12/100) * (1 - 20/100)] * 0.45
            // = (5000 * 0.88 * 0.8) * 0.45
            // = 1584

            Assert.AreEqual(396, result);
        }

        #endregion

        #region CalculateCropResiduesAtInterval (Eq. 2.6.4-2)

        // Equation 2.6.4-2 (AAFC Holos v4.0 algorithm document):
        //   N_CropResidues(t)
        //     = (YoungPoolAGresidue_N(t-1) - YoungPoolAGresidue_N(t) - Grain_N(t) - Straw_N(t))
        //     + (YoungPoolBGresidue_N(t-1) - YoungPoolBGresidue_N(t) - Root_N(t)  - Exudate_N(t))
        //   If N_CropResidues(t) < 0, N_CropResidues(t) = 0.
        //
        // These tests pin the formula against the spec so a future "tidy-up" can't silently
        // flip the sign back. The original v5 port had both a sign error on the crop-N term and
        // a year-index error (previous-interval rather than current-interval crop N); the v4
        // sign fix shipped in v4 commit 0188886. See MEMORY.md "Phase 4 follow-up #1" for trail.

        [TestMethod]
        public void CalculateCropResiduesAtInterval_AppliesSpecFormula()
        {
            // AG: pool drops 80 -> 30 (50 kg N ha-1 mineralized from AG pool over the interval),
            //     of which 22 came from the current-year above-ground crop additions.
            // BG: pool drops 60 -> 25 (35), of which 17 came from current-year roots/exudate.
            // Net availability from residue decomposition = (50 - 22) + (35 - 17) = 46.
            var result = _sut.CalculateCropResiduesAtInterval(
                aboveGroundResidueNitrogenForFieldAtCurrentInterval: 30,
                aboveGroundResidueNitrogenForFieldAtPreviousInterval: 80,
                aboveGroundResidueNitrogenForCropAtCurrentInterval: 22,
                belowGroundResidueNitrogenForFieldAtCurrentInterval: 25,
                belowGroundResidueNitrogenForFieldAtPreviousInterval: 60,
                belowGroundResidueNitrogenForCropAtCurrentInterval: 17);

            Assert.AreEqual(46, result, 1e-9);
        }

        [TestMethod]
        public void CalculateCropResiduesAtInterval_ClampsNegativeResultToZero()
        {
            // Pool drop is smaller than the current-year crop-N input (i.e. the field accumulated
            // more residue this year than decomposed). The raw formula yields (50-22-..) net = -4
            // here; the spec clamps to 0 rather than reporting negative mineralization.
            //   AG: (50 - 30) - 22 = -2
            //   BG: (40 - 25) - 17 = -2
            //   raw sum = -4 -> clamps to 0
            var result = _sut.CalculateCropResiduesAtInterval(
                aboveGroundResidueNitrogenForFieldAtCurrentInterval: 30,
                aboveGroundResidueNitrogenForFieldAtPreviousInterval: 50,
                aboveGroundResidueNitrogenForCropAtCurrentInterval: 22,
                belowGroundResidueNitrogenForFieldAtCurrentInterval: 25,
                belowGroundResidueNitrogenForFieldAtPreviousInterval: 40,
                belowGroundResidueNitrogenForCropAtCurrentInterval: 17);

            Assert.AreEqual(0, result, 1e-9);
        }

        [TestMethod]
        public void CalculateCropResiduesAtInterval_AllZeroInputs_ReturnsZero()
        {
            var result = _sut.CalculateCropResiduesAtInterval(
                aboveGroundResidueNitrogenForFieldAtCurrentInterval: 0,
                aboveGroundResidueNitrogenForFieldAtPreviousInterval: 0,
                aboveGroundResidueNitrogenForCropAtCurrentInterval: 0,
                belowGroundResidueNitrogenForFieldAtCurrentInterval: 0,
                belowGroundResidueNitrogenForFieldAtPreviousInterval: 0,
                belowGroundResidueNitrogenForCropAtCurrentInterval: 0);

            Assert.AreEqual(0, result, 1e-9);
        }

        #endregion

        #endregion
    }
}
