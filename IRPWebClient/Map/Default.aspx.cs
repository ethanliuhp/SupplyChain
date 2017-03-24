using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Component.Util;

public partial class Map_Default : PageBaseClass
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == false)
        {
            LoadOperationOrgTree();

            //ifrmMap.Attributes.Add("src", "BaiduMap.aspx");
        }
    }
    private void LoadOperationOrgTree()
    {
        Hashtable hashtable = new Hashtable();
        try
        {
            NavigateTree.Nodes.Clear();

            //机构类型 h 总公司；hd 总公司部门；b 分公司；bd 分公司部门；zgxmb 直管项目部；fgsxmb 分公司项目部

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("State", 1));

            Disjunction dis = new Disjunction();
            dis.Add(Expression.Eq("OperationType", "h"));
            dis.Add(Expression.Eq("OperationType", "b"));
            dis.Add(Expression.Eq("OperationType", "zgxmb"));
            dis.Add(Expression.Eq("OperationType", "fgsxmb"));
            //dis.Add(Expression.Eq("CategoryNodeType", VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.RootNode));
            oq.AddCriterion(dis);

            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));
            IList list = MGWBS.GWBSSrv.ObjectQuery(typeof(OperationOrg), oq);

            foreach (OperationOrg childNode in list)
            {
                if (childNode.State == 0)
                    continue;

                TreeNode tnTmp = new TreeNode();
                tnTmp.Text = childNode.Name;
                tnTmp.Value = childNode.Id;
                tnTmp.NavigateUrl = "/IRPWebClient/map/BaiduMap.aspx?ProjectId=" + childNode.Id + "&ProjectName=" + HttpUtility.UrlEncode(childNode.Name) + "&ProjectType=" + childNode.OperationType;

                if (childNode.Level == 2 && childNode.OperationType.ToLower() == "h")
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
            if (list.Count > 0)
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
}
