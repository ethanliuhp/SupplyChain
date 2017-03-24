using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
//测试
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using System.IO;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using IRPServiceModel.Domain.Document;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.Util;
using Application.Resource.CommonClass.Domain;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng
{
    public partial class VDocumentMasterInfo : TBasicDataView
    {

        public MDocumentCategory model = new MDocumentCategory();

        //所属项目
        public CurrentProjectInfo projectInfo = null;
        private bool isModel = false;
        public PersonInfo person = null;
        private DocumentMaster master = null;
        private DocumentCategory cate = null;
        private DocumentMaster result;
        //private Login login = null;
        public DocumentMaster Result
        {
            get { return result; }
            set { result = value; }
        }
        public VDocumentMasterInfo()
        {
            InitializeComponent();
        }

        public VDocumentMasterInfo(DocumentMaster m, DocumentCategory c,bool isOrNotModel)
        {
            InitializeComponent();
            master = m;
            cate = c;
            isModel = isOrNotModel;
            InitEvent();
            InitData();
            InitcomboBoxData();
            if (m != null)
            {
                InitUpdateData();
            }
        }
        void InitcomboBoxData()
        {
            //文档信息类型
            foreach (string infoType in Enum.GetNames(typeof(DocumentInfoTypeEnum)))
            {
                cmbDocumentInforType.Items.Add(infoType);
            }
            //文档状态
            foreach (DocumentState state in Enum.GetValues(typeof(DocumentState)))
            {
                cmbDocumentStatus.Items.Add(ClientUtil.GetDocStateName(state));
            }
            //文档密级
            foreach (string securityLevel in Enum.GetNames(typeof(DocumentSecurityLevelEnum)))
            {
                cmbSecurityLevel.Items.Add(securityLevel);
            }
            //文档检出状态
            //foreach (string checkoutState in Enum.GetNames(typeof(DocumentCheckOutStateEnum)))
            //{
            //    cmbCheckoutState.Items.Add(checkoutState);
            //}
            ////文档更新模式
            //foreach (string updateMode in Enum.GetNames(typeof(DocumentUpdateModeEnum)))
            //{
            //    cmbUpdateMode.Items.Add(updateMode);
            //}
            this.cmbDocumentInforType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbDocumentStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbSecurityLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCheckoutState.DropDownStyle = ComboBoxStyle.DropDownList;
            //this.cmbUpdateMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDocumentInforType.SelectedIndex = 0;
            cmbDocumentStatus.SelectedIndex = 0;
            cmbSecurityLevel.SelectedIndex = 0;
            //cmbCheckoutState.SelectedIndex = 0;
            //cmbUpdateMode.SelectedIndex = 0;
        }

        void InitData()
        {
            object[] os = new object[] { txtResideProject };
            ObjectLock.Lock(os);

            //归属项目
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            if (projectInfo != null)
            {
                txtResideProject.Tag = projectInfo;
                txtResideProject.Text = projectInfo.Name;
            }
            person = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;
            //login = CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login;

            //if (master == null ||master.Id == null)
            //{
            //    cmbUpdateMode.Enabled = false;
            //}
        }

        void InitUpdateData()
        {
            txtDocumentName.Text = master.Name;
            txtDocumentAuthor.Text = master.Author;
            txtDocumentCode.Text = master.Code;
            txtDocumentKeywords.Text = master.KeyWords;
            txtDocumentExplain.Text = master.Description;
            txtDocumentTitle.Text = master.Title;
            cmbDocumentInforType.Text = master.DocType.ToString();
            cmbDocumentStatus.Text = ClientUtil.GetDocStateName(master.State);
            cmbSecurityLevel.Text = master.SecurityLevel.ToString();
            //cmbCheckoutState.Text = master.CheckoutState.ToString();
            //if (master.IsInspectionLot)
            //{
            //    cbIsOrTemp.Checked = true;
            //}
        }

        void InitEvent()
        {
            btnSave.Click += new EventHandler(btnSave_Click);
            btnQuit.Click += new EventHandler(btnQuit_Click);
        }

        //保存
        void btnSave_Click(object sender, EventArgs e)
        {
            if (!Verify()) return;

            DocumentInfoTypeEnum docInfoType = 0;
            foreach (DocumentInfoTypeEnum type in Enum.GetValues(typeof(DocumentInfoTypeEnum)))
            {
                if (type.ToString() == cmbDocumentInforType.Text.Trim())
                {
                    docInfoType = type;
                    break;
                }
            }
            DocumentState docState = 0;
            foreach (DocumentState state in Enum.GetValues(typeof(DocumentState)))
            {
                if (ClientUtil.GetDocStateName(state) == cmbDocumentStatus.Text.Trim())
                {
                    docState = state;
                    break;
                }
            }

            DocumentSecurityLevelEnum securityLevel = 0;
            foreach (DocumentSecurityLevelEnum level in Enum.GetValues(typeof(DocumentSecurityLevelEnum)))
            {
                if (level.ToString() == cmbSecurityLevel.Text.Trim())
                {
                    securityLevel = level;
                    break;
                }
            }
            //DocumentCheckOutStateEnum checkOutState = 0;
            //foreach (DocumentCheckOutStateEnum check in Enum.GetValues(typeof(DocumentCheckOutStateEnum)))
            //{
            //    if (check.ToString() == cmbCheckoutState.Text.Trim())
            //    {
            //        checkOutState = check;
            //        break;
            //    }
            //}
            //DocumentUpdateModeEnum updateMode = 0;
            //foreach (DocumentUpdateModeEnum mode in Enum.GetValues(typeof(DocumentUpdateModeEnum)))
            //{
            //    if (mode.ToString() == cmbUpdateMode.Text.Trim())
            //    {
            //        updateMode = mode;
            //        break;
            //    }
            //}
            if (master == null || master.Id==null)//|| updateMode == DocumentUpdateModeEnum.添加一个新版次文件
            {
                master = new DocumentMaster();
                if (cate != null)
                {
                    master.Category = cate;
                    master.CategoryCode = cate.Code;
                    master.CategoryName = cate.Name;
                    master.CategorySysCode = cate.SysCode;
                }
                if (isModel)
                {
                    master.ProjectCode = "KB";
                    master.ProjectName = "知识库";
                }
                else
                {
                    master.ProjectCode = projectInfo.Code;
                    master.ProjectId = projectInfo.Id;
                    master.ProjectName = projectInfo.Name;
                }
                master.CreateTime = DateTime.Now;
            }

            master.OwnerID = person;
            master.OwnerName = person.Name;
            master.OwnerOrgSysCode = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheOperationOrg.SysCode;

            master.Name = txtDocumentName.Text;
            master.Code = txtDocumentCode.Text;
            master.Author = txtDocumentAuthor.Text;
            master.KeyWords = txtDocumentKeywords.Text;
            master.Description = txtDocumentExplain.Text;
            master.Title = txtDocumentTitle.Text;

            master.DocType = docInfoType;
            //master.CheckoutState = checkOutState;
            master.SecurityLevel = securityLevel;
            master.State = docState;

            //if (cbIsOrTemp.Checked)
            //{
            //    master.IsInspectionLot = true;
            //}
            //else
            //{
            //    master.IsInspectionLot = false;
            //}
            master.IsInspectionLot = isModel;
            //master.NGUID
            if (string.IsNullOrEmpty(master.VersionMajor))
                master.SetNewVersion();
            if (string.IsNullOrEmpty(master.Revision))
                master.SetNewRevision();

            
            master.UpdateTime = DateTime.Now;
            master = model.SaveOrUpdate(master) as DocumentMaster;
            result = master;
            this.Close();
        }
        //保存前验证
        bool Verify()
        {
            if (txtDocumentName.Text.Trim() == "")
            {
                MessageBox.Show("文档名称不能为空！");
                return false;
            }
            if (txtDocumentCode.Text.Trim() == "")
            {
                MessageBox.Show("文档代码不能为空！");
                return false;
            }
            return true;
        }

        // 放弃
        void btnQuit_Click(object sender, EventArgs e)
        {
            result = null;
            this.Close();
        }
    }
}
