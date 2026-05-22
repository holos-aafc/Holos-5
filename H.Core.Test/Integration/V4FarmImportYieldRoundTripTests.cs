using H.Core.Calculators.UnitsOfMeasurement;
using H.Core.Enumerations;
using H.Core.Factories;
using H.Core.Factories.Crops;
using H.Core.Mappers;
using H.Core.Models;
using H.Core.Models.LandManagement.Fields;
using H.Core.Services.Animals;
using Moq;
using Newtonsoft.Json;

#nullable disable

namespace H.Core.Test.Integration
{
    /// <summary>
    /// Guards the v4-farm import path for crop yield against the v5 WetYield rename.
    /// <para>
    /// A v4 farm persists the wet-weight yield under the property name <c>Yield</c> (v4 had no
    /// separate wet-weight property). v5's domain <see cref="CropViewItem"/> keeps the same
    /// <c>Yield</c> name; <c>WetYield</c> exists only on the GUI DTO and is never serialized. So
    /// import is pure JSON deserialization into the domain model and never touches the DTO/mapper
    /// layer. These tests pin that:
    /// </para>
    /// <list type="number">
    ///   <item>a v4-shaped JSON deserializes the wet yield straight onto <c>CropViewItem.Yield</c>; and</item>
    ///   <item>once opened in the editor, the DTO's <c>WetYield</c> reflects it and edits round-trip
    ///   back to <c>CropViewItem.Yield</c> (the value the carbon model consumes).</item>
    /// </list>
    /// </summary>
    [TestClass]
    public class V4FarmImportYieldRoundTripTests
    {
        [TestMethod]
        public void V4Json_DeserializesWetYieldOntoCropViewItemYield()
        {
            // A v4 farm stores the wet-weight yield as "Yield"; there is no WetYield in the JSON.
            var v4Json = "{ \"Yield\": 5000.0, \"DryYield\": 4400.0, \"Year\": 2015, \"CropType\": \"Wheat\" }";

            var imported = JsonConvert.DeserializeObject<CropViewItem>(v4Json);

            Assert.IsNotNull(imported);
            Assert.AreEqual(5000.0, imported.Yield, "v4 'Yield' must deserialize onto CropViewItem.Yield (wet weight).");
        }

        [TestMethod]
        public void ImportedV4Crop_RoundTripsWetYieldThroughTheEditor()
        {
            // Arrange: a crop as it exists right after loading a v4 farm (Yield populated, no DTO yet).
            var v4Json = "{ \"Yield\": 5000.0, \"Year\": 2015, \"CropType\": \"Wheat\" }";
            var imported = JsonConvert.DeserializeObject<CropViewItem>(v4Json);

            var units = new Mock<IUnitsOfMeasurementCalculator>();
            units.Setup(x => x.GetUnitsOfMeasurement()).Returns(MeasurementSystemType.Metric);

            // Real factory clone (DTO -> DTO) so the transfer's internal copy preserves WetYield.
            var cropDtoFactory = new Mock<IFactory<CropDto>>();
            cropDtoFactory.Setup(f => f.CreateDto(It.IsAny<Farm>())).Returns(() => new CropDto());
            cropDtoFactory.Setup(f => f.CreateDtoFromDtoTemplate(It.IsAny<IDto>()))
                .Returns<IDto>(template => new CropDtoToCropDtoMapper().Map((CropDto)template));

            // Real mappers + real transfer service - the path the editor uses to display/save edits.
            ITransferService<CropViewItem, CropDto> transfer = new TransferService<CropViewItem, CropDto>(
                units.Object,
                cropDtoFactory.Object,
                new CropDtoToCropViewItemMapper(),
                new CropViewItemToCropDtoMapper());

            // Act 1 - open in editor (domain -> DTO): the wet yield must surface on WetYield.
            var dto = transfer.TransferDomainObjectToDto(imported);
            Assert.AreEqual(5000.0, dto.WetYield, "Imported Yield must surface as WetYield in the editor.");

            // Act 2 - user edits the wet yield and saves (DTO -> domain).
            dto.WetYield = 6200.0;
            transfer.TransferDtoToDomainObject(dto, imported);

            // Assert: the edit persisted to the domain Yield the carbon model consumes.
            Assert.AreEqual(6200.0, imported.Yield, "Editing WetYield must persist to CropViewItem.Yield for imported v4 crops.");
        }
    }
}
