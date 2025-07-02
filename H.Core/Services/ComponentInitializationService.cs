using H.Core.Models;
using H.Core.Models.LandManagement.Fields;
using H.Core.Services.LandManagement.Fields;
using H.Core.Services.StorageService;

namespace H.Core.Services;

public class ComponentInitializationService : IComponentInitializationService
{
    #region Fields

    private readonly IFieldComponentService _fieldComponentService;
    private readonly IStorageService _storageService;

    #endregion

    #region Constructors

    public ComponentInitializationService(IStorageService storageService, IFieldComponentService fieldComponentService)
    {
        if (storageService != null)
        {
            _storageService = storageService;
        }
        else
        {
            throw new ArgumentNullException(nameof(storageService));
        }

        if (fieldComponentService != null)
        {
            _fieldComponentService = fieldComponentService;
        }
        else
        {
            throw new ArgumentNullException(nameof(fieldComponentService));
        }
    }

    #endregion

    #region Public Methods

    public void Initialize(ComponentBase componentBase)
    {
        var activeFarm = _storageService.GetActiveFarm();

        if (componentBase.GetType() == typeof(FieldSystemComponent))
        {
            _fieldComponentService.InitializeFieldSystemComponent(activeFarm, componentBase as FieldSystemComponent);
        }
    } 

    #endregion
}