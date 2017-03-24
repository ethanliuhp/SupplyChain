using System;
using System.Windows.Forms;
using VirtualMachine.Component.Util;
using C1.Win.C1FlexGrid;
using System.Collections;
using Application.Resource.BasicData.Domain;
//using Application.Business.Erp.SupplyChain.CostingMng.InitData.Domain;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.Basic.Controls;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppTableSetUI
{
    public partial class VAppTableSet : Application.Business.Erp.SupplyChain.Client.Basic.Template.TBasicDataView
    {
        private MAppTableSet Model = new MAppTableSet();
        private IList Lst = new ArrayList();
        public VAppTableSet()
        {
            InitializeComponent();
            this.InitEvent();
            InitData();
        }

        private void InitEvent()
        {
            this.btnSave.Click += new EventHandler(btnSave_Click);

        }
        void InitData()
        {
            CurrentProjectInfo ProjectInfo = StaticMethod.GetProjectInfo();
            Dgv.Rows.DefaultSize = 35;
            ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("Status",0));
            //oq.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));
            Lst = Model.GetObjects(oq);
            foreach (AppTableSet item in Lst)
            {
                Row row = this.Dgv.Rows.Add();
                row["TableName"] = item.TableName;
                row["ClassName"] = item.ClassName;
                row["PhysicsName"] = item.PhysicsName;
                row["DetailClassName"] = item.DetailClassName;
                row["DetailPhysicsName"] = item.DetailPhysicsName;
                row["StatusName"] = item.StatusName;
                row["StatusValueAgr"] = item.StatusValueAgr;
                row["StatusValueDis"] = item.StatusValueDis;
                row["ExecCode"] = item.ExecCode;
                row["QueryCode"] = item.QueryCode;
                row["MasterNameSpace"] = item.MasterNameSpace;
                row["DetailNameSpace"] = item.DetailNameSpace;
                row["ExecCode"] = item.ExecCode;
                row["Remark"] = item.Remark;
                if (item.Status == 0)
                {
                    row["Status"] = "启用";
                }
                else
                {
                    row["Status"] = "停用";
                }

                row.UserData = item;
            }
        }


        void btnSave_Click(object sender, EventArgs e)
        {
            if (!valideValue()) return;
            ViewToModel();
            Lst = Model.Save(Lst);
            MessageBox.Show("保存成功！");
        }
        private bool valideValue()
        {
            for (int i = 1; i < this.Dgv.Rows.Count - 1; i++)
            {
                Row row = this.Dgv.Rows[i];
                if (ClientUtil.ToString(this.Dgv[i, "TableName"]) == "")
                {
                    MessageBox.Show("第【" + i + "】行 表单名称 不能为空！");
                    return false;
                }
                if (ClientUtil.ToString(this.Dgv[i, "ClassName"]) == "")
                {
                    MessageBox.Show("第【" + i + "】行 主表类名称 不能为空！");
                    return false;
                }
                if (ClientUtil.ToString(this.Dgv[i, "PhysicsName"]) == "")
                {
                    MessageBox.Show("第【" + i + "】行 主表物理名称 不能为空！");
                    return false;
                }
                //if (ClientUtil.ToString(this.Dgv[i, "DetailClassName"]) == "")
                //{
                //    MessageBox.Show("第【" + i + "】行 明细类名称 不能为空！");
                //    return false;
                //}
                //if (ClientUtil.ToString(this.Dgv[i, "DetailPhysicsName"]) == "")
                //{
                //    MessageBox.Show("第【" + i + "】行 明细物理名称 不能为空！");
                //    return false;
                //}
                if (ClientUtil.ToString(this.Dgv[i, "StatusName"]) == "")
                {
                    MessageBox.Show("第【" + i + "】行 状态名称 不能为空！");
                    return false;
                }
                if (ClientUtil.ToString(this.Dgv[i, "StatusValueAgr"]) == "")
                {
                    MessageBox.Show("第【" + i + "】行 通过状态值 不能为空！");
                    return false;
                }
                if (ClientUtil.ToString(this.Dgv[i, "StatusValueDis"]) == "")
                {
                    MessageBox.Show("第【" + i + "】行 原状态值 不能为空！");
                    return false;
                }
                if (ClientUtil.ToString(this.Dgv[i, "MasterNameSpace"]) == "")
                {
                    MessageBox.Show("第【" + i + "】行 主表类的命名空间不能为空！");
                    return false;
                }
                //if (ClientUtil.ToString(this.Dgv[i, "DetailNameSpace"]) == "")
                //{
                //    MessageBox.Show("第【" + i + "】行 明细类的命名空间不能为空！");
                //    return false;
                //}
            }
            return true;
        }
        private void ViewToModel()
        {
            CurrentProjectInfo ProjectInfo = StaticMethod.GetProjectInfo();
            Lst.Clear();
            for (int i = 1; i < this.Dgv.Rows.Count - 1; i++)
            {
                Row row = this.Dgv.Rows[i];

                AppTableSet Cmc = row.UserData as AppTableSet;
                if (Cmc == null)
                {
                    Cmc = new AppTableSet();
                }
                Cmc.TableName = ClientUtil.ToString(Dgv[i, "TableName"]);
                Cmc.ClassName = ClientUtil.ToString(Dgv[i, "ClassName"]);
                Cmc.PhysicsName = ClientUtil.ToString(Dgv[i, "PhysicsName"]);
                Cmc.DetailClassName = ClientUtil.ToString(Dgv[i, "DetailClassName"]);
                Cmc.DetailPhysicsName = ClientUtil.ToString(Dgv[i, "DetailPhysicsName"]);
                Cmc.StatusName = ClientUtil.ToString(Dgv[i, "StatusName"]);
                Cmc.StatusValueAgr = ClientUtil.ToString(Dgv[i, "StatusValueAgr"]);
                Cmc.StatusValueDis = ClientUtil.ToString(Dgv[i, "StatusValueDis"]);
                Cmc.QueryCode = ClientUtil.ToString(Dgv[i, "QueryCode"]);
                Cmc.ExecCode = ClientUtil.ToString(Dgv[i, "ExecCode"]);
                Cmc.MasterNameSpace = ClientUtil.ToString(Dgv[i, "MasterNameSpace"]);
                Cmc.DetailNameSpace = ClientUtil.ToString(Dgv[i, "DetailNameSpace"]);
                Cmc.Remark = ClientUtil.ToString(Dgv[i, "Remark"]);
                Cmc.ProjectId = ProjectInfo.Id;
                Cmc.ProjectName = ProjectInfo.Name;
                if (ClientUtil.ToString(Dgv[i, "Status"]) == "启用")
                {
                    Cmc.Status = 0;
                }
                else
                {
                    Cmc.Status = 1;
                }
                Lst.Add(Cmc);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Lst.Clear();
            foreach (Row item in Dgv.Rows.Selected)
            {
                AppTableSet C = item.UserData as AppTableSet;
                if (C != null)
                {
                    Lst.Add(C);
                }
            }
            if (Model.Delete(Lst) == true)
            {
                foreach (Row item in Dgv.Rows.Selected)
                {
                    Dgv.Rows.Remove(item);
                }
            }
            else
            {
                MessageBox.Show("删除失败！");
            }
        }
    }
}