using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery
{
    public partial class VChosePersonByRole : TBasicToolBarByMobile
    {
        private AutomaticSize automaticSize = new AutomaticSize();
        private MProjectTaskQuery model = new MProjectTaskQuery();

        private GWBSTree gwbsTree;
        private CurrentProjectInfo projectInfo;
        private IList selectProjectList;

        private int page = 1;//当前页
        private int pageCount = 0;//页数
        private int pageSize = 6;//每页显示的checkbox控件的数
        /// <summary>
        /// 多选标志 0单选 1多选
        /// </summary>
        private int multipleChoiceFlag;
        List<CheckBox> listCheckBox = new List<CheckBox>();
        private IList list;
        private IList personList = new ArrayList();
        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VChosePersonByRole(CurrentProjectInfo proInfo, GWBSTree gwbsTree, IList selectedList, int flag)
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            this.projectInfo = proInfo;
            this.gwbsTree = gwbsTree;
            this.selectProjectList = selectedList;
            this.multipleChoiceFlag = flag;
            PersonAndCode();
            InitEvent();
        }

        public void InitEvent()
        {
            btnBack.Click += new EventHandler(btnBack_Click);
            btnNext.Click += new EventHandler(btnNext_Click);
            btnOK.Click += new EventHandler(btnOK_Click);

            this.Load += new EventHandler(VChosePersonByRole_Load);
        }

        void VChosePersonByRole_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            showPersonAndCode();
        }

        void btnBack_Click(object sender, EventArgs e)
        {
            page--;
            showPersonAndCode();
        }

        void btnNext_Click(object sender, EventArgs e)
        {
            page++;
            showPersonAndCode();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listCheckBox.Count; i++)
            {
                CheckBox box = listCheckBox[i] as CheckBox;
                if (box.Checked)
                {
                    int index = (page - 1) * pageSize + i;
                    StandardPerson person = personList[index] as StandardPerson; //box.Tag as StandardPerson;
                    result.Add(person);
                }
            }
            if (result == null || result.Count == 0)
            {
                VMessageBox vmb = new VMessageBox();
                vmb.txtInformation.Text = "请选择一条数据！";
                vmb.ShowDialog();
                return;
            }
            if (multipleChoiceFlag == 0 && result.Count > 1)
            {
                VMessageBox vmb = new VMessageBox();
                vmb.txtInformation.Text = "只能选择一条数据！";
                vmb.ShowDialog();
                return;
            }
            this.btnOK.FindForm().Close();
        }

        private void PersonAndCode()
        {
            ObjectQuery objectQuery = new ObjectQuery();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            string projectId = projectInfo.Id;
            string roleId = "";
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectId));
            list = model.GetPersonListByProjectAndRole(projectId, roleId);
            if (list == null || list.Count == 0)
            {
                VMessageBox v_show = new VMessageBox();
                v_show.txtInformation.Text = "该项目下没有角色和人员";
                v_show.ShowDialog();
            }
            if (gwbsTree == null)
            {
                personList = list;
            }
            else
            {
                if (selectProjectList != null && selectProjectList.Count > 0)
                {
                    StandardPerson sub = selectProjectList[0] as StandardPerson;
                    if (sub != null)
                    {
                        personList.Add(sub);
                    }
                }
            }
            if (personList.Count % pageSize != 0)// 如果除不尽，结果+1，强转int
            {
                pageCount = personList.Count / pageSize + 1;
            }
            else
            {
                pageCount = personList.Count / pageSize;
            }
        }

        private void showPersonAndCode()
        {
            listCheckBox.Clear();
            this.pnlShow.Controls.Clear();
            int begionNum = (page - 1) * pageSize;
            int endNum = begionNum + pageSize;
            if (page == pageCount)
            {
                endNum = personList.Count;
                btnNext.Enabled = false;
            }
            else
                btnNext.Enabled = true;

            if (page == 1)
                btnBack.Enabled = false;
            else
                btnBack.Enabled = true;


            int width = pnlShow.Width - 20;
            int height = pnlShow.Height / pageSize;

            for (int i = begionNum; i < endNum; i++)
            {
                StandardPerson person = personList[i] as StandardPerson;
                CheckBox cb = new CheckBox();
                cb.Location = new Point(20, 0);
                cb.AutoEllipsis = true;
                cb.AutoSize = false;
                cb.Size = new Size(width, height);
                cb.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                cb.Name = "cb" + i;
                cb.Text = person.Name + " + " + person.Code;
                listCheckBox.Add(cb);
                cb.Visible = false;
                this.pnlShow.Controls.Add(cb);
            }

            for (int i = 0; i < listCheckBox.Count; i++)
            {
                CheckBox box = listCheckBox[i] as CheckBox;
                box.Visible = true;
                box.Top = height * i;
            }
        }

    }
}
