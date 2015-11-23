using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CustomSecurity
{
    /// <summary>
    /// A collection of MBRLL.Security.MembershipUser objects
    /// </summary>
    [DataObject]
    [Serializable]
    public sealed class MembershipUserCollection
    {
        private int _count;
        private bool _isSynchronized;
        private object _syncRoot; 
        

        #region Public Properties
        /// <summary>
        /// Summary:
        ///     Gets the number of membership user objects in the collection
        ///     
        /// Returns:
        ///     The number of Security.MembershipUser objects in the collection.
        /// </summary>
        public int Count
        {
            get { return _count; }
        }
        /// <summary>
        /// Summary:
        ///     Gets a value indicating whether the membership user collection is thread safe.
        ///     
        /// Returns:
        ///     Always false because thread-safe membership user collections are not supported.
        /// </summary>
        public bool IsSynchronized
        {
            get { return _isSynchronized; }
        }
        /// <summary>
        /// Summary:
        ///     Gets the synchronization root.
        ///     
        /// Returns:
        ///     Always this, because synchronization of membership user collections is not supported.
        /// </summary>
        public object SyncRoot
        {
            get { return _syncRoot; }
        }
        /// <summary>
        /// Summary: 
        ///     Gets the membership user in the collection referenced by the specified
        ///     user name.
        /// </summary>
        /// <param name="name">
        /// The Security.MembershipUser.UserName of the Security.MembershipUser
        /// to retrieve from the collection.
        /// </param>
        /// <returns>A Security.MembershipUser object representing the user specified by name.</returns>
        public MembershipUser this[string name] 
        {
            get { return null; } 
        }
        #endregion

        #region Constructors and Destructors
        /// <summary>
        /// Creates a new, empty membership user collection.
        /// </summary>
        public MembershipUserCollection()
        {
        }
        #endregion

        #region Functions and Sub Routines
        /// <summary>
        /// Gets an enumeration that can iterate through the membership user collection.
        /// </summary>
        /// <returns>An System.Collections.IEnumerator for the entire Security.MembershipUserCollection.</returns>
        public IEnumerator GetEnumerator()
        {
            return null;
        }
        /// <summary>
        /// Makes the contents of the membership user collection read-only.
        /// </summary>
        public void SetReadOnly()
        {

        }

        /// <summary>
        /// Adds the specified membership user to the collection
        /// </summary>
        /// <param name="user">a Security.MembershipUser object to add to the collection.</param>
        /// <exception>
        /// System.NotSupportedException:
        ///     The collection is read-only.
        ///     
        /// System.ArgumentNullException:
        ///     The Security.MembershipUser.UserName of the user is null.
        ///     
        /// System.ArgumentException:
        ///     A Security.MembershipUser object with the same Security.MembershipUser.UserName
        ///     value as user already exists in the collection.
        /// </exception>
        public void Add(MembershipUser user)
        {

        }

        /// <summary>
        /// Removes the membership user object with the specified user name from the collection.
        /// </summary>
        /// <param name="name">The user name of the Security.MembershipUser object to remove from the collection</param>
        /// <exception>
        /// System.NotSupportedException:
        ///     The collection is read-only.
        /// </exception>
        public void Remove(string name)
        {
            
        }

        /// <summary>
        /// Removes all membership user objects from the collection.
        /// </summary>
        public void Clear()
        {

        }

        /// <summary>
        /// Copies the membership user collection to a one-dimension array.
        /// </summary>
        /// <param name="array">
        /// A one-dimension array of type Security.MembershipUser that is 
        /// the destination of the elements copied from the Security.MembershipUserCollection.
        /// The array must have zero-based indexing.
        /// </param>
        /// <param name="index">The zero-based index in the array at which coppying begins.</param>
        public void CopyTo(MembershipUser[] array, int index)
        {

        }
        #endregion
    }
}
