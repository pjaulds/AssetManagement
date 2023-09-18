using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qtech.AssetManagement.Validation;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The BusinessCollectionBase class serves as the base class for all main business entity collections.
    /// It overcomes limitations of the generic Collection&lt;T&gt; class by implementing a Sort method.
    /// </summary>
    /// <typeparam name="T">A class that inherits ValidationBase.</typeparam>
    public class BusinessCollectionBase<T> : ValidationCollectionBase<T> where T : ValidationBase
    {
        /// <summary>
        /// Initializes a new instance of the BusinessCollectionBase class.
        /// </summary>
        public BusinessCollectionBase() { }

        /// <summary>
        /// Initializes a new instance of the BusinessCollectionBase class and populates it with the initial list.
        /// </summary>
        public BusinessCollectionBase(IList<T> initialList) : base(initialList) { }

        /// <summary>
        /// Sorts the collection based on the specified comparer.
        /// </summary>
        /// <param name="comparer">The comparer that is used to sort this collection.</param>
        public void Sort(IComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer", "Comparer is null.");
            }
            List<T> list = this.Items as List<T>;
            if (list == null)
            {
                return;
            }
            list.Sort(comparer);
        }
    }
}
