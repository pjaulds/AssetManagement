using Infragistics.Win.UltraWinGrid;
using Qtech.AssetManagement.Bll;
using Qtech.AssetManagement.BusinessEntities;
using Qtech.AssetManagement.Utilities;
using Qtech.AssetManagement.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace Qtech.AssetManagement.Maintenance.Supplier
{
    public partial class Default : Form, ICRUD
    {
        public Default()
        {
            InitializeComponent();
        }

        #region Private Variables
        bool allow_select;
        bool allow_insert;
        bool allow_update;
        bool allow_delete;
        bool allow_print;
        #endregion

        #region Private Members
        private int _mId
        {
            get
            {
                if (ultraGrid1.ActiveRow.Index == -1)
                    return 0;
                else
                    return ((BusinessEntities.Supplier)ultraGrid1.ActiveRow.ListObject).mId;
            }
        }
        #endregion

        #region Private Methods
        private void LoadSupplier()
        {
            ultraGrid1.SetDataBinding(SupplierManager.GetList(), null, true);
            ultraGrid1.Refresh();
        }

        private int SaveSupplier()
        {
            BusinessEntities.Supplier item = new BusinessEntities.Supplier();
            LoadSupplierFromFormControls(item);

            //validate if all the rules of Status has been meet
            if (item.Validate())
            {
                Int32 id = SupplierManager.Save(item);
                EndEditing();
                LoadSupplier();

                return id;

            }
            else
            {
                ValidationListForm validationForm = new ValidationListForm();
                validationForm.mBrokenRules = item.BrokenRules;
                validationForm.ShowDialog();
                return 0;
            }
        }

        private void LoadSupplierFromFormControls(BusinessEntities.Supplier mySupplier)
        {
            mySupplier.mId = int.Parse(Idlabel.Text);
            mySupplier.mCode = CodetextBox.Text;
            mySupplier.mName = NametextBox.Text;
            mySupplier.mAddress = AddresstextBox.Text;
            mySupplier.mTin = TintextBox.Text;
            mySupplier.mContactNo = ContactNotextBox.Text;
            mySupplier.mEmail = EmailtextBox.Text;
            mySupplier.mSalesPerson = SalesPersontextBox.Text;
            mySupplier.mVatRegistered = VatRegisteredcheckBox.Checked;
            mySupplier.mVatRate = ControlUtil.TextBoxDecimal(VatRatetextBox);
            mySupplier.mWitholdingTax = ControlUtil.TextBoxDecimal(WitholdingTaxtextBox);
            mySupplier.mBusinessStyle = BusinessStyletextBox.Text;
            mySupplier.mActive = ActivecheckBox.Checked;
            mySupplier.mUserId = SessionUtil.mUser.mId;
        }

        private void LoadFormControlsFromUser(BusinessEntities.Supplier mySupplier)
        {
            Idlabel.Text = mySupplier.mId.ToString();
            CodetextBox.Text = mySupplier.mCode;
            NametextBox.Text = mySupplier.mName;
            AddresstextBox.Text = mySupplier.mAddress;
            TintextBox.Text = mySupplier.mTin;
            ContactNotextBox.Text = mySupplier.mContactNo;
            EmailtextBox.Text = mySupplier.mEmail;
            SalesPersontextBox.Text = mySupplier.mSalesPerson;
            VatRegisteredcheckBox.Checked = mySupplier.mVatRegistered;
            VatRatetextBox.Text = mySupplier.mVatRate.ToString();
            WitholdingTaxtextBox.Text = mySupplier.mWitholdingTax.ToString();
            BusinessStyletextBox.Text = mySupplier.mBusinessStyle;
            ActivecheckBox.Checked = mySupplier.mActive;
        }

        private void EndEditing()
        {
            ControlUtil.ClearConent(splitContainer1.Panel2);
            ControlUtil.HidePanel(splitContainer1);
            Idlabel.Text = "0";
        }
        #endregion

        #region ICRUD Members

        public void NewRecord()
        {
            if (!allow_insert)
            {
                MessageUtil.NotAllowedInsertAccess();
                return;
            }

            EndEditing();
            ControlUtil.ExpandPanel(splitContainer1);

            CodetextBox.Focus();

        }

        public int SaveRecords()
        {
            BrokenRulesCollection rules = new BrokenRulesCollection();

            SupplierCriteria criteria = new SupplierCriteria();
            criteria.mId = int.Parse(Idlabel.Text);
            criteria.mName = NametextBox.Text;
            if (SupplierManager.SelectCountForGetList(criteria) > 0)
                rules.Add(new BrokenRule("", "Name already exists."));

            criteria = new SupplierCriteria();
            criteria.mId = int.Parse(Idlabel.Text);
            criteria.mCode = CodetextBox.Text;
            if (SupplierManager.SelectCountForGetList(criteria) > 0)
                rules.Add(new BrokenRule("", "Code already exists."));

            if (rules.Count > 0)
            {
                ValidationListForm validationForm = new ValidationListForm();
                validationForm.mBrokenRules = rules;
                validationForm.ShowDialog();

                return 0;
            }
            return SaveSupplier();
        }

        public void CancelTransaction()
        {
            if (MessageUtil.AskCancelEdit())
                EndEditing();
        }

        public void DeleteRecords()
        {
            if (!allow_delete)
            {
                MessageUtil.NotAllowedDeleteAccess();
                return;
            }

            if (MessageUtil.AskDelete())
            {
                BusinessEntities.Supplier item = SupplierManager.GetItem(_mId);
                item.mUserId = SessionUtil.mUser.mId;

                SupplierManager.Delete(item);

                LoadSupplier();

            }
        }

        public void PrintRecords()
        {
            if (!allow_print)
            {
                MessageUtil.NotAllowedPrintAccess();
                return;
            }
        }

        #endregion

        #region Ultragrid 
        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            ThemeUtil.UltraGridThemeColor(sender, e);
        }

        private void ultraGrid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            if (e.Row.Index == -1)
                return;

            if (!allow_update)
            {
                MessageUtil.NotAllowedUpdateAccess();
                return;
            }

            BusinessEntities.Supplier item = SupplierManager.GetItem(_mId);
            LoadFormControlsFromUser(item);

            ControlUtil.ExpandPanel(splitContainer1);
            CodetextBox.Focus();
        }

        private void ultraGrid1_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
        {
            if (!allow_delete)
                MessageUtil.NotAllowedDeleteAccess();
            else
                DeleteRecords();

            e.Cancel = true;
        }
        #endregion

        private void expandPanelControl1__ExpandPanel(object sender, EventArgs e)
        {
            if (Idlabel.Text == "0" && allow_insert)
                ControlUtil.ExpandPanel(splitContainer1);
            else if (Idlabel.Text != "0" && allow_update)
                ControlUtil.ExpandPanel(splitContainer1);
        }

        private void collapsePanelControl1__HidePanel(object sender, EventArgs e)
        {
            ControlUtil.HidePanel(splitContainer1);
        }

        private void Default_Load(object sender, EventArgs e)
        {
            SessionUtil.UserValidate(ref allow_select, ref allow_insert,
                ref allow_update, ref allow_delete, ref allow_print,
                (int)Modules.Supplier);

            EndEditing();

            ThemeUtil.Controls(this);
            ControlUtil.TextBoxEnterLeaveEventHandler(splitContainer1.Panel2);
            LoadSupplier();
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            SaveSupplier();
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            CancelTransaction();
        }

        int cnt = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            //channel.QueueDeclare(queue: "letterbox", durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.ExchangeDeclare(exchange: "pubsub", type: ExchangeType.Fanout);//pubsub pattern
           
            cnt += 1;

            var encodedMessage = Encoding.UTF8.GetBytes(cnt.ToString());
            //channel.BasicPublish("", "letterbox", null, encodedMessage);
            channel.BasicPublish("pubsub", "", null, encodedMessage);//pubsub pattern
            connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: "pubsub", type: ExchangeType.Fanout);//pubsub pattern
            var queueName = channel.QueueDeclare().QueueName;//pubsub pattern
            channel.QueueBind(queue: queueName, exchange: "pubsub", routingKey: ""); //pubsub pattern


            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false); //round robin style consumer much better to use this

            //channel.BasicConsume("letterbox", false, consumer);
            channel.BasicConsume(queueName, false, consumer); //pubsub pattern
            //noAck was set to false originally it's set as true, we have to manually acknowledge the message so thatwe are sure that we processed the message correctly
            //the code is found bellow the event of Consumer_Received
            //EventingBasicConsumer channel = (EventingBasicConsumer)sender;
            //channel.Model.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);//manual acknowledment of receipt



        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            
            Audit audit = new Audit();
            audit.mUserId = SessionUtil.mUser.mId;
            AuditManager.Save(audit);
            EventingBasicConsumer channel = (EventingBasicConsumer)sender;
            channel.Model.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
        }
    }
}
