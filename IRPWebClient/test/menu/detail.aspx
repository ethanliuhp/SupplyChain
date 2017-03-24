<%@ Page Language="C#" AutoEventWireup="true" CodeFile="detail.aspx.cs" Inherits="test_menu_detail" %>
<%@ Register Src="~/UserControls/GridViewDataMng.ascx" TagName="GridViewDataMng" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
       
   <link rel="stylesheet" href="../../JavaScript/jquery-ui-1.11.4/jquery-ui.min.css">
    <link rel="stylesheet" href="../../JavaScript/jquery-ui-1.11.4/jquery-ui.theme.min.css">
    <script src="../../JavaScript/jquery-ui-1.11.4/external/jquery/jquery.js"></script>
    <script src="../../JavaScript/jquery-ui-1.11.4/jquery-ui.min.js"></script>
    <link rel="stylesheet" href="../../JavaScript/jquery-ui-1.11.4/jquery-ui.structure.min.css">
         <script src="../../JavaScript/Util.js"></script>
        <script type="text/javascript" src="../../JavaScript/upFile.js"></script>
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
            <asp:Button runat="server" ID="btnClick" Text="点击"  OnClick="btnImportExcel_Click"/><br />
            <input type="button" value="上传Excel" onclick="btnImportExcel()" />
        </div>
         <asp:GridView ID="gvManageCost" runat="server" AutoGenerateColumns="false"    Width="99.5%" Height="100%"  >
                                   
                                  <Columns>
                                      <asp:BoundField HeaderText="ID"   Visible="false"  ></asp:BoundField>
                                      <asp:BoundField HeaderText="序号" HeaderStyle-Width="40"    ></asp:BoundField>
                                      <asp:BoundField HeaderText="会计科目ID"   DataField="AccountTitle"  Visible="false"  ></asp:BoundField>
                                      <asp:BoundField HeaderText="费用类别"    DataField="AccountTitleName" ></asp:BoundField>
                                      <asp:BoundField HeaderText="所属部门ID"   DataField="OrgInfo"  Visible="false"  ></asp:BoundField>
                                      <asp:BoundField HeaderText="所属部门"    DataField="OrgInfoName" ></asp:BoundField>
                                      <asp:BoundField HeaderText="预算金额(元)"    DataField="BudgetMoney"  DataFormatString="{0:N2}"></asp:BoundField>
                                      <asp:BoundField HeaderText="实际支出(元)"    DataField="Money" DataFormatString="{0:N2}"></asp:BoundField>
                                      <asp:BoundField HeaderText="比率(%)"    DataField="Rate" DataFormatString="{0:F2}" ></asp:BoundField>
                                      <asp:BoundField HeaderText="备注"    DataField="Descript" ></asp:BoundField>
                                   </Columns>
                             </asp:GridView>
                             <uc1:GridViewDataMng ID="gvManageCostSource" runat="server"  /> 
    </form>
 
</body>
</html>
