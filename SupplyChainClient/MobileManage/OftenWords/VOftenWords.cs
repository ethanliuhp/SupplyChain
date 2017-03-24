using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using VirtualMachine.Core;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
//using Spring.Expressions;



namespace Application.Business.Erp.SupplyChain.Client.MobileManage.OftenWords
{
    public partial class VOftenWords : TBasicDataViewByMobile
    {
        private AutomaticSize automaticSize = new AutomaticSize();

        private MOftenWords mo = new MOftenWords();
        private string userID = string.Empty;
        private string interphaseID = string.Empty;
        private string controlID = string.Empty;
        private string editPhrase = string.Empty;

        private string result;
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public string Result
        {
            get { return result; }
            set { result = value; }
        }

        public VOftenWords()
        {
            InitializeComponent();
            //ShowOftenWords();
            InitEvent();
            automaticSize.SetTag(this);
        }
        public VOftenWords(string user_ID, string interphase_ID, string control_ID, string oftenWord)
        {
            InitializeComponent();
            InitEvent();
            userID = user_ID;
            interphaseID = interphase_ID;
            controlID = control_ID;
            txtEditPhrase.Text = oftenWord;
            automaticSize.SetTag(this);
            ShowOftenWords();
        }

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            //this.btnSave.Click += new EventHandler(btnSave_Click);
            //this.dgOftenWords.CellContentDoubleClick +=new DataGridViewCellEventHandler(dgOftenWords_CellContentDoubleClick);
        }
        /// <summary>
        /// 查询常用短语
        /// </summary>
        private void ShowOftenWords()
        {
            ObjectQuery objectQuery = new ObjectQuery();
            //添加查询条件
            objectQuery.AddCriterion(Expression.Eq("UserID", userID));
            objectQuery.AddCriterion(Expression.Eq("InterphaseID", interphaseID));
            objectQuery.AddCriterion(Expression.Eq("ControlID", controlID));
            if (txtEditPhrase.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("OftenWords", txtEditPhrase.Text, MatchMode.Anywhere));
            }
            IList SearchResult = mo.CurrentProjectSrv.GetOftenWords(objectQuery);
            this.dgOftenWords.Rows.Clear();
            if (SearchResult.Count > 0 && SearchResult != null)
            {
                foreach (OftenWord ow in SearchResult)
                {
                    int rowIndex = this.dgOftenWords.Rows.Add();
                    dgOftenWords[oftenWords.Name, rowIndex].Value = ow.OftenWords;
                    //dgOftenWords[userName.Name, rowIndex].Value = ow.UserName;
                    this.dgOftenWords.Rows[rowIndex].Tag = ow;
                }
            }
        }
        /// <summary>
        /// 保存常用短语
        /// </summary>
        private void SaveOftenWords(OftenWord ow)
        {
            if (txtEditPhrase.Text != "")
            {
                OftenWord word = mo.CurrentProjectSrv.GetOftenWordByOftenWord(userID, interphaseID, controlID, txtEditPhrase.Text);
                if (word == null)
                {
                    mo.CurrentProjectSrv.SaveOftenWords(ow);
                    editPhrase = txtEditPhrase.Text;
                    MessageBox.Show("保存成功！");                    
                    txtEditPhrase.Text = "";
                    ShowOftenWords();
                }
                else
                {
                    MessageBox.Show("此短语已经保存过！");
                }
            }
            else
            {
                MessageBox.Show("保存数据不能为空！");
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ShowOftenWords();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OftenWord ow = new OftenWord();
            //ow.Id = "8";
            //ow.Version = 1;
            ow.OftenWords = txtEditPhrase.Text;
            ow.UserID = userID;
            ow.InterphaseID = interphaseID;
            ow.ControlID = controlID;
            SaveOftenWords(ow);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgOftenWords.Rows.Count == 0)
            {
                MessageBox.Show("未有选中项");
            }
            else
            {

                if (MessageBox.Show("确定删除吗？", "删除提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DeleteOftenWord();
                }
            }
        }
        /// <summary>
        /// 删除常用短语
        /// </summary>
        private void DeleteOftenWord()
        {
            //if (dgOftenWords.Rows.Count == 0)
            //{
            //    MessageBox.Show("未有选中项");
            //}
            //else
            //{
            OftenWord ow = dgOftenWords.CurrentRow.Tag as OftenWord;
            int index = dgOftenWords.CurrentRow.Index;
            mo.CurrentProjectSrv.DeleteByDao(ow);
            dgOftenWords.Rows.RemoveAt(index);
            //}
        }

        private void dgOftenWords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("123");
            OftenWord ow = dgOftenWords.CurrentRow.Tag as OftenWord;
            MessageBox.Show(ow.OftenWords);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string returnOftenWord = GetReturnOfrenWord();
            //MessageBox.Show(returnOftenWord);
            result = returnOftenWord;
            this.Close();
            editPhrase = string.Empty;
        }
        string GetReturnOfrenWord()
        {
            if (dgOftenWords.Rows.Count == 0)
            {
                if (editPhrase == "")
                {
                    editPhrase = txtEditPhrase.Text;
                }
                return editPhrase;
            }
            else
            {
                OftenWord ow = dgOftenWords.CurrentRow.Tag as OftenWord;
                return ow.OftenWords;
            }
        }

    }
}
