using H.Core.Models;
using H.Core.Models.LandManagement.Fields;
using H.Core.Services.LandManagement.Fields;
using H.Core.Services.StorageService;

namespace H.Core.Services;

public class ComponentInitializationService : IComponentInitializationService
{
    #region Fields

    private readonly IFieldInitializationService _fieldInitializationService;
    private readonly IStorageService _storageService;

    #endregion

    #region Constructors

    public ComponentInitializationService(IStorageService storageService, IFieldInitializationService fieldInitializationService)
    {
        if (storageService != null)
        {
            _storageService = storageService;
        }
        else
        {
            throw new ArgumentNullException(nameof(storageService));
        }

        if (fieldInitializationService != null)
        {
            _fieldInitializationService = fieldInitializationService;
        }
        else
        {
            throw new ArgumentNullException(nameof(fieldInitializationService));
        }
    }

    #endregion

    #region Public Methods

    public void Initialize(ComponentBase componentBase)
    {
        var activeFarm = _storageService.GetActiveFarm();

        if (componentBase.GetType() == typeof(FieldSystemComponent))
        {
            _fieldInitializationService.Initialize(activeFarm, componentBase as FieldSystemComponent);
        }
    } 

    #endregion
}