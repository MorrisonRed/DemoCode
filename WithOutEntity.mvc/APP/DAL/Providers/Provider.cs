using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;

/// <summary>
/// Generic Database Provider
/// </summary>
namespace Providers
{
/// <summary>
/// A base class for all custom providers to inherit from
/// </summary>
public abstract class Provider: ProviderBase
{

    #region Constructors and Destructors
    public Provider()
	{

    }
    #endregion 
}
}