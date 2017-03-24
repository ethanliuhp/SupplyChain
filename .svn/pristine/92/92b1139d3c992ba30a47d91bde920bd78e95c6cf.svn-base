using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using System.Collections;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.StockManage.Base.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.Base.StockOutPurposeUI
{
    public enum StockOutPurposeExcType
    {
        commonSelect
    }
    public partial class VStockOutPurpose : TBasicDataView
    {
        private static IMStockOutPurpose theMStockOutPurpose = StaticMethod.GetRefModule(typeof(MStockOutPurpose)) as IMStockOutPurpose;
        private IList lstObj = new ArrayList();
        public VStockOutPurpose()
        {
            InitializeComponent();
            this.Start();
            InitEvent();
            this.Title = "出库用途编码";
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

            if (theMStockOutPurpose.Delete((e.Row.Tag as StockOutPurpose)))
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
            lstObj = theMStockOutPurpose.GetData();
            this.grdDetail.Rows.Clear();
            foreach (StockOutPurpose var in lstObj)
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
                lstObj = theMStockOutPurpose.Save(lstObj);
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
                        var.Tag = new StockOutPurpose();
                    StockOutPurpose theStockOutPurpose = var.Tag as StockOutPurpose;
                    theStockOutPurpose.Code = ClientUtil.ToString(var.Cells["Code"].Value);
                    theStockOutPurpose.Name = ClientUtil.ToString(var.Cells["Name"].Value);
                    theStockOutPurpose.State = Convert.ToInt32(var.Cells["State"].Value);
                    lstObj.Add(theStockOutPurpose);
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
    public class CStockOutPurpose
    {
        private static IFramework framework;
        public CStockOutPurpose(IFramework fw)
        {
            if (framework == null)
                framework = fw;
        }
        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                VStockOutPurpose a = new VStockOutPurpose();
                a.ViewCaption = "出库用途编码";
                framework.AddMainView(a);
                a.Start();
            }
            else
            {
                object o = args[0];
                StockOutPurposeExcType excuteType = (StockOutPurposeExcType)o;
                IList oq = null;
                if (args.Length > 1)
                    oq = args[1] as ArrayList;
                switch (excuteType)
                {
                    case StockOutPurposeExcType.commonSelect:
                        VCommonStockOutPurpose aa = new VCommonStockOutPurpose();
                        aa.OpenSelectView(oq, args[2] as IWin32Window);
                        return aa.Result;
                    default:
                        break;
                }
            }
            return null;
        }
    }
    public class MStockOutPurpose : IMStockOutPurpose
    {
        private static IStockOutPurposeSrv theStockOutPurposeSrv = null;
        public MStockOutPurpose()
        {
            if (theStockOutPurposeSrv == null)
            {
                theStockOutPurposeSrv = StaticMethod.GetService("StockOutPurposeSrv") as IStockOutPurposeSrv;
            }
        }
        public IList Save(IList lst)
        {
            return theStockOutPurposeSrv.SaveOrUpdateByDao(lst);
        }
        public bool Delete(StockOutPurpose obj)
        {
            return theStockOutPurposeSrv.DeleteByDao(obj);
        }
        public IList GetData()
        {
            return theStockOutPurposeSrv.FindAll(typeof(StockOutPurpose));
        }
    }
    public interface IMStockOutPurpose
    {
        IList Save(IList lst);
        bool Delete(StockOutPurpose obj);
        IList GetData();
    }
}