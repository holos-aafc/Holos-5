using AutoMapper;
using H.Core.Models;
using H.Core.Models.Animals;
using H.Core.Models.LandManagement.Fields;
using H.Core.Providers;
using H.Core.Providers.Animals;
using H.Core.Providers.Climate;
using H.Core.Providers.Evapotranspiration;
using H.Core.Providers.Feed;
using H.Core.Providers.Precipitation;
using H.Core.Providers.Soil;
using H.Core.Providers.Temperature;

namespace H.Core.Services
{
    public class FarmResultsService_NEW : IFarmResultsService_NEW
    {
        #region Fields

        private readonly IMapper _farmMapper;
        private readonly IMapper _defaultsMapper;
        private readonly IMapper _climateDataMapper;
        private readonly IMapper _dailyClimateDataMapper;
        private readonly IMapper _detailsScreenCropViewItemMapper;
        private readonly IMapper _geographicDataMapper;
        private readonly IMapper _soilDataMapper;
        private readonly IMapper _customYieldDataMapper;

        private readonly IFieldComponentHelper _fieldComponentHelper = new FieldComponentHelper();
        private readonly IAnimalComponentHelper _animalComponentHelper = new AnimalComponentHelper();

        #endregion

        #region Constructors

        public FarmResultsService_NEW()
        {
            #region Farm Mapping

            var farmMapperConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<Farm, Farm>()
                    .ForMember(y => y.Name, z => z.Ignore())
                    .ForMember(y => y.Guid, z => z.Ignore())
                    .ForMember(y => y.Defaults, z => z.Ignore())
                    .ForMember(y => y.StageStates, z => z.Ignore())
                    .ForMember(y => y.ClimateData, z => z.Ignore())
                    .ForMember(y => y.GeographicData, z => z.Ignore())
                    .ForMember(y => y.Components, z => z.Ignore())
                    .ForMember(y => y.AnimalComponents, z => z.Ignore())
                    .ForMember(y => y.DairyComponents, z => z.Ignore())
                    .ForMember(y => y.PoultryComponents, z => z.Ignore())
                    .ForMember(y => y.FieldSystemComponents, z => z.Ignore())
                    .ForMember(y => y.SheepComponents, z => z.Ignore())
                    .ForMember(y => y.SwineComponents, z => z.Ignore())
                    .ForMember(y => y.AnaerobicDigestionComponents, z => z.Ignore())
                    .ForMember(y => y.BeefCattleComponents, z => z.Ignore())
                    .ForMember(y => y.OtherLivestockComponents, z => z.Ignore());

                x.CreateMap<Table_15_Default_Soil_N2O_Emission_BreakDown_Provider,
                    Table_15_Default_Soil_N2O_Emission_BreakDown_Provider>();
                x.CreateMap<Table_30_Default_Bedding_Material_Composition_Data,
                    Table_30_Default_Bedding_Material_Composition_Data>();
                x.CreateMap<DefaultManureCompositionData, DefaultManureCompositionData>();

                x.CreateMap<Diet, Diet>();
            });

            _farmMapper = farmMapperConfiguration.CreateMapper();

            #endregion

            #region Defaults Mapping

            var defaultMapperConfiguration = new MapperConfiguration(x => { x.CreateMap<Defaults, Defaults>(); });

            _defaultsMapper = defaultMapperConfiguration.CreateMapper();

            #endregion

            #region Details Screen Mapping

            var detailsScreenCropViewItemMapperConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<CropViewItem, CropViewItem>();
            });

            _detailsScreenCropViewItemMapper = detailsScreenCropViewItemMapperConfiguration.CreateMapper();

            #endregion

            #region Climate Mappers

            var climateDataMapper = new MapperConfiguration(x =>
            {
                x.CreateMap<PrecipitationData, PrecipitationData>();
                x.CreateMap<TemperatureData, TemperatureData>();
                x.CreateMap<EvapotranspirationData, EvapotranspirationData>();
                x.CreateMap<ClimateData, ClimateData>()
                    .ForMember(y => y.DailyClimateData, z => z.Ignore())
                    .ForMember(y => y.Guid, z => z.Ignore());
            });

            _climateDataMapper = climateDataMapper.CreateMapper();

            var dailyclimateDataMapper = new MapperConfiguration(x =>
            {
                x.CreateMap<DailyClimateData, DailyClimateData>();
            });

            _dailyClimateDataMapper = dailyclimateDataMapper.CreateMapper();

            #endregion

            #region GeographicData Mappers

            var geographicDataMapper = new MapperConfiguration(x =>
            {
                x.CreateMap<GeographicData, GeographicData>()
                    .ForMember(y => y.SoilDataForAllComponentsWithinPolygon, z => z.Ignore())
                    .ForMember(y => y.DefaultSoilData, z => z.Ignore())
                    .ForMember(y => y.CustomYieldData, z => z.Ignore())
                    .ForMember(y => y.Guid, z => z.Ignore());
            });

            _geographicDataMapper = geographicDataMapper.CreateMapper();

            var soilDataMapper = new MapperConfiguration(x =>
            {
                x.CreateMap<SoilData, SoilData>();
            });

            _soilDataMapper = soilDataMapper.CreateMapper();

            var customYieldMapper = new MapperConfiguration(x =>
            {
                x.CreateMap<CustomUserYieldData, CustomUserYieldData>();
            });

            _customYieldDataMapper = customYieldMapper.CreateMapper();

            #endregion
        }

        #endregion

        #region Public Methods

        // TODO: Add support for AD components
        public Farm ReplicateFarm(Farm farm)
        {
            var replicatedFarm = new Farm();

            _farmMapper.Map(farm, replicatedFarm);
            _defaultsMapper.Map(farm.Defaults, replicatedFarm.Defaults);
            _climateDataMapper.Map(farm.ClimateData, replicatedFarm.ClimateData);
            _geographicDataMapper.Map(farm.GeographicData, replicatedFarm.GeographicData);

            replicatedFarm.Name = farm.Name;

            #region Animal Components

            foreach (var animalComponent in farm.AnimalComponents.Cast<AnimalComponentBase>())
            {
                var replicatedAnimalComponent = _animalComponentHelper.ReplicateAnimalComponent(animalComponent);

                replicatedFarm.Components.Add(replicatedAnimalComponent);
            }

            #endregion

            #region FieldSystemComponents

            foreach (var fieldSystemComponent in farm.FieldSystemComponents)
            {
                var replicatedFieldSystemComponent = _fieldComponentHelper.Replicate(fieldSystemComponent);

                replicatedFarm.Components.Add(replicatedFieldSystemComponent);
            }

            #endregion

            #region DailyClimateData

            foreach (var dailyClimateData in farm.ClimateData.DailyClimateData)
            {
                var replicatedDailyClimateData = new DailyClimateData();
                _dailyClimateDataMapper.Map(dailyClimateData, replicatedDailyClimateData);
                replicatedFarm.ClimateData.DailyClimateData.Add(dailyClimateData);
            }

            #endregion

            #region SoilData and CustomYieldData

            foreach (var soilData in farm.GeographicData.SoilDataForAllComponentsWithinPolygon)
            {
                var replicatedSoilData = new SoilData();
                _soilDataMapper.Map(soilData, replicatedSoilData);
                replicatedFarm.GeographicData.SoilDataForAllComponentsWithinPolygon.Add(replicatedSoilData);
            }

            _soilDataMapper.Map(farm.GeographicData.DefaultSoilData, replicatedFarm.GeographicData.DefaultSoilData);

            foreach (var customYieldData in farm.GeographicData.CustomYieldData)
            {
                var replicatedCustomYieldData = new CustomUserYieldData();
                _customYieldDataMapper.Map(customYieldData, replicatedCustomYieldData);
                replicatedFarm.GeographicData.CustomYieldData.Add(replicatedCustomYieldData);
            }

            #endregion

            #region StageStates

            foreach (var fieldSystemDetailsStageState in farm.StageStates.OfType<FieldSystemDetailsStageState>().ToList())
            {
                var stageState = new FieldSystemDetailsStageState();
                replicatedFarm.StageStates.Add(stageState);

                foreach (var detailsScreenViewCropViewItem in fieldSystemDetailsStageState.DetailsScreenViewCropViewItems)
                {
                    var viewItem = new CropViewItem();

                    _detailsScreenCropViewItemMapper.Map(detailsScreenViewCropViewItem, viewItem);

                    stageState.DetailsScreenViewCropViewItems.Add(viewItem);
                }
            }

            #endregion

            return replicatedFarm;
        }

        #endregion
    }
}
