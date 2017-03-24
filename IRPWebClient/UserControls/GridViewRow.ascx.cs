using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Resource_GridViewRow : System.Web.UI.UserControl
{
    public string DataSourceGridName
    {
        get { return this.hdControlName.Value; }
        set { this.hdControlName.Value = value; }
    }
    public GridView DataSourceGrid
    {
        get {
            string sControlID = DataSourceGridName;
            GridView oGridView = null;
            try
            {
                oGridView =Page. FindControl(sControlID) as GridView;
            }
            catch
            {
                
            }
            return oGridView;
             
        }
        set 
        {
            if (value != null)
            {
                DataSourceGridName = value.ID;
            }
        }
    }
    public bool IsCallBack
    {
        get { return bool.Parse(this.hdIsCallBack.Value); }
        set { hdIsCallBack.Value = (value ? "1" : "0"); }
    }
    public  event PublicClass.SelectedIndexChanged   SelectedIndexChanged;
     
    protected void Page_Load(object sender, EventArgs e)
    {
        GridView oGridView = DataSourceGrid;
        string sScript = string.Empty;
      //  string sCtlName = Request.Form["__EVENTTARGET"];
       
        //if (oGridView.Rows.Count > 0)
        //{
        //    sScript = "<script>InitalGridView(); </script>";
        //    MessageBox(sScript);
        //}
        //if (oGridView.Rows.Count > 0)
        //{
        //    this.hdSelectRowIndex.Value = oGridView.SelectedIndex.ToString();
        //}
        if (hdButton.Value == "0")
        {
            hdSelectRowIndex.Value = DataSourceGrid.SelectedIndex.ToString();
        }
        if (!IsPostBack)
        {
            hdButton.Value = "0";
            //if (oGridView != null)
            //{
            //    int index = oGridView.SelectedIndex;
            //    if (index > -1 && index < oGridView.Rows.Count)
            //    {
            //        hdSelectRowIndex.Value = index.ToString();
            //          sScript = string.Format("<script>SelectRowByIndexStyle('{0},{1}'); </script>", oGridView.ClientID, index);
            //        MessageBox(sScript);
            //    }
               
            //}
        }
     
        
    }
     
    protected void btnClick_Click(object sender, EventArgs e)
    {
        
        int index = -1;
        if (this.SelectedIndexChanged  != null)
        {
            try
            {
                hdButton.Value = "0";
                  index = int.Parse(hdSelectRowIndex.Value);
                if (index > -1&&index<DataSourceGrid.Rows.Count)
                {
                    DataSourceGrid.SelectedIndex = index;
                    
                }

                this.SelectedIndexChanged(DataSourceGrid, e);
                //string sScript = string.Format("<script>SelectRowByIndexStyle('{0},{1}'); </script>", DataSourceGrid.ClientID, index);
            }
            catch
            {
            }
        }
    }
    public void MessageBox(string sScript)
    {
        if (!string.IsNullOrEmpty(sScript))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), DateTime.Now.ToLongTimeString(), sScript);
        }
    }
}
