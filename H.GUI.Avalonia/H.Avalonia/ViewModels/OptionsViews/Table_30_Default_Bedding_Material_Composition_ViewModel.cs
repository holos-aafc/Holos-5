using System;
using H.Core.Providers.Animals;
using H.Core.Calculators.UnitsOfMeasurement;
using H.Core.Enumerations;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class Table_30_Default_Bedding_Material_Composition_ViewModel : ViewModelBase
    {
        #region Fields
        
        private Table_30_Default_Bedding_Material_Composition_Data _dataClassInstance;
        private readonly IUnitsOfMeasurementCalculator _unitsCalculator;
        private double _totalNitrogenKilogramsDryMatter;
        private double _totalPhosphorusKilogramsDryMatter;
        private double _totalCarbonKilogramsDryMatter;
        private double _carbonToNitrogenRatio;
        private bool _initializationFlag;

        #endregion

        #region Constructors
        /// <summary>
        /// This class is a view model wrapper over <see cref="Table_30_Default_Bedding_Material_Composition_Data"/> that provides data validation 
        /// </summary>
        /// <param name="dataClassInstance"></param>
        public Table_30_Default_Bedding_Material_Composition_ViewModel(Table_30_Default_Bedding_Material_Composition_Data dataClassInstance, IUnitsOfMeasurementCalculator unitsCalculator)
        {
            if (dataClassInstance != null)
            {
                _dataClassInstance = dataClassInstance;
            }
            else
            {
                throw (new ArgumentNullException(nameof(dataClassInstance)));
            }
            if (unitsCalculator != null)
            {
                _unitsCalculator = unitsCalculator;
            }
            else
            {
                throw (new ArgumentNullException(nameof(unitsCalculator)));
            }
        }

        #endregion

        #region Properties

        public string ComponentCategoryString
        {
            get { return _dataClassInstance.ComponentCategoryString; }
        }

        public string BeddingMaterialString
        {
            get { return _dataClassInstance.BeddingMaterialString; }
        }

        public double TotalNitrogenKilogramsDryMatter
        {
            get
            {
                if (_unitsCalculator.IsMetric)
                {
                    return _totalNitrogenKilogramsDryMatter;
                }
                else
                {
                    return _unitsCalculator.GetUnitsOfMeasurementValue(MeasurementSystemType.Imperial, MetricUnitsOfMeasurement.KilogramsNitrogen, _totalNitrogenKilogramsDryMatter);
                }
            }
            set
            {
                double metricValue = value;

                if (!_initializationFlag && !_unitsCalculator.IsMetric)
                {
                    metricValue = _unitsCalculator.GetUnitsOfMeasurementValue(MeasurementSystemType.Metric, ImperialUnitsOfMeasurement.PoundsNitrogen, metricValue);
                }

                if (SetProperty(ref _totalNitrogenKilogramsDryMatter, metricValue))
                {
                    if (ValidateNumericProperty(nameof(TotalNitrogenKilogramsDryMatter), metricValue) && !_initializationFlag)
                    {
                        _dataClassInstance.TotalNitrogenKilogramsDryMatter = metricValue;
                    }
                }
            }
        }

        public double TotalPhosphorusKilogramsDryMatter
        {
            get => _totalPhosphorusKilogramsDryMatter;
            set
            {
                if (SetProperty(ref _totalPhosphorusKilogramsDryMatter, value))
                {
                    if(ValidateNumericProperty(nameof(TotalPhosphorusKilogramsDryMatter), value) && !_initializationFlag)
                    {
                        _dataClassInstance.TotalPhosphorusKilogramsDryMatter = value;
                    }
                }
            }
                
        }

        public double TotalCarbonKilogramsDryMatter
        {
            get => _totalCarbonKilogramsDryMatter;
            set
            {
                if(SetProperty(ref _totalCarbonKilogramsDryMatter, value))
                {
                    if(ValidateNumericProperty(nameof(TotalCarbonKilogramsDryMatter), value) && !_initializationFlag)
                    {
                        _dataClassInstance.TotalCarbonKilogramsDryMatter = value;
                    }
                }
            }
        }

        public double CarbonToNitrogenRatio
        {
            get => _carbonToNitrogenRatio;
            set
            {
                if (SetProperty(ref _carbonToNitrogenRatio, value))
                {
                    if (ValidateNumericProperty(nameof(CarbonToNitrogenRatio), value) && !_initializationFlag)
                    {
                        _dataClassInstance.CarbonToNitrogenRatio = value;
                    }
                }
            }
        }

        #endregion

        #region Public Methods

        public void SetInitializationFlag(bool flag)
        {
            _initializationFlag = flag;
        }

        public void UpdateUnitsOfMeasurementDependentProperties() 
        {
            RaisePropertyChanged(nameof(TotalNitrogenKilogramsDryMatter));
        }

        #endregion

        #region Private Methods

        private bool ValidateNumericProperty(string propertyName, double property)
        {
            RemoveError(propertyName);

            if (property < 0.0)
            {
                AddError(propertyName, H.Core.Properties.Resources.ErrorMustBeNonNegative);
                return false;
            }
            
            return true;
        }

        #endregion
    }
}
