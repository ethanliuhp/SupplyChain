using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Main_Botton : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtPerson.Text = PublicClass.GetUseInfo(Session).CurrentPerson.Name;
            this.txtOperationJob.Text = PublicClass.GetUseInfo(Session).CurrentJob.Name;
            this.txtOperationOrg.Text = PublicClass.GetUseInfo(Session).CurrentOrg.Name;
            //if (PublicClass.GetUseInfo(Session).CurrentAccOrg != null)
            //{
            //    this.txtOperationAccOrg.Text = PublicClass.GetUseInfo(Session).CurrentAccOrg.Name;
            //}
            this.txtTime.Text = DateTime.Now.ToString();
        }
    }
}
