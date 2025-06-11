using H.Core.Models;

namespace H.Core.Services;

public interface IComponentInitializationService
{
    void Initialize(ComponentBase componentBase);
}