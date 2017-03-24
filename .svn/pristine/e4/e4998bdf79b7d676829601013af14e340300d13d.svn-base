using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.MobileManage.FilesUpload;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.OftenWords
{
    public partial class VTest : TBasicDataViewByMobile
    {
        string interphaseID = string.Empty;
        string controlID = string.Empty;
        string userID = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
        private AutomaticSize automaticSize = new AutomaticSize(); 
        public VTest()
        {
            InitializeComponent();
            interphaseID = this.Name;
            automaticSize.SetTag(this);
            //txtControlID.Text = "txtOftenWord";
        }

        private void btnGoSearch_Click(object sender, EventArgs e)
        {
            controlID = "txtOftenWord";
            string oftenWord = txtOftenWord.Text;
            VOftenWords vow = new VOftenWords(userID,interphaseID,controlID,oftenWord);
            vow.ShowDialog();
            string result = vow.Result;
            if (result != null)
            {
                txtOftenWord.Text = result;
            }
        }

        private void btnFileUpLoad_Click(object sender, EventArgs e)
        {
            string fileObjectType = "IRPDOCUMENT";
            string objectID = "1";
            string projectID = "2";
            string documentSort = "0101";
            string documentWorkflow = "";
            VFilesUpLoad vful = new VFilesUpLoad(fileObjectType, objectID, projectID, documentSort, documentWorkflow);
            vful.ShowDialog();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            controlID = "txtWord";
            string oftenWord = txtWord.Text;
            VOftenWords vow = new VOftenWords(userID, interphaseID, controlID, oftenWord);
            vow.ShowDialog();
            string result = vow.Result;
            if (result != null)
            {
                txtWord.Text = result;
            }
        }

        private void btnSelectDocument_Click(object sender, EventArgs e)
        {
            VSelectDocumentsList vsdl = new VSelectDocumentsList();
            vsdl.ShowDialog();
            IList result = vsdl.ResultList;
        }

        private void btnTemplateImport_Click(object sender, EventArgs e)
        {
            VTemplateImport vti = new VTemplateImport();
            vti.ShowDialog();
        }
    }
}
