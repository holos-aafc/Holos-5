using System.Collections.ObjectModel;
using H.Avalonia.ViewModels.ComponentViews.LandManagement;
using H.Avalonia.ViewModels.ComponentViews.LandManagement.Field;
using H.Core.Factories;
using H.Core.Models.Animals.Beef;
using H.Core.Models.LandManagement.Fields;
using H.Core.Providers.Feed;
using H.Core.Services.StorageService;
using Moq;
using Prism.Events;
using Prism.Regions;

namespace H.Avalonia.Test.ViewModels.ComponentViews.LandManagement;

[TestClass]
public class FieldComponentViewModelTest
{
    #region Fields

    private FieldComponentViewModel _viewModel;
    private Mock<IFieldComponentDtoFactory> _mockFieldComponentDtoFactory;

    #endregion

    #region Initialization

    [ClassInitialize]
    public static void ClassInitialize(TestContext testContext)
    {
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
    }

    [TestInitialize]
    public void TestInitialize()
    {
        var mockRegionManager = new Mock<IRegionManager>();
        var mockEventAggregator = new Mock<IEventAggregator>();
        var mockStorageService = new Mock<IStorageService>();
        _mockFieldComponentDtoFactory = new Mock<IFieldComponentDtoFactory>();

        var mockCropDtoFactory = new Mock<ICropDtoFactory>();

        _mockFieldComponentDtoFactory.Setup(x => x.Create()).Returns(new FieldSystemComponentDto());
        _mockFieldComponentDtoFactory.Setup(x => x.Create(It.IsAny<FieldSystemComponent>())).Returns(new FieldSystemComponentDto());

        _viewModel = new FieldComponentViewModel(mockRegionManager.Object, mockEventAggregator.Object, mockStorageService.Object, _mockFieldComponentDtoFactory.Object, mockCropDtoFactory.Object);
    }

    [TestCleanup]
    public void TestCleanup()
    {
    }

    #endregion

    #region Tests

    [TestMethod]
    public void InitializeViewModelSetFieldSystemComponentToNonNull()
    {
        _mockFieldComponentDtoFactory.Setup(factory => factory.Create()).Returns(new FieldSystemComponentDto());

        _viewModel.InitializeViewModel(new FieldSystemComponent());

        Assert.IsNotNull(_viewModel.SelectedFieldSystemComponentDto);
    }

    [TestMethod]
    public void InitializeViewModelSetFieldSystemComponentToNull()
    {
        _viewModel.InitializeViewModel(new BackgroundingComponent());

        Assert.IsNull(_viewModel.SelectedFieldSystemComponentDto);
    }

    [TestMethod]
    public void InitializeViewModelSetCropViewItemToNotNull()
    {
        _viewModel.InitializeViewModel(new FieldSystemComponent() {CropViewItems = new ObservableCollection<CropViewItem>()});

        Assert.IsNull(_viewModel.SelectedCropDto);
    }

    [TestMethod]
    public void InitializeViewModelSetCropViewItemToNull()
    {
        _viewModel.InitializeViewModel(new FieldSystemComponent());

        Assert.IsNull(_viewModel.SelectedCropDto);
    }

    [TestMethod]
    public void InitializeViewModelSetCropDtoCollectionToNonEmpty()
    {
        _viewModel.InitializeViewModel(new FieldSystemComponent() {CropViewItems = new ObservableCollection<CropViewItem>() {new CropViewItem()}});

        Assert.IsTrue(_viewModel.CropDtos.Any());
    }

    [TestMethod]
    public void InitializeViewModelSetCropDtoCollectionToEmpty()
    {
        _viewModel.InitializeViewModel(new FieldSystemComponent() { CropViewItems = new ObservableCollection<CropViewItem>() {  } });

        Assert.IsFalse(_viewModel.CropDtos.Any());
    }

    #endregion
}