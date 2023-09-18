using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qtech.AssetManagement.Utilities
{
    public interface IComboSelection
    {
        /// <summary>
        /// refresh the data selection of all combobox, ultracombo
        /// datagridview column
        /// so that the user will not need to close the window
        /// and open in 2nd time
        /// </summary>
        void RefreshAllSelection();
    }

    public interface IComboSelectionWithInactive : IComboSelection
    {

        void RefreshAllSelectionWithInactive();
    }
}
