using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using NHibernate.Criterion;
using NHibernate;

using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Main;

using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.HumanResource;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    public partial class VDSSViewFreeDistribute : TBasicDataView
    {
        private MCubeManager mCube = new MCubeManager();
        private string selectedJob;
        private string front_kjn;
        private string front_kjy;

        public VDSSViewFreeDistribute()
        {
            InitializeComponent();
        }

        internal void Start()
        {
            GetFrontKjq();
            LoadCubeList();
            ClearView();
            InitialControls();
        }

        private void GetFrontKjq()
        {
            if (ConstObject.LoginDate.Month == 1)
            {
                front_kjn = (ConstObject.LoginDate.Year - 1) + "";
                front_kjy = "12";
            }
            else
            {
                front_kjn = ConstObject.LoginDate.Year + "";
                front_kjy = (ConstObject.LoginDate.Month - 1) + "";
                if (int.Parse(front_kjy) < 10)
                {
                    front_kjy = "0" + front_kjy;
                }
            }

        }

        /// <summary>
        /// 设置控件的一些属性
        /// </summary>
        private void InitialControls()
        {
            btnGwSelect.Enabled = false;
            btnDistributeOk.Enabled = false;
            btnGwDel.Enabled = false;
            btnAddDistribute.Enabled = false;
        }

        /// <summary>
        /// 激活控件的一些属性
        /// </summary>
        private void ActiveControls()
        {
            btnGwSelect.Enabled = true;
            btnDistributeOk.Enabled = true;
            btnGwDel.Enabled = true;
        }

        private void LoadCubeList()
        {
            SystemRegister sr = new SystemRegister();
            sr.Id = ConstObject.TheSystemCode.ToString();
            IList list = mCube.CubeManagerSrv.GetPartSystemCubeRegister(sr);
            cboCubeSelect.DisplayMember = "CubeName";
            if (list.Count > 0)
            {
                foreach (CubeRegister res in list)
                {
                    cboCubeSelect.Items.Add(res);
                }
            }
        }

        private void ClearView() 
        {
            dgvGwSelect.Rows.Clear();
        }

        private void cboCubeSelect_SelectedValueChanged(object sender, EventArgs e)
        {
            lstViewSel.Items.Clear();
            CubeRegister reg = cboCubeSelect.SelectedItem as CubeRegister;
            IList list = mCube.ViewService.GetViewMainByCubeIdAndType(reg, "5");
            lstViewSel.DisplayMember = "ViewName";
            foreach (ViewMain vm in list)
            {
                lstViewSel.Items.Add(vm);
            }
        }

        private void btnGwSelect_Click(object sender, EventArgs e)
        {
            IList list = UCL.Locate("人员上岗维护", CPersonOnJob.PersonExecuteType.CommonSelect, selectedJob) as IList;
            if (list.Count > 0)
            {
                int oldrows = dgvGwSelect.RowCount;
                foreach (OperationJob job in list)
                {
                    if (selectedJob.Contains(";" + job.Id + ";") == false)
                    {  
                        ViewDistribute vd = new ViewDistribute();
                        dgvGwSelect.Rows.Add();
                        DataGridViewRow r = dgvGwSelect.Rows[oldrows];
                        vd.TheJob = job;
                        r.Tag = vd;
                        r.Cells["opeOrg"].Value = job.OperationOrg.Name;
                        r.Cells["job"].Value = job.Name;
                        selectedJob += job.Id + ";";
                        oldrows++;
                    }
                                       
                }
            }
        }

        private void btnGwDel_Click(object sender, EventArgs e)
        {
            if (dgvGwSelect.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvGwSelect.SelectedRows)
                {
                    ViewDistribute vd = row.Tag as ViewDistribute;
                    selectedJob = selectedJob.Replace(";" + vd.TheJob.Id + ";", ";");
                    dgvGwSelect.Rows.Remove(row);
                    if (!string.IsNullOrEmpty(vd.Id))
                    {
                        mCube.ViewService.DeleteViewDistribute(vd);
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择要删除的岗位");
            }
        }

        private void btnDistributeOk_Click(object sender, EventArgs e)
        {
            if (lstViewSel.SelectedItem != null)
            {
                if (dgvGwSelect.Rows.Count > 0)
                {
                    ViewMain vMain = lstViewSel.SelectedItem as ViewMain;
                    IList list = new ArrayList();
                    try
                    {
                        foreach (DataGridViewRow row in dgvGwSelect.Rows)
                        {
                            ViewDistribute vDistribute = row.Tag as ViewDistribute;
                            if (!string.IsNullOrEmpty(vDistribute.Id))
                            {
                                vDistribute.Author = ConstObject.LoginPersonInfo;
                                vDistribute.DistributeDate = ConstObject.LoginDate;
                                vDistribute.StateName = front_kjn + front_kjy;
                                vDistribute.ViewName = vMain.ViewName;
                            }
                            else
                            {
                                vDistribute.TheOpeOrg = vDistribute.TheJob.OperationOrg;
                                vDistribute.Author = ConstObject.LoginPersonInfo;
                                vDistribute.DistributeDate = ConstObject.LoginDate;

                                vDistribute.StateName = front_kjn + front_kjy;
                                vDistribute.ViewName = vMain.ViewName;
                                vDistribute.Main = vMain; 
                            }
                            list.Add(vDistribute);
                        }
                        mCube.ViewService.SaveViewDistribute(list);

                        MessageBox.Show("分发模板成功！");
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show("分发模板出错：" + StaticMethod.ExceptionMessage(ex));
                      }
                }
                else
                {
                    MessageBox.Show("请选择要分发的岗位！");
                }
            }
            else
            {
                MessageBox.Show("请选择一个模板！");
            }
        }

        private void lstViewSel_MouseClick(object sender, MouseEventArgs e)
        {           
            ViewMain vm = lstViewSel.SelectedItem as ViewMain;
            if (vm != null)
            {
                //初始化
                ActiveControls();
                ClearView();

                edtCreateDate.Text = vm.CreatedDate.ToShortDateString();
                edtViewType.Text = vm.ViewTypeName;

                //取得本标准分发的最近的一次分发流水号
                string sql = "select max(distributeSerial) from knviewdistribute where viewid = " + vm.Id;
                string maxcode = mCube.CubeManagerSrv.GetResultBySql(sql);

                //查询已经分发的岗位
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Main.Id", vm.Id));
                oq.AddCriterion(Expression.Eq("DistributeSerial", maxcode));
                oq.AddFetchMode("TheOpeOrg", FetchMode.Eager);
                oq.AddFetchMode("TheJob", FetchMode.Eager);
                IList list = mCube.ViewService.GetViewDistributeByQuery(oq);
                selectedJob = ";";
                foreach (ViewDistribute vd in list)
                {
                    int i = dgvGwSelect.Rows.Add();
                    DataGridViewRow r = dgvGwSelect.Rows[i];
                    r.Tag = vd;
                    r.Cells["opeOrg"].Value = vd.TheOpeOrg.Name;
                    r.Cells["job"].Value = vd.TheJob.Name;
                    r.Cells["distributedate"].Value = vd.DistributeDate.ToShortDateString();
                    selectedJob += vd.TheJob.Id + ";";
                }
            }                          
        }

        private void btnAddDistribute_Click(object sender, EventArgs e)
        {
            dgvGwSelect.Rows.Clear();
            selectedJob = ";";
        }
    }
}