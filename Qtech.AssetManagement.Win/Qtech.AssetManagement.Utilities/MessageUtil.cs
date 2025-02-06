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
        public static string add_exemption = "You do not have the necessary permission to add new";
        public static string edit_exemption = "You do not have the necessary permission to modify";
        public static string delete_exemption = "You do not have the necessary permission to delete";
        public static string print_exemption = "You are not permitted to print record(s).";
        public static string close_message = "Are you sure you want to close window?.";
        public static string not_allow_approve = "You are not permitted to approve this transaction.";

        public static void NotAllowedInsertAccess()
        {
            MessageBox.Show(add_exemption, "Add Record", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static void NotAllowedInsertAccess(string module)
        {
            Message message = new Message();
            message.mWarning = true;
            message.mMessage = add_exemption + " " + module;
            message.ShowDialog();
        }

        static bool accept;
        public static bool SaveConfirm(string module)
        {
            accept = false;
            MessageQuestion message = new MessageQuestion();
            message.mMessage = "Are you sure you want to save this new " + " " + module +"?";
            message.FormClosing += MessageQuestion_FormClosing;
            message.ShowDialog();

            return accept;
        }

        public static void SaveSuccessfully(string msg)
        {
            Message message = new Message();
            message.mMessage = msg + " has been successfully created.";
            message.ShowDialog();
        }

        public static void UpdatedSuccessfully(string msg)
        {
            Message message = new Message();
            message.mMessage = msg + " details have been successfully updated.";
            message.ShowDialog();
        }

        public static void DeletedSuccessfully(string msg)
        {
            Message message = new Message();
            message.mMessage = msg + " has been deleted successfully.";
            message.ShowDialog();
        }

        public static bool DeleteConfirm(string msg)
        {
            accept = false;
            MessageQuestion message = new MessageQuestion();
            message.mMessage = "Are you sure you want to delete " + " " + msg + "? This action cannot be undone";
            message.FormClosing += MessageQuestion_FormClosing;
            message.ShowDialog();

            return accept;
        }

        public static bool ResetConfirm()
        {
            accept = false;
            MessageQuestion message = new MessageQuestion();
            message.mMessage = "Are you sure you want to reset all details entered in the transaction entry form?";
            message.FormClosing += MessageQuestion_FormClosing;
            message.ShowDialog();

            return accept;
        }

        private static void MessageQuestion_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageQuestion message = (MessageQuestion)sender;
            accept = message.mAccept;
        }

        public static bool CancelUpdateConfirm()
        {
            accept = false;
            MessageQuestion message = new MessageQuestion();
            message.mMessage = "You have unsaved changes. Do you want to discard the changes and exit?";
            message.FormClosing += MessageQuestion_FormClosing;
            message.ShowDialog();

            return accept;
        }

        public static void Error()
        {
            Message message = new Message();
            message.mWarning = true;
            message.mMessage = "An unexpected error occured while processing your request. Please try again or contact your system administrator if the issue persists.";
            message.ShowDialog();
        }

        public static void Error(string err)
        {
            Message message = new Message();
            message.mWarning = true;
            message.mMessage = "An unexpected error occured while processing your request. Please try again or contact your system administrator if the issue persists." + Environment.NewLine + err;
            message.ShowDialog();
        }

        public static void ErrorRetrieve()
        {
            Message message = new Message();
            message.mWarning = true;
            message.mMessage = "Unable to retrieve data due to a connection issue. Please ensure the database is accessible and try again";
            message.ShowDialog();
        }

        public static void Message(string msg)
        {
            Message message = new Message();
            message.mInformation = true;
            message.mMessage = msg;
            message.ShowDialog();
        }

        public static void Created(string msg)
        {
            Message message = new Message();
            message.mInformation = true;
            message.mMessage = msg + " has been successfully created.";
            message.ShowDialog();
        }

        public static void Updated(string msg)
        {
            Message message = new Message();
            message.mInformation = true;
            message.mMessage = msg + " have been successfully updated.";
            message.ShowDialog();
        }

        public static void Deleted(string msg)
        {
            Message message = new Message();
            message.mInformation = true;
            message.mMessage = msg + " has been successfully deleted.";
            message.ShowDialog();
        }

        public static void NotAllowedUpdateAccess()
        {
            MessageBox.Show(edit_exemption, "Update Record", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static void NotAllowedUpdateAccess(string module)
        {
            Message message = new Message();
            message.mWarning = true;
            message.mMessage = edit_exemption + " " + module;
            message.ShowDialog();
        }


        public static void NotAllowedDeleteAccess()
        {
            Message message = new Message();
            message.mWarning = true;
            message.mMessage = delete_exemption;
            message.ShowDialog();
        }

        public static void NotAllowedDeleteAccess(string module)
        {
            Message message = new Message();
            message.mWarning = true;
            message.mMessage = delete_exemption + " " + module;
            message.ShowDialog();
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
            //return MessageBox.Show("Are you sure you want to delete record(s)?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
            return Banner.Systems.Utilities.MessageUtil.AskDelete();
        }

        public static Boolean AskCancelEdit()
        {
            return MessageBox.Show("Are you sure you want cancel updating record(s)?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}
