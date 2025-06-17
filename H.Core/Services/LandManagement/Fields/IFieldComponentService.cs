using H.Core.Factories;
using H.Core.Models;
using H.Core.Models.LandManagement.Fields;

namespace H.Core.Services.LandManagement.Fields;

public interface IFieldComponentService :  IFieldComponentDtoFactory, ICropDtoFactory
{
    #region Public Methods

    /// <summary>
    /// When adding a new crop to the field, the year must be the next in order so that all years of the field history are consecutive.
    /// </summary>
    /// <param name="fieldComponentDto">The field containing the crops</param>
    /// <returns>The next consecutive year to should be used</returns>
    int GetNextCropYear(IFieldComponentDto fieldComponentDto);

    /// <summary>
    /// Once we have new instance of a <see cref="CropDto"/>, we must initialize it so that a minimal set of properties
    /// have sensible defaults
    /// </summary>
    void InitializeCropDto(IFieldComponentDto fieldComponentDto, ICropDto cropDto);
    void InitializeFieldSystemComponent(Farm farm, FieldSystemComponent fieldSystemComponent);

    /// <summary>
    /// All years in a collection of crops must be consecutive with no years missing. If a crop is removed, ensure all years are represented from start to finish of collection.
    /// </summary>
    /// <param name="cropDtos">The list of crops and associated years representing the history of the field</param>
    void ResetAllYears(IEnumerable<ICropDto> cropDtos);

    /// <summary>
    /// Crates a unique component name when adding a field to the farm
    /// </summary>
    string GetUniqueFieldName(IEnumerable<FieldSystemComponent> components);

    #endregion

    FieldSystemComponent TransferFieldDtoToSystem(IFieldComponentDto fieldComponentDto,
        FieldSystemComponent fieldSystemComponent);

    /// <summary>
    /// Create a new instance that is based on the state of an existing <see cref="FieldSystemComponent"/>. This method is used to create a
    /// new instance of a <see cref="FieldSystemComponentDto"/> that will be bound to a view.
    /// </summary>
    /// <param name="template">The <see cref="FieldSystemComponent"/> that will be used to provide default values for the new <see cref="FieldSystemComponentDto"/> instance</param>
    /// <returns></returns>
    IFieldComponentDto TransferToFieldComponentDto(FieldSystemComponent template);

    CropViewItem TransferCropDtoToSystem(ICropDto cropDto, CropViewItem cropViewItem);
}