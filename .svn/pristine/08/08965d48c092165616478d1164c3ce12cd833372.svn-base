using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.BasicData.Domain;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.BasicData
{
    public partial class VBasicData : TBasicDataView
    {
        private MBasicData model = new MBasicData();

        private int dgvBasicDataTempRowCount = 15;
        
        public VBasicData()
        {
            InitializeComponent();
        }

        internal void Start()
        {
            ListAllBasicDatas();
            dgvBasicDataAddBlankRow();

            DataGridViewRow row = dgvBasicData.Rows[0];
            row.Selected=true;
        }

        private void ListAllBasicDatas()
        {
            try
            {
                IList list = model.BasicDataSrv.ListAllBasicDatas();
                foreach (BasicDatas obj in list)
                {
                    int i = dgvBasicData.Rows.Add();
                    DataGridViewRow row = dgvBasicData.Rows[i];
                    row.Tag = obj;
                    row.Cells["colId"].Value = obj.Id;
                    row.Cells["colName"].Value = obj.Name;
                }
            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("查询基础数据出错。",ex);
            }
        }

        /// <summary>
        /// 主表添加空行
        /// </summary>
        private void dgvBasicDataAddBlankRow()
        {
            if (dgvBasicData.Rows.Count < dgvBasicDataTempRowCount)
            {
                dgvBasicData.Rows.Add(dgvBasicDataTempRowCount - dgvBasicData.Rows.Count);
            }
        }

        private void btnNewBasicData_Click(object sender, EventArgs e)
        {
            txtBasicTableName.Text = "";
            txtBasicTableName.Tag = null;
            dgvBasicDataDetail.Rows.Clear();

            if (dgvBasicData.Rows.Count > dgvBasicDataTempRowCount)
            {
                int i = dgvBasicData.Rows.Add();
                dgvBasicData.Rows[i].Selected = true;
            }
            else
            {
                foreach (DataGridViewRow row in dgvBasicData.Rows)
                {
                    if (row.Tag == null)
                    {
                        row.Selected = true;
                        break;
                    }
                }
            }

            txtBasicTableName.Focus();
        }

        private void btnSaveBasicData_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            string basicTableName = txtBasicTableName.Text;
            if (basicTableName == null || basicTableName.Trim().Equals(""))
            {
                KnowledgeMessageBox.InforMessage("基础表名不能为空。");
                return;
            }

            BasicDatas bd = new BasicDatas();
            if (txtBasicTableName.Tag != null)
            {
                bd = txtBasicTableName.Tag as BasicDatas;
            }

            bd.Name = basicTableName;
            try
            {
                IList deletedDetail = UpdateBasicDataDetail(bd);
                //model.BasicDataSrv.SaveBasicDatas(bd, deletedDetail);
                BasicDatas temp = model.BasicDataSrv.SaveBasicDatas(bd);
                txtBasicTableName.Tag = temp;
                FillBasicDataRow(temp);
                dgvBasicDataDetail.Rows.Clear();
                ListBasicDataDetail(temp);
                KnowledgeMessageBox.InforMessage("保存成功。");
            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("保存基础数据出错。", ex);
            }        
        }

        private IList UpdateBasicDataDetail(BasicDatas bd)
        {
            IList updateDetail = new ArrayList();
            IList deletedDetail = new ArrayList();
            
            foreach (DataGridViewRow row in dgvBasicDataDetail.Rows)
            {
                object colDetailName = row.Cells["colDetailName"].Value;
                object colDetailCode = row.Cells["colDetailCode"].Value;
                string code = "";
                if (colDetailCode != System.DBNull.Value && colDetailCode != null)
                {
                    code = colDetailCode.ToString();
                }

                BasicDataDetail detail = new BasicDataDetail();
                if (colDetailName != System.DBNull.Value && colDetailName != null && !colDetailName.ToString().Trim().Equals(""))
                {
                    if (row.Tag != null)
                    {
                        detail = row.Tag as BasicDataDetail;
                        detail.Name = colDetailName.ToString();
                        detail.Code = code;
                        
                        detail.BasicData = bd;
                        updateDetail.Add(detail);
                    }
                    else
                    {
                        detail.Name = colDetailName.ToString();
                        detail.Code = code;
                        detail.BasicData = bd;
                        updateDetail.Add(detail);
                    }
                }
                else
                {
                    if (row.Tag != null)
                    {
                        deletedDetail.Add(((BasicDataDetail)row.Tag));
                        row.Tag = null;
                    }
                }
            }
            bd.BasicDataDetails = updateDetail;
            return deletedDetail;
        }

        private void FillBasicDataRow(BasicDatas bd)
        {
            if (bd == null) return;
            foreach (DataGridViewRow row in dgvBasicData.SelectedRows)
            {
                row.Tag = bd;
                row.Cells["colId"].Value = bd.Id;
                row.Cells["colName"].Value = bd.Name;
                break;
            }
        }

        private void btnDeleteBasicData_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == KnowledgeMessageBox.QuestionMessage("确定要删除当前基础表吗？"))
            {
                foreach (DataGridViewRow row in dgvBasicData.SelectedRows)
                {
                    BasicDatas bd = row.Tag as BasicDatas;
                    if (bd != null)
                    {
                        try
                        {
                            model.BasicDataSrv.DeleteBasicDatas(bd);
                            dgvBasicData.Rows.Remove(row);
                        }
                        catch (Exception ex)
                        {
                            KnowledgeMessageBox.InforMessage("删除当前基础表出错。",ex);
                        }
                    }
                    break;
                }
            }
        }

        private void btnAddBasicDataDetail_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dgvBasicDataDetail.Rows[dgvBasicDataDetail.Rows.Count - 1];
            //dgvBasicDataDetail.CurrentCell= row.Cells["colDetailName"];
            dgvBasicDataDetail.CurrentCell = row.Cells["colDetailCode"];
            dgvBasicDataDetail.BeginEdit(false);
        }

        private void btnSaveBasicDataDetail_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btnDeleteBasicDataDetail_Click(object sender, EventArgs e)
        {
            if(DialogResult.Yes==KnowledgeMessageBox.QuestionMessage("确定要删除当前选择的记录吗？"))
            {
                foreach (DataGridViewRow row in dgvBasicDataDetail.SelectedRows)
                {
                    try
                    {
                        BasicDataDetail detail = row.Tag as BasicDataDetail;
                        if (detail != null)
                        {
                            BasicDatas temp = txtBasicTableName.Tag as BasicDatas;
                            if (temp != null)
                            {
                                IList details=model.BasicDataSrv.GetDetailByMaster(temp);
                                if (details != null && details.Count>0)
                                {
                                    details.Remove(detail);
                                    temp.BasicDataDetails = details;
                                    model.BasicDataSrv.DeleteBasicDataDetail(detail);
                                    //BasicDatas bd=model.BasicDataSrv.SaveBasicDatas(temp);
                                    txtBasicTableName.Tag = temp;
                                    dgvBasicData.SelectedRows[0].Tag = temp;
                                    dgvBasicDataDetail.Rows.Remove(row);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        KnowledgeMessageBox.InforMessage("删除明细出错。",ex); ;
                    }
                }
            }
        }

        private void ListBasicDataDetail(BasicDatas bd)
        {
            try
            {
                IList list = model.BasicDataSrv.GetDetailByMaster(bd);
                foreach (BasicDataDetail detail in list)
                {
                    int r = dgvBasicDataDetail.Rows.Add();
                    DataGridViewRow row = dgvBasicDataDetail.Rows[r];
                    row.Tag = detail;
                    row.Cells["colDetailId"].Value = detail.Id;
                    row.Cells["colDetailCode"].Value = detail.Code;
                    row.Cells["colDetailName"].Value = detail.Name;
                }
            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("查询明细数据出错。",ex);
            }
        }

        private void dgvBasicData_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow row = dgvBasicData.SelectedRows[0];
            if (row != null)
            {
                BasicDatas bd = row.Tag as BasicDatas;
                if (bd != null)
                {
                    txtBasicTableName.Tag = bd;
                    txtBasicTableName.Text = bd.Name;
                    dgvBasicDataDetail.Rows.Clear();
                    ListBasicDataDetail(bd);
                }
                else
                {
                    txtBasicTableName.Tag = null;
                    txtBasicTableName.Text = "";
                    dgvBasicDataDetail.Rows.Clear();
                }
            }
        }
    }
}