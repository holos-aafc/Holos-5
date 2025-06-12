using System.Collections;
using System.ComponentModel;
using H.Infrastructure;

namespace H.Core.Helpers;

public class ErrorValidationBase : ModelBase, INotifyDataErrorInfo
{
    #region Fields

    private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    #endregion

    public ErrorValidationBase()
    {
    }

    #region Properties

    public bool HasErrors => _errors.Any();

    IEnumerable INotifyDataErrorInfo.GetErrors(string? propertyName)
    {
        return GetErrors(propertyName);
    }

    #endregion

    #region Public Methods

    public void AddError(string propertyName, string error)
    {
        if (!_errors.ContainsKey(propertyName))
        {
            _errors[propertyName] = new List<string>();
        }

        if (!_errors[propertyName].Contains(error))
        {
            _errors[propertyName].Add(error);
            OnErrorsChanged(propertyName);
            this.RaisePropertyChanged(nameof(HasErrors));
        }
    }
    public void RemoveError(string propertyName)
    {
        if (_errors.ContainsKey(propertyName))
        {
            _errors[propertyName].Clear();
            _errors.Remove(propertyName);
            OnErrorsChanged(propertyName);
            this.RaisePropertyChanged(nameof(HasErrors));
        }
    }

    public void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    public IEnumerable GetErrors(string propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName) || !_errors.ContainsKey(propertyName))
        {
            return null;
        }
        return _errors[propertyName];
    }

    #endregion
}