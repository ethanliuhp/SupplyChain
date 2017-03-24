using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.ApprovalMng.AppTableSetMng.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.MobileManage;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatMng
{
    public partial class VAppDetail : TBasicToolBarByMobile
    {
        private MAppPlatMng Model = new MAppPlatMng();

        private AutomaticSize automaticSize = new AutomaticSize();
        object currMaster = new object();
        AppTableSet currATSet = new AppTableSet();//接受传过来的变量
        IEnumerable<object> listDtl = null;
        int pageIndex = 0;
        int pageCount = 0;
        public IList listDetailDataList = new ArrayList();
        public AppTableSet OptAppTable = null;// 定义一个全局变量
        public VAppDetail(object master, AppTableSet appTSet)
        {
            InitializeComponent();
            currMaster = master;
            currATSet = appTSet;
            automaticSize.SetTag(this);
            InitEnent();
            InitDate();
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
            object detailsO = currMaster.GetType().GetProperty("Details").GetValue(currMaster, null);
            NHibernate.Collection.PersistentSet set = (NHibernate.Collection.PersistentSet)detailsO;

            listDtl = set.OfType<object>();
            pageCount = listDtl.Count();
            if (pageCount > 0)
            {
                object currDetail = listDtl.ElementAt(pageIndex);
                AddDetail(currDetail);
            }
            btnBack.Enabled = false;
            if (pageCount <= 1)
            {
                btnNext.Enabled = false;
            }
            if(pageCount<=1)
            {
                btnBack.Visible = false;
                btnNext.Visible = false;
            }
            label1.Text = "第【" + (pageIndex + 1) + "】条";
            label2.Text = "共【" + pageCount + "】条";
        }
        public void InitEnent()
        {
            btnNext.Click += new EventHandler(btnNext_Click);
            btnBack.Click += new EventHandler(btnBack_Click);
            
        }
        void btnBack_Click(object sender, EventArgs e)
        {
            pageIndex -= 1;
            if (pageIndex == 0)
            {
                btnBack.Enabled = false;
            }
            else
            {
                btnBack.Enabled = true;
                btnNext.Enabled = true;
            }
            dgDetail.Rows.Clear();
            label1.Text = "第【" + (pageIndex + 1) + "】条";
            label2.Text = "共【" + pageCount + "】条";
            AddDetail(listDtl.ElementAt(pageIndex));
        }
        void btnNext_Click(object sender, EventArgs e)
        {
            pageIndex += 1;

            if (pageIndex == pageCount - 1)
            {
                btnNext.Enabled = false;
            }
            else
            {
                btnNext.Enabled = true;
                btnBack.Enabled = true;
            }
            dgDetail.Rows.Clear();
            label1.Text = "第【" + (pageIndex + 1) + "】条";
            label2.Text = "共【" + pageCount + "】条";
            AddDetail(listDtl.ElementAt(pageIndex));
        }
        void AddDetail(object currDetail)
        {
            //object listDtl = this.currMaster.GetType().GetProperty("Details").GetValue(currMaster, null);
            //NHibernate.Collection.PersistentSet set = (NHibernate.Collection.PersistentSet)listDtl;
            listDetailDataList = new ArrayList();
            IList List_DetailProperty = Model.GetAppDetailProperties(currATSet.Id);
            if (List_DetailProperty != null)
            {
                foreach (AppDetailPropertySet DetailProperty in List_DetailProperty)
                {
                    if (DetailProperty.DetailPropertyVisible == true)
                    {
                        int rowIndex = dgDetail.Rows.Add();
                        dgDetail["ColName", rowIndex].Value = DetailProperty.DetailPropertyChineseName;//给第一列命名（取名称）
                        foreach (System.Reflection.PropertyInfo pi in currDetail.GetType().GetProperties())
                        {
                            if (DetailProperty.DetailPropertyName == pi.Name)
                            {
                                dgDetail["ColValue", rowIndex].Value = ClientUtil.ToString(pi.GetValue(currDetail, null));//给第二列取值
                                AppDetailData theAppDetailData = new AppDetailData();
                                theAppDetailData.PropertyName = pi.Name;
                                theAppDetailData.PropertyValue = ClientUtil.ToString(pi.GetValue(currDetail, null));
                                theAppDetailData.BillId = currATSet.Id;
                                theAppDetailData.BillDtlId = currATSet.Id;
                                theAppDetailData.PropertyChineseName = DetailProperty.DetailPropertyChineseName;
                                theAppDetailData.PropertySerialNumber = DetailProperty.SerialNumber;
                                theAppDetailData.AppStatus = 2L;
                                theAppDetailData.AppTableSet = currATSet.Id;
                                listDetailDataList.Add(theAppDetailData);
                            }
                        }
                    }
                }
            }

        }

        private void VAppDetail_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
