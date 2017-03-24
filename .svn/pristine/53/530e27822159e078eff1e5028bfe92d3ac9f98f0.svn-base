using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Collections;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;

public partial class Map_BaiduMap : PageBaseClass
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == false)
        {
            if (Request.QueryString["ProjectId"] != null)
            {
                string projectId = Request.QueryString["ProjectId"];
                string ProjectName = Request.QueryString["ProjectName"];
                string projectType = Request.QueryString["ProjectType"];
                string ProjectSyscode = Request.QueryString["ProjectSyscode"];


                string projectNames = txtProjectName.Value;
                string isUsedDefineIcons = txtProjectType.Value;
                string places = txtPlace.Value;
                string citys = txtCity.Value;
                string pointXs = txtPointX.Value;
                string pointYs = txtPointY.Value;
                string tels = txtTel.Value;

                string projectIdStr = txtProjectIdStr.Value;
                string projectSyscodeStr = txtProjectSyscodeStr.Value;

                ObjectQuery oq = new ObjectQuery();
                IList listProject = null;
                if (projectType == "h" || projectType == "b")
                {
                    oq.AddCriterion(Expression.Like("SysCode", ProjectSyscode, MatchMode.Start));
                    oq.AddCriterion(Expression.IsNotNull("MapPoint"));
                    listProject = MGWBS.GWBSSrv.ObjectQuery(typeof(OperationOrg), oq);
                    if (listProject.Count > 0)
                    {
                        foreach (OperationOrg projectInfo in listProject)
                        {
                            projectNames += projectInfo.Name + "|";
                            isUsedDefineIcons += (string.IsNullOrEmpty(projectInfo.OperationType) ? "" : projectInfo.OperationType.ToLower()) + "|";
                            projectIdStr += projectInfo.Id + "|";
                            projectSyscodeStr += projectInfo.SysCode + "|";

                            places += (!string.IsNullOrEmpty(projectInfo.Place) ? projectInfo.Place : "&nbsp;") + "|";
                            citys += "|";//用于定位地图上的城市
                            tels += "&nbsp;|";

                            bool hasPoint = false;
                            if (!string.IsNullOrEmpty(projectInfo.MapPoint))
                            {
                                string[] point = projectInfo.MapPoint.Split(new char[] { ',', '，' });
                                if (point.Length > 1)
                                {
                                    pointXs += point[0] + "|";
                                    pointYs += point[1] + "|";

                                    hasPoint = true;
                                }
                            }

                            if (hasPoint == false)
                            {
                                pointXs += "|";
                                pointYs += "|";
                            }
                        }
                    }
                }


                oq.Criterions.Clear();
                oq.AddCriterion(Expression.Like("OwnerOrgSysCode", ProjectSyscode, MatchMode.Start));
                oq.AddCriterion(Expression.IsNotNull("MapPoint"));
                oq.AddFetchMode("OwnerOrg", NHibernate.FetchMode.Eager);
                listProject = MGWBS.GWBSSrv.ObjectQuery(typeof(CurrentProjectInfo), oq);
                if (listProject.Count > 0)
                {
                    foreach (CurrentProjectInfo projectInfo in listProject)
                    {
                        projectNames += projectInfo.Name + "|";
                        isUsedDefineIcons += (string.IsNullOrEmpty(projectInfo.OwnerOrg.OperationType) ? "" : projectInfo.OwnerOrg.OperationType.ToLower()) + "|";
                        projectIdStr += projectInfo.OwnerOrg.Id + "|";
                        projectSyscodeStr += projectInfo.OwnerOrg.SysCode + "|";
                        tels += "&nbsp;|";

                        string place = (string.IsNullOrEmpty(projectInfo.ProjectLocationProvince) ? "" : projectInfo.ProjectLocationProvince + "省")
                                    + (string.IsNullOrEmpty(projectInfo.ProjectLocationCity) ? "" : projectInfo.ProjectLocationCity + "市")
                                    + projectInfo.ProjectLocationDescript;

                        places += (!string.IsNullOrEmpty(place) ? place : "&nbsp;") + "|";
                        citys += (string.IsNullOrEmpty(projectInfo.ProjectLocationCity) ? "" : projectInfo.ProjectLocationCity.IndexOf("市") > -1 ? projectInfo.ProjectLocationCity : projectInfo.ProjectLocationCity + "市") + "|";//用于定位地图上的城市

                        bool hasPoint = false;
                        if (!string.IsNullOrEmpty(projectInfo.MapPoint))
                        {
                            string[] point = projectInfo.MapPoint.Split(new char[] { ',', '，' });
                            if (point.Length > 1)
                            {
                                pointXs += point[0] + "|";
                                pointYs += point[1] + "|";

                                hasPoint = true;
                            }
                        }

                        if (hasPoint == false)
                        {
                            pointXs += "|";
                            pointYs += "|";
                        }
                    }
                }

                if (projectNames.Length > 0)
                    projectNames = projectNames.Substring(0, projectNames.Length - 1);
                if (isUsedDefineIcons.Length > 0)
                    isUsedDefineIcons = isUsedDefineIcons.Substring(0, isUsedDefineIcons.Length - 1);
                if (tels.Length > 0)
                    tels = tels.Substring(0, tels.Length - 1);
                if (places.Length > 0)
                    places = places.Substring(0, places.Length - 1);
                if (citys.Length > 0)
                    citys = citys.Substring(0, citys.Length - 1);
                if (pointXs.Length > 0)
                    pointXs = pointXs.Substring(0, pointXs.Length - 1);
                if (pointYs.Length > 0)
                    pointYs = pointYs.Substring(0, pointYs.Length - 1);
                if (projectIdStr.Length > 0)
                    projectIdStr = projectIdStr.Substring(0, projectIdStr.Length - 1);
                if (projectSyscodeStr.Length > 0)
                    projectSyscodeStr = projectSyscodeStr.Substring(0, projectSyscodeStr.Length - 1);

                txtProjectName.Value = projectNames;
                txtProjectType.Value = isUsedDefineIcons;
                txtTel.Value = tels;
                txtPlace.Value = places;
                txtCity.Value = citys;
                txtPointX.Value = pointXs;
                txtPointY.Value = pointYs;
                txtProjectIdStr.Value = projectIdStr;
                txtProjectSyscodeStr.Value = projectSyscodeStr;
            }
        }
    }
}
