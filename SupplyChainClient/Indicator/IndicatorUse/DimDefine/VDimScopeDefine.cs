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
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinControls;
using Application.Business.Erp.SupplyChain.Client.BasicData;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.Client.Indicator.BasicData;
using Application.Business.Erp.SupplyChain.BasicData.Domain;


namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimDefine
{
    public partial class VDimScopeDefine : Form
    {
        private MIndicatorUse model = new MIndicatorUse();
        private MBasicData basicDataModel = new MBasicData();
        private DimensionCategory category = new DimensionCategory();
        private IList scopeTypeList = new ArrayList();

        public VDimScopeDefine(DimensionCategory category)
        {
            InitializeComponent();
            new DataGridInputValidator(dgvScopeDef, InputType.Float).Validate("beginValue,endValue,score").AlertMode=InputAlertMode.AlertBoxCancel|InputAlertMode.AlertColor; 
            this.category = category;
            InitControls();            
        }

        private void InitControls() 
        {
            //指定下拉框的初始值
            scopeTypeList = model.InitialIndicatorList(KnowledgeUtil.ScopeTypeCode, KnowledgeUtil.ScopeTypeName, true);
            scopeType.DataSource = scopeTypeList;
            scopeType.DisplayMember = "Name";
            scopeType.ValueMember = "Code";

            //查询已经定义的指标区间
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Category.Id", category.Id));
            IList list = model.DimDefSrv.GetDimensionScopeByQuery(oq);
            foreach (DimensionScope ds in list)
            {
                int i = dgvScopeDef.Rows.Add();
                DataGridViewRow r = dgvScopeDef.Rows[i];
                r.Tag = ds;
                r.Cells["scopeType"].Value = ds.ScopeType+"";
                r.Cells["beginValue"].Value = ds.BeginValue;
                r.Cells["endValue"].Value = ds.EndValue;
                r.Cells["score"].Value = ds.Score;
            }
        }

        public bool Open(IWin32Window owner, ref DimensionCategory category)
        {
            this.ShowDialog(owner);
            category.DimensionScope = this.scopeTypeList;
            return true;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (validInput())
            {
                foreach (DataGridViewRow row in dgvScopeDef.Rows)
                {
                    DimensionScope ds = row.Tag as DimensionScope;
                    if (ds == null)
                    {
                        ds = new DimensionScope();
                        ds.Category = category;
                    }
                    object scopeType = row.Cells["scopeType"].Value;
                    if (scopeType != null && !"".Equals(scopeType))
                    {
                        ds.ScopeType = int.Parse(scopeType.ToString());
                        if (scopeTypeList != null)
                        {
                            //区间类型名称
                            foreach (IndicatorBasicValue ibv in scopeTypeList)
                            {
                                if (ibv.Code != null && ibv.Code.Equals(scopeType))
                                {
                                    ds.ScopeName = ibv.Name;
                                    break;
                                }
                            }
                        }
                    }

                    object beginValue = row.Cells["beginValue"].Value;
                    if (beginValue != null && !"".Equals(beginValue))
                    {
                        ds.BeginValue = double.Parse(beginValue.ToString());
                    }
                    object endValue = row.Cells["endValue"].Value;
                    if (endValue != null && !"".Equals(endValue))
                    {
                        ds.EndValue = double.Parse(endValue.ToString());
                    }
                    object score = row.Cells["score"].Value;
                    if (score != null && !"".Equals(score))
                    {
                        ds.Score = double.Parse(score.ToString());
                    }

                    if (row.IsNewRow == false)
                    {
                        model.DimDefSrv.SaveDimensionScope(ds);
                    }
                }
                MessageBox.Show("保存区间定义成功！");
            }
            
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            
            if (dgvScopeDef.SelectedRows.Count > 0)
            {
                if (DialogResult.Yes == KnowledgeMessageBox.QuestionMessage("确定要删除当前区间定义吗？"))
                {
                    foreach (DataGridViewRow row in dgvScopeDef.SelectedRows)
                    {
                        if (row.IsNewRow == false)
                        {
                            DimensionScope ds = row.Tag as DimensionScope;
                            dgvScopeDef.Rows.Remove(row);
                            if (ds != null && !string.IsNullOrEmpty(ds.Id))
                            {
                                model.DimDefSrv.DeleteDimensionScope(ds);
                            }
                        }                                         
                    }
                }
                
            }
            else
            {
                MessageBox.Show("请选择要删除的区间！");
            }
        }

        private bool validInput() 
        {
            int k = 0;
            foreach (DataGridViewRow row in dgvScopeDef.Rows)
            {
                k++;
                if (row.IsNewRow == false)
                {
                    object scopeType = row.Cells["scopeType"].Value;
                    if (scopeType == null || "".Equals(scopeType.ToString()))
                    {
                        KnowledgeMessageBox.InforMessage("第"+k+"行的区间类型未选！");
                        return false;
                    }
                    object beginValue = row.Cells["beginValue"].Value;
                    object endValue = row.Cells["endValue"].Value;
                    if ( (beginValue == null || "".Equals(beginValue.ToString()) ) && ( (endValue == null || "".Equals(endValue.ToString())) ) )
                    {
                        KnowledgeMessageBox.InforMessage("第" + k + "行的开始值和结束值不能同时为空！");
                        return false;
                    }
                    
                    object score = row.Cells["score"].Value;
                    if (score == null || "".Equals(score.ToString()))
                    {
                        KnowledgeMessageBox.InforMessage("第"+k+"行的分值不能为空！");
                        return false;
                    }
                }
            }
            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}