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

public partial class UpFile_UpFile : System.Web.UI.Page
{

     
   
    public  string fileExtent = string.Empty;
    public string  fileSize = string.Empty ;
    public string folderPath = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        fileExtent = Request.QueryString["fileExtent"];
        fileSize = Request.QueryString["fileSize"];
        folderPath = Request.QueryString["folderPath"];
        if (!string.IsNullOrEmpty(fileExtent))
        {
            fileExtent = UtilClass.DecodeURIComponent(fileExtent);
            fileExtent.ToUpper(); 
        }
        if (string.IsNullOrEmpty(fileSize))
        {
            fileSize = long.MaxValue.ToString();
        }
        fileExtent = "png";
        if (!string.IsNullOrEmpty(folderPath))
        {
            folderPath = UtilClass.DecodeURIComponent(folderPath);

        }
        else
        {
            folderPath = "~/temp/";
        }
    }
   
    
}
