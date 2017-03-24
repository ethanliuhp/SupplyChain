using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyPlanMng;
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using VirtualMachine.Core;
using System.IO;
using System.Diagnostics;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyOrderMngCompany
{
    public partial class VSupplyOrderCompany : TMasterDetailView
    {
        private MSupplyOrderMng model = new MSupplyOrderMng();
        private MSupplyPlanMng Supplymodle = new MSupplyPlanMng();
        private SupplyPlanMaster supplyMaster;
        private SupplyOrderMaster curBillMaster;
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        IList list_price = new ArrayList();

        private ProObjectRelaDocument oprDocument = null;
        string userName = string.Empty;
        string jobId = string.Empty;
        string fileObjectType = string.Empty;

        private EnumSupplyType supplyType;
        /// <summary>
        /// 用来区分专业
        /// </summary>
        public EnumSupplyType SupplyType
        {
            get { return supplyType; }
            set { supplyType = value; }
        }

        /// <summary>
        /// 当前单据
        /// </summary>
        public SupplyOrderMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VSupplyOrderCompany()
        {
            InitializeComponent();
            InitEvent();
            InitZYFL();
            InitProjectStatus();
        }

        private void InitProjectStatus()
        {
            //添加工程状态下拉框
            VBasicDataOptr.InitProjectStatus(colProjectStatus, false);
            dgDetail.ContextMenuStrip = cmsDg;
            VBasicDataOptr.InitProfessionCategory(cboProfessionCat, false);
        }

        private void InitZYFL()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            this.txtMaterialCategory.rootCatCode = "01";
            //添加专业分类下拉框
            VBasicDataOptr.InitProfessionCategory(colzyfl, false);

            dgDetail.ContextMenuStrip = cmsDg;
            dgProject.ContextMenuStrip = cmsDgProject;

            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode + "-";
            userName = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Code;
            jobId = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.TheSysRole.Id;
            list_price.Add("I1100101");
            list_price.Add("I1100102");
            list_price.Add("I1100110");
            list_price.Add("I1100111");
            list_price.Add("I1100112");
            list_price.Add("I1100113");
        }

        private void InitEvent()
        {
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dgDetail_EditingControlShowing);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            //this.dgDetail.UserDeletingRow += new DataGridViewRowCancelEventHandler(dgDetail_UserDeletingRow);
            //按钮事件
            //this.btnDemandSearch.Click += new EventHandler(btnDemandSearch_Click);//需求总计划
            //this.btnSupplySearch.Click += new EventHandler(btnSupplySearch_Click);//采购计划
            this.btnSet.Click += new EventHandler(btnSet_Click);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            txtMaterialCategory.Leave += new EventHandler(txtMaterialCategory_Leave);
            cboProfessionCat.SelectedIndexChanged += new EventHandler(cboProfessionCat_SelectedIndexChanged);

            //相关文档
            btnAddDocument.Click += new EventHandler(btnAddDocument_Click);
            btnModifyDocument.Click += new EventHandler(btnModifyDocument_Click);
            btnDeleteDocument.Click += new EventHandler(btnDeleteDocument_Click);
            btnSaveDocument.Click += new EventHandler(btnSaveDocument_Click);
            btnBrowse.Click += new EventHandler(btnBrowse_Click);
            btnDownLoad.Click += new EventHandler(btnDownLoad_Click);

            //项目信息
            dgProject.CellDoubleClick += new DataGridViewCellEventHandler(dgProject_CellDoubleClick);
            tsmiDelDgProject.Click += new EventHandler(tsmiDelDgProject_Click);
        }

        void tsmiDelDgProject_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr =dgProject.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgProject.Rows.Remove(dr);
                if (dr.Tag != null)
                {
                    curBillMaster.ProjectDetails.Remove(dr.Tag as SupplyOrderProjectDetail);
                }
            }
        }

        void dgProject_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (dgProject.Columns[e.ColumnIndex].Name == colProjectName.Name)
            {
                VProjectSelector vps = new VProjectSelector(this.model);
                vps.ShowDialog();
                IList projectLst = vps.Result;
                if (projectLst.Count > 0)
                {
                    foreach (CurrentProjectInfo pi in projectLst)
                    {
                        if (ProjectAdded(pi)) continue;

                        AddProject(pi);
                    }
                }
            }
        }

        private bool ProjectAdded(CurrentProjectInfo pi)
        {
            foreach (DataGridViewRow dr in dgProject.Rows)
            {
                CurrentProjectInfo tempPi = dr.Cells[colProjectName.Name].Tag as CurrentProjectInfo;
                if (tempPi != null && tempPi.Id == pi.Id)
                {
                    return true;
                }
            }
            return false;
        }

        private void AddProject(CurrentProjectInfo pi)
        {
            int i = dgProject.Rows.Add();
            dgProject[colProjectName.Name, i].Value = pi.Name;
            dgProject[colProjectName.Name, i].Tag = pi;
        }

        void cboProfessionCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pc = cboProfessionCat.SelectedItem as string;
            if (pc != null)
            {
                foreach (DataGridViewRow dr in dgDetail.Rows)
                {
                    if (dr.IsNewRow) break;
                    dr.Cells[colzyfl.Name].Value = pc;
                }
            }
        }

        void txtMaterialCategory_Leave(object sender, EventArgs e)
        {
            //IList result = txtMaterialCategory.Result;
            //if (result != null && result.Count > 0)
            //{
            //MaterialCategory mc = result[0] as MaterialCategory;
            //if (mc.Level == 2) return;
            //MaterialCategory firstMc = FindFirstCategory(mc);
            //result.Clear();
            //result.Add(firstMc);
            //txtMaterialCategory.Text = firstMc.Name;
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                if (dr.IsNewRow) break;
                dr.Cells[colwzfl.Name].Value = txtMaterialCategory.Text;
                dr.Cells[colwzfl.Name].Tag = txtMaterialCategory.Result[0] as MaterialCategory;
            }
            //}
        }

        /// <summary>
        /// 查找一级分类
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        private MaterialCategory FindFirstCategory(MaterialCategory mc)
        {
            //if (mc.Level == 2) return mc;
            return FindFirstCategory((MaterialCategory)mc.ParentNode);
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
        }


        //void btnDemandSearch_Click(object sender, EventArgs e)
        //{
        //    if (customLabel8.Text.Equals("需求总计划:"))
        //    {
        //        VDemandMasterPlanSelector vmros = new VDemandMasterPlanSelector();
        //        vmros.ShowDialog();
        //        IList list = vmros.Result;
        //        if (list == null || list.Count == 0) return;
        //        DemandMasterPlanMaster supplyMaster = list[0] as DemandMasterPlanMaster;
        //        txtDemandPlanCode.Tag = supplyMaster.Id;
        //        txtDemandPlanCode.Text = supplyMaster.Code;

        //        if (this.txtMaterialCategory.Result != null)
        //        {
        //            txtMaterialCategory.Result.Clear();
                    
        //        }
        //        IList result = new ArrayList();
        //        result.Add(supplyMaster.MaterialCategory);
        //        this.txtMaterialCategory.Text = supplyMaster.MaterialCategoryName;
        //        this.txtMaterialCategory.Tag = supplyMaster.MaterialCategory;
        //        txtMaterialCategory.Result = result;

        //        ////处理旧明细
        //        foreach (DataGridViewRow dr in dgDetail.Rows)
        //        {
        //            DemandMasterPlanDetail dtl = dr.Tag as DemandMasterPlanDetail;
        //            if (dtl != null)
        //            {
        //                if (CurBillMaster != null)
        //                {
        //                    CurBillMaster.Details.Remove(dtl);
        //                    if (dtl.Id != null)
        //                    {
        //                        movedDtlList.Add(dtl);
        //                    }
        //                }
        //            }
        //        }

        //        ////显示引用的明细
        //        this.dgDetail.Rows.Clear();
        //        decimal summoney = 0;
        //        foreach (DemandMasterPlanDetail var in supplyMaster.Details)
        //        {
        //            if (var.IsSelect == false) continue;
        //            int i = this.dgDetail.Rows.Add();
        //            this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
        //            this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
        //            this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
        //            this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
        //            this.dgDetail[colStuff.Name, i].Value = var.MaterialStuff;
        //            this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
        //            this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnitName;
        //            this.dgDetail[colDescript.Name, i].Value = var.Descript;
        //            this.dgDetail[colForwardDtlId.Name, i].Value = var.Id;
        //            this.dgDetail[colQuantity.Name, i].Value = var.DemandLeftQuantity;
        //            this.dgDetail[colPrice.Name, i].Value = var.Price;
        //            this.dgDetail[colwzfl.Name, i].Value = supplyMaster.MaterialCategoryName;
        //            this.dgDetail[colwzfl.Name, i].Tag = supplyMaster.MaterialCategory;
        //            decimal money = ClientUtil.ToDecimal(this.dgDetail[colQuantity.Name, i].Value) * ClientUtil.ToDecimal(this.dgDetail[colPrice.Name, i].Value);
        //            this.dgDetail[colRJMoney.Name, i].Value = ClientUtil.ToString(money);
        //            this.dgDetail[colzyfl.Name, i].Value = var.SpecialType;
        //            summoney += money;
        //            //this.dgDetail[colQuantityTemp.Name, i].Value = var.Quantity;
        //        }
        //        this.txtRJMoney.Text = summoney.ToString("#,###.####");
        //    }
        //    if (customLabel8.Text.Equals("采购计划:"))
        //    {
        //        VSupplyPlanSelector vmros = new VSupplyPlanSelector();
        //        vmros.ShowDialog();
        //        IList list = vmros.Result;
        //        if (list == null || list.Count == 0) return;
        //        SupplyPlanMaster supplyMaster = list[0] as SupplyPlanMaster;
        //        txtDemandPlanCode.Tag = supplyMaster.Id;
        //        txtDemandPlanCode.Text = supplyMaster.Code;

        //        ////处理旧明细
        //        foreach (DataGridViewRow dr in dgDetail.Rows)
        //        {
        //            SupplyPlanDetail dtl = dr.Tag as SupplyPlanDetail;
        //            if (dtl != null)
        //            {
        //                if (CurBillMaster != null)
        //                {
        //                    CurBillMaster.Details.Remove(dtl);
        //                    if (dtl.Id != null)
        //                    {
        //                        movedDtlList.Add(dtl);
        //                    }
        //                }
        //            }
        //        }

        //        ////显示引用的明细
        //        this.dgDetail.Rows.Clear();
        //        decimal summoney = 0;
        //        foreach (SupplyPlanDetail var in supplyMaster.Details)
        //        {
        //            if (var.IsSelect == false) continue;
        //            int i = this.dgDetail.Rows.Add();
        //            this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
        //            this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialCode;
        //            this.dgDetail[colMaterialName.Name, i].Value = var.MaterialName;
        //            this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialSpec;
        //            this.dgDetail[colStuff.Name, i].Value = var.MaterialStuff;
        //            this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
        //            this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnitName;
        //            this.dgDetail[colDescript.Name, i].Value = var.Descript;
        //            this.dgDetail[colForwardDtlId.Name, i].Value = var.Id;
        //            this.dgDetail[colzyfl.Name, i].Value = var.SpecialType;
        //            this.dgDetail[colPrice.Name, i].Value = var.Price;
        //            this.dgDetail[colQuantity.Name, i].Value = var.Quantity;
        //            decimal money = ClientUtil.ToDecimal(this.dgDetail[colQuantity.Name, i].Value) * ClientUtil.ToDecimal(this.dgDetail[colPrice.Name, i].Value);
        //            summoney += money;
        //            this.dgDetail[colRJMoney.Name, i].Value = ClientUtil.ToString(money);
                    
        //            //this.dgDetail[colQuantity.Name, i].Value = var.DemandLeftQuantity;
        //            //this.dgDetail[colQuantityTemp.Name, i].Value = var.Quantity;
        //        }
        //        this.txtRJMoney.Text = summoney.ToString("##,###.####");
        //    }
        //}

        void btnSet_Click(object sender, EventArgs e)
        {
            //获得第一行专业分类的信息，将其他行的专业分类信息都改成和第一行相同的信息
            string strSpecialType = null;
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                strSpecialType = this.dgDetail.Rows[0].Cells["colzyfl"].Value.ToString();
                var.Cells[colzyfl.Name].Value = strSpecialType;

            }
        }

        void tsmiDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = dgDetail.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgDetail.Rows.Remove(dr);
                if (dr.Tag != null)
                {
                    curBillMaster.Details.Remove(dr.Tag as SupplyOrderDetail);
                }
            }
        }


        #region 相关文档
        //添加文档
        void btnAddDocument_Click(object sender, EventArgs e)
        {
            if (dgDetail.CurrentRow == null)
            {
                MessageBox.Show("请先选择明细信息");
                return;
            }
            else
            {
                if (dgDetail.CurrentRow.Tag == null)
                {
                    return;
                }
            }
            DataGridViewRow theCurrentRow = dgSitePictureVideo.CurrentRow;
            if (theCurrentRow != null)
            {
                if (theCurrentRow.Cells[colState.Name].Value.ToString() == "0")
                {
                    if (MessageBox.Show("有尚未保存的相关文档信息，要保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (theCurrentRow.Cells[colDocuName.Name].Value == null)
                        {
                            MessageBox.Show("文档名称不能为空！");
                            return;
                        }
                        if (theCurrentRow.Cells[colDocuName.Name].Value == null)
                        {
                            MessageBox.Show("文档路径不能为空！");
                            return;
                        }
                        SaveSitePictureVideo();
                        theCurrentRow.Cells[colState.Name].Value = 1;
                    }
                }
            }
            if (dgDetail.Rows.Count > 0 && dgDetail.SelectedRows != null)
            {
                int i = dgSitePictureVideo.Rows.Add();

                DataGridViewRow newRow = dgSitePictureVideo.Rows[i];
                newRow.Cells[colDocuDate.Name].Value = DateTime.Now.Date;
                newRow.Cells[colPerson.Name].Tag = ConstObject.TheLogin.ThePerson;
                newRow.Cells[colPerson.Name].Value = ConstObject.TheLogin.ThePerson.Name;
                newRow.Cells[colState.Name].Value = 0;//操作状态 0：未保存  1：已保存
            }
            else
            {
                MessageBox.Show("请先选择一条检查记录单！");
                return;
            }
            RefreshControls(MainViewState.Modify);
        }
        //修改文档
        void btnModifyDocument_Click(object sender, EventArgs e)
        {
            if (dgDetail.Rows.Count > 0 && dgDetail.SelectedRows != null)
            {
                if (dgSitePictureVideo.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请选择要修改的行！");
                    return;
                }
                if (dgSitePictureVideo.SelectedRows.Count > 1)
                {
                    MessageBox.Show("一次只能修改一行！");
                    return;
                }
                oprDocument = dgSitePictureVideo.SelectedRows[0].Tag as ProObjectRelaDocument;
                this.txtDoucumentName.Text = "";
            }
            RefreshControls(MainViewState.Modify);

        }
        //保存文档
        void btnSaveDocument_Click(object sender, EventArgs e)
        {
            if (dgDetail.Rows.Count > 0 && dgDetail.SelectedRows != null)
            {
                DataGridViewRow theCurrentRow = dgSitePictureVideo.CurrentRow;
                if (dgSitePictureVideo.Rows.Count > 0 && dgSitePictureVideo.SelectedRows != null)
                {
                    if (theCurrentRow.Cells[colDocuName.Name].Value == null)
                    {
                        MessageBox.Show("文档名称不能为空！");
                        return;
                    }
                    if (theCurrentRow.Cells[colDocuName.Name].Value == null)
                    {
                        MessageBox.Show("文档路径不能为空！");
                        return;
                    }
                    SaveSitePictureVideo();
                }
                else
                {
                    MessageBox.Show("请先选择一条相关文档信息！");
                    return;
                }
            }
            else
            {
                MessageBox.Show("请先选择一条检查记录单！");
                return;
            }
        }
        //删除文档
        void btnDeleteDocument_Click(object sender, EventArgs e)
        {
            if (dgSitePictureVideo.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dgSitePictureVideo.Focus();
                return;
            }

            if (MessageBox.Show("确认要删除选择文档吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                List<ProObjectRelaDocument> listDoc = new List<ProObjectRelaDocument>();
                List<string> listDocId = new List<string>();
                foreach (DataGridViewRow row in dgSitePictureVideo.SelectedRows)
                {
                    ProObjectRelaDocument doc = row.Tag as ProObjectRelaDocument;
                    listDoc.Add(doc);
                    listDocId.Add(doc.DocumentGUID);
                }

                //删除IRP文档信息
                PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.DeleteDocumentByCustom(listDocId.ToArray()
                    , "1", null, userName, jobId, null);

                if (es != null)
                {

                    MessageBox.Show("删除IRP文档时出错，错误信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //删除MBP中对象关联文档信息
                ObjectQuery oq = new ObjectQuery();
                NHibernate.Criterion.Disjunction dis = new NHibernate.Criterion.Disjunction();
                foreach (ProObjectRelaDocument doc in listDoc)
                {
                    dis.Add(NHibernate.Criterion.Expression.And(
                        NHibernate.Criterion.Expression.Eq("ProObjectGUID", doc.ProObjectGUID),
                        NHibernate.Criterion.Expression.Eq("DocumentGUID", doc.DocumentGUID)));
                }
                oq.AddCriterion(dis);

                IList list = model.SupplyOrderSrv.ObjectQuery(typeof(ProObjectRelaDocument), oq);

                model.SupplyOrderSrv.Delete(list);

                //从列表中移除
                List<int> listRowIndex = new List<int>();
                foreach (DataGridViewRow row in dgSitePictureVideo.SelectedRows)
                {
                    listRowIndex.Add(row.Index);
                }
                listRowIndex.Sort();
                for (int i = listRowIndex.Count - 1; i > -1; i--)
                {
                    dgSitePictureVideo.Rows.RemoveAt(listRowIndex[i]);
                }

                txtDoucumentName.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //浏览文件
        void btnBrowse_Click(object sender, EventArgs e)
        {
            if (dgDetail.Rows.Count > 0 && dgDetail.SelectedRows != null)
            {
                OpenFileDialog OpenFD = new OpenFileDialog();
                OpenFD.Filter = "所有文件(*.*)|*.*";
                OpenFD.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (OpenFD.ShowDialog() == DialogResult.OK)
                {
                    txtDoucumentName.Text = OpenFD.FileName;
                    dgSitePictureVideo.CurrentRow.Cells[colDocuName.Name].Value = Path.GetFileName(txtDoucumentName.Text);
                    dgSitePictureVideo.CurrentRow.Cells[colDocuURL.Name].Value = txtDoucumentName.Text;
                    this.btnDownLoad.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("请先选择一条检查记录单！");
                return;
            }
        }
        //保存文档信息
        private void SaveSitePictureVideo()
        {
            try
            {
                DataGridViewRow theCurrentRow = dgSitePictureVideo.CurrentRow;
                if (theCurrentRow.Cells[colDocuName.Name].Value != null)
                {
                    string strState = ClientUtil.ToString(theCurrentRow.Cells[colState.Name].Value);
                    if (strState == "0")
                    {
                        //新增保存
                        try
                        {
                            #region 上传文件
                            if (theCurrentRow.Cells[colDocuName.Name].Value != null)
                            {
                                string filePath = ClientUtil.ToString(theCurrentRow.Cells[colDocuURL.Name].Value);
                                FileInfo file = new FileInfo(filePath);
                                if (file.Exists)
                                {
                                    List<object> listFileBytes = new List<object>();

                                    FileStream fileStream = file.OpenRead();
                                    int FileLen = (int)file.Length;
                                    Byte[] FileData = new Byte[FileLen];
                                    //将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
                                    fileStream.Read(FileData, 0, FileLen);

                                    listFileBytes.Add(FileData);//文件字节

                                    List<string> listFileNames = new List<string>();
                                    listFileNames.Add(file.Name);//文件名称

                                    string fileName = ClientUtil.ToString(theCurrentRow.Cells[colDocuName.Name].Value);
                                    List<string> listNames = new List<string>();
                                    List<object> listValues = new List<object>();
                                    IList listDoc = new List<ProObjectRelaDocument>();
                                    listNames.Add("Name");
                                    listValues.Add(fileName);

                                    listNames.Add("DOCUMENTTITLE");
                                    listValues.Add(fileName);

                                    //listNames.Add("DOCUMENTDESCRIPTION");
                                    //listValues.Add(fileDesc);

                                    PLMWebServices.DictionaryObjectInfo dic = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.DictionaryObjectInfo();
                                    dic.InfoNames = listNames.ToArray();
                                    dic.InfoValues = listValues.ToArray();

                                    List<PLMWebServices.DictionaryObjectInfo> listDic = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.DictionaryObjectInfo>();
                                    listDic.Add(dic);


                                    string[] listFileIds = null;



                                    List<string> listParams = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetUploadFileParamsByMBP_IRP();

                                    fileObjectType = listParams[0];



                                    PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.AddDocumentByCustom(out listFileIds, listFileBytes.ToArray(),
                                        listFileNames.ToArray(), fileObjectType, "1", listDic.ToArray(), null, userName, jobId, null);

                                    if (es != null)
                                    {
                                        MessageBox.Show("修改文件失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                    if (listFileIds != null)
                                    {

                                        #region 保存MBP关联文档信息
                                        listDoc.Clear();

                                        for (int i = 0; i < listFileIds.Length; i++)
                                        {
                                            string fileId = listFileIds[i];
                                            ProObjectRelaDocument doc = new ProObjectRelaDocument();
                                            doc.DocumentGUID = fileId;
                                            doc.DocumentName = dgSitePictureVideo.CurrentRow.Cells[colDocuName.Name].Value.ToString();
                                            object desc = dgSitePictureVideo.CurrentRow.Cells[colDocuContent.Name].Value;
                                            doc.DocumentDesc = desc == null ? "" : desc.ToString();

                                            //doc.FileURL = getFileURL(file);//使用WebService方式下载，此处不存文件路径，其次文件柜可能变迁
                                            doc.ProObjectGUID = (txtCode.Tag as SupplyOrderMaster).Id;
                                            doc.ProObjectName = "RectificationNoticeDetail";
                                            doc.DocumentOwner = ConstObject.LoginPersonInfo;
                                            doc.DocumentOwnerName = ConstObject.LoginPersonInfo.Name;
                                            if (projectInfo != null)
                                            {
                                                doc.TheProjectGUID = projectInfo.Id;
                                                doc.TheProjectName = projectInfo.Name;
                                            }

                                            listDoc.Add(doc);
                                        }

                                        listDoc = model.SupplyOrderSrv.SaveOrUpdateByDao(listDoc);

                                        foreach (ProObjectRelaDocument doc in listDoc)
                                        {
                                            DataGridViewRow row = dgSitePictureVideo.CurrentRow;
                                            row.Cells[colDocuName.Name].Value = doc.DocumentName;
                                            row.Cells[colPerson.Name].Value = doc.DocumentOwnerName;
                                            row.Cells[colDocuContent.Name].Value = doc.DocumentDesc;
                                            row.Cells[colState.Name].Value = "1";
                                            row.Tag = doc;
                                            this.txtDoucumentName.Text = "";
                                        }
                                        #endregion
                                    }
                                }
                            }
                            MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("保存数据出错，" + ex.ToString());
                            return;
                        }
                    }
                    if (strState == "1")
                    {
                        //更新保存
                        try
                        {
                            if (oprDocument != null)
                            {
                                List<string> listFileIds = new List<string>();
                                listFileIds.Add(oprDocument.DocumentGUID);

                                ObjectQuery oq = new ObjectQuery();
                                oq.AddCriterion(NHibernate.Criterion.Expression.Eq("Id", oprDocument.Id));
                                oprDocument = model.SupplyOrderSrv.ObjectQuery(typeof(ProObjectRelaDocument), oq)[0] as ProObjectRelaDocument;

                                oprDocument.DocumentName = ClientUtil.ToString(dgSitePictureVideo.CurrentRow.Cells[colDocuName.Name].Value);
                                oprDocument.DocumentDesc = ClientUtil.ToString(dgSitePictureVideo.CurrentRow.Cells[colDocuContent.Name].Value);

                                if (txtDoucumentName.Text != "" && File.Exists(txtDoucumentName.Text))//更换了文件
                                {
                                    FileInfo file = new FileInfo(txtDoucumentName.Text);

                                    List<byte[]> listFileBytes = new List<byte[]>();
                                    List<string> listFileNames = new List<string>();
                                    List<PLMWebServices.DictionaryObjectInfo> listDicKeyValue = new List<Application.Business.Erp.SupplyChain.Client.PLMWebServices.DictionaryObjectInfo>();

                                    FileStream fileStream = file.OpenRead();
                                    int FileLen = (int)file.Length;
                                    Byte[] FileData = new Byte[FileLen];
                                    //将文件数据放到FileData数组对象实例中，0代表数组指针的起始位置,FileLen代表指针的结束位置
                                    fileStream.Read(FileData, 0, FileLen);

                                    listFileBytes.Add(FileData);


                                    string fileName = ClientUtil.ToString(dgSitePictureVideo.CurrentRow.Cells[colDocuName.Name].Value);
                                    string fileDesc = ClientUtil.ToString(dgSitePictureVideo.CurrentRow.Cells[colDocuContent.Name].Value);


                                    listFileNames.Add(fileName + Path.GetExtension(file.Name));


                                    List<string> listNames = new List<string>();
                                    List<object> listValues = new List<object>();

                                    listNames.Add("Name");
                                    listValues.Add(fileName);

                                    listNames.Add("DOCUMENTTITLE");
                                    listValues.Add(fileName);

                                    listNames.Add("DOCUMENTDESCRIPTION");
                                    listValues.Add(fileDesc);

                                    PLMWebServices.DictionaryObjectInfo dic = new Application.Business.Erp.SupplyChain.Client.PLMWebServices.DictionaryObjectInfo();
                                    dic.InfoNames = listNames.ToArray();
                                    dic.InfoValues = listValues.ToArray();

                                    listDicKeyValue.Add(dic);


                                    PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.UpdateDocumentByCustom(listFileIds.ToArray(),
                                         fileObjectType, listFileBytes.ToArray(), listFileNames.ToArray(), "1", listDicKeyValue.ToArray(), null, userName, jobId, null);

                                    if (es != null)
                                    {
                                        MessageBox.Show("修改文件失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }

                                oprDocument = model.SupplyOrderSrv.SaveOrUpdate(oprDocument);

                                UpdateDocument(oprDocument);

                                MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("保存失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("保存数据出错，" + ex.ToString());
                return;
            }
        }

        private void UpdateDocument(ProObjectRelaDocument doc)
        {
            foreach (DataGridViewRow row in dgSitePictureVideo.Rows)
            {
                ProObjectRelaDocument docTemp = row.Tag as ProObjectRelaDocument;
                if (docTemp.Id == doc.Id)
                {
                    row.Cells[colDocuName.Name].Value = doc.DocumentName;
                    row.Cells[colDocuContent.Name].Value = doc.DocumentDesc;

                    row.Tag = doc;

                    dgSitePictureVideo.CurrentCell = row.Cells[1];
                    break;
                }
            }
        }

        void btnDownLoad_Click(object sender, EventArgs e)
        {
            if (dgSitePictureVideo.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要下载的文档！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dgSitePictureVideo.Focus();
                return;
            }

            List<string> listFileIds = new List<string>();
            foreach (DataGridViewRow row in dgSitePictureVideo.SelectedRows)
            {
                ProObjectRelaDocument doc = row.Tag as ProObjectRelaDocument;
                listFileIds.Add(doc.DocumentGUID);
            }

            object[] listFileBytes = null;
            string[] listFileNames = null;

            PLMWebServices.ErrorStack es = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.DownLoadDocumentByCustom(out listFileBytes, out listFileNames, listFileIds.ToArray(),
    null, userName, jobId, null);

            if (es != null)
            {
                MessageBox.Show("文件下载失败，异常信息：" + GetExceptionMessage(es), "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (listFileBytes != null)
            {
                string selectedDir = string.Empty;
                for (int i = 0; i < listFileBytes.Length; i++)
                {
                    byte[] by = listFileBytes[i] as byte[];
                    if (by != null && by.Length > 0)
                    {
                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                        saveFileDialog1.Filter = "All files(*.*)|*.*";
                        saveFileDialog1.RestoreDirectory = true;
                        saveFileDialog1.FileName = listFileNames[i];

                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            //进行赋值 
                            string filename = saveFileDialog1.FileName;
                            CreateFileFromByteAarray(by, filename);

                            selectedDir = saveFileDialog1.FileName.Substring(0, saveFileDialog1.FileName.LastIndexOf("\\"));
                        }
                    }
                }
                if (Directory.Exists(selectedDir))
                {
                    Process.Start(selectedDir);
                }
            }
        }

        public static void CreateFileFromByteAarray(byte[] stream, string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                fs.Write(stream, 0, stream.Length);
                fs.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private string GetExceptionMessage(PLMWebServices.ErrorStack es)
        {
            string msg = es.Message;
            PLMWebServices.ErrorStack esTemp = es.InnerErrorStack;
            while (esTemp != null && !string.IsNullOrEmpty(esTemp.Message))
            {
                msg += "；\n" + esTemp.Message;
                esTemp = esTemp.InnerErrorStack;
            }

            if ((msg.IndexOf("不能在具有唯一索引") > -1 && msg.IndexOf("插入重复键") > -1) || msg.IndexOf("违反唯一约束") > -1)
            {
                msg = "已存在同名文档，请重命名该文档名称.";
            }

            return msg;
        }
        #endregion



        #region 固定代码
        /// <summary>
        /// 启动本窗体,(设置状态或重新加载已有的数据)
        /// </summary>
        /// <param name="code">窗体Caption</param>
        public void Start(string Id)
        {
            try
            {
                if (Id == "空")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    curBillMaster = model.SupplyOrderSrv.GetSupplyOrderByIdCompany(Id);
                    ModelToView();
                    RefreshState(MainViewState.Browser);

                    //判断是否为制单人
                    PersonInfo pi = this.txtCreatePerson.Tag as PersonInfo;
                    string perid = ConstObject.LoginPersonInfo.Id;
                    if (pi != null && !pi.Id.Equals(perid))
                    {
                        RefreshStateByQuery();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("视图启动出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// 刷新状态(按钮状态)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshState(MainViewState state)
        {
            base.RefreshState(state);
            //控制表格
            switch (state)
            {
                case MainViewState.AddNew:
                case MainViewState.Modify:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditOnEnter;
                    cmsDg.Enabled = true;
                    cmsDgProject.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    cmsDg.Enabled = false;
                    cmsDgProject.Enabled = false;
                    break;
                default:
                    break;
            }
        }
        #endregion

        /// <summary>
        /// 刷新控件(窗体中的控件)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshControls(MainViewState state)
        {
            base.RefreshControls(state);

            //控制自身控件
            if (ViewState == MainViewState.AddNew || ViewState == MainViewState.Modify)
            {
                ObjectLock.Unlock(pnlFloor, true);
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
            }

            //永久锁定
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtHandlePerson, txtSumQuantity, txtSumMoney,  txtRJMoney };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colMaterialName.Name, colMaterialSpec.Name, colUnit.Name, colSumMoney.Name ,colRJMoney.Name,colzyfl.Name,colwzfl.Name};
            dgDetail.SetColumnsReadOnly(lockCols);
        }

        //清空数据
        private void ClearView()
        {
            if (this.txtMaterialCategory.Visible == true)
            {
                this.txtMaterialCategory.Text = "";
                this.txtMaterialCategory.Result = null;
            }
            ClearControl(pnlFloor);
        }

        private void ClearControl(Control c)
        {
            foreach (Control cd in c.Controls)
            {
                ClearControl(cd);
            }
            //自定义控件清空
            if (c is CustomEdit)
            {
                c.Tag = null;
                c.Text = "";
            }
            else if (c is CustomDataGridView)
            {
                (c as CustomDataGridView).Rows.Clear();
            }
        }

        //文档上传按钮状态
        private void btnStates(int i)
        {
            if (i == 0)
            {
                btnAddDocument.Enabled = false;
                btnBrowse.Enabled = false;
                btnDeleteDocument.Enabled = false;
                btnDownLoad.Enabled = false;
                btnModifyDocument.Enabled = false;
                btnSaveDocument.Enabled = false;
            }
            if (i == 1)
            {
                btnAddDocument.Enabled = true;
                btnBrowse.Enabled = true;
                btnDeleteDocument.Enabled = true;
                btnDownLoad.Enabled = true;
                btnModifyDocument.Enabled = true;
                btnSaveDocument.Enabled = true;
            }
        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <returns></returns>
        public override bool NewView()
        {
            try
            {
                base.NewView();
                ClearView();

                this.curBillMaster = new SupplyOrderMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.CreateDate = ConstObject.LoginDate;
                curBillMaster.SignDate = ConstObject.LoginDate;
                curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.DocState = DocumentState.Edit;

                //curBillMaster.Special = Enum.GetName(typeof(EnumSupplyType), SupplyType);

                //制单人
                txtCreatePerson.Tag = ConstObject.LoginPersonInfo;
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //制单日期
                txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
                //责任人
                txtHandlePerson.Tag = ConstObject.LoginPersonInfo;
                txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
                //归属项目
               
                //if (projectInfo != null)
                //{
                //    txtProject.Tag = projectInfo;
                //    txtProject.Text = projectInfo.Name;
                //    curBillMaster.ProjectId = projectInfo.Id;
                //    curBillMaster.ProjectName = projectInfo.Name;
                //}
                //txtContractNo.Focus();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
            {
                base.ModifyView();
                curBillMaster = model.SupplyOrderSrv.GetSupplyOrderByIdCompany(curBillMaster.Id);
                ModelToView();
                modifyClick();
                btnStates(1);
                return true;
            }
            string message = "此单状态为非编辑状态，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
            MessageBox.Show(message);

            return false;
        }

        private void modifyClick()
        {
            dgSitePictureVideo.Rows.Clear();
            ObjectQuery oqDoc = new ObjectQuery();
            oqDoc.AddCriterion(NHibernate.Criterion.Expression.Eq("ProObjectGUID", (txtCode.Tag as SupplyOrderMaster).Id));
            IList listDocument = model.SupplyOrderSrv.ObjectQuery(typeof(ProObjectRelaDocument), oqDoc);
            if (listDocument != null && listDocument.Count > 0)
            {
                foreach (ProObjectRelaDocument doc in listDocument)
                {
                    InsertIntoGridDocument(doc);
                }
            }
        }
        private void InsertIntoGridDocument(ProObjectRelaDocument doc)
        {
            int index = dgSitePictureVideo.Rows.Add();
            DataGridViewRow row = dgSitePictureVideo.Rows[index];
            row.Cells[colDocuName.Name].Value = doc.DocumentName;
            row.Cells[colPerson.Name].Value = doc.DocumentOwnerName;
            row.Cells[colDocuContent.Name].Value = doc.DocumentDesc;
            //row.Cells[colDocuURL.Name].Value = doc.FileURL;
            row.Cells[colState.Name].Value = "1";
            row.Tag = doc;
            this.txtDoucumentName.Text = "";
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            try
            {
                if (!ViewToModel()) return false;
                bool flag = false;
                if (string.IsNullOrEmpty(curBillMaster.Id))
                {
                    flag = true;
                }
                curBillMaster = model.SupplyOrderSrv.SaveSupplyOrderCompany(curBillMaster);
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                
                LogData log = new LogData();
                log.BillId = curBillMaster.Id;
                log.BillType = "采购合同单(公司)";
                log.Code = curBillMaster.Code;
                log.Descript = "";
                log.OperPerson = ConstObject.LoginPersonInfo.Name;
                log.ProjectName = "";
                if (flag)
                {
                    log.OperType = "新增";
                }
                else
                {
                    log.OperType = "修改";
                }
                StaticMethod.InsertLogData(log);
                this.ViewCaption = ViewName + "-" + txtCode.Text;
                MessageBox.Show("保存成功！");
                btnStates(0);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                curBillMaster = model.SupplyOrderSrv.GetSupplyOrderByIdCompany(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    if (!model.SupplyOrderSrv.DeleteSupplyOrderCompany(curBillMaster)) return false;
                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "采购合同单(公司)";
                    log.Code = curBillMaster.Code;
                    log.OperType = "删除";
                    log.Descript = "";
                    log.OperPerson = ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = curBillMaster.ProjectName;
                    StaticMethod.InsertLogData(log);
                    ClearView();
                    MessageBox.Show("删除成功！");
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能删除！");
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据删除错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 撤销
        /// </summary>
        /// <returns></returns>
        public override bool CancelView()
        {
            try
            {
                switch (ViewState)
                {
                    case MainViewState.Modify:
                        //重新查询数据
                        curBillMaster = model.SupplyOrderSrv.GetSupplyOrderByIdCompany(curBillMaster.Id);
                        ModelToView();
                        break;
                    default:
                        ClearView();
                        break;
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据撤销错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public override void RefreshView()
        {
            try
            {
                //重新获得当前对象的值
                curBillMaster = model.SupplyOrderSrv.GetSupplyOrderByIdCompany(curBillMaster.Id);
                //给界面赋值
                ModelToView();
            }
            catch (Exception e)
            {
                MessageBox.Show("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// 保存数据前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            this.txtCode.Focus();
            if (txtSupply.Text == "" || txtSupply.Result.Count == 0)
            {
                MessageBox.Show("供应商不能为空。");
                txtSupply.Focus();
                return false;
            }
            //if (supplyType == EnumSupplyType.土建)
            //{
            //    if(txtMaterialCategory.Text.Equals("") || txtMaterialCategory.Text.Equals(null))
            //    {
            //        MessageBox.Show("物资分类为必选项！");
            //        return false;
            //    }
            //}
            //if (SupplyType == EnumSupplyType.安装)
            //{
            //    if (this.cboProfessionCat.SelectedItem != null)
            //    { }else
            //    {
            //        MessageBox.Show("专业分类为必选项！");
            //        return false;
            //    }
            //}
            if (ClientUtil.ToString(this.txtContratcMaterial.Text) == "")
            {
                MessageBox.Show("合同物资不能为空！");
                txtContratcMaterial.Focus();
                return false;
            }

            if (ClientUtil.ToString(this.txtContactPerson.Text) == "")
            {
                MessageBox.Show("联系人不能为空！");
                txtContactPerson.Focus();
                return false;
            }

            if (ClientUtil.ToString(this.txtContactPhone.Text) == "")
            {
                MessageBox.Show("联系电话不能为空！");
                txtContactPhone.Focus();
                return false;
            }

            if (ClientUtil.ToString(this.txtSupplyContract.Text) == "")
            {
                MessageBox.Show("采购合同号不能为空！");
                txtSupplyContract.Focus();
                return false;
            }

            string validMessage = "";
            if (this.dgDetail.Rows.Count - 1 == 0)
            {
                MessageBox.Show("明细不能为空！");
                return false;
            }

            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));
            if (validMessage != "")
            {
                MessageBox.Show(validMessage);
                return false;
            }

            //取得该供应商，该项目的合同物资信息
            Hashtable ht_material = model.SupplyOrderSrv.GetOrderMaterialInfo(projectInfo.Id, (txtSupply.Result[0] as SupplierRelationInfo).Id);
            //明细表数据校验
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                //最后一行不进行校验
                if (dr.IsNewRow) break;

                if (dr.Cells[colMaterialCode.Name].Tag == null)
                {
                    MessageBox.Show("物料不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colMaterialCode.Name];
                    return false;
                }
                else {
                    string materialId = (dr.Cells[colMaterialCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material).Id;
                    if (curBillMaster.Id == null)
                    {
                        if (ht_material.Contains(materialId))
                        {
                            MessageBox.Show("物料编码[" + dr.Cells[colMaterialCode.Name].Value + "]规格[" + dr.Cells[this.colMaterialSpec.Name].Value + "]在该项目中已经签订合同！");
                            return false;
                        }
                    }
                    else {
                        if (ht_material.Contains(materialId))
                        {
                            IList list_order = (ArrayList)ht_material[materialId];
                            if (list_order.Count > 1)
                            {
                                MessageBox.Show("物料编码[" + dr.Cells[colMaterialCode.Name].Value + "]规格[" + dr.Cells[this.colMaterialSpec.Name].Value + "]在该项目中已经签订合同！");
                                return false;
                            }
                            else {
                                if (!list_order.Contains(curBillMaster.Id))
                                {
                                    MessageBox.Show("物料编码[" + dr.Cells[colMaterialCode.Name].Value + "]规格[" + dr.Cells[this.colMaterialSpec.Name].Value + "]在该项目中已经签订合同！");
                                    return false;
                                }
                            }
                        }
                    }
                }

                if (dr.Cells[colUnit.Name].Tag == null)
                {
                    MessageBox.Show("计量单位不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colUnit.Name];
                    return false;
                }

                if (dr.Cells[colQuantity.Name].Value == null || dr.Cells[colQuantity.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colQuantity.Name].Value) < 0)
                {
                    MessageBox.Show("数量不允许为空或小于0！");
                    dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                    return false;
                }

                if (dr.Cells[this.colBrand.Name].Value == null || dr.Cells[colBrand.Name].Value.ToString() == "")
                {
                    MessageBox.Show("品牌不能为空！");
                    dgDetail.CurrentCell = dr.Cells[colBrand.Name];
                    return false;
                }

                if (SupplyType == EnumSupplyType.安装)
                {
                    if (dr.Cells[colPrice.Name].Value == null || dr.Cells[colPrice.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colPrice.Name].Value) < 0)
                    {
                        MessageBox.Show("认价单价不允许为空！");
                        dgDetail.CurrentCell = dr.Cells[colPrice.Name];
                        return false;
                    }
                }
                //foreach (string matCat in list_price)
                //{
                //    if (ClientUtil.ToString(dr.Cells[colMaterialCode.Name].Value).IndexOf(matCat) != -1 && ClientUtil.ToDecimal(dr.Cells[colSupplyPrice.Name].Value) <= 0)
                //    {
                //        MessageBox.Show("编码为[" + dr.Cells[colMaterialCode.Name].Value + "]名称[" + dr.Cells[this.colMaterialName.Name].Value + "]规格[" 
                //            + dr.Cells[this.colMaterialSpec.Name].Value + "]的物资采购单价不允许为空或小于等于0！");
                //        dgDetail.CurrentCell = dr.Cells[colSupplyPrice.Name];
                //        return false;
                //    }
                //}

                if (dr.Cells[colSupplyPrice.Name].Value == null || dr.Cells[colSupplyPrice.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colSupplyPrice.Name].Value) < 0)
                {
                    MessageBox.Show("采购单价不允许为空或小于0！");
                    dgDetail.CurrentCell = dr.Cells[colSupplyPrice.Name];
                    return false;
                }

                //if (btnDemandSearch.Enabled == false)
                //{
                //    object forwardDtlId = dr.Cells[colForwardDtlId.Name].Value;
                //    SupplyPlanDetail forwardDetail = model.SupplyOrderSrv.GetSupplyDetailById(forwardDtlId.ToString());
                //    //SupplyPlanDetail forwardDetail = Supplymodle.SupplyPlanSrv.GetSupplyDetailById(forwardDtlId.ToString());
                //    if (forwardDetail == null)
                //    {
                //        MessageBox.Show("未找到前续单据明细,请重新引用。");
                //        //dgDetail.CurrentCell = dr.Cells[colQuantityTemp.Name];
                //        return false;
                //    }
                //    else
                //    {
                //        decimal canUseQty = forwardDetail.Quantity - forwardDetail.RefQuantity;//可利用数量
                //        decimal currentQty = decimal.Parse(dr.Cells[colQuantity.Name].Value.ToString());
                //        object qtyTempObj = dr.Cells[colQuantityTemp.Name].Value;//临时数量
                //        decimal qtyTemp = 0;
                //        if (qtyTempObj != null && !qtyTempObj.ToString().Equals(""))
                //        {
                //            qtyTemp = decimal.Parse(qtyTempObj.ToString());
                //        }

                //        if (currentQty - qtyTemp - canUseQty > 0)
                //        {
                //            MessageBox.Show("输入数量【" + currentQty + "】大于可引用数量【" + (canUseQty + qtyTemp) + "】。");
                //            dgDetail.CurrentCell = dr.Cells[colQuantity.Name];
                //            return false;
                //        }
                //    }
                //}

            }

            //付款方式明细校验
            
            foreach (DataGridViewRow dr in dgExtDetail.Rows)
            {
                //最后一行不进行校验
                if (dr.IsNewRow) break;
                if (dr.Cells[colProjectStatus.Name].Value == null || dr.Cells[colProjectStatus.Name].Value.ToString() == "")
                {
                    MessageBox.Show("工程状态不能为空！");
                    return false;
                }
                if (dr.Cells[colProportion.Name].Value == null || dr.Cells[colProportion.Name].Value.ToString() == "" || ClientUtil.TransToDecimal(dr.Cells[colProportion.Name].Value) <= 0 || ClientUtil.TransToDecimal(dr.Cells[colProportion.Name].Value) >= 100)
                {
                    MessageBox.Show("付款比例不允许为空或小于等于0大于等于100！");
                    //dgDetail.CurrentCell = dr.Cells[colProportion.Name];
                    return false;
                }
            }


            if (dgProject.Rows.Count - 1 == 0)
            {
                //MessageBox.Show("项目信息不能为空，请双击选择。");
                //tabControl1.SelectTab(tabProject.Name);
                //return false;
            }

            dgExtDetail.Update();
            dgDetail.Update();
            dgProject.Update();
            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                this.txtCode.Focus();
                //curBillMaster.RealOperationDate = dtpDateBegin.Value.Date;
                curBillMaster.SignDate = ClientUtil.ToDateTime(this.dtpSignDate.Text);
                //物资分类
                if (txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
                {
                    curBillMaster.MaterialCategory = txtMaterialCategory.Result[0] as MaterialCategory;
                    curBillMaster.MaterialCategoryName = txtMaterialCategory.Text;
                    curBillMaster.MaterialCategoryCode = curBillMaster.MaterialCategory.Code;
                }
                curBillMaster.SpecialType = cboProfessionCat.SelectedItem == null ? null : cboProfessionCat.SelectedItem.ToString();
                curBillMaster.ContractMoney = ClientUtil.ToDecimal(this.txtSumMoney.Text);
                curBillMaster.ContractMatDes = ClientUtil.ToString(this.txtContratcMaterial.Text);//合同物资
                curBillMaster.OldContractNum = ClientUtil.ToString(this.txtSupplyContract.Text);//采购合同号
                curBillMaster.ContactPerson = ClientUtil.ToString(this.txtContactPerson.Text);//联系人
                curBillMaster.Telephone = ClientUtil.ToString(this.txtContactPhone.Text);//联系电话
                //curBillMaster.AttachmentDocPath = ClientUtil.ToString(this.txtAttachment.Text);//附件上传
                curBillMaster.Descript = ClientUtil.ToString(this.txtContractDescript.Text);//合同描述
                curBillMaster.TheSupplierRelationInfo = txtSupply.Result[0] as SupplierRelationInfo;
                curBillMaster.RJSumMoney = ClientUtil.ToDecimal(this.txtRJMoney.Text);//认价总金额
                curBillMaster.SupplierName = txtSupply.Text;//供应商
                curBillMaster.Descript = this.txtContractDescript.Text;
                curBillMaster.PumpMoney = ClientUtil.ToDecimal(this.txtPump.Text);//泵送费
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    SupplyOrderDetail curBillDtl = new SupplyOrderDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as SupplyOrderDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.MaterialResource = var.Cells[colMaterialCode.Name].Tag as Application.Resource.MaterialResource.Domain.Material;
                    curBillDtl.MaterialName = ClientUtil.ToString(var.Cells[colMaterialName.Name].Value);//物资名称
                    curBillDtl.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);//规格型号
                    curBillDtl.MaterialStuff = ClientUtil.ToString(var.Cells[colStuff.Name].Value);//材质
                    curBillDtl.MaterialCode = ClientUtil.ToString(var.Cells[colMaterialCode.Name].Value);//物资编码
                    curBillDtl.MatStandardUnit = var.Cells[colUnit.Name].Tag as StandardUnit;//计量单位
                    curBillDtl.MatStandardUnitName = ClientUtil.ToString(var.Cells[colUnit.Name].Value);//计量单位
                    curBillDtl.Quantity = ClientUtil.TransToDecimal(var.Cells[colQuantity.Name].Value);//数量
                    curBillDtl.QuantityTemp = ClientUtil.ToDecimal(var.Cells[colQuantityTemp.Name].Value);//临时数量
                    curBillDtl.Money = ClientUtil.TransToDecimal(var.Cells[colSumMoney.Name].Value);//金额
                    curBillDtl.ConfirmPrice = ClientUtil.TransToDecimal(var.Cells[colPrice.Name].Value);//认价单价
                    curBillDtl.RJMoney = ClientUtil.ToDecimal(var.Cells[colRJMoney.Name].Value);//认价金额
                    curBillDtl.SupplyPrice = ClientUtil.TransToDecimal(var.Cells[colSupplyPrice.Name].Value);//采购单价
                    curBillDtl.ModifyPrice = ClientUtil.TransToDecimal(var.Cells[colSupplyPrice.Name].Value);//调后价格先暂定为采购价
                    curBillDtl.SpecialType = ClientUtil.ToString(var.Cells[colzyfl.Name].Value);//专业分类
                    curBillDtl.MaterialCategory = var.Cells[colwzfl.Name].Tag as MaterialCategory;
                    curBillDtl.MaterialCategoryName = ClientUtil.ToString(var.Cells[colwzfl.Name].Value);//材料类别
                    curBillDtl.Grade = ClientUtil.ToString(var.Cells[colGrade.Name].Value);//档次
                    curBillDtl.Brand = ClientUtil.ToString(var.Cells[colBrand.Name].Value);//品牌
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);//备注
                    curBillDtl.DiagramNumber = ClientUtil.ToString(var.Cells[this.colDiagramNum.Name].Value);//图号
                    curBillMaster.ForwardBillId = ClientUtil.ToString(var.Cells[colForwardDtlId.Name].Value);//前驱明细
                    curBillDtl.ForwardDetailId = ClientUtil.ToString(var.Cells[colForwardDtlId.Name].Value);
                    curBillMaster.AddDetail(curBillDtl);
                    var.Tag = curBillDtl;
                }
                //保存付款方式
                foreach (DataGridViewRow var in this.dgExtDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    SupplyOrderPayment curBillPaymant = new SupplyOrderPayment();
                    if (var.Tag != null)
                    {
                        curBillPaymant = var.Tag as SupplyOrderPayment;
                        if (curBillPaymant.Id == null)
                        {
                            curBillMaster.PaymentDetails.Remove(curBillPaymant);
                        }
                    }
                    curBillPaymant.PaymentProportion = ClientUtil.ToDecimal(var.Cells[colProportion.Name].Value);//付款比例
                    curBillPaymant.ProjectState = ClientUtil.ToString(var.Cells[colProjectStatus.Name].Value);//工程状态
                    curBillPaymant.Descript = ClientUtil.ToString(var.Cells[colRemark.Name].Value);//备注
                    curBillMaster.AddPaymentDetail(curBillPaymant);
                    var.Tag = curBillPaymant;
                }

                //保存项目信息
                foreach (DataGridViewRow var in this.dgProject.Rows)
                {
                    if (var.IsNewRow) break;
                    SupplyOrderProjectDetail detail = new SupplyOrderProjectDetail();
                    if (var.Tag != null)
                    {
                        detail = var.Tag as SupplyOrderProjectDetail;
                        if (detail.Id == null)
                        {
                            curBillMaster.ProjectDetails.Remove(detail);
                        }
                    }
                    CurrentProjectInfo pi = var.Cells[colProjectName.Name].Tag as CurrentProjectInfo;
                    detail.ProjectId = pi;
                    detail.ProjectName = var.Cells[colProjectName.Name].Value + "";
                    curBillMaster.AddProjectDetail(detail);
                    var.Tag = detail;
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 在光标跳自动转到下一列时，首先执行校验，确定是否跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
        }

        /// <summary>
        /// 物料编码列增加事件监听，支持处理键盘回车查询操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox textBox = e.Control as TextBox;
            if (textBox != null)
            {
                textBox.PreviewKeyDown -= new PreviewKeyDownEventHandler(textBox_PreviewKeyDown);
                textBox.KeyPress -= new KeyPressEventHandler(textBox_KeyPress);

                if (this.dgDetail.Columns[this.dgDetail.CurrentCell.ColumnIndex].Name.Equals("MaterialCode"))
                {
                    textBox.PreviewKeyDown += new PreviewKeyDownEventHandler(textBox_PreviewKeyDown);
                    textBox.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
                }

            }
        }

        /// <summary>
        /// 键盘回车查询处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (base.ViewState == MainViewState.Browser || base.ViewState == MainViewState.Initialize) return;

            if (e.KeyValue == 13)
            {
                CommonMaterial materialSelector = new CommonMaterial();

                TextBox textBox = sender as TextBox;
                if (textBox.Text != null && !textBox.Text.Equals(""))
                {
                    materialSelector.OpenSelect(textBox.Text);
                }
                else
                {
                    materialSelector.OpenSelect();
                }
                IList list = materialSelector.Result;

                if (list != null && list.Count > 0)
                {
                    Application.Resource.MaterialResource.Domain.Material selectedMaterial = list[0] as Application.Resource.MaterialResource.Domain.Material;
                    this.dgDetail.CurrentRow.Cells[colMaterialCode.Name].Tag = selectedMaterial;
                    this.dgDetail.CurrentCell.Value = selectedMaterial.Code;
                    this.dgDetail.CurrentRow.Cells[colMaterialName.Name].Value = selectedMaterial.Name;
                    this.dgDetail.CurrentRow.Cells[colMaterialSpec.Name].Value = selectedMaterial.Specification;
                    this.dgDetail.CurrentRow.Cells[colStuff.Name].Value = selectedMaterial.Stuff;

                    //动态分类复合单位                    
                    DataGridViewComboBoxCell cbo = this.dgDetail.CurrentRow.Cells[colUnit.Name] as DataGridViewComboBoxCell;
                    cbo.Items.Clear();

                    StandardUnit first = null;
                    foreach (StandardUnit cu in selectedMaterial.GetMaterialAllUnit())
                    {
                        cbo.Items.Add(cu.Name);
                    }
                    first = selectedMaterial.BasicUnit;
                    this.dgDetail.CurrentRow.Cells[colUnit.Name].Tag = first;
                    cbo.Value = first.Name;
                    this.dgDetail.RefreshEdit();
                }
            }
        }

        /// <summary>
        /// 在物料编码列，敲击键盘时，取消原来已经选择的物料，
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.dgDetail.CurrentCell.Tag = null;
            this.dgDetail.Rows[this.dgDetail.CurrentCell.RowIndex].Cells[colMaterialName.Name].Value = "";
            this.dgDetail.Rows[this.dgDetail.CurrentCell.RowIndex].Cells[colMaterialSpec.Name].Value = "";
        }

        /// <summary>
        /// 物料编码列，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialCode.Name))
            {
                if (this.txtMaterialCategory.Visible == true && (this.txtMaterialCategory.Result == null || this.txtMaterialCategory.Result.Count == 0))
                {
                    MessageBox.Show("请先选择物资分类！");
                    return;
                }
                MaterialCategory cat = this.txtMaterialCategory.Result[0] as MaterialCategory;
                if (cat.Level != 3)
                {
                    MessageBox.Show("请选择一级物资分类！");
                    return;
                }

                CommonMaterial materialSelector = new CommonMaterial();
                if (this.txtMaterialCategory.Visible == true)
                {
                    string catCode = (txtMaterialCategory.Result[0] as MaterialCategory).Code;
                    //if (catCode.StartsWith("I1100112") || catCode.StartsWith("I1100113") || catCode.StartsWith("I1100102") || catCode.StartsWith("I1100101")
                    //    || catCode.StartsWith("I1100111") || catCode.StartsWith("I1100110") || catCode.StartsWith("I11201"))
                    //{
                    //    MessageBox.Show("请关联需求总计划单获取物资明细！");
                    //    return;
                    //}
                    if (txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
                    {
                        materialSelector.materialCatCode = (txtMaterialCategory.Result[0] as MaterialCategory).Code;
                    }
                }
                DataGridViewCell cell = this.dgDetail[e.ColumnIndex, e.RowIndex];
                object tempValue = cell.EditedFormattedValue;
                materialSelector.OpenSelect();

                IList list = materialSelector.Result;
                foreach (Application.Resource.MaterialResource.Domain.Material theMaterial in list)
                {
                    int i = dgDetail.Rows.Add();
                    this.dgDetail[colMaterialCode.Name, i].Tag = theMaterial;
                    this.dgDetail[colMaterialCode.Name, i].Value = theMaterial.Code;
                    this.dgDetail[colMaterialName.Name, i].Value = theMaterial.Name;
                    this.dgDetail[colMaterialSpec.Name, i].Value = theMaterial.Specification;
                    this.dgDetail[colStuff.Name, i].Value = theMaterial.Stuff;
                    this.dgDetail[colUnit.Name, i].Tag = theMaterial.BasicUnit;
                    if (theMaterial.BasicUnit != null)
                        this.dgDetail[colUnit.Name, i].Value = theMaterial.BasicUnit.Name;
                    if (txtMaterialCategory.Result != null)
                    {
                        dgDetail[colwzfl.Name, i].Value = txtMaterialCategory.Value;
                        dgDetail[colzyfl.Name, i].Tag = txtMaterialCategory.Result[0] as MaterialCategory;
                    }
                    i++;
                }
            }
        }

        //显示数据
        private bool ModelToView()
        {
            try
            {
                this.txtCode.Text = curBillMaster.Code;
                this.txtCode.Tag = curBillMaster;
                dtpSignDate.Value = curBillMaster.SignDate;
                txtHandlePerson.Tag = curBillMaster.HandlePerson;
                txtHandlePerson.Text = curBillMaster.HandlePersonName;
                txtContractDescript.Text = curBillMaster.Descript;
                txtCreateDate.Text = curBillMaster.CreateDate.ToShortDateString();
                this.txtCreatePerson.Tag = curBillMaster.CreatePerson;
                txtCreatePerson.Text = curBillMaster.CreatePersonName;
                this.txtCreatePerson.Text = curBillMaster.CreatePersonName;
                //专业分类
                if (curBillMaster.SpecialType != null)
                {
                    cboProfessionCat.SelectedItem = curBillMaster.SpecialType;
                }

                //物资分类
                if (curBillMaster.MaterialCategory != null)
                {
                    txtMaterialCategory.Result.Clear();
                    txtMaterialCategory.Result.Add(curBillMaster.MaterialCategory);
                    txtMaterialCategory.Value = curBillMaster.MaterialCategoryName;
                }
                txtSumQuantity.Text = ClientUtil.ToString(curBillMaster.SumQuantity.ToString("#,###.####"));
                txtSumMoney.Text = ClientUtil.ToString(curBillMaster.SumMoney.ToString("#,###.####"));
                this.txtPump.Text = ClientUtil.ToString(curBillMaster.PumpMoney.ToString("#,###.##"));
                this.txtContractDescript.Text = ClientUtil.ToString(curBillMaster.Descript);
                this.txtSupplyContract.Text = ClientUtil.ToString(curBillMaster.OldContractNum);//原始合同号
                if (curBillMaster.TheSupplierRelationInfo != null)
                {
                    txtSupply.Result.Clear();
                    txtSupply.Result.Add(curBillMaster.TheSupplierRelationInfo);
                    this.txtSupply.Value = curBillMaster.SupplierName;//供应商
                }
                this.txtRJMoney.Text = curBillMaster.RJSumMoney.ToString("##,###,#####");
                //this.txtAttachment.Text = ClientUtil.ToString(curBillMaster.AttachmentDocPath);//附件上传
                this.txtContactPerson.Text = ClientUtil.ToString(curBillMaster.ContactPerson);//联系人
                this.txtContactPhone.Text = ClientUtil.ToString(curBillMaster.Telephone);//联系电话
                this.txtContratcMaterial.Text = ClientUtil.ToString(curBillMaster.ContractMatDes);//合同物资
                this.dgDetail.Rows.Clear();

                foreach (SupplyOrderDetail var in curBillMaster.Details)
                {
                    int i = this.dgDetail.Rows.Add();

                    this.dgDetail[colMaterialCode.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialCode.Name, i].Value = var.MaterialResource.Code;
                    this.dgDetail[colMaterialName.Name, i].Value = var.MaterialResource.Name;
                    this.dgDetail[colMaterialSpec.Name, i].Value = var.MaterialResource.Specification;
                    this.dgDetail[colStuff.Name, i].Value = var.MaterialResource.Stuff;
                    //设置该物料的计量单位
                    this.dgDetail[colUnit.Name, i].Tag = var.MatStandardUnit;
                    this.dgDetail[colUnit.Name, i].Value = var.MatStandardUnit.Name;
                    this.dgDetail[colBrand.Name, i].Value = var.Brand;
                    this.dgDetail[colGrade.Name, i].Value = var.Grade;//商标牌子
                    this.dgDetail[colPrice.Name, i].Value = var.ConfirmPrice;//认价单价
                    this.dgDetail[colSupplyPrice.Name, i].Value = var.SupplyPrice;//采购价格
                    this.dgDetail[colQuantity.Name, i].Value = var.Quantity;//数量
                    this.dgDetail[colQuantityTemp.Name, i].Value = var.Quantity;
                    this.dgDetail[colSumMoney.Name, i].Value = var.Money;
                    this.dgDetail[colzyfl.Name, i].Value = var.SpecialType;
                    this.dgDetail[colwzfl.Name, i].Tag = var.MaterialCategory;
                    this.dgDetail[colwzfl.Name, i].Value = var.MaterialCategoryName;
                    this.dgDetail[colForwardDtlId.Name, i].Value = var.ForwardDetailId;
                    this.dgDetail[colDescript.Name, i].Value = var.Descript;
                    this.dgDetail[this.colDiagramNum.Name, i].Value = var.DiagramNumber;//图号
                    this.dgDetail[colRJMoney.Name, i].Value = var.RJMoney;
                    this.dgDetail.Rows[i].Tag = var;
                }
                //显示付款方式
                dgExtDetail.Rows.Clear();
                foreach (SupplyOrderPayment var in curBillMaster.PaymentDetails)
                {
                    int i = this.dgExtDetail.Rows.Add();
                    this.dgExtDetail[colRemark.Name, i].Value = var.Descript;//备注
                    this.dgExtDetail[colProportion.Name, i].Value = var.PaymentProportion;//比例
                    this.dgExtDetail[colProjectStatus.Name, i].Value = var.ProjectState;//工程状态
                    this.dgExtDetail.Rows[i].Tag = var;
                }

                //显示付款方式
                dgProject.Rows.Clear();
                foreach (SupplyOrderProjectDetail var in curBillMaster.ProjectDetails)
                {
                    int i = this.dgProject.Rows.Add();
                    dgProject[colProjectName.Name, i].Value = var.ProjectName;
                    dgProject[colProjectName.Name, i].Tag = var.ProjectId;
                    this.dgProject.Rows[i].Tag = var;
                }

                modifyClick();

                return true;
            }
            catch (Exception e)
            {
                throw e;
                //MessageBox.Show("数据映射错误：" + StaticMethod.ExceptionMessage(e));
                //return false;
            }
        }

        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            bool flag = true;
            if (colName == colSupplyPrice.Name || colName == colQuantity.Name)
            {
                if (dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value != null)
                {
                    string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                    {
                        MessageBox.Show("数量为数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value = "";
                        flag = false;
                    }
                }

                if (dgDetail.Rows[e.RowIndex].Cells[colSupplyPrice.Name].Value != null)
                {
                    string temp_price = dgDetail.Rows[e.RowIndex].Cells[colSupplyPrice.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_price);
                    if (validity == false)
                    {
                        MessageBox.Show("采购单价为数字！");
                        this.dgDetail.Rows[e.RowIndex].Cells[colSupplyPrice.Name].Value = "";
                        flag = false;
                    }
                }
                if (flag)
                {
                    //根据单价和数量计算金额  
                    object price = dgDetail.Rows[e.RowIndex].Cells[colSupplyPrice.Name].Value;
                    object quantity = dgDetail.Rows[e.RowIndex].Cells[colQuantity.Name].Value;

                    decimal sumqty = 0;
                    decimal money = 0;
                    decimal summoney = 0;
                    for (int i = 0; i <= dgDetail.RowCount - 1; i++)
                    {
                        quantity = dgDetail.Rows[i].Cells[colQuantity.Name].Value;
                        if (quantity == null) quantity = 0;
                        //if (SupplyType == EnumSupplyType.安装)
                        //{
                        if (ClientUtil.ToString(dgDetail.Rows[i].Cells[colQuantity.Name].Value) != "" && ClientUtil.ToString(dgDetail.Rows[i].Cells[colSupplyPrice.Name].Value) != "")
                        {
                            money = ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colQuantity.Name].Value) * ClientUtil.ToDecimal(dgDetail.Rows[i].Cells[colSupplyPrice.Name].Value);
                            dgDetail.Rows[i].Cells[colSumMoney.Name].Value = ClientUtil.ToString(money);
                            summoney = summoney + money;
                        }
                        //}
                        sumqty = sumqty + ClientUtil.TransToDecimal(quantity);
                    }

                    txtSumQuantity.Text = sumqty.ToString();
                    txtSumMoney.Text = summoney.ToString();
                }
                
            }
        }

        ///// <summary>
        ///// 打印预览
        ///// </summary>
        ///// <returns></returns>
        //public override bool Preview()
        //{
        //    if (LoadTempleteFile(@"料具租赁管理\料具租赁合同打印.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.PrintPreview(true,true,true,0,0,0,0,0);
        //    return true;
        //}

        //public override bool Print()
        //{
        //    if (LoadTempleteFile(@"料具租赁管理\料具租赁合同打印.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.Print();
        //    return true;
        //}

        //public override bool Export()
        //{
        //    if (LoadTempleteFile(@"料具租赁管理\料具租赁合同打印.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.ExportToExcel("料具租赁合同【" + curBillMaster.Code + "】", false, false, true);
        //    return true;
        //}

        //private bool LoadTempleteFile(string modelName)
        //{
        //    ExploreFile eFile = new ExploreFile();
        //    string path = eFile.Path;
        //    if (eFile.IfExistFileInServer(modelName))
        //    {
        //        eFile.CreateTempleteFileFromServer(modelName);
        //        //载入格式和数据
        //        flexGrid1.OpenFile(path + "\\" + modelName);//载入格式
        //    }
        //    else
        //    {
        //        MessageBox.Show("未找到模板格式文件【" + modelName + "】");
        //        return false;
        //    }
        //    return true;
        //}

        //private void FillFlex(MaterialRentalOrderMaster billMaster)
        //{
        //    int detailStartRowNumber = 7;//7为模板中的行号
        //    int detailCount = billMaster.Details.Count;

        //    //插入明细行
        //    flexGrid1.InsertRow(detailStartRowNumber, detailCount);

        //    //设置单元格的边框，对齐方式
        //    FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
        //    range.Alignment = FlexCell.AlignmentEnum.RightCenter;
        //    range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
        //    range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
        //    range.Mask = FlexCell.MaskEnum.Digital;

        //    //主表数据

        //    flexGrid1.Cell(2, 1).Text = "使用单位：";
        //    flexGrid1.Cell(2, 4).Text = "登记时间：" + DateTime.Now.ToShortDateString();
        //    flexGrid1.Cell(2, 7).Text = "制单编号：" + billMaster.Code;
        //    flexGrid1.Cell(4, 2).Text = billMaster.OriginalContractNo;
        //    flexGrid1.Cell(4, 5).Text = "";//合同名称
        //    flexGrid1.Cell(4, 7).Text = "";//材料分类
        //    flexGrid1.Cell(5, 2).Text = "";//租赁单位
        //    flexGrid1.Cell(5, 2).WrapText = true;
        //    flexGrid1.Cell(5, 5).Text = "";//承租单位
        //    flexGrid1.Row(5).AutoFit();
        //    flexGrid1.Cell(5, 7).Text = billMaster.RealOperationDate.ToShortDateString();//签订日期

        //    FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
        //    pageSetup.LeftFooter = "   制单人：" + billMaster.CreatePersonName;
        //    pageSetup.RightFooter = "第 &P 页/共 &N 页      ";

        //    System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("自定义纸张", 910, 470);
        //    pageSetup.PaperSize = paperSize;

        //    //填写明细数据
        //    for (int i = 0; i < detailCount; i++)
        //    {
        //        MaterialRentalOrderDetail detail = (MaterialRentalOrderDetail)billMaster.Details.ElementAt(i);
        //        //物资名称
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialResource.Name;
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Alignment = FlexCell.AlignmentEnum.LeftCenter;

        //        //规格型号
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialResource.Specification;
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Alignment = FlexCell.AlignmentEnum.LeftCenter;

        //        //结算规则
        //        flexGrid1.Cell(detailStartRowNumber + i, 3).Text = Enum.GetName(typeof(EnumMaterialMngBalRule), detail.BalRule);
        //        flexGrid1.Cell(detailStartRowNumber + i, 3).Alignment = FlexCell.AlignmentEnum.LeftCenter;

        //        //数量
        //        flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.Quantity.ToString();

        //        //日租金
        //        flexGrid1.Cell(detailStartRowNumber + i, 5).Text = "";

        //        //金额
        //        flexGrid1.Cell(detailStartRowNumber + i, 6).Text = "";

        //        //备注
        //        flexGrid1.Cell(detailStartRowNumber + i, 7).Text = detail.Descript;
        //        flexGrid1.Cell(detailStartRowNumber + i, 7).Alignment = FlexCell.AlignmentEnum.LeftCenter;
        //    }
        //}
    }
}
