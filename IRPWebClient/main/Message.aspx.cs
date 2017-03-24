using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using VirtualMachine.Component.Util;
using VirtualMachine.Patterns.BusinessEssence.Domain;

public partial class Main_Message : System.Web.UI.Page
{
    //ISubjectSrv model = MSubjectMng.SubjectSrv;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        //{
        //    gvMessages.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
        //    gvMessages.DataSource = model.GetStuMessages(PublicClass.GetUseInfo(Session).CurrentPerson);
        //    gvMessages.DataBind();
        //    Bind();
        //}
    }
    protected void gvMessages_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //if (e.CommandName == "message")
        //{
        //    frameMessage1.Attributes["src"] = "../StudentMng/MessagesShow.aspx?SendPersonId=" + e.CommandArgument.ToString();
        //    SendPersonId.Value = e.CommandArgument.ToString();
        //    checkBox.Checked = true;
        //}
    }
    //查看完信息后 修改状态
    protected void btnChangeState_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    string sqlStr = "update msg_Messages set MessageState= 3 where ReceivePerson ='" + PublicClass.GetUseInfo(Session).CurrentPerson.Id + "' and SendPerson='" + SendPersonId.Value + "' and MessageState= 0 ";
        //    GlabalClass.BasicDataOptrSrv.ExecuteSql(sqlStr);
        //    SendPersonId.Value = "";

        //    gvMessages.DataSource = model.GetStuMessages(PublicClass.GetUseInfo(Session).CurrentPerson);
        //    gvMessages.DataBind();
        //}
        //catch (Exception ex)
        //{
        //    SendPersonId.Value = "";
        //    Message(ex.Message);
        //}
    }
    public void Message(string sMsg)
    {
        //if (!string.IsNullOrEmpty(sMsg))
        //{
        //    string sTemp = sMsg;
        //    sTemp = string.Format("<script>alert('{0}');</script>", sTemp);
        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "", sTemp);
        //}
    }
    private void Bind()
    {
    //    string sSQL = "select  ROW_NUMBER() over (order by  t.ID) as rownum ,* from Thd_FriendlyLink t   where 1=1";
    //    System.Data.DataSet ds= GlabalClass.BasicDataOptrSrv.DataSet(sSQL);
    //    gvFriendLink.DataSource = ds;
    //    gvFriendLink.DataBind();
    }
    //public string FriendLink()
    //{
    //    //string sContext = "";
    //    //ObjectQuery oq = new ObjectQuery();
     
    //    //IList list = GlabalClass.BasicDataOptrSrv.ObjectQuery(typeof(FriendlyLink), oq);
    //    //gvFriendLink.DataSource = list;
    //    //gvFriendLink.DataBind();
    //    //if (list != null && list.Count > 0)
    //    //{
    //    //    sContext = "<ul>";
    //    //    foreach (FriendlyLink oFriendlyLink in list)
    //    //    {
    //    //        sContext += string.Format("<li > <a onclick=\"OpenLink('http://{0}')\"  href=''>{1}</a><p></p> </li>", oFriendlyLink.LinkAddress.ToLower().Replace("htpp://", ""), oFriendlyLink.LinkName);
    //    //    }
    //    //    sContext += "</ul>";
    //    //}

    //    return sContext;
    //}
}
