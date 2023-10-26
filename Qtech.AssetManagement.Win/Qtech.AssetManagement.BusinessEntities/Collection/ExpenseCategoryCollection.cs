// -----------------------------------------------------------------------
// <copyright file="ExpenseCategoryCollection.cs" company="qtechbsi.com">
//   Copyright 2019 - 2020 - qtechbsi.com. All rights reserved.
// </copyright>// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Qtech.AssetManagement.BusinessEntities
{
    /// <summary>
    /// The ExpenseCategoryCollection class is designed to work with lists of instances of ExpenseCategory.
    /// </summary>
    public class ExpenseCategoryCollection : BusinessCollectionBase<ExpenseCategory>
    {

        /// <summary>
        /// Initializes a new instance of the ExpenseCategoryCollection class.
        /// </summary>
        public ExpenseCategoryCollection() { }

        /// <summary>
        /// Initializes a new instance of the ExpenseCategoryCollection class.
        /// </summary>
        public ExpenseCategoryCollection(IList<ExpenseCategory> initialList) : base(initialList) { }
    }
}