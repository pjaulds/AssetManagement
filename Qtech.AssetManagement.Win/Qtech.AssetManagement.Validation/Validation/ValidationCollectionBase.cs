// -----------------------------------------------------------------------
// <copyright file="ValidationCollectionBase.cs" company="Imar.Spaanjaars.Com">
//   Copyright 2008 - 2009 - Imar.Spaanjaars.Com. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Qtech.AssetManagement.Validation
{
    /// <summary>
    /// The ValidationCollectionBase class serves as the base class for collections like BusinessCollectionBase.
    /// The entire collection class provides validation by checking the validity of the ValidationBase
    /// instances in its Items collection.
    /// </summary>
    /// <typeparam name="T">A class inheriting from ValidationBase.</typeparam>
    public abstract class ValidationCollectionBase<T> : Collection<T> where T : ValidationBase
    {
        /// <summary>
        /// Initializes a new instance of the ValidationCollection class.
        /// </summary>
        public ValidationCollectionBase() : base(new List<T>()) { }

        /// <summary>
        /// Initializes a new instance of the ValidationCollection class and populates it with the initial list.
        /// </summary>
        public ValidationCollectionBase(IList<T> initialList) : base(initialList) { }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool Validate()
        {
            foreach (T item in this)
            {
                if (!item.Validate())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
