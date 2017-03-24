<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test5.aspx.cs" Inherits="test_test5" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
 <head id="Head1" runat="server">
    <title>职位申请记录</title>
    <style type="text/css">
        body,form{  margin:0px 0px; padding:0px 0px; width:100%; height:100%; overflow:hidden;}
    </style>
    <link rel="stylesheet" href="../JavaScript/jquery-ui-1.11.4/jquery-ui.min.css" />
    <link rel="stylesheet" href="../JavaScript/jquery-ui-1.11.4/jquery-ui.theme.min.css" />
    <script src="../JavaScript/jquery-ui-1.11.4/external/jquery/jquery.js"></script>
    <script src="../JavaScript/jquery-ui-1.11.4/jquery-ui.min.js"></script>
    <link rel="stylesheet" href="../JavaScript/jquery-ui-1.11.4/jquery-ui.structure.min.css">
         <script type="text/javascript" src="../JavaScript/upFile.js"></script>
 <script type="text/javascript">
  function btnImportExcel(){
      var oBtnImport=$('#<%=btnClick.ClientID %>');
      var oFilePath=$('#<%=txtFilePath.ClientID %>');
      var  oUpFile=new upFile({
                                 name:"excel",
                                 sure:function(data ){   oFilePath.val(data.FilePath);oBtnImport.click();}
                              });
     oUpFile.open();
  }
 </script>
   </head>
<body>  
    <form id="form1" runat="server">
        <div>
            <asp:TextBox runat="server" ID="txtFilePath"></asp:TextBox>
            <asp:Button runat="server" ID="btnClick" Text="点击"  OnClick="btnClick_Click"/><br />
            <input type="button" value="上传Excel" onclick="btnImportExcel()" />
        </div>
    </form>
 
</body>
</html>
