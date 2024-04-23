using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qtech.Qasa.PluginInterface;
using System.Windows.Forms;
using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.DepreciationSchedule
{
    public class DepreciationSchedule : IPlugin
    {
        #region IPlugin Members

        IPluginHost myPluginHost = null;
        string myPluginName = "DepreciationS chedule";
        string myPluginAuthor = "Paul";
        string myPluginDescription = "This module is the fixed asset form";
        string myPluginVersion = "1.0.0";
        string myGroupKey = "Fixed Asset";
        string myItemKey = "DepreciationSchedule";
        string myImageName = @"images\purchase_request_50px.png";
        int myModuleId = 2006;
        string myModulePart = "AssetManagement";
        Form myForm = new Default2();

        public IPluginHost Host
        {
            get { return myPluginHost; }
            set
            {
                myPluginHost = value;
            }
        }

        public string mName
        {
            get { return myPluginName; }
        }

        public string mDescription
        {
            get { return myPluginDescription; }
        }

        public string mAuthor
        {
            get { return myPluginAuthor; }
        }

        public string mVersion
        {
            get { return myPluginVersion; }
        }

        public string mGroupKey
        {
            get { return myGroupKey; }
        }

        public string mItemKey
        {
            get { return myItemKey; }
        }

        public string mImageName
        {
            get { return myImageName; }
        }

        public int mModuleId
        {
            get { return myModuleId; }
        }
        public Form mForm
        {
            get { return myForm; }
        }

        public void Initialize()
        {
            myForm = new Default2();
            Default2 f = (Default2)myForm;
            f.PluginHost = this.Host;
            f.Plugin = this;
        }

        public void Dispose()
        {

        }

        public string mModulePart
        {
            get { return myModulePart; }
        }
        #endregion
    }
}
