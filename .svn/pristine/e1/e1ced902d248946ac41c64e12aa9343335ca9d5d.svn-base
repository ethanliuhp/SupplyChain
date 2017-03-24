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

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimDefine
{
    public partial class VCalExpressionDefine : Form
    {

        private MIndicatorUse model = new MIndicatorUse();
        public bool isOkClicked = false;
        public string calExp = "";
        public string dimSysCode = "";

        public VCalExpressionDefine()
        {
            InitializeComponent();
        }

        private void btnClearExp_Click(object sender, EventArgs e)
        {
            txtCalExp.Text = "";
            txtCalExp.Tag = null;
        }

        private void UpdateTxtTag(string text)
        {
            string calExp = txtCalExp.Tag as string;
            if (calExp == null) calExp = "";
            calExp += text;
            txtCalExp.Tag = calExp;
        }

        private void btnValidateExp_Click(object sender, EventArgs e)
        {
            if (ValidateExp())
            {
                KnowledgeMessageBox.InforMessage("校验通过。");
            }
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

            string patten="}{";
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

            try
            {
                Microsoft.JScript.Vsa.VsaEngine ve = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
                object o = Microsoft.JScript.Eval.JScriptEvaluate(calExp, ve);
                
                if(o.Equals(double.NaN))
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
            catch
            {
                KnowledgeMessageBox.InforMessage("校验不通过，表达式有错误。");
                return false;
            }
            return true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateExp())
            {
                isOkClicked = true;
                calExp = txtCalExp.Text;
                calExp += VDimensionDefine.CalExpSplit.ToString();
                if (txtCalExp.Tag != null)
                {
                    calExp += txtCalExp.Tag.ToString();
                }
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

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

        private void VCalExpressionDefine_Load(object sender, EventArgs e)
        {
            if (calExp != null && calExp.IndexOf(VDimensionDefine.CalExpSplit) >= 0)
            {
                string[] calExpArray = calExp.Split(VDimensionDefine.CalExpSplit);
                txtCalExp.Text = calExpArray[0];
                txtCalExp.Tag = calExpArray[1];
            }
            else
            {
                txtCalExp.Text = calExp;
            }
            LoadIndicatorCategoryTree();
        }
        /// <summary>
        /// 加载维度数
        /// </summary>
        private void LoadIndicatorCategoryTree()
        {
            try
            {
                Hashtable table = new Hashtable();
                TVCategory.Nodes.Clear();

                //通过处理，把100.3.102.106.112.得到前两位100.3.
                int endSub = dimSysCode.IndexOf(".", dimSysCode.IndexOf(".")+1);
                string handleSysCode = dimSysCode.Substring(0, endSub+1);
                IList list = model.DimDefSrv.GetAllChildNodesBySysCode(handleSysCode);

                if (list == null || list.Count == 0)
                {
                    KnowledgeMessageBox.InforMessage("未找到下级｛指标｝维度，请先定义。");
                    return;
                }

                bool addRoot = true;

                foreach (DimensionCategory cat in list)
                {
                    if (addRoot)
                    {
                        DimensionCategory parent = cat.ParentNode as DimensionCategory;
                        TreeNode root=new TreeNode(parent.Name);
                        root.Name=parent.Id.ToString();
                        TVCategory.Nodes.Add(root);
                        table.Add(root.Name, root);
                        addRoot = false;
                    }

                    TreeNode tmpNode = new TreeNode();
                    tmpNode.Name = cat.Id.ToString();
                    tmpNode.Text = cat.Name;
                    tmpNode.Tag = cat;
                    if (cat.ParentNode != null)
                    {
                        TreeNode node = table[cat.ParentNode.Id.ToString()] as TreeNode;
                        node.Nodes.Add(tmpNode);
                    }
                    else
                    {
                        TVCategory.Nodes.Add(tmpNode);
                    }
                    table.Add(tmpNode.Name, tmpNode);
                }
                //选中顶级节点，并且展开一级子节点
                if (list.Count > 0)
                {
                    TVCategory.SelectedNode = TVCategory.Nodes[0];
                    TVCategory.SelectedNode.Expand();
                }
            }
            catch (Exception ex)
            {
                KnowledgeMessageBox.InforMessage("装载维度分类树出错。", ex);
            }
        }

        private void btnCalExpAddDim_Click(object sender, EventArgs e)
        {
            CalExpAddDimension();
        }

        private void CalExpAddDimension()
        {
            TreeNode node = TVCategory.SelectedNode;
            if (node == null || node.Level == 0)
            {
                KnowledgeMessageBox.InforMessage("请先选择一个指标。");
                return;
            }
            DimensionCategory cat = node.Tag as DimensionCategory;
            if (cat == null)
            {
                KnowledgeMessageBox.InforMessage("请先选择一个指标。");
                return;
            }

            txtCalExp.AppendText(cat.Name);
            UpdateTxtTag("{" + cat.Id + "}");
        }
    }
}