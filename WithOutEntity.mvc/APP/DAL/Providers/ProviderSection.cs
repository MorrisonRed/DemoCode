using System.Configuration;

namespace Providers
{
    /// <summary>
    /// A configuration section for web.config
    /// </summary>
    /// <remarks>
    /// In ths config section you can specify the provider you want to use for site
    /// </remarks>
    public class ProviderSection : ConfigurationSection
    {

        #region Public Properties
        /// <summary>
        /// Get/Set the name of the default provider
        /// </summary>
        [StringValidator(MinLength=1)]
        [ConfigurationProperty("defaultProvider", DefaultValue="XmlBlogProvider")]
        public string DefaultProvider
        {
            get { return (string)base["defaultProvider"]; }
            set { base["defaultProvider"] = value; }
        }
        #endregion

        #region Constructors and Destructors
        public ProviderSection()
        {

        }
        #endregion
    }
}