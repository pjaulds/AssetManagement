using Infragistics.Win.UltraWinEditors;
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

                //if (ctl is TextBox)
                //    ((TextBox)ctl).CharacterCasing = CharacterCasing.Upper;

                //if (ctl is UltraTextEditor)
                //    ((UltraTextEditor)ctl).CharacterCasing = CharacterCasing.Upper;

                if (ctl is Button && ctl.Tag == null)
                    ((Button)ctl).BackColor = Color.FromArgb(28, 97, 161);

                if (ctl is DataGridView)
                {
                    DataGridViewColorTheme(((DataGridView)ctl));
                }

                //all font
                ctl.Font = new Font("Arial", ctl.Font.Size);
                
                if (ctl.Tag != null)
                {
                    switch (ctl.Tag.ToString())
                    {
                        case "headerlabel":
                            ctl.Font = new Font("Arial", float.Parse("8.25"), FontStyle.Bold);
                            ctl.ForeColor = Color.WhiteSmoke;
                            break;
                        case "headerlabelblack":
                            ctl.Font = new Font("Arial", float.Parse("8.25"), FontStyle.Bold);
                            ctl.ForeColor = Color.Black;
                            break;
                        case "headerLabelPopUpForm":
                            ctl.Font = new Font("Arial", float.Parse("10.25"), FontStyle.Bold);
                            ctl.ForeColor = Color.WhiteSmoke;
                            break;
                        case "headerpanel"://51, 63, 79
                            ctl.BackColor = Color.FromArgb(28, 97, 161);
                            break;
                    }
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
            grid.Font = new Font("Arial", float.Parse("8.25"));

            //column header
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 97, 161);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = grid.ColumnHeadersDefaultCellStyle.BackColor;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //alternating row style
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

            //row headers
            grid.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(224, 224, 224);
            grid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            //row
            grid.RowsDefaultCellStyle.Font = new Font("Arial", float.Parse("8.25"));
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
            e.Layout.Override.RowAppearance.FontData.Name = "Arial";
            e.Layout.Override.RowAppearance.FontData.SizeInPoints = float.Parse("8.25");

            //seleted row
            e.Layout.Override.ActiveRowAppearance.BackColor = Color.FromArgb(28, 97, 161);

            //header backcolor
            e.Layout.Override.HeaderAppearance.BackColor2 = Color.FromArgb(28, 97, 161);
            e.Layout.Override.HeaderAppearance.BackColor = Color.FromArgb(28, 97, 161);
            e.Layout.Override.HeaderAppearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            e.Layout.Override.HeaderAppearance.FontData.Name = "Arial";
            e.Layout.Override.HeaderAppearance.FontData.SizeInPoints = float.Parse("9");
            e.Layout.Override.HeaderAppearance.ForeColor = Color.White;
            e.Layout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

            //Remove expansion indicators and connectors
            e.Layout.Bands[0].Indentation = 0;
        }
    }
}
