<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentOrderMaster.aspx.cs" Inherits="PaymentOrder_PaymentOrderMaster" %>

<%@ Register Src="~/UserControls/GridViewSource.ascx" TagName="GridViewSource" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/MainWeb.css" rel="Stylesheet" type="text/css" />
    <link href="../css/xforms.css" rel="stylesheet" type="text/css" />
    <link href="../css/system.icon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .table
        {
            width: 100%;
            height:auto;
            padding:0px;
            margin:0px;
        }
        .tdTitle
        {
        	width:15%;
        	height:30px;
        	text-align:right;
        }
        .tdContent
        {
        	width:30%;
        	height:30px;
        	text-align:left;
        }
        .tdEmpty
        {
        	width:10%;
        	height:30px;
        	text-align:right;
        }
        .style2
        {
            width: 63px;
        }
        .style3
        {
            width: 82px;
        }
        .style4
        {
            width: 280px;
        }
        .style5
        {
            width: 350px;
        }
        .style6
        {
            width: 84px;
        }
        .style7
        {
            width: 198px;
        }
        .style8
        {
            width: 74px;
        }
        .style11
        {
            width: 129px;
        }
        .dialog
        {
            display: block;
        }
        .dialog1
        {
            display: none;
        }
        .style13
        {
            width: 196px;
        }
        .style14
        {
            width: 60px;
        }
        .style15
        {
            width: 110px;
        }
        .style16
        {
            width: 55px;
        }
    </style>

    <script src="<%=ResolveUrl("../JavaScript/progressLoader.js")%>" type="text/javascript"></script>

    <script src="<%=ResolveUrl("../JavaScript/jquery-1.10.2-vsdoc.js")%>" type="text/javascript"></script>

    <script src="<%=ResolveUrl("../JavaScript/jquery-1.10.2.js")%>" type="text/javascript"></script>

    <script src="<%=ResolveUrl("../JavaScript/My97DatePicker/WdatePicker.js")%>" type="text/javascript"></script>

    <script src="<%=ResolveUrl("../JavaScript/GetResource.js")%>" type="text/javascript"></script>

    <%--弹出层js css --%>

    <script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.core.js")%>" type="text/javascript"></script>

    <script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.widget.js")%>" type="text/javascript"></script>

    <script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.button.js")%>" type="text/javascript"></script>

    <script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.mouse.js")%>" type="text/javascript"></script>

    <script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.draggable.js")%>" type="text/javascript"></script>

    <script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.resizable.js")%>" type="text/javascript"></script>

    <script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.position.js")%>" type="text/javascript"></script>

    <script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.dialog.js")%>" type="text/javascript"></script>

    <link href="<%=ResolveUrl("../css/jquery-ui[1].css")%>" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        $(document).ready(function() {
            loadProgress();
            $("#frameDetails").dialog({
                open: function() {
                    $("#frameDetails").addClass("dialog");
                },
                autoOpen: false,
                width: 1020,
                height: 450,
                modal: true,
                bgiframe: true,
                zIndex: 900,
                close: function() {
                    $("#frameDetails").removeClass("dialog");
                    $("#<%=btnSearch.ClientID %>").click();
                }
            });

            $("#<%=divAddNew.ClientID %>").dialog({
                open: function() {
                    $("#<%=divAddNew.ClientID %>").addClass("dialog");
                },
                autoOpen: document.getElementById("<%=checkBox.ClientID %>").checked,
                width: 980,
                modal: true,
                bgiframe: true,
                zIndex: 990,
                buttons: {
                    "保存": function() {
                        $("#<%=divAddNew.ClientID %>").appendTo('form:first');
                        $("#<%=btnSave.ClientID %>").click();
                        $(this).dialog("close");
                    },
                    "取消": function() {
                        $("#<%=btnCanel.ClientID %>").click();
                        $(this).dialog("close");

                    }
                },
                close: function() {
                    $("#<%=divAddNew.ClientID %>").removeClass("dialog");
                    $("#<%=divAddNew.ClientID %>").addClass("dialog1");
                    document.getElementById("<%=checkBox.ClientID %>").checked = false;
                }
            });
        })

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
        };

        function CheckDetails(gridName) {
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
                    alert("请先选择要查看的行！");
                    return false;
                }
                else if (count > 1) {
                    alert("一次只能查看一条记录！");
                    return false;
                }
                else {
                    document.getElementById("txtIdHidden").value = value;
                    var url = "PaymentOrderDetail.aspx?Id=" + value;
                    $("#frameDetails1").attr("src", url);
                    $("#frameDetails").dialog("open");
                    return false;
                }
            }
            else {
                alert("当前没有要查看的记录！");
                return false;
            }
            return false;
        }

        function GetPersonInfo(flag) {
            var vReturnVlaues = GetPerson("true");
            if (vReturnVlaues != null) {
                if (flag == 1) {
                    document.getElementById("<%=txtPayeeId.ClientID %>").value = vReturnVlaues[0].ID;
                    document.getElementById("<%=txtPayeeName.ClientID %>").value = vReturnVlaues[0].Name;
                }
               
            }
            return false;
        }

        function CheckSubmit() {
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
                    alert("请先选择要提交的行！");
                    return false;
                }
                else {
                    idList = idList.substr(0, idList.length - 1);
                    document.getElementById("<%=txtIdListHidden.ClientID %>").value = idList;
                }
            }
            else {
                alert("当前没有要删除的记录！");
                return false;
            }

        };
         
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <fieldset>
                <legend>付款单查询</legend>
                <table class=" table" style="width: 100%; white-space: nowrap;">
                    <tr>
                        <td class=" tdTitle">
                           付款方式：
                        </td>
                        <td class=" tdContent">
                           <asp:DropDownList runat="server" ID="dpLstPaymentType_Search"></asp:DropDownList>
                        </td>
                        <td class="tdTitle">
                            创建时间：
                        </td>
                        <td class="tdContent">
                            <asp:TextBox ID="txtBeginDate_Search" runat="server" onfocus="new WdatePicker(this,'%Y-%M-%D',true,'whyGreen')"></asp:TextBox>
                            至
                            <asp:TextBox ID="txtEndDate_Search" runat="server" onfocus="new WdatePicker(this,'%Y-%M-%D',true,'whyGreen')"></asp:TextBox>
                        </td>
                         
                    </tr>
                    <tr>
                       
                        <td class=" tdTitle">
                            收款方：
                        </td>
                        <td class=" tdContent">
                            <asp:TextBox ID="txtPayee_Search" runat="server"></asp:TextBox>
                        </td>
                         <td class="tdTitle">
                            <table cellpadding="0" cellspacing="0" class="button-blue xforms-control xforms-trigger xforms-appearance-minimal xui-button ">
                                <tbody>
                                    <tr>
                                        <td class="xxf-value">
                                            <button class="xui-button-label xforms-trigger-image" type="button" onclick="$('#<%=btnSearch.ClientID %>').click();">
                                                <i class="icon icon-system-search"></i><span class="xforms-label ">查询 </span>
                                            </button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td class=" tdContent">
                             
                        </td>
                         
                    </tr>
                </table>
            </fieldset>
        </div>
           <div id="divOprBtn" style="width: 98%; text-align: left; padding-top: 10px; padding-left: 10px; padding-bottom: 10px">
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
                        <td style="padding-left: 5px;">
                            <table cellpadding="0" cellspacing="0" class="button-yellow xforms-control xforms-trigger xforms-appearance-minimal xui-button ">
                                <tbody>
                                    <tr>
                                        <td class="xxf-value">
                                            <button class="xui-button-label xforms-trigger-image" type="button" onclick="$('#<%=btnSubmit.ClientID %>').click();">
                                                <i class="icon icon-system-up"></i><span class="xforms-label ">提交 </span>
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
                                            <button class="xui-button-label xforms-trigger-image" type="button" style="width: 90px;"
                                                onclick="$('#<%=btnCollectionOrderDetail.ClientID %>').click();">
                                                <i class="icon icon-system-th-list"></i><span class="xforms-label ">付款明细 </span>
                                            </button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        <div>
            <fieldset>
                <legend>付款单列表</legend>
                <div>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" Width="100%" CssClass="grid">
                        <AlternatingRowStyle BackColor="#F7FAFF" />
                        <Columns>
                            <asp:TemplateField HeaderText="选择">
                                <ItemTemplate>
                                    <input id="chkSelect" type="checkbox" runat="server" /><input type="hidden" id="hidden"
                                        value="<%#Eval("Id") %>" />
                                </ItemTemplate>
                                <ItemStyle CssClass="colSelect" />
                            </asp:TemplateField>
                           
                            <asp:BoundField DataField="PayeeName" HeaderText="收款方">
                                <ItemStyle CssClass="colContent_sen" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Money" HeaderText="付款金额">
                                <ItemStyle CssClass="colContent_sen" />
                            </asp:BoundField>
                             <asp:BoundField DataField="TheBankName" HeaderText="银行名称">
                                <ItemStyle CssClass="colContent_sen" />
                            </asp:BoundField>
                             <asp:BoundField DataField="TheBankCode" HeaderText="银行账号">
                                <ItemStyle CssClass="colContent_sen" />
                            </asp:BoundField>
                            
                            <asp:TemplateField   HeaderText="付款方式">
                             <ItemTemplate>
                                    <asp:Label ID="lblPaymentType" runat="server" Text='<%# Eval("PaymentType")  %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="colContent_sen" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="状态">
                                <ItemTemplate>
                                    <asp:Label ID="lblState" runat="server" Text='<%#  Eval("DocState") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="colContent_sen" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div>
                    <uc1:GridViewSource ID="GridViewSource1" runat="server" />
                </div>
            </fieldset>
        
        </div>
        <div>
        </div>
        <div id="divAddNew" style="width: 950px; display: none; " runat="server">
            <div>
                <table class="table">
                    <tr>
                        
                        <td class="tdTitle">
                            付款方式:
                        </td>
                        <td class="tdContent">
                            <asp:DropDownList ID="ddlPaymentTypeName" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td class="tdTitle">
                            收款方：
                        </td>
                        <td class="tdContent">
                            <asp:TextBox ID="txtPayeeName" runat="server" Width="97px"></asp:TextBox>
                            <table cellpadding="0" cellspacing="0" class="button-blue xforms-control xforms-trigger xforms-appearance-minimal xui-button ">
                                <tbody>
                                    <tr>
                                        <td class="xxf-value">
                                            <button class="xui-button-label xforms-trigger-image" type="button" onclick="return GetPersonInfo(1)">
                                                <i class="icon icon-system-check"></i><span class="xforms-label ">选择 </span>
                                            </button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <span style="display: none">
                                <asp:HiddenField ID="txtPayeeId" runat="server" />
                            </span>
                        </td>
                        <td class="tdEmpty"></td>
                      </tr>
                      <tr>
                        <td class="tdTitle">
                            银行名称:
                        </td>
                        <td class="tdContent">
                            <asp:TextBox runat="server" ID="txtTheBankName" Width="90%"></asp:TextBox>
                        </td>
                        <td class="tdTitle">
                            银行账号:
                        </td>
                        <td class="tdContent">
                            <asp:TextBox runat="server" ID="txtTheBankCode" Width="90%"></asp:TextBox>
                        </td>
                          <td class="tdEmpty"></td>
                    </tr>
                    <tr>
                        <td class="tdTitle">
                            备注:
                        </td>
                        <td class="tdContent" colspan="4">
                            <asp:TextBox ID="txtDescript" runat="server" TextMode="MultiLine" Width="99%" Height="56px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="display: none">
                <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="CommonButton" OnClick="btnSave_Click" />
                <asp:Button ID="btnCanel" runat="server" Text="取消" CssClass="CommonButton" OnClick="btnCanel_Click" />
                <asp:CheckBox ID="checkBox" runat="server" EnableViewState="true" />
            </div>
        </div>
        <div>
            <asp:HiddenField ID="txtIdHidden" runat="server" />
            <asp:HiddenField ID="txtIdListHidden" runat="server" />
        </div>
        <div id="frameDetails" title="付款单明细" style="width: 920px; display: none;" runat="server">
            <iframe id="frameDetails1" style="display: block" width="100%" height="100%" frameborder="0"
                runat="server"></iframe>
        </div>
    </div>
    <div id="divHidden" style="display: none;">
        <asp:Button ID="btnSelect" runat="server" Text="选择" CssClass="CommonButton" OnClientClick="return GetPersonInfo(2)" />
        <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="CommonButton" OnClick="btnSearch_Click" />
        <asp:Button ID="btnAddNew" runat="server" CssClass="CommonButton" Text="新增" OnClick="btnAddNew_Click" />
        <asp:Button ID="btnUpdate" runat="server" CssClass="CommonButton" Text="修改" OnClientClick="return CheckUpdate('<%=GridView1.ClientID %>')"
            OnClick="btnUpdate_Click" />
        <asp:Button ID="btnDelete" runat="server" CssClass="CommonButton" Text="删除" OnClientClick="return CheckDelete('<%=GridView1.ClientID %>')"
            OnClick="btnDelete_Click" />
        <asp:Button ID="btnCollectionOrderDetail" runat="server" CssClass="CommonButton"
            Text="付款明细" OnClientClick="return CheckDetails('<%=GridView1.ClientID %>')" />
        <asp:Button ID="btnSubmit" runat="server" CssClass="CommonButton" Text="提交" OnClick="btnSubmit_Click"
            OnClientClick="return CheckSubmit('<%=GridView1.ClientID %>')" />
        <asp:Button ID="btn" runat="server" Text="选择" OnClientClick="return GetPersonInfo(1)" />
    </div>
    </form>
</body>
</html>
