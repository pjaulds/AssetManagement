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
    public static class ControlUtil
    {
        public static void ClearConent(Control parent)
        {
            foreach (Control ctl in parent.Controls)
            {
                //clear contained controls
                if (ctl.Controls.Count > 0) ClearConent(ctl);

                if (ctl is TextBox)
                    ((TextBox)ctl).Text = string.Empty;
                else if (ctl is UltraTextEditor)
                    ((UltraTextEditor)ctl).Text = string.Empty;
                else if (ctl is MaskedTextBox)
                    ((MaskedTextBox)ctl).Text = string.Empty;
                else if (ctl is Label && ctl.Tag == "id")
                    ((Label)ctl).Text = "0";
                else if (ctl is CheckBox)
                    ((CheckBox)(ctl)).Checked = false;
                else if (ctl is RadioButton)
                    ((RadioButton)(ctl)).Checked = false;
                else if (ctl is ComboBox)
                    ((ComboBox)(ctl)).SelectedIndex = -1;
                else if (ctl is UltraCombo)
                {
                    ((UltraCombo)(ctl)).Value = null;
                    ((UltraCombo)(ctl)).Text = string.Empty;
                }
                else if (ctl is DateTimePicker)
                {
                    ((DateTimePicker)(ctl)).Value = DateTime.Now;
                    ((DateTimePicker)(ctl)).Checked = false;
                }
            }

        }

        public static void ExpandPanel(SplitContainer mySplitContainer)
        {
            mySplitContainer.Panel1MinSize = 150;
            mySplitContainer.Panel2MinSize = 30;
            mySplitContainer.Panel2Collapsed = false;
            mySplitContainer.SplitterDistance = 79;
        }

        public static void HidePanel(SplitContainer mySplitContainer)
        {
            mySplitContainer.Panel2Collapsed = true;
        }
        public static void TextBoxEnterLeaveEventHandler(Control panel)
        {
            foreach (Control control in panel.Controls)
            {
                if (control is TextBox || control is UltraCombo || control is CheckBox || control is RadioButton || control is DateTimePicker || control is ComboBox || control is MaskedTextBox || control is UltraTextEditor)
                {
                    control.KeyDown += new KeyEventHandler(control_KeyDown);
                    control.Enter += new EventHandler(control_Enter);
                    control.Leave += new EventHandler(control_Leave);
                }

                if (control.HasChildren)
                {
                    TextBoxEnterLeaveEventHandler(control);
                }
            }
        }

        public static void TextBoxEnterLeaveNoKeyDownEventHandler(Control panel)
        {
            foreach (Control control in panel.Controls)
            {
                if (control is TextBox)
                {
                    control.Enter += new EventHandler(control_Enter);
                    control.Leave += new EventHandler(control_Leave);
                }

                if (control.HasChildren)
                {
                    TextBoxEnterLeaveNoKeyDownEventHandler(control);
                }
            }
        }

        static void control_Leave(object sender, EventArgs e)
        {
            ((Control)sender).BackColor = Color.FromKnownColor(KnownColor.Window);
        }

        static void control_Enter(object sender, EventArgs e)
        {
            ((Control)sender).BackColor = Color.Yellow;
        }

        static void control_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Return) return;

            e.Handled = false;
            SendKeys.Send("{TAB}");
        }

        public static Int32 UltraComboReturnValue(UltraCombo myUltraCombo)
        {
            return Convert.ToInt32(myUltraCombo.Value);
        }

        public static void TextBoxNumericDecimal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            { e.Handled = false; }
            else if (Convert.ToInt32(e.KeyChar) == 8)
                e.Handled = false;
            else if (Convert.ToInt32(e.KeyChar) == 46)
                e.Handled = false;
            else
                e.Handled = true;
        }

        public static Decimal TextBoxDecimal(TextBox myTextBox)
        {
            return myTextBox.Text == string.Empty ? 0 : Convert.ToDecimal(myTextBox.Text);
        }

        public static int TextBoxInt(TextBox myTextBox)
        {
            return myTextBox.Text == string.Empty ? 0 : Convert.ToInt32(myTextBox.Text);
        }

        public static short TextBoxShort(TextBox myTextBox)
        {
            return myTextBox.Text == string.Empty ? Convert.ToInt16(0) : Convert.ToInt16(myTextBox.Text);
        }

        public static Byte TextBoxByte(TextBox myTextBox)
        {
            return myTextBox.Text == string.Empty ? Convert.ToByte(0) : Convert.ToByte(myTextBox.Text);
        }

        public static bool IsTextBoxEmpty(TextBox myTextBox)
        {
            return string.IsNullOrEmpty(myTextBox.Text.Trim());
        }

        public static bool IsUltraComboEmpty(UltraCombo myUltraCombo)
        {
            return ControlUtil.UltraComboReturnValue(myUltraCombo) == 0;
        }
    }
}
