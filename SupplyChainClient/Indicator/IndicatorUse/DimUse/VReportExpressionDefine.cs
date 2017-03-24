using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
using System.Text.RegularExpressions;
using Application.Business.Erp.SupplyChain.Util;


namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    public partial class VReportExpressionDefine : Form
    {
        private MCubeManager mCubeManager = new MCubeManager();
        private MIndicatorUse model = new MIndicatorUse();
        public bool isOkClicked = false;
        public ViewMain vMain;
        public string calExp_id = "";
        public string calExp_name = "";
        public Hashtable ht_dims = new Hashtable();

        public VReportExpressionDefine()
        {
            InitializeComponent();                                   
        }

        private void VReportExpressionDefine_Load(object sender, EventArgs e)
        {
            LoadDimensionList();
            //把事实添加进ht_dims
            IList factList = mCubeManager.GetFactDefineByCubeRegisterId(vMain.CubeRegId.Id);
            foreach (FactDefine obj in factList)
            {
                if (!ht_dims.Contains(obj.Id))
                {
                    ht_dims.Add(obj.Id, obj.FactName);
                }
            }
            InitCboFact(); 
            txtCalExp.Tag = calExp_id;
            txtCalExp.Text = model.TransIdToName(calExp_id, ht_dims);
        }

        private void InitCboFact()
        {
            IList factList = mCubeManager.GetFactDefineByCubeRegisterId(vMain.CubeRegId.Id);
            cboFact.DataSource = factList;
            cboFact.DisplayMember = "FactName";
            cboFact.ValueMember = "Id";
        }

        #region 事件
        private void btnClearExp_Click(object sender, EventArgs e)
        {
            txtCalExp.Text = "";
            txtCalExp.Tag = "";
        }

        private void btnValidateExp_Click(object sender, EventArgs e)
        {
            if (ValidateExp())
            {
                KnowledgeMessageBox.InforMessage("校验通过。");
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateExp())
            {
                isOkClicked = true;
                calExp_name = txtCalExp.Text;
                //calExp += VDimensionDefine.CalExpSplit.ToString();
                calExp_id = txtCalExp.Tag.ToString();
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCalExpAddDim_Click(object sender, EventArgs e)
        {
            string dim_id_link = "";
            string dim_name_link = "";
            string curr_express = txtCalExp.Text;

            foreach (DataGridViewRow row in dgvDim.Rows)
            {
                string curr_id = row.Cells["selecteddim"].Tag as string;
                string curr_name = KnowledgeUtil.TransToString(row.Cells["selecteddim"].Value);
                if (curr_id == null || "".Equals(curr_id))
                {
                    string dim_name = KnowledgeUtil.TransToString(row.Cells["dim"].Value);
                    KnowledgeMessageBox.InforMessage(dim_name + "未选择！");
                    return;
                }
                dim_id_link +=MCubeManager.SaveExpressDelimiter[0] + curr_id;
                dim_name_link += MCubeManager.SaveExpressDelimiter[0] + curr_name;
            }
            if (cboFact.SelectedItem == null)
            {
                MessageBox.Show("事实未选择。");
                return;
            }
            dim_id_link += MCubeManager.SaveExpressDelimiter[0] + cboFact.SelectedValue.ToString();
            dim_name_link += MCubeManager.SaveExpressDelimiter[0] + (cboFact.SelectedItem as FactDefine).FactName;
            txtCalExp.AppendText("[" + dim_name_link + "]");
            UpdateTxtTag("[" + dim_id_link + "]");
        }


        private void dgvDim_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            int colDimIndex = dgvDim.Columns.IndexOf(dim);
            int currentColIndex = e.ColumnIndex;
            if (colDimIndex == currentColIndex)
            {
                DimensionCategory cat = dgvDim.Rows[e.RowIndex].Tag as DimensionCategory;
                if (cat.Name.IndexOf(KnowledgeUtil.TIME_DIM_STR) == -1)
                {
                    VReportDimension vrd = new VReportDimension();
                    
                    vrd.main_cat = cat;
                    vrd.ShowDialog();
                    if (vrd.isOkClicked)
                    {
                        string dimId = vrd.selectDimId + "";
                        dgvDim.Rows[e.RowIndex].Cells["selecteddim"].Tag = dimId;
                        dgvDim.Rows[e.RowIndex].Cells["selecteddim"].Value = ht_dims[dimId].ToString();
                    }
                }
                else {
                    VTimeSelect vts = new VTimeSelect();
                    vts.ShowDialog();
                    if (vts.isOkClicked)
                    {
                        dgvDim.Rows[e.RowIndex].Cells["selecteddim"].Tag = vts.timeName;
                        dgvDim.Rows[e.RowIndex].Cells["selecteddim"].Value = vts.timeName;
                    }
                }
            }
        }
        #endregion

        #region 数字、运算符事件处理

        private void AddButtonText(String text)
        {
            txtCalExp.AppendText(text);
            UpdateTxtTag(text);
        }

        private void btnSeven_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnEight_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnNine_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnFour_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnFive_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnSix_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnOne_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnTwo_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnThree_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnZero_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnMod_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnDiv_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnMulti_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnLeftBracket_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }

        private void btnRightBracket_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text);
        }
        #endregion

        #region 私有函数

        private void UpdateTxtTag(string text)
        {
            string calExp = txtCalExp.Tag as string;
            if (calExp == null) calExp = "";
            calExp += text;
            txtCalExp.Tag = calExp;
        }

        /// <summary>
        /// 校验表达式
        /// </summary>
        /// <param name="errorStr"></param>
        /// <returns></returns>
        private bool ValidateExp()
        {
            string calExp = txtCalExp.Tag as string;
            if (calExp == null) return true;

            string patten = "}{";
            Regex reg = new Regex(patten);
            if (reg.Matches(calExp).Count > 0)
            {
                KnowledgeMessageBox.InforMessage("校验不通过，缺少运算符。");
                return false;
            }
            patten = @"}\d+|\d+{";
            reg = new Regex(patten);
            if (reg.Matches(calExp).Count > 0)
            {
                KnowledgeMessageBox.InforMessage("校验不通过，缺少运算符。");
                return false;
            }

            //把括号去掉，转化成四则表达式
            calExp = calExp.Replace("{", "");
            calExp = calExp.Replace("}", "");
            int firstIndex = calExp.IndexOf("[");
            int lastIndex=calExp.IndexOf("]");
            //取出表达式中的Id那一段
            string idStr = calExp.Substring(firstIndex, lastIndex - firstIndex+1);
            //string[] calExpArray = calExp.Split(']');
            //把前面Id那一段用1代替
            calExp = calExp.Replace(idStr, "1");
            try
            {
                Microsoft.JScript.Vsa.VsaEngine ve = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
                object o = Microsoft.JScript.Eval.JScriptEvaluate(calExp, ve);

                if (o.Equals(double.NaN))
                {
                    KnowledgeMessageBox.InforMessage("校验不通过，表达式有错误，NaN(计算结果非数字)。");
                    return false;
                }

                if (o.Equals(Microsoft.JScript.GlobalObject.Infinity))
                {
                    KnowledgeMessageBox.InforMessage("校验不通过，表达式有错误，计算结果无穷大。");
                    return false;
                }
            }
            catch(Exception ex)
            {
                KnowledgeMessageBox.InforMessage("校验不通过，表达式有错误。\n"+ex.Message);
                return false;
            }
            return true;
        }

        private void LoadDimensionList()
        {
            if (vMain == null) return;
            dgvDim.Rows.Clear();

            CubeRegister cubeReg = vMain.CubeRegId;

            //添加维度列表
            try
            {
                IList list = model.CubeSrv.GetCubeAttrByCubeResgisterId(cubeReg);
                foreach (CubeAttribute cubeAttr in list)
                {
                    int i = dgvDim.Rows.Add();
                    DataGridViewRow row = dgvDim.Rows[i];
                    DimensionCategory cate = model.DimDefSrv.GetDimensionCategoryById(cubeAttr.DimensionId);
                    row.Tag = cate;
                    row.Cells["dim"].Value = cate.Name;
                    if (cate.Name.IndexOf(KnowledgeUtil.TIME_DIM_STR) != -1)
                    {
                        //row.Cells["selecteddim"].Value = KnowledgeUtil.TIME_DIM_STR;
                        //row.Cells["selecteddim"].Tag = KnowledgeUtil.TIME_DIM_STR;
                    }
                    if (cate.Name.IndexOf(KnowledgeUtil.YWZZ_DIM_STR) != -1)
                    {
                        //row.Cells["selecteddim"].Value = KnowledgeUtil.YWZZ_DIM_STR;
                        //row.Cells["selecteddim"].Tag = KnowledgeUtil.YWZZ_DIM_STR;
                    }

                }
            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("查找维度属性出错。", ex);
            }
        }
        #endregion

    }
}