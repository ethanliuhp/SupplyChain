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
using IRPServiceModel.Domain.CompanyMng;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

public partial class CompanyMng_CompanyMng : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            IntialTreeView();

        }
    }

    protected void trvOrg_SelectedNodeChanged(object sender, EventArgs e)
    {
        this.ShowCompanyInfo();
    }

    public void IntialTreeView()
    {
        this.trvOrg.Nodes.Clear();
        try
        {
            Hashtable hashtable = new Hashtable();

            IList listAll = GlobalClass.CompanyInfoSvr.GetTree();
            // lstInstance = listAll[1] as IList;
            //IList list = listAll[0] as IList;
            if (listAll == null || listAll.Count == 0)
            {
                CompanyInfo oCompanyInfo = GlobalClass.CompanyInfoSvr.SavePlaceTreeRootNode();
                listAll = new ArrayList();
                listAll.Add(oCompanyInfo);

            }
            foreach (CompanyInfo childNode in listAll)
            {
                if (childNode.State == 0)
                    continue;
                TreeNode tnTmp = new TreeNode();
                tnTmp.Text = childNode.Name;
                tnTmp.Value = childNode.Id;
                tnTmp.Target = childNode.OrderNo.ToString();
                if (childNode.ParentNode != null && !string.IsNullOrEmpty(childNode.ParentNode.Id))
                {
                    TreeNode tnp = null;

                    tnp = hashtable[childNode.ParentNode.Id.ToString()] as TreeNode;
                    if (tnp != null)
                    {
                        tnp.ChildNodes.AddAt(tnp.ChildNodes.Count, tnTmp);

                    }

                }
                else
                {
                    trvOrg.Nodes.Add(tnTmp);
                }
                hashtable.Add(tnTmp.Value, tnTmp);
            }
            //if (trvOrg.Nodes.Count > 0 && trvOrg.Nodes[0].ChildNodes.Count > 0)
            //{
            //    trvOrg.Nodes[0].ChildNodes[0].Select();
            //    ShowOrgINfo();
            //}
        }
        catch (Exception ex)
        {
            string sMsg = string.Format("加载公司树失败:{0}", ex.Message);
            MessageBox(sMsg);

        }

    }
   
    
    
    public void MessageBox(string sMessage)
    {
        string sMsg = string.Empty;
        if (!string.IsNullOrEmpty(sMessage))
        {
            sMsg = string.Format("<script>alert('{0}');</script>", sMessage);
            //Response.Write(sMsg);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", sMsg);
        }
    }
    private void ShowCompanyInfo()
    {
        string sMsg = string.Empty;
        try
        {
            CompanyInfo oNode = GlobalClass.CompanyInfoSvr.GetCompanyInfoById(this.trvOrg.SelectedNode.Value);
            if (oNode != null)
            {
                this.txtAddress.Value = oNode.Address;
                this.txtCode.Value = oNode.Code;
                this.txtName.Value = oNode.Name;
                this.txtPersonNum.Value = Convert.ToString(oNode.PersonNum);
                this.txtPersonMngID.Value = oNode.PersonMng == null ? "" : oNode.PersonMng.Id;
                this.txtPersonMngName.Value = oNode.PersonMng == null ? "" : oNode.PersonMng.Name;
            }
            else
            {
                sMsg = string.Format("未找到【{0}】节点，无法显示", this.trvOrg.SelectedNode.Text);
            }
        }
        catch (Exception ex)
        {
            sMsg = string.Format("显示失败:{0}", ex.Message);
        }
        this.hdOperationState.Value = "0";
        MessageBox(sMsg);
    }
    
    public void AddCompanyInfo()
    {
        string sMsg = string.Empty;
        try
        {
            CompanyInfo oParent = GlobalClass.CompanyInfoSvr.GetCompanyInfoById(this.trvOrg.SelectedNode.Value);
            //OperationOrg oParent = ResourceSvr.OperationOrgService.GetOperationOrgById(this.trvOrg.SelectedNode.Value);
            if (oParent != null)
            {
                CompanyInfo oChild = new CompanyInfo();
                oChild.ParentNode = oParent;
                oChild.TheTree = oParent.TheTree;
                oChild.Address = this.txtAddress.Value;
                oChild.Code = this.txtCode.Value;
                oChild.Name = this.txtName.Value;
                int temp = 0;
                int.TryParse(this.txtPersonNum.Value, out temp);
                oChild.PersonNum =  temp;
                oChild.PersonMng = string.IsNullOrEmpty(this.txtPersonMngID.Value.Trim()) ? null : GlobalClass.CompanyInfoSvr.QueryById(typeof(PersonInfo), this.txtPersonMngID.Value.Trim()) as PersonInfo;
                oChild = GlobalClass.CompanyInfoSvr.SaveOrUpdate(oChild);
                TreeNode oNode = new TreeNode();
                oNode.Text = oChild.Name;
                oNode.Value = oChild.Id;
                oNode.Target = oChild.OrderNo.ToString();
                this.trvOrg.SelectedNode.ChildNodes.AddAt(this.trvOrg.SelectedNode.ChildNodes.Count, oNode);
                sMsg = "添加成功";
             
            }
            else
            {
                sMsg = string.Format("未找到【{0}】节点的父节点，无法添加", this.trvOrg.SelectedNode.Text);
            }
        }
        catch (Exception ex)
        {
            sMsg = string.Format("添加失败:{0}", ex.Message);
        }
        MessageBox(sMsg);
    }
    public void UpdateCompanyInfo()
    {
        string sMsg = string.Empty;
        try
        {
            CompanyInfo oChild = GlobalClass.CompanyInfoSvr.GetCompanyInfoById(this.trvOrg.SelectedNode.Value);
            //OperationOrg oChild = ResourceSvr.OperationOrgService.GetOperationOrgById(this.trvOrg.SelectedNode.Value);
            if (oChild != null)
            {
                oChild.Address = this.txtAddress.Value;
                oChild.Code = this.txtCode.Value;
                oChild.Name = this.txtName.Value;
                int temp = 0;
                 int.TryParse(this.txtPersonNum.Value,out temp);
                oChild.PersonNum = temp;
                oChild.PersonMng = string.IsNullOrEmpty(this.txtPersonMngID.Value.Trim()) ? null : GlobalClass.CompanyInfoSvr.QueryById(typeof(PersonInfo), this.txtPersonMngID.Value.Trim()) as PersonInfo;
                oChild = GlobalClass.CompanyInfoSvr.SaveOrUpdate(oChild);
                this.trvOrg.SelectedNode.Value = oChild.Id;
                this.trvOrg.SelectedNode.Text = oChild.Name;
                this.trvOrg.SelectedNode.Target = oChild.OrderNo.ToString();
               
                sMsg = "保存成功";
            }
            else
            {
                sMsg = "找不到该节点";
            }
        }
        catch (Exception ex)
        {
            sMsg = string.Format("保存出错：{0}", ex.Message);
        }
        MessageBox(sMsg);
    }
    public void btnDel_Click(object sender, EventArgs e)
    {
        string sMsg = string.Empty;
        try
        {
            if (this.trvOrg.SelectedNode != null)
            {
                if (this.trvOrg.SelectedNode.Parent != null)
                {
                    CompanyInfo oChild = GlobalClass.CompanyInfoSvr.GetCompanyInfoById(this.trvOrg.SelectedNode.Value);
                    if (GlobalClass.CompanyInfoSvr.DeleteCompanyInfoByID(this.trvOrg.SelectedNode.Value))
                    {

                        this.trvOrg.SelectedNode.Parent.ChildNodes.Remove(this.trvOrg.SelectedNode);
                        sMsg = "删除成功";
                        if (this.trvOrg.Nodes.Count > 0)
                        {
                            this.trvOrg.Nodes[0].Select();
                            this.ShowCompanyInfo();
                        }

                       // PublicClass.WriteLog(oChild.Id, "培训场所", oChild.Code, "删除培训场所", Session, "[删除]");
                    }
                    else
                    {
                        sMsg = "删除失败";
                    }
                }
                else
                {
                    sMsg = "请不要删除此节点";
                }
            }
            else
            {
                sMsg = "请选择组织节点";
            }
        }
        catch (Exception ex)
        {
            sMsg = ex.Message;
        }
        MessageBox(sMsg);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string sOperationState = this.hdOperationState.Value;
        switch (sOperationState)
        {
            case "0"://显示
                {
                    UpdateCompanyInfo();
                    break;
                }
            case "1":  //修改
                {
                    UpdateCompanyInfo();
                    break;
                }
            case "2"://添加
                {
                    this.AddCompanyInfo();
                    break;
                }

        }
    }
}
