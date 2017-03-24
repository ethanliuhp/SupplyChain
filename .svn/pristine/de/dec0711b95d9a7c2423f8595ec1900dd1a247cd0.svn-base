<%@ Page Language="C#" AutoEventWireup="true" CodeFile="master.aspx.cs" Inherits="test_menu_master" %>
<%@ Register Src="~/UserControls/ListSource.ascx" TagName="ListSource" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
       
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divMain" style=" padding:0px; margin:0px; text-align:center; ">    
        <div id="divLst">
             <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" Width="100%" Height="100%" CssClass="grid">
                  <AlternatingRowStyle BackColor="#F7FAFF" />
                  <Columns>
                              <asp:BoundField DataField="Code" HeaderText="编号">
                                <ItemStyle CssClass="colContent_sen" />
                            </asp:BoundField>
                           
                            <asp:BoundField DataField="CreatePersonName" HeaderText="制单人">
                                <ItemStyle CssClass="colContent_sen" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CreateDate" HeaderText="业务时间">
                                <ItemStyle CssClass="colContent_sen" />
                            </asp:BoundField>
                             <asp:BoundField DataField="Year" HeaderText="年">
                                <ItemStyle CssClass="colContent_sen" />
                            </asp:BoundField>
                             <asp:BoundField DataField="Month" HeaderText="月">
                                <ItemStyle CssClass="colContent_sen" />
                            </asp:BoundField>
                            
                           
                            
                            <asp:TemplateField HeaderText="状态">
                                <ItemTemplate>
                                    <asp:Label ID="lblState" runat="server" Text='<%#  Eval("State") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="colContent_sen" />
                            </asp:TemplateField>
                        </Columns>
             </asp:GridView>
        </div>
         <div id="divPageSize"     >
             <uc1:ListSource ID="GridViewSource1" runat="server" DataSQL="select    rownum num , t.* from THD_IndirectCostMaster t"  DestGridViewControlName="GridView1" IsSelect="true" PageSize="2" />
        </div>
                   
    </div>
    </form>
</body>
</html>
