[![fr-CA](https://img.shields.io/badge/lang-fr--CA-blue.svg)](./ARCHITECTURE.fr-CA.md)

# Holos Application Architecture Guide

## Overview

Holos is a sophisticated desktop application built using modern .NET technologies and architectural patterns. This application is designed for agricultural carbon footprint calculation and farm management, utilizing a robust, modular architecture that promotes maintainability, testability, and scalability.

## Core Technologies & Patterns

### **Application Framework**
- **Avalonia UI**: Cross-platform .NET UI framework for desktop applications
- **.NET 9**: Latest .NET framework providing modern language features and performance improvements
- **C# 13.0**: Latest C# language version with enhanced features

### **Architectural Patterns**
- **MVVM (Model-View-ViewModel)**: Separates UI logic from business logic, enabling better testability and maintainability
- **Dependency Injection (DI)**: Promotes loose coupling and enables easy testing and component swapping
- **Prism Framework**: Provides modular application development with navigation, commands, and event aggregation

### **Dependency Injection Container**
- **DryIoc**: High-performance IoC container used as the underlying DI container
- **Prism.DryIoc**: Integration layer that combines Prism's application framework with DryIoc's container capabilities
- **`CoreModule`** (`H.Core/CoreModule.cs`): the single source of the shared calculation graph (carbon input calculators, soil/N calculators, `IAnimalService`, `IFieldResultsService`, `IFarmAnalysisService`, climate/diet/table providers, caching). Both the GUI and the CLI register it, so they resolve an identical calculator graph rather than each hand-wiring it. See [Shared core registrations](#shared-core-registrations-coremodule-and-the-two-composition-roots).

## Application Bootstrap Process

Understanding the application's startup and initialization process is crucial for any developer working on this codebase. The entire application lifecycle begins with a single, critical class that acts as the **application bootloader**.

## Starting Point: App.axaml.cs - The Application Bootloader

**Location**: `H.GUI.Avalonia\H.Avalonia\App.axaml.cs`

The `App` class is the **entry point and orchestrator** of the entire application. It inherits from `PrismApplication` and is responsible for:

### **Why This Class Is Critical**

1. **Application Lifecycle Management**: Controls the entire application startup, initialization, and shutdown process
2. **Dependency Injection Setup**: Configures and registers all services, views, and components in the DI container
3. **Framework Integration**: Bridges Avalonia UI, Prism, and DryIoc frameworks
4. **Cross-Cutting Concerns**: Sets up logging, caching, language localization, and error handling

### **Key Responsibilities**

#### **Initialization Phase**
- Loads XAML resources and initializes the Avalonia framework
- Sets up the application lifetime management

#### **Dependency Registration**
- Configures unified logging through NLog. Every class in the codebase logs through the same pipeline — `ILogger` injected via DI for classes the container constructs, or a static `NLog.Logger` field via `LogManager.GetCurrentClassLogger()` for classes it doesn't (providers, helpers, partial classes). See `NLog.config` at `H.GUI.Avalonia/H.Avalonia/NLog.config`.
- Registers the shared calculation graph via `new CoreModule().RegisterTypes(...)` first, then the GUI-only services, views, factories, and providers on top of it
- Wires up the project's custom `PropertyMapper` and the per-type `IModelMapper<,>` implementations under `H.Core/Mappers/`. The codebase deliberately does **not** use AutoMapper — the mapping layer is a small reflection-driven copy-by-name engine that produces compiled delegates for hot paths.
- Configures caching and transfer services

#### **Application Shell Creation**
- Creates the main application window through dependency injection
- Ensures proper ViewModel location and binding

#### **Post-Initialization Configuration**
- Sets up language and culture settings
- Registers views with their designated UI regions
- Initializes geographic and data providers

#### **Application Lifecycle Events**
- Handles application shutdown with proper data persistence

### **Understanding the Flow**

```mermaid
graph TD
    A[App.Initialize] --> B[Load XAML Resources]
    B --> C[OnFrameworkInitializationCompleted]
    C --> D[RegisterTypes - DI Setup]
    D --> E[SetUpLogging]
    E --> F[ContainerRegistrationService]
    F --> G[CreateShell - Main Window]
    G --> H[OnInitialized]
    H --> I[SetLanguage]
    I --> J[Register View Regions]
    J --> K[Initialize Providers]
    K --> L[Application Ready]
```

### **DI Bootstrap Sequence (Detailed)**

The high-level flow above is the conceptual order. This sequence diagram shows the actual
inter-class calls during startup — what registers what, and where each major actor enters
the picture. Use it when you need to add a new service / view / provider and want to know
which seam it slots into.

```mermaid
sequenceDiagram
    autonumber
    participant Av as Avalonia framework
    participant App as App (PrismApplication)
    participant Log as LoggerFactory + NLog
    participant DI as Container (DryIoc / Prism)
    participant CRS as ContainerRegistrationService
    participant FRS as FieldResultsService

    Av->>App: Initialize()
    App->>App: AvaloniaXamlLoader.Load(this)
    Av->>App: RegisterTypes(containerRegistry)

    rect rgb(245,245,245)
    Note over App,Log: SetUpLogging(containerRegistry)
    App->>Log: LoggerFactory.Create + AddNLog
    Log-->>App: ILogger
    App->>DI: RegisterInstance(typeof(ILogger), logger)
    App->>App: ConfigureDryIocLogging(...)
    end

    App->>DI: Container.Resolve<ILogger>()
    DI-->>App: ILogger

    rect rgb(245,245,245)
    Note over App,CRS: ContainerRegistrationService.RegisterAllTypes
    App->>CRS: new(Container, logger).RegisterAllTypes(containerRegistry)
    CRS->>DI: new CoreModule().RegisterTypes(...) — shared calc graph (first)
    CRS->>DI: Register GUI-only services / views / factories / mappers / dialogs
    CRS->>CRS: PreWarmHeavyServices()
    CRS-)FRS: Task.Run → Resolve<IFieldResultsService> (off-thread)
    Note right of FRS: SmallAreaYieldProvider<br/>parses ~1M-row CSV<br/>so first user click<br/>doesn't pay the cost
    end

    Av->>App: OnFrameworkInitializationCompleted()
    App->>DI: CreateShell() → Container.Resolve<MainWindow>()
    DI-->>App: MainWindow

    Av->>App: OnInitialized()
    App->>App: SetLanguage() (reads app.config)
    App->>DI: Resolve IRegionManager
    App->>DI: regionManager.RegisterViewWithRegion(...) ×N
    App->>DI: Resolve GeographicDataProvider, KmlHelpers
    Note over App: Application is ready
```

**Why this matters for new contributors:**

- New service → register inside `ContainerRegistrationService.RegisterAllTypes`, after the
  `SetUpLogging` step but before the `MainWindow` is resolved. Prism will then inject it
  into any view-model that declares it as a ctor parameter.
- New view → registered the same way, with `containerRegistry.RegisterForNavigation<TView, TViewModel>()`.
  The view region wiring happens later in `OnInitialized`.
- Heavy startup work → if a service has a multi-second cold-start cost (large CSV parse,
  HTTP probe, etc.), use the `PreWarmHeavyServices` Task.Run pattern so the first user
  interaction doesn't block. `SmallAreaYieldProvider`'s 1M-row parse is the existing
  example.

### **Shared core registrations: `CoreModule` and the two composition roots**

The carbon/nitrogen/animal calculation graph is needed by **two** front-ends — the Avalonia
GUI and the command-line `H.CLI` — so its DI registrations live in one place:
`H.Core/CoreModule.cs`. `CoreModule` is a Prism `IModule` that registers the whole shared
stack (carbon-input calculators, soil/N calculators, `N2OEmissionFactorCalculator`,
`IAnimalService`, `IFieldResultsService`, `IFarmAnalysisService`, `ICropInitializationService`,
manure/digestate/field-component helpers, the climate/diet/feed/table providers, and
`IMemoryCache` / `ICacheService`).

Each front-end has its own **composition root** that registers `CoreModule` plus the
infrastructure that front-end owns:

| Front-end | Composition root | Adds on top of `CoreModule` |
|---|---|---|
| GUI | `ContainerRegistrationService.RegisterAllTypes` (`H.GUI.Avalonia/H.Avalonia/Infrastructure/DependencyInjection/`) | Views + view-models, mappers, factories, transfer services, dialogs, GUI services (storage, notifications, error handling), and `ILogger` (from `App.SetUpLogging`). `IEventAggregator` comes from `PrismApplication`. |
| CLI | `CliCompositionRoot.Build` (`H.CLI/Infrastructure/`) | `ILogger` = `NullLogger`, an `IMemoryCache`, and `ICacheService` → `InMemoryCacheService`. `Program.cs` then resolves `IFieldResultsService` from the container. |

```mermaid
flowchart TD
    CM["CoreModule.RegisterTypes<br/>(shared calc graph)"]
    GUI["ContainerRegistrationService<br/>.RegisterAllTypes"] --> CM
    GUI --> GUIextra["+ Views / ViewModels<br/>Mappers / Factories<br/>Dialogs / GUI services"]
    CLI["CliCompositionRoot.Build"] --> CM
    CLI --> CLIextra["+ NullLogger<br/>IMemoryCache<br/>InMemoryCacheService"]
```

**Why it's wired this way:** before this consolidation `CoreModule` was dead code and the GUI
hand-registered everything, while the CLI hand-`new`ed its calculator stack with no container
at all. Centralising the shared registrations means a new calculator is registered once, in
`CoreModule`, and both front-ends pick it up.

**Test coverage for the DI graph** (there is no test that launches the real app, so these
guard the wiring):

- `H.Core.Test/CoreModuleResolutionTests.cs` — builds a DryIoc container over `CoreModule`
  (plus the host infra it expects) and resolves the top-level calculation services. Guards the
  shared graph used by both front-ends.
- `H.GUI.Avalonia/H.Avalonia.Test/Infrastructure/FullContainerResolutionTests.cs` — builds the
  **full** GUI container through `RegisterAllTypes` and resolves the GUI-only top-level services.
  Complements the manual GUI smoke test.
- `H.CLI.Test/Infrastructure/CliCompositionRootTest.cs` — resolves the calculation services from
  the CLI composition root.

### **Modern Architecture Benefits**

The architecture implemented in this bootloader provides:

- **Modularity**: Clean separation of concerns with dedicated registration services
- **Testability**: Comprehensive dependency injection enables easy unit testing
- **Observability**: Extensive logging throughout the initialization process
- **Maintainability**: Well-organized, documented code with clear responsibilities
- **Performance**: Optimized container configuration with efficient service resolution

### **Next Steps for Developers**

To fully understand this application:

1. **Start Here**: Study the `App.axaml.cs` class thoroughly - it's your roadmap to the entire application
2. **Follow the DI Trail**: Examine the `ContainerRegistrationService` to understand service registrations
3. **Understand the MVVM Structure**: Look at how views and view models are registered and resolved
4. **Explore Navigation**: Study how Prism regions and navigation work within the application

This bootloader class is not just initialization code - it's the **architectural blueprint** that defines how the entire application is structured, configured, and operated. Master this class, and you'll have a solid foundation for understanding the rest of the Holos application architecture.

---

## Related Documentation

This guide focuses on the application bootstrap and the overall architectural shape. Deeper
material lives in adjacent files:

- **[`H.Content/Documentation/Developer Guide/Carbon_Model_Flow.md`](H.Content/Documentation/Developer%20Guide/Carbon_Model_Flow.md)** — end-to-end Mermaid diagram of the carbon analysis pipeline (View → Analysis → Results), with a class-by-class file index. Essential reading before touching the carbon or nitrogen calculators; the ordering invariants (carbon before nitrogen, animal results primed between stage-state build and final pass) are not obvious from the call sites alone.
- **[`H.Content/Documentation/Developer Guide/Developer_Guide_EN.md`](H.Content/Documentation/Developer%20Guide/Developer_Guide_EN.md)** — IDE setup for Visual Studio / VS Code / Rider, dotnet CLI commands, solution layout, logging + localization workflow.
- **[`CODING_STYLE_GUIDE.md`](CODING_STYLE_GUIDE.md)** — naming conventions, region organization, the Avalonia `StringFormat` pitfall, and the unified logging pattern.
- **[`DEVELOPER_ONBOARDING_GUIDE.md`](DEVELOPER_ONBOARDING_GUIDE.md)** — full first-time setup including SDK install, repository clone, and the typical troubleshooting list.

In-code documentation: the ~60 files most central to the carbon pipeline carry detailed
class-level XML docstrings naming their role, collaborators, and ordering invariants. Pull
up any of `FarmAnalysisService`, `FieldResultsService`, `ICBMSoilCarbonCalculator`,
`IPCCTier2SoilCarbonCalculator`, `N2OEmissionFactorCalculator`, `AnimalResultsService`,
`ManureService`, or the Table_* providers and the class header should orient you within a
few seconds.

Each layer builds upon the foundation established by understanding the application
bootloader process documented above.