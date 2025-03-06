using System;
using H.Core.Providers.Animals;

namespace H.Avalonia.ViewModels.OptionsViews
{
    public class Table_30_Default_Bedding_Material_Composition_ViewModel : ViewModelBase
    {
        #region Fields
        
        private Table_30_Default_Bedding_Material_Composition_Data _dataClassInstance;
        private double _totalNitrogenKilogramsDryMatter;
        private double _totalPhosphorusKilogramsDryMatter;
        private double _totalCarbonKilogramsDryMatter;
        private double _carbonToNitrogenRatio;

        #endregion

        #region Constructors
        /// <summary>
        /// This class is a view model wrapper over <see cref="Table_30_Default_Bedding_Material_Composition_Data"/> that provides data validation 
        /// </summary>
        /// <param name="dataClassInstance"></param>
        public Table_30_Default_Bedding_Material_Composition_ViewModel(Table_30_Default_Bedding_Material_Composition_Data dataClassInstance)
        {
            if (dataClassInstance != null)
            {
                _dataClassInstance = dataClassInstance;

                _totalNitrogenKilogramsDryMatter = _dataClassInstance.TotalNitrogenKilogramsDryMatter;
                _totalPhosphorusKilogramsDryMatter = _dataClassInstance.TotalPhosphorusKilogramsDryMatter;
                _totalCarbonKilogramsDryMatter = _dataClassInstance.TotalCarbonKilogramsDryMatter;
                _carbonToNitrogenRatio = _dataClassInstance.CarbonToNitrogenRatio;
            }
            else
            {
                throw (new ArgumentNullException(nameof(dataClassInstance)));
            }
        }

        #endregion

        #region Properties

        public int MyProperty { get; set; }

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
            get => _totalNitrogenKilogramsDryMatter;
            set
            {
                if(SetProperty(ref _totalNitrogenKilogramsDryMatter, value))
                {
                    if(ValidateNumericProperty(nameof(TotalNitrogenKilogramsDryMatter), value))
                    {
                        _dataClassInstance.TotalNitrogenKilogramsDryMatter = value;
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
                    if(ValidateNumericProperty(nameof(TotalPhosphorusKilogramsDryMatter), value))
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
                    if(ValidateNumericProperty(nameof(TotalCarbonKilogramsDryMatter), value))
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
                    if (ValidateNumericProperty(nameof(CarbonToNitrogenRatio), value))
                    {
                        _dataClassInstance.CarbonToNitrogenRatio = value;
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private bool ValidateNumericProperty(string propertyName, double property)
        {
            RemoveError(propertyName);

            if (property < 0.0)
            {
                AddError(propertyName, H.Core.Properties.Resources.ErrorMustBeGreaterThan0);
                return false;
            }
            
            return true;
        }

        #endregion
    }
}
