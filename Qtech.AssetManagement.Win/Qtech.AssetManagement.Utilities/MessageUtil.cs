using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Qtech.AssetManagement.Utilities
{
    public static class MessageUtil
    {
        public static string view_exemption = "You are not permitted to view record(s).";
        public static string add_exemption = "You are not permitted to add record(s).";
        public static string edit_exemption = "You are not permitted to update record(s).";
        public static string delete_exemption = "You are not permitted to delete record(s).";
        public static string print_exemption = "You are not permitted to print record(s).";
        public static string close_message = "Are you sure you want to close window?.";
        public static string not_allow_approve = "You are not permitted to approve this transaction.";

        public static void NotAllowedInsertAccess()
        {
            MessageBox.Show(add_exemption, "Add Record", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static void NotAllowedUpdateAccess()
        {
            MessageBox.Show(edit_exemption, "Update Record", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static void NotAllowedDeleteAccess()
        {
            MessageBox.Show(delete_exemption, "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static void NotAllowedPrintAccess()
        {
            MessageBox.Show(print_exemption, "Print", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static void NotAllowedViewAccess()
        {
            MessageBox.Show(view_exemption, "View Records", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static void NotAllowedApproveAccess()
        {
            MessageBox.Show(not_allow_approve, "Approve", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static void RecordAlreadyApprovedOrPosted()
        {
            MessageBox.Show("Cannot continue.\nSelected record was already approved or posted", "Update", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static void RecordNotApprove()
        {
            MessageBox.Show("Cannot continue.\nSelected record is not yet approve", "Record", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static void RecordNotPosted()
        {
            MessageBox.Show("Cannot continue.\nSelected record is not yet posted", "Record", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static Boolean AskDelete()
        {
            return MessageBox.Show("Are you sure you want to delete record(s)?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        public static Boolean AskCancelEdit()
        {
            return MessageBox.Show("Are you sure you want cancel updating record(s)?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}
