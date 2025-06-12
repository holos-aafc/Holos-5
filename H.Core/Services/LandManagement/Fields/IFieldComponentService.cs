using H.Core.Factories;
using H.Core.Models;
using H.Core.Models.LandManagement.Fields;

namespace H.Core.Services.LandManagement.Fields;

public interface IFieldComponentService :  IFieldComponentDtoFactory
{
    #region Public Methods

    int GetNextCropYear(IFieldComponentDto fieldComponentDto);
    void Initialize(IFieldComponentDto fieldComponentDto, ICropDto cropDto);
    void Initialize(Farm farm, FieldSystemComponent fieldSystemComponent);
    void ResetAllYears(IEnumerable<ICropDto> cropDtos);

    #endregion
}