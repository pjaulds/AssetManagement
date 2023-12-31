﻿using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qtech.AssetManagement.Utilities
{
    public static class ThemeUtil
    {
        public static void Controls(Control parent)
        {
            foreach (Control ctl in parent.Controls)
            {
                //clear contained controls
                if (ctl.Controls.Count > 0) Controls(ctl);

                if (ctl is TextBox)
                    ((TextBox)ctl).CharacterCasing = CharacterCasing.Upper;

                if (ctl is UltraTextEditor)
                    ((UltraTextEditor)ctl).CharacterCasing = CharacterCasing.Upper;

                if (ctl is Button)
                    ((Button)ctl).BackColor = Color.FromArgb(51, 63, 79);

                if (ctl is DataGridView)
                {
                    DataGridViewColorTheme(((DataGridView)ctl));
                }

            }
        }

        public static void ControlsWithoutUpperCase(Control parent)
        {
            foreach (Control ctl in parent.Controls)
            {
                //clear contained controls
                if (ctl.Controls.Count > 0) ControlsWithoutUpperCase(ctl);
                
                if (ctl is DataGridView)
                {
                    DataGridViewColorTheme(((DataGridView)ctl));
                }

            }
        }

        public static void DataGridViewColorTheme(DataGridView grid)
        {
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.GridColor = SystemColors.Control;
            grid.Font = new Font("Tahoma", float.Parse("8.25"));

            //column header
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 63, 79);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", float.Parse("8.25"));
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = grid.ColumnHeadersDefaultCellStyle.BackColor;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //alternating row style
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

            //row headers
            grid.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(224, 224, 224);
            grid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            //row
            grid.RowsDefaultCellStyle.Font = new Font("Tahoma", float.Parse("8.25"));
            grid.RowsDefaultCellStyle.BackColor = Color.White;
            grid.RowsDefaultCellStyle.SelectionBackColor = grid.ColumnHeadersDefaultCellStyle.SelectionBackColor;
        }
        public static void UltraGridThemeColor(object sender, InitializeLayoutEventArgs e)
        {
            //change the selection type
            e.Layout.Appearance.BackColor = Color.White;
            e.Layout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            e.Layout.Override.CellClickAction = CellClickAction.RowSelect;

            //for allow column swapping
            e.Layout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;

            // for filtering
            e.Layout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Override.FilterUIType = FilterUIType.FilterRow;

            //allow edit rows or not allow
            e.Layout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;

            //row
            e.Layout.Override.RowAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
            e.Layout.Override.RowAppearance.FontData.Name = "Tahoma";
            e.Layout.Override.RowAppearance.FontData.SizeInPoints = float.Parse("8.25");

            //seleted row
            e.Layout.Override.ActiveRowAppearance.BackColor = Color.FromArgb(51, 63, 79);

            //header backcolor
            e.Layout.Override.HeaderAppearance.BackColor2 = Color.FromArgb(51, 63, 79);
            e.Layout.Override.HeaderAppearance.BackColor = Color.FromArgb(51, 63, 79);
            e.Layout.Override.HeaderAppearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            e.Layout.Override.HeaderAppearance.FontData.Name = "Segoe UI Semibold";
            e.Layout.Override.HeaderAppearance.FontData.SizeInPoints = float.Parse("9");
            e.Layout.Override.HeaderAppearance.ForeColor = Color.White;

            //Remove expansion indicators and connectors
            e.Layout.Bands[0].Indentation = 0;
        }
    }
}
