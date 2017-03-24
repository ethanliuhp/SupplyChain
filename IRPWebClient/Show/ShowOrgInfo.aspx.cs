using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;

public partial class Show_ShowOrgInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            IntialTree();
        }
    }
    public void IntialTree()
    {
        Hashtable hashtable = new Hashtable();
        IList lstTitles = ResourceSvr.OperationOrgService.GetOpeOrgsByInstance();
        int count = 0;
        this.tvTitle.Nodes.Clear();
        this.tvTitle.Attributes.Add("onclick", "checkNode(this)");
        if (lstTitles != null && lstTitles.Count > 0)
        {
            IList lst = lstTitles[0] as IList;
            foreach (OperationOrg childNode in lst)
            {

                TreeNode tnTmp = new TreeNode();
                tnTmp.Value = childNode.Id.ToString();
                tnTmp.Target = childNode.Id.ToString();
                tnTmp.Text = childNode.Name;
                //tnTmp.Tag = childNode;
                tnTmp.ShowCheckBox = true;
                tnTmp.SelectAction = TreeNodeSelectAction.Expand;
                // tnTmp.DataItem = childNode;
                if (childNode.ParentNode != null)
                {
                    TreeNode tnp = null;
                    tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                    if (tnp != null)
                        tnp.ChildNodes.Add(tnTmp);
                }
                else
                {
                    this.tvTitle.Nodes.Add(tnTmp);
                }
                hashtable.Add(tnTmp.Value, tnTmp);

            }
        }
    }
  
    public void btnSureClick(object sender, EventArgs e)
    {
        if (this.tvTitle.CheckedNodes.Count > 0)
        {
            string sID = this.tvTitle.CheckedNodes[0].Value;
            OperationOrg oAccount = ResourceSvr.OperationOrgService.GetOperationOrgById(sID);
            if (oAccount != null)
            {
                hdSelectData.Value = UtilClass.ObjectToJson(oAccount);
                UtilClass.ExecuteScript(this, "Sure();");
            }
        }

    }
}
