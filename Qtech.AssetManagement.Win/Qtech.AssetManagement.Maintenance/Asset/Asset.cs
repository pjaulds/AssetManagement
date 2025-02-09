﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qtech.Qasa.PluginInterface;
using System.Windows.Forms;
using Qtech.AssetManagement.BusinessEntities;

namespace Qtech.AssetManagement.Maintenance
{
    public class FixedAsset : IPlugin
    {
        #region IPlugin Members

        IPluginHost myPluginHost = null;
        string myPluginName = "Asset Registration";
        string myPluginAuthor = "Paul";
        string myPluginDescription = "This module is the fixed asset form";
        string myPluginVersion = "1.0.0";
        string myGroupKey = "Fixed Asset";
        string myItemKey = "FixedAsset";
        string myImageName = @"images\purchase_request_50px.png";
        int myModuleId = 2000; // 2000 is for qasa integration Convert.ToInt32(Modules.FixedAsset);
        string myModulePart = "Asset";
        Form myForm = new Asset.Default();

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
            myForm = new Asset.Default();
            Asset.Default f = (Asset.Default)myForm;
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
