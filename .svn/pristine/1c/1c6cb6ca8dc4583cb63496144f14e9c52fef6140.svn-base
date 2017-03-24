using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Main;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonForm;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VSelectGWBSTree_cbx : TBasicDataView
    {
        private IList _rtnGWBSTreeNodes;

        ///<summary>
        ///返回的GWBS树节点列表
        ///</summary>
        public virtual IList RtnGWBSTreeNodes
        {
            set { this._rtnGWBSTreeNodes = value; }
            get { return this._rtnGWBSTreeNodes; }
        }

        private IList _paramGWBSTreeNodes;

        ///<summary>
        ///传入的ＧＷＢＳ树节点列表
        ///</summary>
        public virtual IList ParamGWBSTreeNodes
        {
            set { this._paramGWBSTreeNodes = value; }
            get { return this._paramGWBSTreeNodes; }
        }

        public VSelectGWBSTree_cbx()
        {
            InitializeComponent();
        }
    }
   
}
