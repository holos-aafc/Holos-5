using H.Core.Models.LandManagement.Fields;

namespace H.Core.Test.Models.LandManagement.Fields
{
    [TestClass]
    public class HayImportViewItemTest
    {
        /// <summary>
        /// The total bale weight reported for carbon-input purposes must be the DRY-matter weight
        /// (bale weight is a wet weight, so moisture is subtracted). This pins the unit so the
        /// historical "wet weight stored as dry weight" mistake cannot be reintroduced.
        /// </summary>
        [TestMethod]
        public void GetTotalDryMatterWeightOfAllBalesSubtractsMoisture()
        {
            var viewItem = new HayImportViewItem
            {
                NumberOfBales = 10,
                BaleWeight = 500,                 // wet kg per bale
                MoistureContentAsPercentage = 12,
            };

            // 500 * (1 - 0.12) * 10 = 4400 kg dry matter (< the 5000 kg gross/wet mass).
            var result = viewItem.GetTotalDryMatterWeightOfAllBales();

            Assert.AreEqual(4400, result, 1e-9);
            Assert.IsTrue(result < viewItem.NumberOfBales * viewItem.BaleWeight,
                "Dry-matter weight must be less than the gross (wet) bale mass.");
        }

        [TestMethod]
        public void GetTotalDryMatterWeightOfAllBalesIsZeroWhenNoBales()
        {
            var viewItem = new HayImportViewItem
            {
                NumberOfBales = 0,
                BaleWeight = 500,
                MoistureContentAsPercentage = 12,
            };

            Assert.AreEqual(0, viewItem.GetTotalDryMatterWeightOfAllBales(), 1e-9);
        }
    }
}
