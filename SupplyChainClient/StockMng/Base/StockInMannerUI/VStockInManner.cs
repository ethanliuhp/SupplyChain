using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.StockManage.Base.Service;
using VirtualMachine.Component;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using System.Collections;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Core;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.Base.StockInMannerUI
{
    public enum StockInMannerExcType
    {
        commonSelect
    }
    public partial class VStockInManner : TBasicDataView
    {
        private IMStockInManner theMStockInManner = StaticMethod.GetRefModule(typeof(MStockInManner)) as IMStockInManner; // new MStockInManner();
        private IList lstObj = new ArrayList();
        public VStockInManner()
        {
            InitializeComponent();
            this.Start();
            this.Title = "入库方式编码";
            InitEvent();
        }

        private void InitEvent()
        {
            this.grdDetail.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(grdDetail_RowHeaderMouseClick);
            this.grdDetail.CellEnter += new DataGridViewCellEventHandler(grdDetail_CellEnter);
            this.grdDetail.DefaultValuesNeeded += new DataGridViewRowEventHandler(grdDetail_DefaultValuesNeeded);
            this.grdDetail.UserDeletingRow += new DataGridViewRowCancelEventHandler(grdDetail_UserDeletingRow);
        }

        void grdDetail_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            //if (MessageBox.Show("是否删除此入库方式?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.No)
            //{
            //    e.Cancel = true;
            //}
            //else
            //{

            if (theMStockInManner.Delete((e.Row.Tag as StockInManner)))
            {
                MessageBox.Show("删除成功！");
            }
            else
            {
                e.Cancel = true;
            }

            //}
        }
        void grdDetail_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["State"].Value = 1;
        }
        void grdDetail_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            grdDetail[e.ColumnIndex, e.RowIndex].ReadOnly = false;
        }

        void grdDetail_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.grdDetail.CurrentCell.ReadOnly = true;
        }
        public void Start()
        {
            try
            {
                InitData();
            }
            catch (Exception e)
            {
                MessageBox.Show("窗体初始化出错：" + ExceptionUtil.ExceptionMessage(e)); ;
            }
        }
        private void InitData()
        {
            lstObj = theMStockInManner.GetData();
            this.grdDetail.Rows.Clear();
            foreach (StockInManner var in lstObj)
            {
                int i = grdDetail.Rows.Add();
                grdDetail.Rows[i].Tag = var;
                grdDetail["Code", i].Value = var.Code;
                grdDetail["Name", i].Value = var.Name;
                grdDetail["State", i].Value = var.State;
            }
            this.grdDetail.CurrentCell = this.grdDetail.Rows[this.grdDetail.Rows.Count - 1].Cells["Code"];
        }
        public override void RefreshControls(MainViewState state)
        {
            ToolMenu.LockItem(ToolMenuItem.Refresh);
            ToolMenu.LockItem(ToolMenuItem.Cancel);
            ToolMenu.LockItem(ToolMenuItem.Modify);
            ToolMenu.UnlockItem(ToolMenuItem.Save);
        }

        //保存修改
        public override bool SaveView()
        {
            try
            {
                if (!ViewToModel()) return false;
                lstObj = theMStockInManner.Save(lstObj);
                //界面赋值

                MessageBox.Show("保存成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存出错：" + ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
            return true;
        }
        private bool ViewToModel()
        {
            string ErrMsg = "";
            try
            {
                lstObj.Clear();
                this.grdDetail.EndEdit();
                foreach (DataGridViewRow var in this.grdDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    if (var.Tag == null)
                        var.Tag = new StockInManner();
                    StockInManner theStockInManner = var.Tag as StockInManner;
                    theStockInManner.Code = ClientUtil.ToString(var.Cells["Code"].Value);
                    theStockInManner.Name = ClientUtil.ToString(var.Cells["Name"].Value);
                    theStockInManner.State = Convert.ToInt32(var.Cells["State"].Value);
                    lstObj.Add(theStockInManner);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
                return false;
            }
            return true;
        }
    }
    public class CStockInManner
    {
        private static IFramework framework;
        public CStockInManner(IFramework fw)
        {
            if (framework == null)
                framework = fw;
        }
        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                VStockInManner a = new VStockInManner();
                a.ViewCaption = "入库方式编码";
                framework.AddMainView(a);
                a.Start();
            }
            else
            {
                object o = args[0];
                StockInMannerExcType excuteType = (StockInMannerExcType)o;
                ObjectQuery oq = null;
                if (args.Length > 1)
                    oq = args[1] as ObjectQuery;
                switch (excuteType)
                {
                    case StockInMannerExcType.commonSelect:
                        VCommonStockInManner aa = new VCommonStockInManner();
                        aa.OpenSelectView(oq, args[2] as IWin32Window);
                        return aa.Result;
                    default:
                        break;
                }
            }

            return null;
        }
    }
    public class MStockInManner : IMStockInManner
    {
        private static IStockInMannerSrv theStockInMannerSrv = null;
        public MStockInManner()
        {
            if (theStockInMannerSrv == null)
            {
                theStockInMannerSrv = StaticMethod.GetService("StockInMannerSrv") as IStockInMannerSrv;
            }
        }
        public IList Save(IList lst)
        {
            return theStockInMannerSrv.SaveOrUpdateByDao(lst);
        }
        public bool Delete(StockInManner obj)
        {
            return theStockInMannerSrv.DeleteByDao(obj);
        }
        public IList GetData()
        {
            return theStockInMannerSrv.FindAll(typeof(StockInManner));
        }
    }
    public interface IMStockInManner
    {
        IList Save(IList lst);
        bool Delete(StockInManner obj);
        IList GetData();
    }
}