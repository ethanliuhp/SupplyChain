using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
using Application.Business.Erp.SupplyChain.Client.BasicData;
using Application.Business.Erp.SupplyChain.Client.Indicator.BasicData;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    public partial class VDimensionRegister : Form
    {
        private MIndicatorUse model = new MIndicatorUse();
        private MBasicData basicModel = new MBasicData();
        
        public VDimensionRegister()
        {
            InitializeComponent();
        }

        private void VDimensionRegister_Load(object sender, EventArgs e)
        {
            InitialComboBox();
            LoadDimensionRegister();
        }

        private void InitialComboBox()
        {
            //是否度量
            colIfMeasure.Items.Add(0);
            colIfMeasure.Items.Add(1);
            //来源类型
            try
            {
                IList originalTypeList = basicModel.BasicDataSrv.GetDetailByBasicTableName("维度注册表-来源类型");
                colOriginType.DataSource = originalTypeList;
                colOriginType.DisplayMember = "Name";
                colOriginType.ValueMember = "Code";
            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("查找基础数据｛来源类型｝出错。",ex);
            }
        }

        private void LoadDimensionRegister()
        {
            try
            {
                IList dimRegList = model.DimDefSrv.GetAllDimensionRegister();
                foreach (DimensionRegister dimReg in dimRegList)
                {
                    int rowIndex = dgvDimReg.Rows.Add();
                    DataGridViewRow row = dgvDimReg.Rows[rowIndex];
                    row.Tag = dimReg;
                    row.Cells[colDimCode.Name].Value=dimReg.Code;
                    row.Cells[colDimName.Name].Value = dimReg.Name;
                    row.Cells[colDimRight.Name].Value = dimReg.DimRights;
                    row.Cells[colRemark.Name].Value = dimReg.Remark;

                    //是否度量
                    if (dimReg.IfMeasure == 1)
                    {
                        row.Cells[colIfMeasure.Name].Value = 1;
                    }
                    else
                    {
                        row.Cells[colIfMeasure.Name].Value = 0;
                    }
                    //来源类型
                    //row.Cells[colOriginType].Value = dimReg.OriginTypeCode;
                }
            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("查找注册维度出错。",ex);
            }
        }

        private void btnDimRegAdd_Click(object sender, EventArgs e)
        {
            int rowIndex = dgvDimReg.Rows.Add();
            DataGridViewRow row = dgvDimReg.Rows[rowIndex];
            row.ReadOnly = false;
            row.Cells[colDimCode.Name].Value = "";
            row.Cells[colDimName.Name].Value = "";
            row.Cells[colDimRight.Name].Value = "";
            row.Cells[colRemark.Name].Value = "";
            row.Cells[colIfMeasure.Name].Value = 0;
            row.Cells[colOriginType.Name].Value = "1";

            //row.Cells[].fo
        }

        private void btnDimRegModify_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvDimReg.SelectedRows)
            {
                EnableEdit(row, false);
            }
        }

        private void EnableEdit(DataGridViewRow row, bool readOnly)
        {
            row.Cells[colDimCode.Name].ReadOnly = readOnly;
            row.Cells[colDimName.Name].ReadOnly=readOnly;
            row.Cells[colOriginType.Name].ReadOnly=readOnly;
            row.Cells[colIfMeasure.Name].ReadOnly=readOnly;
            row.Cells[colRemark.Name].ReadOnly=readOnly;
        }

        /// <summary>
        /// 删除维度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDimRegDel_Click(object sender, EventArgs e)
        {
            if (KnowledgeMessageBox.QuestionMessage("确定要删除当前选择的维度吗？") == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dgvDimReg.SelectedRows)
                {
                    bool canDelete=false;
                    DimensionRegister dimReg = row.Tag as DimensionRegister;
                    if (dimReg != null && !string.IsNullOrEmpty(dimReg.Id))
                    {
                        try
                        {
                            model.DimDefSrv.Delete(dimReg);
                            canDelete = true;
                        }
                        catch (Exception ex)
                        {
                            KnowledgeMessageBox.InforMessage("删除维度出错。",ex);
                            canDelete = false;
                        }
                        if (canDelete)
                        {
                            dgvDimReg.Rows.Remove(row);
                        }
                    }
                    else
                    {
                        dgvDimReg.Rows.Remove(row);
                    }
                }
            }
        }

        /// <summary>
        /// 保存维度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDimRegSave_Click(object sender, EventArgs e)
        {
            IList dimRegList = new ArrayList();
            foreach (DataGridViewRow row in dgvDimReg.Rows)
            {
                DimensionRegister dimReg = row.Tag as DimensionRegister;
                if (dimReg == null) dimReg = new DimensionRegister();

                //if (dimReg.Id >= 100)
                //{
                //    KnowledgeMessageBox.InforMessage("维度编号不能超过100，请与系统管理员联系。");
                //    return;
                //}

                //维度编号
                object objCode = row.Cells[colDimCode.Name].Value;
                if (objCode == null || objCode.ToString().Equals(""))
                {
                    KnowledgeMessageBox.InforMessage("请输入维度编号。");
                    return;
                }
                dimReg.Code = objCode.ToString();

                //维度名称
                object objName = row.Cells[colDimName.Name].Value;
                if (objName == null || objName.ToString().Equals(""))
                {
                    KnowledgeMessageBox.InforMessage("请输入维度名称。");
                    return;
                }
                dimReg.Name = objName.ToString();

                //维度权限
                object objRight= row.Cells[colDimRight.Name].Value;
                if (objRight == null)
                {
                    dimReg.DimRights = "";
                }
                else
                {
                    dimReg.DimRights = objRight.ToString();
                }

                //备注
                object objRemark = row.Cells[colRemark.Name].Value;
                if (objRemark == null)
                {
                    dimReg.Remark = "";
                }
                else
                {
                    dimReg.Remark = objRemark.ToString();
                }

                //是否度量
                object objIfMeasure = row.Cells[colIfMeasure.Name].Value;
                if (objIfMeasure == null)
                {
                    dimReg.IfMeasure = 0;
                }
                else
                {
                    dimReg.IfMeasure = int.Parse(objIfMeasure.ToString());
                }

                //来源类型
                //object objOriginalType = row.Cells[colOriginType.Name].Value;
                //if (objOriginalType != null)
                //{
                //    dimReg.OriginTypeCode = objOriginalType.ToString();
                //    dimReg.OriginTypeName=row.Cells[colOriginType.Name].
                //}
            }
        }
    }
}