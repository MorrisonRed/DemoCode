using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web.Security;

namespace CustomSecurity
{
    /// <summary>
    /// Manages user membership in roles for authorization checking in an ASP.NET application.
    /// This class cannot be inherited.
    /// </summary>
    class Roles
    {
        private static string _appName;
        private static bool _cacheRolesInCookie;
        private static string _cookieName; 
        private static string _cookiePath;
        private static CookieProtection _cookieProtectionValue;
        private static bool _cookieRequireSSL;
        private static bool _cookieSlidingExpiration;
        private static int _cookieTimeout;
        private static bool _createPersistentCookie;
        private static string _domain;
        private static bool _enabled;
        private static int _maxCachedResults; 

#region Public Properties
        /// <summary>
        /// Summary:
        ///     Gets or sets name of the application to store and retrieve role information for.
        ///     
        /// Returns:
        ///     The name of the application to store and retrieve role information for.
        /// </summary>
        public static string ApplicationName
        {
            get { return _appName; }
            set { _appName = value; } 
        }
        /// <summary>
        /// Summary:
        ///     Gets a value indicating whether the current user's roles are cached in a cookie.
        ///     
        /// Returns:
        ///     true if the current user's roles are cached in a cookie; otherwise, false.
        ///     The default is true.
        /// </summary>
        public static bool CacheRolesInCookie
        {
            get { return _cacheRolesInCookie; }
        }
        /// <summary>
        /// Summary:
        ///     Gets the name of the cookie where role name are cached.
        ///     
        /// Returns:
        ///     The name of the cookie where role names are cached.  
        ///     The default is .ASPXROLES.
        /// </summary>
        public static string CookieName
        {
            get { return _cookieName; }
        }
        /// <summary>
        /// Summary:
        ///     Gets the path for the cached role names cookie.
        ///     
        /// Returns:
        ///     The pat of the cookie where role names are cached.  The defualt is /.
        /// </summary>
        public static string CookiePath
        {
            get { return _cookiePath; }
        }
        /// <summary>
        /// Summary:
        ///     Gets a value that indicates how role names cached in a cookie are protected.
        ///     
        /// Returns:
        ///     One of the System.Web.Security.CookieProtection enumeration values indicating
        ///     how role names that are cached in a cookie are protected. The default is
        ///     All.
        /// </summary>
        public static CookieProtection CookieProtectionValue
        {
            get { return _cookieProtectionValue; }
        }
        /// <summary>
        /// Summary:
        ///     Gets a value indicating whether the role names cookie requires SSL in order
        ///     to be returned to the server.
        ///     
        /// Returns:
        ///     true if SSL required to return the role names cookie to the server; otherwise, 
        ///     false.  The default is false.
        /// </summary>
        public static bool CookieRequireSSL
        {
            get { return _cookieRequireSSL; }
        }
        /// <summary>
        /// Summary:
        ///     Indicates whether the role names cookie expiration date/time will be 
        ///     reset periodically
        ///     
        /// Returns: 
        ///     true if the role names cookie expiration date/time will be reset periodically; 
        ///     otherwise, false.  The default is true.
        /// </summary>
        public static bool CookieSlidingExpiration
        {
            get { return _cookieSlidingExpiration; }
        }
        /// <summary>
        /// Summary:
        ///     Gets the number of minutes before the roles cookie expires.
        ///     
        /// Returns: 
        ///     An integer specifying the number of minutes before the roles cookie expires.
        ///     The default is 30 minutes.
        /// </summary>
        public static int CookieTimeout
        {
            get { return _cookieTimeout; }
        }
        /// <summary>
        /// Summary:
        ///     Gets a value indicating whether the role-names cookie is session-based or 
        ///     persistent.
        ///     
        /// Returns:
        ///     true if the role-names cookie is a persistent cookie; otherwise false.
        ///     The default is false.
        /// </summary>
        public static bool CreatePersistentCookie
        {
            get { return _createPersistentCookie; }
        }
        /// <summary>
        /// Summary: 
        ///     Gets the value of the domain of the role-names cookie.
        ///     
        /// Returns: 
        ///     The System.Web.HttpCookie.Domain of the role names cookie.
        /// </summary>
        public static string Domain
        {
            get { return _domain; }
        }
        /// <summary>
        /// Summary: 
        ///     Gets or sets a value indicating whether role management is enabled 
        ///     for the current Web application.
        ///     
        /// Returns:
        ///     true if role management is enabled; otherwise, false.
        ///     The default is false.
        /// </summary>
        public static bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }
        /// <summary>
        /// Summary: 
        ///     Gets the maximum number of roles names to be cached for a user.
        ///     
        /// Returns:
        ///     The maximun number of role names to be cached for a user.  
        ///     The default is 25.
        /// </summary>
        public static int MaxCachedResults
        {
            get { return _maxCachedResults; } 
        }
#endregion
    }
}
