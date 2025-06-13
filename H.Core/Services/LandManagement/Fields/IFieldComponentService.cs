using H.Core.Factories;
using H.Core.Models;
using H.Core.Models.LandManagement.Fields;

namespace H.Core.Services.LandManagement.Fields;

public interface IFieldComponentService :  IFieldComponentDtoFactory, ICropDtoFactory
{
    #region Public Methods

    int GetNextCropYear(IFieldComponentDto fieldComponentDto);
    void InitializeCropDto(IFieldComponentDto fieldComponentDto, ICropDto cropDto);
    void Initialize(Farm farm, FieldSystemComponent fieldSystemComponent);
    void ResetAllYears(IEnumerable<ICropDto> cropDtos);

    #endregion
}