using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DefaultHeader : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnEnter_Click(object sender, EventArgs e)
    {

        string groupName = listGroupJob.SelectedValue;

        lblCurrJobName.Text = groupName;
        Page.ClientScript.RegisterStartupScript(this.GetType(), ClientID, "<script type='text/javascript'>document.getElementById('popupContactClose').click();</script>");//window.location.replace('" + txtCurrLocation.Value + "');
    }
}
