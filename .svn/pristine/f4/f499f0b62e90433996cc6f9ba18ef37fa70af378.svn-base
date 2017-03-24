using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Core;
using NHibernate.Criterion;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.ProInRecord
{
    public partial class VProRecord :TBasicToolBarByMobile
    {
        private AutomaticSize automaticSize = new AutomaticSize();
        private IList listDtl = new ArrayList();
        private int pageIndex = 0;
        private ProfessionInspectionRecordDetail precord = new ProfessionInspectionRecordDetail();
        private ProfessionInspectionRecordMaster curBillMaster;
        private string rproject = null;//检查专业
        MProRecord model = new MProRecord();
        IList movedDtlList = new ArrayList();//存储删除的明细。新增、保存、修改时清空
        public VProRecord( IList list,string recProject)
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            listDtl = list;
            rproject = recProject;
            InitDate();
            Initevents();
        }
        /// <summary>
        /// 当前单据
        /// </summary>
        public ProfessionInspectionRecordMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        private void Initevents()
        {
            cbResult.SelectedIndexChanged += new EventHandler(cbResult_SelectedIndexChanged);
            btnback.Click += new EventHandler(btnback_Click);
            btnFirst.Click += new EventHandler(btnFirst_Click);
            btnnext.Click += new EventHandler(btnnext_Click);
            btnlast.Click += new EventHandler(btnlast_Click);
            this.Load += new EventHandler(VProRecord_Load);
        }

        void VProRecord_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        void btnnext_Click(object sender, EventArgs e)
        {
            pageIndex += 1;
            if (pageIndex == listDtl.Count - 1)
            {
                btnnext.Enabled = false;
                btnlast.Enabled = false;
            }
            btnback.Enabled = true;
            btnFirst.Enabled = true;
            ModelToView();
        }

        void btnlast_Click(object sender, EventArgs e)
        {
            pageIndex = listDtl.Count - 1;
            btnlast.Enabled = false;
            btnnext.Enabled = false;
            btnFirst.Enabled = true;
            btnback.Enabled = true;
            ModelToView();
        }

        void btnFirst_Click(object sender, EventArgs e)
        {
            pageIndex = 0;
            btnFirst.Enabled = false;
            btnback.Enabled = false;
            btnnext.Enabled = true;
            btnlast.Enabled = true;
            ModelToView();
        }

        void btnback_Click(object sender, EventArgs e)
        {
            pageIndex -= 1;
            if (pageIndex == 0)
            {
                btnback.Enabled = false;
                btnFirst.Enabled = false;
            }
            btnnext.Enabled = true;
            btnlast.Enabled = true;
            ModelToView();
        }

        void cbResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbResult.SelectedItem.Equals("检查通过"))
            {
                rbNo.Checked = true;
                rbNo.Enabled = false; ;
                rbYes.Enabled = false;
            }
            if (cbResult.SelectedItem.Equals("检查不通过"))
            {
                rbYes.Checked = true;
                rbNo.Enabled = true;
                rbYes.Enabled = true;
            }
        }

        public override void 功能菜单1Item_Click(object sender, EventArgs e)
        {
            base.功能菜单1Item_Click(sender, e);
            SaveView();//保存
        }
        public override void 功能菜单2Item_Click(object sender, EventArgs e)
        {
            base.功能菜单2Item_Click(sender, e);
            SubmitView();
        }
        public override void 功能菜单3Item_Click(object sender, EventArgs e)
        {
            base.功能菜单3Item_Click(sender, e);
            this.FindForm().Close();
            //this.btnOK.FindForm().Close();
        }
        public override void TBtnPageUp_Click(object sender, EventArgs e)
        {
            base.TBtnPageUp_Click(sender, e);
            this.FindForm().Close();
        }
        public override void TBtnPageDown_Click(object sender, EventArgs e)
        {
            base.TBtnPageDown_Click(sender, e);
            VMessageBox v_show = new VMessageBox();
            v_show.txtInformation.Text = "当前是最后一步！";
            v_show.ShowDialog();
            return;
        }
        private void InitDate()
        {
            this.功能菜单1Item.Visible = true;
            this.功能菜单1Item.Text = "保存专业检查";

            this.功能菜单2Item.Visible = true;
            this.功能菜单2Item.Text = "提交专业检查";

            this.功能菜单3Item.Visible = true;
            this.功能菜单3Item.Text = "放弃本次操作！";
            cbResult.DataSource = (Enum.GetNames(typeof(EnumConclusionType)));
            
            ModelToView();
            NewView();
            btnFirst.Enabled = false;
            btnback.Enabled = false;
            if (listDtl.Count <=1)
            {
                btnnext.Enabled = false;
                btnlast.Enabled = false;
            }
        }
        public bool NewView()
        {
            try
            {
                //ObjectQuery oq = new ObjectQuery();
                //oq.AddCriterion(Expression.Eq("Id",precord.Master.Id));
                curBillMaster = model.ProfessionInspectionSrv.GetProfessionInspectionRecordById(precord.Master.Id);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        public void ModelToView()
        {
            if (listDtl.Count >= 1)
            {
                precord = listDtl[pageIndex] as ProfessionInspectionRecordDetail;
                string id = precord.Id;
                cbTaskHandler.Text = precord.InspectionSupplierName;
                //cbTaskHandler.Items.Add(precord.InspectionSupplierName);//承担队伍
               long Ispass =precord.Master.Version;//检查结果
               if (Ispass==0)
               {
                   cbResult.Text = "检查通过";
               }
               else
               {
                   cbResult.Text = "检查不通过";
               }
               lbxResult.Text = precord.Descript;

            } 
            if(listDtl.Count==1)
            {
                btnFirst.Visible = false;
                btnback.Visible = false;
                btnnext.Visible = false;
                btnlast.Visible = false;
            }
            label6.Text = "第【" + (pageIndex+1) + "】条";
            label7.Text = "共【 "+ listDtl.Count + "】条";

        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public bool SubmitView()
        {
            try
            {
                if (!ViewToModel()) return false;
                curBillMaster.DocState = DocumentState.InExecute;
                curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                curBillMaster.AuditDate = ConstObject.LoginDate;//制单时间
                curBillMaster.AuditYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.AuditMonth = ConstObject.LoginDate.Month;//制单月
                curBillMaster = model.ProfessionInspectionSrv.SaveProfessionInspectionRecordPlan(curBillMaster);
               // MessageBox.Show();
               // precord = listDtl[pageIndex] as ProfessionInspectionRecordDetail;
                VMessageBox v_show = new VMessageBox();
                v_show.txtInformation.Text = "提交成功！";
                v_show.ShowDialog();
               
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public  bool SaveView()
        {
            try
            {
                if (!ViewToModel()) return false;
                bool flag = false;
                if (string.IsNullOrEmpty(curBillMaster.Id))
                {
                    flag = true;
                }
                curBillMaster = model.ProfessionInspectionSrv.SaveProfessionInspectionRecordPlan(curBillMaster);

                //更新Caption
                LogData log = new LogData();
                log.BillId = curBillMaster.Id;
                log.BillType = "专业检查记录单";
                log.Code = curBillMaster.Code;
                log.Descript = "";
                log.OperPerson = ConstObject.LoginPersonInfo.Name;
                log.ProjectName = curBillMaster.ProjectName;
                if (flag)
                {
                    log.OperType = "新增";
                }
                else
                {
                    log.OperType = "修改";
                }
                StaticMethod.InsertLogData(log);
                //MessageBox.Show();
                VMessageBox v_show = new VMessageBox();
                v_show.txtInformation.Text = "保存成功！";
                v_show.ShowDialog();

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }
        /// <summary>
        /// 保存前的数据验证
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            if (cbTaskHandler.Text == "")
            {
                MessageBox.Show("请选择承担队伍的名称！");
                return false;
            }
            if (cbResult.Text == "")
            {
                MessageBox.Show("检查结论不能为空！");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            if (curBillMaster.DocState == DocumentState.InExecute)
            {
                string message = "此单状态为【{0}】，不能修改！";
                message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
                VMessageBox v_show = new VMessageBox();
                v_show.txtInformation.Text = message;
                v_show.ShowDialog();
                return false;
            }
            try
            {
                curBillMaster.RealOperationDate = ConstObject.LoginDate;//实际业务日期
                curBillMaster.Descript = lbxResult.Text;// 检查结果，备注
                curBillMaster.InspectionSpecail = rproject;//检查专业
                curBillMaster.CreateDate = ConstObject.LoginDate;
                curBillMaster.HandlePerson = precord.InspectionPerson;//负责人
                curBillMaster.HandlePersonName =precord.InspectionPersonName;// 负责人名称
                curBillMaster.AuditPerson = ConstObject.LoginPersonInfo;//制单人编号
                curBillMaster.AuditPersonName = ConstObject.LoginPersonInfo.Name;//制单人名称
                curBillMaster.AuditDate = ConstObject.LoginDate;//制单时间
                curBillMaster.AuditYear = ConstObject.LoginDate.Year;//制单年
                curBillMaster.AuditMonth = ConstObject.LoginDate.Month;//制单月
                curBillMaster.DocState = DocumentState.Edit;

                ProfessionInspectionRecordDetail curBillDtl = new ProfessionInspectionRecordDetail();
                curBillDtl.Descript = lbxResult.Text;//备注
                string strName = cbResult.Text;
                if (strName.Equals("检查通过"))
                {
                    curBillDtl.InspectionConclusion = 0;
                }
                else
                {
                    curBillDtl.InspectionConclusion = 1;
                }
                if (rbNo.Checked == true)
                {
                    curBillDtl.CorrectiveSign = 0;
                }
                else
                {
                    curBillDtl.CorrectiveSign = 1;
                }
                curBillDtl.InspectionContent =precord.InspectionContent;//检查内容
                curBillDtl.InspectionDate = ConstObject.LoginDate;//要求整改时间
                curBillDtl.InspectionPerson = precord.InspectionPerson;//受检管理负责人
                curBillDtl.InspectionPersonName = precord.InspectionPersonName;//受检管理负责人名称
                //curBillDtl.InspectionSituation = ClientUtil.ToString(var.Cells[colInspectionSituation.Name].Value);//检查情况
                curBillDtl.InspectionSupplier = precord.InspectionSupplier;//受检承担单位
                curBillDtl.InspectionSupplierName = precord.InspectionSupplierName;//受检承担单位名称
                curBillMaster.AddDetails(curBillDtl);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private bool ModityView()
        {
            if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
            {
                curBillMaster = model.ProfessionInspectionSrv.GetProfessionInspectionRecordById(curBillMaster.Id);
                ModelToView();
                return true;
            }
            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
            MessageBox.Show(message);
            return false;
        }
        ///// <summary>
        ///// 撤销
        ///// </summary>
        ///// <returns></returns>
        //public bool CancelView()
        //{
        //    try
        //    {
        //        switch (ViewState)
        //        {
        //            case MainViewState.Modify:
        //                //重新查询数据
        //                curBillMaster = model.ProfessionInspectionSrv.GetProfessionInspectionRecordById(curBillMaster.Id);
        //                break;
        //            default:
        //                Clear();
        //                break;
        //        }
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show("数据撤销错误：" + ExceptionUtil.ExceptionMessage(e));
        //        return false;
        //    }
        //}
        public void Clear() 
        {
            cbTaskHandler.Text = "";
            cbResult.Text = "";
            lbxResult.Text = "";
        }
    }

}
