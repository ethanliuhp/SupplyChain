using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.GwbsConfirm
{
    public partial class VChoiseTeam : TBasicToolBarByMobile
    {

        MConfirmmng model = new MConfirmmng();

        private CurrentProjectInfo projectInfo;
        private GWBSTree wbs;
        private IList selectSubContractProjectList;
        private SubContractType subType;
        /// <summary>
        /// 多选标志 0单选 1多选
        /// </summary>
        private int multipleChoiceFlag;


        private IList subContractProjectList;
        private IList operatSubContractProjectList = new ArrayList();

        private AutomaticSize automaticSize = new AutomaticSize();

        public GWBSTaskConfirm taskCofirm = null;

        //CheckBox cb = new CheckBox();

        private int pageCount = 0;//页数
        //private int index = 0;//对应的checkbox的下标
        private int pageSize = 8;//每页显示的checkbox控件的数
        //private IList lisPage = new ArrayList();
        private int page = 1;//当前页
        List<CheckBox> listCheckBox = new List<CheckBox>();

        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gbConfirm"></param>
        public VChoiseTeam()
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            InitData();
            InitEvent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="proInfo"></param>
        /// <param name="w">操作{工程项目任务} 没有则为NULL</param>
        /// <param name="selectedList">当前已经选中的{分包项目}集合（没有则为NULL） 集合里数据关联的分包合同契约要一起传过来</param>
        /// <param name="type">分包合同类型</param>
        /// <param name="flag">多选标志 0单选 1多选</param>
        public VChoiseTeam(CurrentProjectInfo proInfo, GWBSTree w, IList selectedList, SubContractType type, int flag)
        {
            InitializeComponent();
            automaticSize.SetTag(this);

            projectInfo = proInfo;
            wbs = w;
            selectSubContractProjectList = selectedList;
            subType = type;
            multipleChoiceFlag = flag;

            InitData();
            InitEvent();

        }

        private void InitEvent()
        {
            btnOK.Click += new EventHandler(btnOK_Click);
            btnNext.Click += new EventHandler(btnnext_Click);
            btnBack.Click += new EventHandler(btnback_Click);
        }

        public override void TBtnPageUp_Click(object sender, EventArgs e)
        {
            base.TBtnPageUp_Click(sender, e);//上一页
            this.Close();
        }

        void btnback_Click(object sender, EventArgs e)
        {
            page--;
            ShowData();
        }

        void btnnext_Click(object sender, EventArgs e)
        {
            page++;
            ShowData();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            //取选中的checkbox，并保存对应的分包项目
            for (int i = 0; i < listCheckBox.Count; i++)
            {
                CheckBox box = listCheckBox[i] as CheckBox;
                if (box.Checked)
                {
                    int index = (page - 1) * pageSize + i;
                    SubContractProject sub = operatSubContractProjectList[index] as SubContractProject; //box.Tag as SubContractProject;
                    result.Add(sub);
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

        private void InitData()
        {
            //SubContractProject c = new SubContractProject();
            //c.TheContractGroup
            //taskCofirm.TaskHandler.ContractType = SubContractType.劳务分包
            ObjectQuery oq = new ObjectQuery();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            //oq.AddCriterion(Expression.Eq("ContractType", SubContractType.劳务分包));
            oq.AddFetchMode("TheContractGroup", NHibernate.FetchMode.Eager);
            subContractProjectList = model.ProductionManagementSrv.ObjectQuery(typeof(SubContractProject), oq);
            //subContractProjectList = model.ContractExcuteSrv.GetContractExcute(oq);
            if (subContractProjectList == null || subContractProjectList.Count == 0)
            {
                //MessageBox.Show("未找到所属项目下的分包项目！");
                VMessageBox vmb = new VMessageBox();
                vmb.txtInformation.Text = "未找到符合条件的分包项目！";
                vmb.ShowDialog();
                return;
                //this.Close();
            }
            if (wbs == null)
            {
                operatSubContractProjectList = subContractProjectList;
            }
            else
            {
                #region
                oq.Criterions.Clear();
                Disjunction dis = new Disjunction();
                //OBSService os = new OBSService();
                //os.SupplierId
                //SubContractProject s = new SubContractProject();

                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                oq.AddCriterion(Expression.Eq("ServiceState", "生效"));
                oq.AddCriterion(Expression.Like("ProjectTaskCode", wbs.SysCode, MatchMode.Start));
                foreach (SubContractProject s in subContractProjectList)
                {
                    dis.Add(Expression.Eq("SupplierId.Id", s.Id));
                }
                oq.AddFetchMode("SupplierId", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("SupplierId.TheContractGroup", NHibernate.FetchMode.Eager);
                IList obsList = model.ProductionManagementSrv.ObjectQuery(typeof(OBSService), oq);

                if (obsList.Count == 0 || obsList == null)
                {
                    //MessageBox.Show("未在服务OBS里找到对应数据！");
                    VMessageBox vmb = new VMessageBox();
                    vmb.txtInformation.Text = "未在服务OBS里找到对应数据！";
                    vmb.ShowDialog();

                    //result = null;
                    //this.Close();
                    return;
                    //this.FindForm().Close();
                }

                foreach (OBSService o in obsList)
                {
                    SubContractProject s = o.SupplierId;
                    operatSubContractProjectList.Add(s);
                }

                if (selectSubContractProjectList != null && selectSubContractProjectList.Count > 0)
                {
                    SubContractProject sub = selectSubContractProjectList[0] as SubContractProject;
                    if (sub != null)
                    {
                        if (obsList.Count == 0 || obsList == null)
                        {
                            operatSubContractProjectList.Add(sub);
                        }
                        else
                        {
                            bool flag = false;
                            foreach (SubContractProject scp in operatSubContractProjectList)
                            {
                                if (sub.Id == scp.Id)
                                    flag = true;
                            }
                            if (!flag)
                                operatSubContractProjectList.Add(sub);
                        }
                    }
                }
                #endregion
            }

            if (operatSubContractProjectList.Count % pageSize != 0)// 如果除不尽，结果+1，强转int
            {
                pageCount = operatSubContractProjectList.Count / pageSize + 1;
            }
            else
            {
                pageCount = operatSubContractProjectList.Count / pageSize;
            }
            
        }

        private void ShowData()
        {
            if (operatSubContractProjectList.Count == 0)
            {
                this.Close();
                return;
            }

            listCheckBox.Clear();
            this.pnlShow.Controls.Clear();
            //this.pnlFloor.Controls.Clear();
            //this.pnlFloor.Controls.Add(btnBack);
            //this.pnlFloor.Controls.Add(btnNext);
            //this.pnlFloor.Controls.Add(btnOK);
            this.WindowState = FormWindowState.Maximized;

            int begionNum = (page - 1) * pageSize;
            int endNum = begionNum + pageSize;
            if (page == pageCount)
            {
                endNum = operatSubContractProjectList.Count;
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
                SubContractProject sub = operatSubContractProjectList[i] as SubContractProject;
                CheckBox cb = new CheckBox();
                cb.Location = new Point(20, 0);
                cb.AutoEllipsis = true;
                cb.AutoSize = false;
                cb.Size = new Size(width, height);
                cb.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

                cb.Name = "cb" + i;

                cb.Text = sub.BearerOrgName;
                if (sub.TheContractGroup != null)
                    cb.Text += " + " + sub.TheContractGroup.ContractName;//分包商服务名称+契约名称
               
                    
                //cb.Tag = sub;
                listCheckBox.Add(cb);
                cb.Visible = false;
                this.pnlShow.Controls.Add(cb);
                //this.pnlFloor.Controls.Add(cb);
            }

            for (int i = 0; i < listCheckBox.Count; i++)
            {
                CheckBox box = listCheckBox[i] as CheckBox;
                box.Visible = true;
                box.Top = height * i;
            }

        }

        private void VChoiseTeam_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            ShowData();
        }
    }
}
