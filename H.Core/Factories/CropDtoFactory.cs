using AutoMapper;
using H.Core.Enumerations;
using H.Core.Models.LandManagement.Fields;
using H.Infrastructure;

namespace H.Core.Factories;

/// <summary>
/// A class used to create new <see cref="CropDto"/> instances. The class will provide basic initialization of a new instance before returning the result to the caller.
/// </summary>
public class CropDtoFactory : ICropDtoFactory
{
    #region Fields

    private readonly IMapper _cropDtoMapper;

    #endregion

    #region Constructors

    public CropDtoFactory()
    {
        var cropDtoMapperConfiguration = new MapperConfiguration(configuration => { configuration.CreateMap<CropViewItem, CropDto>(); });

        _cropDtoMapper = cropDtoMapperConfiguration.CreateMapper();
    } 

    #endregion

    #region Public Methods

    /// <summary>
    /// Create a new instance with no additional configuration to a default instance.
    /// </summary>
    public ICropDto CreateCropDto()
    {
        return new CropDto();
    }

    /// <summary>
    /// Create a new instance that is based on the state of an existing <see cref="CropViewItem"/>. This method is used to create a
    /// new instance of a <see cref="CropDto"/> that will be bound to a view.
    /// </summary>
    /// <param name="template">The <see cref="CropViewItem"/> that will be used to provide default values for the new <see cref="CropDto"/> instance</param>
    public ICropDto Create(CropViewItem template)
    {
        var cropDto = new CropDto();

        _cropDtoMapper.Map(template, cropDto);

        return cropDto;
    }

    public string GetPrintFriendy(MetricUnitsOfMeasurement metricUnitsOfMeasurement)
    {
        if (metricUnitsOfMeasurement == MetricUnitsOfMeasurement.KilogramsMethane)
        {
            // This contains a subscript 4.

            return "kg CH4";
        }
        else
        {
            return metricUnitsOfMeasurement.GetDescription();
        }
    }

    #endregion
}