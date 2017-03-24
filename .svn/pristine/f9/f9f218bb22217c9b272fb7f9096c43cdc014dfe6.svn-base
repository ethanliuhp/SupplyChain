using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Collections;
using VirtualMachine.Component.Util;

public partial class UserControls_ProjectBaseInfo : System.Web.UI.UserControl, IPageLink
{
    private string _ProjectId;
    public string ProjectId
    {
        get { return _ProjectId; }
        set { _ProjectId = value; }
    }

    private string _ProjectSyscode;
    public string ProjectSyscode
    {
        get { return _ProjectSyscode; }
        set { _ProjectSyscode = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == false)
        {
            BindChart1Data();
        }
    }
    private void BindChart1Data()
    {
        ObjectQuery oq = new ObjectQuery();
        oq.AddCriterion(Expression.Eq("OwnerOrg.Id", ProjectId));
        IList listProject = MGWBS.GWBSSrv.ObjectQuery(typeof(CurrentProjectInfo), oq);
        CurrentProjectInfo projectInfo = null;
        if (listProject.Count > 0)
        {
            projectInfo = listProject[0] as CurrentProjectInfo;

            LoadProject(projectInfo);

            LoasdOrder(projectInfo);
        }
    }

    //工程简介
    void LoadProject(CurrentProjectInfo projectInfo)
    {
        txtProjectName.InnerText = projectInfo.Name;
        txtProName.InnerText = projectInfo.ManagerDepart;
        //txtCode.InnerText = ProjectInfo.Code;
        //txtProjectType.InnerText = projectInfo.ProjectType;//工程类型
        //txtContractWay.InnerText = projectInfo.ContractType;//承包方式
        txtProvince.InnerText = (string.IsNullOrEmpty(projectInfo.ProjectLocationProvince) ? "" : projectInfo.ProjectLocationProvince + "省")
            + (string.IsNullOrEmpty(projectInfo.ProjectLocationCity) ? "" : projectInfo.ProjectLocationCity + "市")
            + projectInfo.ProjectLocationDescript;
        txtContracte.InnerText = projectInfo.ContractArea; //承包范围

        //txtStructType.InnerText = projectInfo.StructureType;//结构类型
        txtBace.InnerText = projectInfo.BaseForm;//基础形式
        txtLifeCycleState.InnerText = EnumUtil<EnumProjectLifeCycle>.GetDescription(projectInfo.ProjectLifeCycle);//生命周期状态
        if (projectInfo.CreateDate > Convert.ToDateTime("2000-01-01"))
        {
            txtCreateDate.InnerText = projectInfo.CreateDate.ToShortDateString();//创建时间
        }
    }
    //合同摘要
    void LoasdOrder(CurrentProjectInfo projectInfo)
    {
        //if (projectInfo.BeginDate > ClientUtil.ToDateTime("2000-01-01"))
        //{
        //    txtStartDate.Value = projectInfo.BeginDate;
        //}
        //if (projectInfo.EndDate > ClientUtil.ToDateTime("2000-01-01"))
        //{
        //    txtCompleteDate.Value = projectInfo.EndDate;
        //}
        //txtQuanlityTarget.InnerText = projectInfo.QuanlityTarget;//质量目标
        //txtQualityReword.InnerText = projectInfo.QualityReword;//质量奖惩
        //txtSaftyTarget.InnerText = projectInfo.SaftyTarget;//安全目标
        //txtSaftyReword.InnerText = projectInfo.SaftyReword;//安全奖惩
        //txtProReword.InnerText = projectInfo.ProjecRewordt;//工期奖惩

        txtMoneySource.InnerText = EnumUtil<EnumSourcesOfFunding>.GetDescription(projectInfo.SourcesOfFunding);//资金来源
        //txtMoneyStates.SelectedIndex = projectInfo.IsFundsAvailabed;//资金到位情况
        txtProjectCost.InnerText = UtilityClass.DecimalRound(projectInfo.ProjectCost / 10000, 4);//工程造价
        txtRealPreMoney.InnerText = UtilityClass.DecimalRound(projectInfo.RealPerMoney / 10000, 4);//实际预算总金额
        txtConstractMoney.InnerText = UtilityClass.DecimalRound(projectInfo.CivilContractMoney / 10000, 4);//土建合同金额
        txtInstallOrderMoney.InnerText = UtilityClass.DecimalRound(projectInfo.InstallOrderMoney / 10000, 4);//安装合同金额
        txtCollectProport.InnerText = UtilityClass.DecimalRound(projectInfo.ContractCollectRatio, 4);//合同收款比例
        txtTurnProport.InnerText = UtilityClass.DecimalRound(projectInfo.ResProportion, 4);//责任上缴比例
        txtGroundPrice.InnerText = UtilityClass.DecimalRound(projectInfo.BigModualGroundUpPrice, 4);
        txtUnderPrice.InnerText = UtilityClass.DecimalRound(projectInfo.BigModualGroundDownPrice, 4);
        txtExplain.InnerText = projectInfo.Descript;//备注信息（项目说明）
    }
}
