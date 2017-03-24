using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.Basic
{
    public partial class VBillComments : TBasicDataView
    {
        private CurrentProjectInfo projectInfo = null;
        private string billId = string.Empty;//单据对象ID
        private string billName = string.Empty;//单据名称
        private string billTypeName = string.Empty;//单据类型名称
        private PersonInfo billHandlePerson = null;//单据责任人
        private string billHandlePersonName = string.Empty;//单据责任人名称
        private OperationOrgInfo theHandlePersonOrg = null;//单据责任人所属组织
        private string theHandlePersonOrgName = string.Empty;//单据责任人所属组织名称
        private string theHandlePersonOrgSyscode = string.Empty;//单据责任人所属组织层次码
        private PersonInfo postPerson = null;//发帖人
        OperationOrgInfo postPersonOrg = null;//发帖人所属组织
        private DateTime billCreateTime;

        public VBillComments()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">单据对象ID</param>
        /// <param name="name">单据名称</param>
        /// <param name="typeName">单据类型名称</param>
        /// <param name="handlePerson">单据责任人</param>
        /// <param name="handlePersonName">单据责任人名称</param>
        /// <param name="handlePersonOrg">单据责任人所属组织</param>
        /// <param name="handlePersonOrgName">单据责任人所属组织名称</param>
        /// <param name="handlePersonOrgSyscode">单据责任人所属组织层次码</param>
        public VBillComments(string id, string name, string typeName, PersonInfo handlePerson, string handlePersonName, OperationOrgInfo handlePersonOrg, string handlePersonOrgName, string handlePersonOrgSyscode, DateTime time)
        {
            InitializeComponent();
            projectInfo = StaticMethod.GetProjectInfo();
            postPerson = ConstObject.LoginPersonInfo;
            postPersonOrg = ConstObject.TheOperationOrg;
            billId = id;
            billName = name;
            billTypeName = typeName;
            billHandlePerson = handlePerson;
            billHandlePersonName = handlePersonName;
            theHandlePersonOrg = handlePersonOrg;
            theHandlePersonOrgName = handlePersonOrgName;
            theHandlePersonOrgSyscode = handlePersonOrgSyscode;
            billCreateTime = time;

            txtShowComment.Text = string.Empty;
            this.Text = "【" + typeName + "-" + name + "】的评论信息";
            InitData();
            InitEvents();
        }
        void InitData()
        {
            LoadComments();
        }
        void InitEvents()
        {
            btnComment.Click += new EventHandler(btnComment_Click);
            btnClose.Click += new EventHandler(btnClose_Click);
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void LoadComments()
        {
            List<BillComments> billCommentList = StaticMethod.GetCommentsByBillId(billId);
            foreach (BillComments bc in billCommentList)
            {
                int start = txtShowComment.Text.Length;
                string postInfo = bc.PostPersonName + "【" + bc.PostPersonOrgName + "-" + bc.PostPersonJobName + "】" + "  " + bc.CommentCommitTime + "\r\n";
                string comments = bc.Comment + "\r\n";
                //txtShowComment.Text += (postInfo + comments);
                txtShowComment.AppendText(postInfo);
                txtShowComment.AppendText(comments);
                txtShowComment.Select(start, (postInfo.Length - 1));
                txtShowComment.SelectionColor = Color.Blue;

                txtShowComment.Select((start + postInfo.Length), comments.Length);
                //txtShowComment.SelectionHangingIndent = 20;
                txtShowComment.SelectionIndent = 20;
            }
            txtShowComment.SelectionStart = txtShowComment.Text.Length;
            txtShowComment.ScrollToCaret();
            //txtEditComment.SelectionStart = txtEditComment.Text.Length;
            txtEditComment.Select();
            txtEditComment.Focus();
        }
        /// <summary>
        /// 发表评论
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnComment_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtEditComment.Text.Trim() == "")
                {
                    ToolTip tooltip = new ToolTip();
                    tooltip.Show("请输入要评论的内容！", txtEditComment, (txtEditComment.Left + 5), (txtEditComment.Top + 20), 2000);
                    txtEditComment.Focus();
                    return;
                }

                BillComments bc = new BillComments();
                bc.TheProjectGUID = projectInfo.Id;
                bc.TheProjectName = projectInfo.Name;
                bc.BillTypeName = billTypeName;
                bc.BillID = billId;
                bc.BillName = billName;
                bc.BillHandlePerson = billHandlePerson;
                bc.BillHandlePersonName = billHandlePersonName;
                bc.TheHandlePersonOrg = theHandlePersonOrg;
                bc.TheHandlePersonOrgName = theHandlePersonOrgName;
                bc.TheHandlePersonOrgSyscode = theHandlePersonOrgSyscode;
                bc.BillCreateTime = billCreateTime;
                bc.PostPerson = postPerson;
                bc.PostPersonName = postPerson.Name;
                bc.PostPersonOrg = postPersonOrg;
                bc.PostPersonOrgName = postPersonOrg.Name;
                bc.PostPersonJobName = ConstObject.TheSysRole.RoleName;
                bc.Comment = txtEditComment.Text;

                if (StaticMethod.SaveBillComment(bc))
                {
                    int start = txtShowComment.Text.Length;
                    string postInfo = bc.PostPersonName + "【" + bc.PostPersonOrgName + "-" + bc.PostPersonJobName + "】" + "  " + bc.CommentCommitTime + "\r\n";
                    string comments = bc.Comment + "\r\n";
                    //txtShowComment.Text += (postInfo + comments);
                    txtShowComment.AppendText(postInfo);
                    txtShowComment.AppendText(comments);

                    txtShowComment.Select(start, (postInfo.Length - 1));
                    txtShowComment.SelectionColor = Color.Blue;
                    txtShowComment.Select((start + postInfo.Length), comments.Length);
                    txtShowComment.SelectionIndent = 20;

                    txtShowComment.SelectionStart = txtShowComment.Text.Length;
                    txtShowComment.ScrollToCaret();

                    txtEditComment.Text = string.Empty;
                    txtEditComment.Focus();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("评论失败：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
    }
}
