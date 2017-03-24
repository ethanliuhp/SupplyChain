using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Component.Util;
using VirtualMachine.Core;
using NHibernate.Criterion;

public partial class MainPage_MainPageLeft : PageBaseClass
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == false)
        {
            string userCode = "";
            if (Request.QueryString["userCode"] != null)
            {

                userCode = Request.QueryString["userCode"];

                txtUserCode.Value = userCode;

                //char[] userCodeChar = Request.QueryString["userCode"].ToCharArray();
                //byte[] userCodeBytes = new byte[userCodeChar.Length];

                //for (int i = 0; i < userCodeChar.Length; i++)
                //{
                //    userCodeBytes[i] = (byte)userCodeChar[i];
                //}

                //userCode = Convert.ToBase64String(userCodeBytes);
            }

            if (Request.QueryString["proInfoAuth"] != null)
                txtProInfoAuth.Value = Request.QueryString["proInfoAuth"].Trim();


            string targetPageType = "projectWarning";
            if (Request.QueryString["targetPageType"] != null)
                targetPageType = Request.QueryString["targetPageType"];

            LoadOperationOrgTree(userCode, targetPageType);
        }
    }

    private void LoadOperationOrgTree(string userCode, string targetPageType)
    {
        Hashtable hashtable = new Hashtable();
        try
        {
            NavigateTree.Nodes.Clear();

            List<OperationOrg> listAllOrg = MGWBS.GWBSSrv.GetOperationOrg();

            //if (listAllOrg.Count == 0)
            //{
            //    divMessage.InnerText = "您所在的业务组织没有可操作的数据.";
            //    return;
            //}

            //foreach (List<OperationOrg> list in listAllOrg)
            //{

            int minLevel = listAllOrg.Min(o => o.Level);

            IList codeList = GetCodeList();

            foreach (OperationOrg childNode in listAllOrg)
            {
                //if (childNode.CategoryNodeType == VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.RootNode)
                //{
                //    childNode.Name += "根节点";
                //    childNode.OperationType = "h";
                //    txtProjectId.Value = "ProjectId=" + childNode.Id;
                //}

                TreeNode tnTmp = new TreeNode();
                tnTmp.Text = childNode.Name;
                tnTmp.Value = childNode.Id;
                if (targetPageType.ToLower() == "projectMap".ToLower())
                    tnTmp.NavigateUrl = "../Map/BaiduMap.aspx?ProjectId=" + childNode.Id + "&ProjectName=" + HttpUtility.UrlEncode(childNode.Name) + "&ProjectSyscode=" + childNode.SysCode + "&ProjectType=" + childNode.OperationType;
                else
                    tnTmp.NavigateUrl = "MainPageBottom.aspx?ProjectId=" + childNode.Id + "&ProjectName=" + HttpUtility.UrlEncode(childNode.Name) + "&ProjectSyscode=" + childNode.SysCode + "&ProjectType=" + childNode.OperationType;

                foreach (string codeStr in codeList)
                {
                    if (childNode.Code == codeStr)
                    {
                        tnTmp.ImageUrl = "../images/imageTemp/webcam.png";
                        //tnTmp.i
                        break;
                    }
                }

                if (childNode.Level == minLevel)//childNode.Level == 2 && childNode.OperationType.ToLower() == "h"
                {
                    NavigateTree.Nodes.Add(tnTmp);
                    txtProjectId.Value = "ProjectId=" + childNode.Id;
                }
                else
                {
                    TreeNode tnp = null;
                    tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                    if (tnp != null)
                        tnp.ChildNodes.Add(tnTmp);
                }
                hashtable.Add(tnTmp.Value, tnTmp);
            }
            //}
            if (listAllOrg.Count > 0)
            {
                //this.NavigateTree.Nodes[0].Select();
                //this.tvwCategory.Nodes[0].Expand();
            }
        }
        catch (Exception e)
        {
            Response.Write("<script type='text/javascript'>alert('" + ExceptionUtil.ExceptionMessage(e) + "');</script>");
        }
    }

   private IList GetCodeList()
    {
        IList codeList = new ArrayList();
        string str = UtilityClass.SendRequest("WebcamServerJsonApi", "cmd=allmonitorproject");
        int startIndex = str.IndexOf('[');
        str = str.Substring(startIndex + 1, str.Length - (startIndex + 1) - 2);
        str = str.Replace("{", "");
        str = str.Replace("}", "");
        string[] list = str.Split(',');

        foreach (string departStr in list)
        {
            if (departStr.Contains("depart"))
            {
                int codeStartIndex = departStr.IndexOf(':');
                string codeStr = departStr.Substring(codeStartIndex + 2, departStr.Length - codeStartIndex - 1 - 2);
                codeList.Add(codeStr);
            }
        }
        return codeList;
    }
}
