using H.Core.Models;
using H.Core.Models.LandManagement.Fields;

namespace H.Core.Services.LandManagement.Fields;

public interface IFieldInitializationService
{
    #region Public Methods

    void Initialize(Farm farm, FieldSystemComponent fieldSystemComponent);

    #endregion
}