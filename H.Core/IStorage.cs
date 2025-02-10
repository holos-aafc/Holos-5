using H.Core.Models;

namespace H.Core;

public interface IStorage
{
    #region Properties

    public ApplicationData ApplicationData { get; set; }

    #endregion

    #region Public Methods
    
    /// <summary>
    /// Tries to load the user's .json data file. If the data file cannot be loaded, the method checks if there
    /// are any backups available. If yes tries to load the most recent backup, otherwise it sets <see cref="ApplicationData"/> to a new instance
    /// of <see cref="Models.ApplicationData"/>
    /// </summary>
    void Load(); 

    #endregion
}