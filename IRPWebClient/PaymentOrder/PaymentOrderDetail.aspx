<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentOrderDetail.aspx.cs" Inherits="PaymentOrder_PaymentOrderDetail" %>

<%@ Register Src="../UserControls/GridViewSource.ascx" TagName="GridViewSource" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/MainWeb.css" rel="Stylesheet" type="text/css" />
    <link href="../css/xforms.css" rel="stylesheet" type="text/css" />
    <link href="../css/system.icon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 850px;
        }
        .style2
        {
        }
        .style3
        {
            width: 63px;
        }
        .style4
        {
            width: 63px;
        }
    </style>

    <script src="<%=ResolveUrl("../javascript/GetResource.js")%>" type="text/javascript"></script>

    <script src="<%=ResolveUrl("../javascript/DataType.js")%>" type="text/javascript"></script>

    <script type="text/javascript">

         window.onload = function() {
            //初始化父容器的高 使不含滚动条
            if (window.parent.document.getElementById("frameDetails")) {
                window.parent.document.getElementById("frameDetails").style.height = this.document.body.scrollHeight + 100;
            }
        };

        function CheckDelete(gridName) {
            var grid = document.getElementById("<%=GridView1.ClientID %>");
            if (grid) {
                var cbs = grid.getElementsByTagName("input");
                var idList = "";
                for (var i = 0; i < cbs.length; i++) {
                    if (cbs[i].type == "checkbox" && cbs[i].checked && cbs[i].id.indexOf("chkSelect") > -1) {
                        idList += cbs[i].nextSibling.value + "|";
                    }
                }
                if (idList == "") {
                    alert("请先选择要删除的行！");
                    return false;
                }
                else {
                    if (confirm("您确认要删除吗？")) {
                        idList = idList.substr(0, idList.length - 1);
                        document.getElementById("<%=txtIdListHidden.ClientID %>").value = idList;
                    }
                    else
                        return false;
                }
            }
            else {
                alert("当前没有要删除的记录！");
                return false;
            }

        };

        function CheckUpdate(gridName) {
            var grid = document.getElementById("<%=GridView1.ClientID %>");
            if (grid) {
                var cbs = grid.getElementsByTagName("input");
                var value = "";
                var count = 0;
                for (var i = 0; i < cbs.length; i++) {
                    if (cbs[i].type == "checkbox" && cbs[i].checked && cbs[i].id.indexOf("chkSelect") > -1) {
                        value = cbs[i].nextSibling.value;
                        count += 1;
                    }
                }
                if (count == 0) {
                    alert("请先选择要修改的行！");
                    return false;
                }
                else if (count > 1) {
                    alert("一次只能修改一条记录！");
                    return false;
                }
                else {
                    document.getElementById("<%=txtIdHidden.ClientID %>").value = value;
                }
                return true;
            }
            else {
                alert("当前没有要修改的记录！");
                return false;
            }
        }

        function IsSave() {
            var nsi = document.getElementById("<%=txtMoney.ClientID %>").value;
            if (IsFloat(nsi)) {
                return true;
            }
            else {
                alert("请输入数字类型！");
                return false
            }
        }
        function ss()
        {
            alert("2323");
        }
    </script>

 

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <fieldset style="width: 850px; text-align: center;">
            <legend>明细信息</legend>
            <div  style="width: 100%;">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" Width="800px"
                    CssClass="grid">
                    <AlternatingRowStyle BackColor="#F7FAFF" />
                    <Columns>
                        <asp:TemplateField HeaderText="选择" ItemStyle-CssClass="colSelect">
                            <ItemTemplate>
                                <input id="chkSelect" type="checkbox" runat="server" /><input type="hidden" id="hidden"
                                    value="<%#Eval("Id") %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PaymentItemName" HeaderText="付款项名称" ItemStyle-CssClass="colContent_sen" />
                        <asp:BoundField DataField="Money" HeaderText="付款金额" ItemStyle-CssClass="colContent_sen" />
                        <asp:BoundField DataField="Describe" HeaderText="备注" ItemStyle-CssClass="colContent_large" />
                    </Columns>
                </asp:GridView>s
            </div>
            <div>
                <uc1:GridViewSource ID="GridViewSource1" runat="server" />
            </div>
        </fieldset>
    </div>
    <div style="width: 98%; text-align: left; padding-top: 10px; padding-left: 10px;
        padding-bottom: 10px">
        <table>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" class="button-blue xforms-control xforms-trigger xforms-appearance-minimal xui-button ">
                        <tbody>
                            <tr>
                                <td class="xxf-value">
                                    <button class="xui-button-label xforms-trigger-image" type="button" onclick="$('#<%=btnAddNew.ClientID %>').click();">
                                        <i class="icon icon-system-plus"></i><span class="xforms-label">新增 </span>
                                    </button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
                <td style="padding-left: 5px;">
                    <table cellpadding="0" cellspacing="0" class="button-blue xforms-control xforms-trigger xforms-appearance-minimal xui-button ">
                        <tbody>
                            <tr>
                                <td class="xxf-value">
                                    <button class="xui-button-label xforms-trigger-image" type="button" onclick="$('#<%=btnUpdate.ClientID %>').click();">
                                        <i class="icon icon-system-edit-alt"></i><span class="xforms-label ">修改 </span>
                                    </button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
                <td style="padding-left: 5px;">
                    <table cellpadding="0" cellspacing="0" class="button-yellow xforms-control xforms-trigger xforms-appearance-minimal xui-button ">
                        <tbody>
                            <tr>
                                <td class="xxf-value">
                                    <button class="xui-button-label xforms-trigger-image" type="button" onclick="$('#<%=btnDelete.ClientID %>').click();">
                                        <i class="icon icon-system-trash"></i><span class="xforms-label ">删除 </span>
                                    </button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div id="divAddNew">
        <div>
            <table  style="width: 100%; white-space: nowrap;">
                <tr>
                    <td class="style3">
                        付款金额：
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="txtMoney" runat="server" onblur="javascript:IsSave()"></asp:TextBox>
                    </td>
                    <td class="style4">
                        付款项名称：
                    </td>
                    <td>
                        <asp:TextBox ID="txtPaymentItemName" runat="server"></asp:TextBox>
                       
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        备注：
                    </td>
                    <td class="style2" colspan="3">
                        <asp:TextBox ID="txtDescribe" runat="server" Height="77px" Width="548px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div style="width: 98%; text-align: left; padding-top: 10px; padding-left: 10px;
            padding-bottom: 10px">
            <table cellpadding="0" cellspacing="0" class="button-blue xforms-control xforms-trigger xforms-appearance-minimal xui-button ">
                <tbody>
                    <tr>
                        <td class="xxf-value">
                            <button class="xui-button-label xforms-trigger-image" type="button" onclick="$('#<%=btnSave.ClientID %>').click();">
                                <i class="icon icon-system-ok"></i><span class="xforms-label ">保存 </span>
                            </button>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table cellpadding="0" cellspacing="0" class="button-blue xforms-control xforms-trigger xforms-appearance-minimal xui-button ">
                <tbody>
                    <tr>
                        <td class="xxf-value">
                            <button class="xui-button-label xforms-trigger-image" type="button" onclick="$('#<%=btnCancl.ClientID %>').click();">
                                <i class="icon icon-system-cancel"></i><span class="xforms-label ">取消 </span>
                            </button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div>
        <asp:HiddenField ID="txtIdHidden" runat="server" />
        <asp:HiddenField ID="txtIdListHidden" runat="server" />
    </div>
    <div id="divHidden" style="display: none;">
        <asp:Button ID="btnAddNew" runat="server" CssClass="CommonButton" Text="新增" OnClick="btnAddNew_Click" />
        <asp:Button ID="btnUpdate" runat="server" CssClass="CommonButton" Text="修改" OnClientClick="return CheckUpdate('<%=GridView1.ClientID %>')"
            OnClick="btnUpdate_Click" />
        <asp:Button ID="btnDelete" runat="server" CssClass="CommonButton" Text="删除" OnClientClick="return CheckDelete('<%=GridView1.ClientID %>')"
            OnClick="btnDelete_Click" />
        <asp:Button ID="btnSelect" runat="server" Text="选择" CssClass="CommonButton" OnClientClick="return GetCostAccountSubject1()" />
        <asp:Button ID="btnSave" runat="server" CssClass="CommonButton" Text="保存" OnClick="btnSave_Click" />
        <asp:Button ID="btnCancl" runat="server" CssClass="CommonButton" Text="取消" />
    </div>
    </form>
</body>
</html>
