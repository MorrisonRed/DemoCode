
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Net.Security;
using System.Security.Principal;
using System.Runtime.InteropServices;

namespace CustomSecurity
{
    public class ImpersonateUser : IDisposable
    {
        private int LOGON32_LOGON_INTERACTIVE = 2;

        private int LOGON32_PROVIDER_DEFAULT = 0;

        private WindowsImpersonationContext impersonationContext;
        [DllImport("advapi32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int LogonUserA(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]

        private static extern int DuplicateToken(IntPtr ExistingTokenHandle, int ImpersonationLevel, ref IntPtr DuplicateTokenHandle);
        [DllImport("advapi32.dll", EntryPoint = "GetUserNameA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        private static extern int GetUserName(string lpBuffer, ref int nSize);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]

        private static extern long RevertToSelf();
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern long CloseHandle(IntPtr handle);

        private bool _impersonationActive;
        private bool _disposed = false;

        #region "Public Properties"
        public bool ImpersonationActive
        {
            get { return _impersonationActive; }
        }
        #endregion

        #region "Constructors and Destructors"
        /// <summary>
        /// Allows creation of a new instance without starting impersonation
        /// </summary>
        /// <remarks></remarks>
        public ImpersonateUser()
        {
        }
        /// <summary>
        /// starts impersonation with specified parameters
        /// </summary>
        /// <param name="userName">username to impersonate</param>
        /// <param name="domain">domain for user</param>
        /// <param name="password">password for user</param>
        /// <remarks></remarks>
        public ImpersonateUser(string userName, string domain, string password)
        {
            StartImpersonation(userName, domain, password);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        { 
            //check to se if dispose has alread been called
            if (!_disposed)
            {
                if (disposing)
                { 
                    //free other state (managed objects)
                }
                //free your own state (unmanaged objects)
                _disposed = true;
            }
        }
        //C# destructor syntax for finalization code
        ~ImpersonateUser()
        {
            if (_impersonationActive)
            {
                impersonationContext.Undo();
                _impersonationActive = false;
            }
            Dispose(false);
        }
        //Do no override object.Finalize. Instead, provide a destructor

        #endregion

        #region "Public Function and Sub Procedures"
        /// <summary>
        /// Get User Name logged in on client computer 
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetUserName()
        {
            int iReturn = 0;
            int nSize = 50;
            string userName = null;
            userName = new string(Convert.ToChar(" "), 50);
            iReturn = GetUserName(userName, ref nSize);
            return userName.Substring(0, userName.IndexOf(Convert.ToChar(0)));
        }
        /// <summary>
        /// starts impersonation with specified parameters
        /// </summary>
        /// <param name="userName">username to impersonate</param>
        /// <param name="domain">domain for user</param>
        /// <param name="password">password for user</param>
        /// <remarks></remarks>
        public bool StartImpersonation(string userName, string domain, string password)
        {
            bool functionReturnValue = false;
            bool ReturnValue = false;
            WindowsIdentity tempWindowsIdentity = default(WindowsIdentity);
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            functionReturnValue = false;

            if (Convert.ToBoolean(RevertToSelf()))
            {
                if (LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        impersonationContext = tempWindowsIdentity.Impersonate();
                        if ((impersonationContext != null))
                        {
                            ReturnValue = true;
                            _impersonationActive = true;
                        }
                    }
                }
            }
            if (!tokenDuplicate.Equals(IntPtr.Zero))
            {
                CloseHandle(tokenDuplicate);
            }
            if (!token.Equals(IntPtr.Zero))
            {
                CloseHandle(token);
            }
            return ReturnValue;
            return functionReturnValue;
        }
        /// <summary>
        /// Ends impersonation session and returns thread back to original user
        /// </summary>
        /// <remarks></remarks>
        public void EndImpersonation()
        {
            if (_impersonationActive == true)
            {
                _impersonationActive = false;
                impersonationContext.Undo();
            }
        }
        #endregion
    }
}

