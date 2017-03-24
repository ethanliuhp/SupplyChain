using System;
using System.Windows.Forms;
using VirtualMachine.Component.Util;
using C1.Win.C1FlexGrid;
using System.Collections;
using Application.Resource.BasicData.Domain;
//using Application.Business.Erp.SupplyChain.CostingMng.InitData.Domain;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using System.Collections.Generic;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatformUI;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppSolutionSetUI
{
    public partial class VAppSolutionSet : Application.Business.Erp.SupplyChain.Client.Basic.Template.TBasicDataView
    {
        private MAppSolutionSet Model = new MAppSolutionSet();
        private IList Lst = new ArrayList();
        private AppTableSet curTableSet = null;
        private AppSolutionSet curSolutionSet = null;
        private AppStepsSet curStepSet = null;
        public VAppSolutionSet()
        {
            InitializeComponent();
            this.InitEvent();
            InitData();
        }

        private void InitEvent()
        {
            this.btnSolutionSave.Click += new EventHandler(btnSave_Click);
            this.Dgv.MouseDown += new MouseEventHandler(Dgv_MouseDown);
            this.FgSolution.MouseDown += new MouseEventHandler(FgSolution_MouseDown);
            this.FgSetp.MouseDown += new MouseEventHandler(FgSetp_MouseDown);
            this.BtnRoleSave.Click += new EventHandler(BtnRoleSave_Click);
            this.BtnRoleDel.Click += new EventHandler(BtnRoleDel_Click);
            this.FgRole.DoubleClick += new EventHandler(FgRole_DoubleClick);
            this.FgRole.LeaveCell += new EventHandler(FgRole_LeaveCell);
            btnAddStep.Click += new EventHandler(btnAddStep_Click);
        }
        /// <summary>
        /// 新增审批步骤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAddStep_Click(object sender, EventArgs e)
        {
            int maxStepOrder = 1;
            if (FgSetp.Rows.Count == 1)
            {
                maxStepOrder = 1;
                FgSetp.Rows.Add();
                FgSetp.Rows[FgSetp.RowSel]["StepOrder"] = maxStepOrder;
            }
            else
            {
                foreach (Row row in FgSetp.Rows)
                {
                    int stepOrder = ClientUtil.ToInt(row["StepOrder"]);
                    if (stepOrder > maxStepOrder)
                    {
                        maxStepOrder = stepOrder;
                    }
                }

                FgSetp.Rows.Add();
                FgSetp.Rows[FgSetp.Rows.Count - 1]["StepOrder"] = maxStepOrder + 1;
            }
        }

        OperationRole sSysRole = null;
        void FgRole_LeaveCell(object sender, EventArgs e)
        {
            try
            {
                if (FgRole.Row >= FgRole.RowSel && FgRole.RowSel > -1)
                {
                    OperationRole tempRole = FgRole.GetUserData(FgRole.RowSel, "AppRole") as OperationRole;
                    if (tempRole != null)
                    {

                        //FgRole.SetUserData(FgRole.RowSel, "AppRole", sSysRole);
                        FgRole[FgRole.RowSel, "AppRole"] = tempRole.RoleName;
                        //FgRole[FgRole.RowSel, "Remark"] = sSysRole;
                    }
                }
            }
            catch { }
        }

        void FgRole_DoubleClick(object sender, EventArgs e)
        {
            if (FgRole.Cols[FgRole.ColSel].Name == "AppRole")
            {
                VSelectOperationRole vs = new VSelectOperationRole();
                vs.ShowDialog();
                List<OperationRole> roleLst = vs.Result;
                if (roleLst.Count > 0)
                {
                    foreach (OperationRole role in roleLst)
                    {
                        if (RoleAdded(role)) continue;
                        object o = FgRole.GetUserData(FgRole.RowSel, "AppRole");
                        if (o == null)
                        {
                            FgRole.SetUserData(FgRole.RowSel, "AppRole", role);
                            //FgRole[FgRole.RowSel, "AppRole"] = role.RoleName;
                            FgRole.SetData(FgRole.RowSel, "AppRole", role.RoleName);
                            FgRole.FinishEditing();
                            //FgRole.EndInit();
                            try
                            {
                                FgRole.Select(FgRole.RowSel + 1, 2);
                            }
                            catch {
                                FgRole.Select(FgRole.RowSel, 2);
                            }
                        }
                        else
                        {
                            Row row = FgRole.Rows.Add();
                            FgRole.SetUserData(row.Index, "AppRole", role);
                            FgRole[row.Index, "AppRole"] = role.RoleName;
                            //FgRole.FinishEditing();
                            //FgRole.EndInit();
                            try
                            {
                                FgRole.Select(row.Index + 1, 2);
                            }
                            catch {
                                FgRole.Select(row.Index , 2);
                            }
                        }
                    }
                    
                }
            }
        }

        private bool RoleAdded(OperationRole roleForAdd)
        {
            for (int i = 1; i <= FgRole.Rows.Count - 1; i++)
            {
                OperationRole tempRole=FgRole.GetUserData(i, "AppRole") as OperationRole;
                if (tempRole == null) return false;
                if (tempRole.Id == roleForAdd.Id)
                {
                    return true;
                }
            }
            return false;
        }

        void BtnRoleDel_Click(object sender, EventArgs e)
        {
            Lst.Clear();
            foreach (Row item in FgSetp.Rows.Selected)
            {
                AppStepsSet C = item.UserData as AppStepsSet;
                if (C != null)
                {
                    Lst.Add(C);
                }
            }
             
            //if (Model.Delete(Lst) == true)
            //{
            //    foreach (Row item in FgSetp.Rows.Selected)
            //    {
            //        FgSetp.Rows.Remove(item);
            //    }
            //}
          
            if(FgSolution.Row >1)
            {
                Row item = FgSolution.Rows[FgSolution.Row - 1];
                if(item !=null)
                {
                    AppSolutionSet oAppSolutionSet = item.UserData as AppSolutionSet;
                    if (oAppSolutionSet != null)
                    {
                        oAppSolutionSet = Model.DeleteAppStep(Lst, oAppSolutionSet);
                        if (oAppSolutionSet != null)
                        {
                            item.UserData = oAppSolutionSet;
                            ShowAppSetpSet(oAppSolutionSet);
                        }
                    }
                    else
                    {
                        MessageBox.Show("删除失败！");

                    }
                }
            }
            
            
        }

        void BtnRoleSave_Click(object sender, EventArgs e)
        {
            if (!valideValueRole()) return;
            ViewToModelRole();
            Lst = Model.Save(Lst);
            GetSolutionSetShow(curTableSet);
            MessageBox.Show("保存成功！");
        }

        void FgSetp_MouseDown(object sender, MouseEventArgs e)
        {
            if (FgSetp.RowSel < 1)
            {
                return;
            }
            curStepSet = FgSetp.Rows[FgSetp.RowSel].UserData as AppStepsSet;
            if (curStepSet != null)
            {
                ShowAppRoleSet(curStepSet);
            }
            else
            {
                FgRole.Rows.Count = 1;
            }
        }

        void FgSolution_MouseDown(object sender, MouseEventArgs e)
        {
            if (FgSolution.RowSel < 1)
            {
                return;
            }
            curSolutionSet = FgSolution.Rows[FgSolution.RowSel].UserData as AppSolutionSet;
            if (curSolutionSet != null)
            {
                ShowAppSetpSet(curSolutionSet);
            }
            else
            {
                FgRole.Rows.Count = 1;
                FgSetp.Rows.Count = 1;
            }
        }

        void InitData()
        {
            FgSetp.Rows.DefaultSize = 40;
            FgSolution.Rows.DefaultSize = 30;
            Dgv.Rows.DefaultSize = 30;
            FgRole.Rows.DefaultSize = 30;
            Dgv.Cols["Status"].Editor = CboxTable;
            FgSolution.Cols["Status"].Editor = CboxSolution;
            FgSetp.Cols["AppRelations"].Editor = CboxStep;
            Search();
        }

        void Search()
        {
            CurrentProjectInfo ProjectInfo = StaticMethod.GetProjectInfo();
            //审批有效表单
            Dgv.Cols["Status"].Editor = CboxTable;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Status", 0));
            Lst = Model.GetObjects(typeof(AppTableSet), oq);
            foreach (AppTableSet item in Lst)
            {
                Row row = this.Dgv.Rows.Add();
                row["TableName"] = item.TableName;
                row["ClassName"] = item.ClassName;
                row["PhysicsName"] = item.PhysicsName;
                row["DetailClassName"] = item.DetailClassName;
                row["DetailPhysicsName"] = item.DetailPhysicsName;
                row["StatusName"] = item.StatusName;
                row["StatusValue"] = item.StatusValueAgr;
                //row["StatusValueDis"] = item.StatusValueDis;
                row["ExecCode"] = item.ExecCode;
                //row["QueryCode"] = item.QueryCode;
                //row["ReferenceSpace"] = item.ReferenceSpace;
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
            if (Dgv.Rows.Count > 2)
            {
                Dgv.Rows[1].Selected = true;
                curTableSet = Dgv.Rows[1].UserData as AppTableSet;
                GetSolutionSetShow(curTableSet);
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (!valideValue()) return;
            ViewToModel();
            Lst = Model.Save(Lst);
            GetSolutionSetShow(curTableSet);
            MessageBox.Show("保存成功！");
        }
        private bool valideValue()
        {
            for (int i = 1; i < this.FgSolution.Rows.Count - 1; i++)
            {
                Row row = this.FgSolution.Rows[i];
                if (ClientUtil.ToString(this.FgSolution[i, "SolutionName"]) == "")
                {
                    MessageBox.Show("第【" + i + "】行 方案名称 不能为空！");
                    return false;
                }
            }
            if (this.FgSolution.Rows.Count > 3)
            {
                for (int i = 1; i < this.FgSolution.Rows.Count - 1; i++)
                {
                    Row row = this.FgSolution.Rows[i];
                    object conObj = FgSolution[i, "Conditions"];
                    if (conObj == null || conObj.ToString().Trim().Equals(""))
                    {
                        MessageBox.Show("第【" + i + "】行 执行条件 不能为空！");
                        return false;
                    }
                    if (!ValidSolution(conObj.ToString()))
                    {
                        return false;
                    }
                }
            }
            for (int i = 1; i < this.FgSetp.Rows.Count - 1; i++)
            {
                Row row = this.FgSetp.Rows[i];
                if (ClientUtil.ToString(this.FgSetp[i, "StepOrder"]) == "")
                {
                    MessageBox.Show("第【" + i + "】行 审批步骤号称 不能为空！");
                    return false;
                }
                if (ClientUtil.ToString(this.FgSetp[i, "StepName"]) == "")
                {
                    MessageBox.Show("第【" + i + "】行 审批步骤名称 不能为空！");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 校验执行条件是否正确
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private bool ValidSolution(string condition)
        {
            if (curTableSet == null) return false;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", "1"));
            oq.AddCriterion(Expression.Sql(condition));
            MAppPlatform MAppPlatform = new MAppPlatform();
            try
            {
                MAppPlatform.Service.GetDomainByCondition(curTableSet.MasterNameSpace, oq);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
            return true;
        }

        private bool valideValueRole()
        {

            for (int i = 1; i < this.FgSetp.Rows.Count - 1; i++)
            {
                Row row = this.FgSetp.Rows[i];
                if (ClientUtil.ToString(this.FgSetp[i, "StepOrder"]) == "")
                {
                    MessageBox.Show("第【" + i + "】行 审批步骤号称 不能为空！");
                    return false;
                }
                if (ClientUtil.ToString(this.FgSetp[i, "StepName"]) == "")
                {
                    MessageBox.Show("第【" + i + "】行 审批步骤名称 不能为空！");
                    return false;
                }
            }
            for (int i = 1; i < this.FgRole.Rows.Count - 1; i++)
            {
                Row row = this.FgRole.Rows[i];
                if (ClientUtil.ToString(this.FgRole[i, "AppRole"]) == "")
                {
                    MessageBox.Show("第【" + i + "】行 审批角色 不能为空！");
                    return false;
                }
            }
            return true;
        }
        private void ViewToModel()
        {
            Lst.Clear();
            for (int j = 1; j < FgSolution.Rows.Count - 1; j++)
            {
                Row row = this.FgSolution.Rows[j];
                AppSolutionSet Cmc = row.UserData as AppSolutionSet;
                if (Cmc == null)
                {
                    Cmc = new AppSolutionSet();
                }
                Cmc.ParentId = curTableSet;
                Cmc.SolutionName = ClientUtil.ToString(row["SolutionName"]);
                Cmc.Conditions = ClientUtil.ToString(row["Conditions"]);
                Cmc.Description = ClientUtil.ToString(row["Description"]);
                //Cmc.Status = ClientUtil.ToInt(row["Status"]);
                if (ClientUtil.ToString(row["Status"]) == "默认")
                {
                    Cmc.Status = 1;
                }
                else
                {
                    Cmc.Status = 0;
                }
                if (FgSolution.Rows[j].Index == FgSolution.RowSel)
                {
                    Cmc.AppStepsSets.Clear();
                    for (int i = 1; i < FgSetp.Rows.Count; i++)
                    {
                        row = FgSetp.Rows[i];
                        AppStepsSet ASS = row.UserData as AppStepsSet;
                        if (ASS == null)
                        {
                            ASS = new AppStepsSet();
                        }
                        ASS.StepOrder = ClientUtil.ToLong(row["StepOrder"]);
                        ASS.StepsName = ClientUtil.ToString(row["StepName"]);
                        ASS.ParentId = Cmc;
                        if (ClientUtil.ToString(row["AppRelations"]) == "或")
                        {
                            ASS.AppRelations = 0;
                        }
                        else
                        {
                            ASS.AppRelations = 1;
                        }
                        ASS.Remark = ClientUtil.ToString(row["Remark"]);
                        ASS.AppTableSet = curTableSet;
                        Cmc.AppStepsSets.Add(ASS);
                    }
                }
                Lst.Add(Cmc);
            }
        }
        private void ViewToModelRole()
        {
            Lst.Clear();
            for (int j = 1; j < FgSetp.Rows.Count; j++)
            {
                Row row = this.FgSetp.Rows[j];

                AppStepsSet Cmc = row.UserData as AppStepsSet;
                if (Cmc == null)
                {
                    Cmc = new AppStepsSet();
                }
                Cmc.ParentId = curSolutionSet;
                Cmc.StepOrder = ClientUtil.ToInt(row["StepOrder"]);
                Cmc.StepsName = ClientUtil.ToString(row["StepName"]);
                if (ClientUtil.ToString(row["AppRelations"]) == "或")
                {
                    Cmc.AppRelations = 0;
                }
                else
                {
                    Cmc.AppRelations = 1;
                }
                Cmc.Remark = ClientUtil.ToString(row["Remark"]);
                Cmc.AppTableSet = curTableSet;
                if (FgSetp.Rows[j].Index == FgSetp.RowSel)
                {
                    Cmc.AppRoleSets.Clear();
                    for (int i = 1; i < FgRole.Rows.Count - 1; i++)
                    {
                        AppRoleSet ASS = null;
                        Row RowRole = FgRole.Rows[i];

                        ASS = RowRole.UserData as AppRoleSet;

                        if (ASS == null)
                        {
                            ASS = new AppRoleSet();
                        }
                        ASS.AppRole = FgRole.GetUserData(i, "AppRole") as OperationRole;
                        ASS.RoleName = ASS.AppRole.RoleName;
                        ASS.Remark = ClientUtil.ToString(RowRole["Remark"]);
                        ASS.ParentId = Cmc;
                        Cmc.AppRoleSets.Add(ASS);
                    }

                }
                Lst.Add(Cmc);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Lst.Clear();
            foreach (Row item in FgSolution.Rows.Selected)
            {
                AppSolutionSet C = item.UserData as AppSolutionSet;
                if (C != null)
                {
                    Lst.Add(C);
                }
            }
            if (Model.Delete(Lst) == true)
            {
                foreach (Row item in FgSolution.Rows.Selected)
                {
                    FgSolution.Rows.Remove(item);
                }
                GetSolutionSetShow(curTableSet);
            }
            else
            {
                MessageBox.Show("删除失败！");

            }
        }

        private void Dgv_MouseDown(object sender, MouseEventArgs e)
        {
            if (Dgv.RowSel < 1)
            {
                return;
            }
            curTableSet = Dgv.Rows[Dgv.RowSel].UserData as AppTableSet;
            if (curTableSet != null)
            {
                GetSolutionSetShow(curTableSet);
            }
        }
        void GetSolutionSetShow(AppTableSet _AppTableSet)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ParentId", _AppTableSet));
            oq.AddFetchMode("AppStepsSets", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AppStepsSets.AppRoleSets", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AppStepsSets.AppRoleSets.AppRole", NHibernate.FetchMode.Eager);


            IList ls = Model.GetObjects(typeof(AppSolutionSet), oq);
            FgSolution.Rows.Count = 1;
            foreach (AppSolutionSet item in ls)
            {
                Row ro = FgSolution.Rows.Add();
                ro["SolutionName"] = item.SolutionName;
                ro["Conditions"] = item.Conditions;
                ro["Description"] = item.Description;
                if (item.Status == 1)
                {
                    ro["Status"] = "默认";
                }
                else
                {
                    ro["Status"] = "无";
                }

                ro.UserData = item;
            }
            if (FgSolution.Rows.Count > 2)
            {
                //FgSolution.Rows[1].Selected = true;
                curSolutionSet = FgSolution.Rows[1].UserData as AppSolutionSet;
                ShowAppSetpSet(curSolutionSet);
            }
            else
            {
                FgSolution.Rows.Count = 1;
                FgSetp.Rows.Count = 1;
                FgRole.Rows.Count = 1;
            }
        }
        void ShowAppSetpSet(AppSolutionSet _AppSolutionSet)
        {
            FgSetp.Rows.Count = 1;
            foreach (AppStepsSet item in _AppSolutionSet.AppStepsSets)
            {
                Row rs = FgSetp.Rows.Add();
                rs["StepOrder"] = ClientUtil.ToLong(item.StepOrder);
                rs["StepName"] = item.StepsName;
                if (item.AppRelations == 0)
                {
                    rs["AppRelations"] = "或";
                }
                else
                {
                    rs["AppRelations"] = "与";
                }
                rs["Remark"] = item.Remark;
                rs.UserData = item;
            }
            if (FgSetp.Rows.Count > 1)
            {
                FgSetp.Rows[1].Selected = true;
                curStepSet = FgSetp.Rows[1].UserData as AppStepsSet;
                ShowAppRoleSet(curStepSet);
            }
            else
            {
                FgSetp.Rows.Count = 1;
                FgRole.Rows.Count = 1;
            }
        }
        void ShowAppRoleSet(AppStepsSet _AppStepsSets)
        {
            FgRole.Rows.Count = 1;
            foreach (AppRoleSet item in _AppStepsSets.AppRoleSets)
            {
                Row rr = FgRole.Rows.Add();
                if (item.AppRole != null)
                {
                    FgRole.SetUserData(rr.Index, "AppRole", item.AppRole);
                    rr["AppRole"] = item.RoleName;
                }
                rr["Remark"] = item.Remark;
                rr.UserData = item;
            }
        }
    }
}