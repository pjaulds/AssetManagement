using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qtech.Qasa.PluginInterface;
using System.Windows.Forms;
using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Setup.DepreciationMethod
{
    public class DepreciationMethod : IPlugin
    {
        #region IPlugin Members

        IPluginHost myPluginHost = null;
        string myPluginName = "Depreciation Method";
        string myPluginAuthor = "Paul";
        string myPluginDescription = "This module is the asset category form";
        string myPluginVersion = "1.0.0";
        string myGroupKey = "Depreciation Method";
        string myItemKey = "DepreciationMethod";
        string myImageName = @"images\purchase_request_50px.png";
        int myModuleId = 2002; 
        string myModulePart = "AssetManagement";
        Form myForm = new Default();

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
            myForm = new Default();
            Default f = (Default)myForm;
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
