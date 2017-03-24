<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Auth_MenuMsg.aspx.cs" Inherits="AuthMsg_Auth_MenuMsg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>菜单维护</title>
    <link href="../css/MainWeb.css" rel="Stylesheet" type="text/css" />
    <link href="../css/xforms.css" rel="stylesheet" type="text/css" />
    <link href="../css/system.icon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #form1
        {
            font-size: 9pt;
            width: 99%;
        }
        .MainDiv
        {
            width: 100%;
            height: 95%;
            border-width: 0px;
            margin: 5px;
            text-align: left;
        }
        .LeftDiv
        {
            width: 29.6%;
            height: 520px;
            margin: 0px;
            padding: 0px;
            top: 0px;
            border: solid 1px #D3D3D3;
            float: left;
            overflow: auto;
            text-align: left;
        }
        .RightDiv
        {
            width: 70%;
            height: 520px;
            float: right;
            margin: 0px;
            padding: 0px;
            top: 0px;
            border: solid 1px #D3D3D3;
            text-align: left;
            border-left-width: 0px;
        }
        .OperateDiv
        {
            width: 100%;
            height: 25px;
            clear: both;
            margin: 0px;
            padding: 0px;
        }
        .MenuDiv
        {
            width: 100%;
            height: 80%;
            clear: both;
            top: 0px;
            margin: 0px;
            padding: 0px;
            border-width: 0px;
            overflow: auto;
            text-align: left;
            border-bottom: solid 1px #D3D3D3;
        }
        .EditMenuDiv
        {
            width: 100%;
            height: 20%;
            clear: both;
            top: 0px;
            margin: 0px;
            padding: 0px;
            border-width: 0px;
            text-align: left;
        }
        .menu
        {
            width: 100%;
            top: 0px;
            padding: 0px;
            margin: 0px;
        }
        .Operation
        {
            width: 100%;
            height: 50%;
            padding: 0px;
            margin: 0px;
            text-align: center;
        }
        .btn
        {
            width: 100px;
            height: 95%;
            margin-right: 10px;
        }
        .Info
        {
            width: 100%;
            height: 100%;
            padding: 0px;
            margin: 0px;
            text-align: left;
        }
        .table
        {
            width: 100%;
            margin: 0px auto;
            padding: 0px;
        }
        .table, .tr
        {
            padding: 0px;
            margin: 0px;
        }
        .table td
        {
            padding: 5px;
            margin: 0px;
        }
        .label
        {
            width: 5%;
            height: 100%;
            padding: 0px;
            margin: 0px;
            text-align: right;
        }
        .text
        {
            width: 40%;
            height: 100%;
            padding: 0px;
            margin: 0px;
            text-align: left;
        }
        .textbox
        {
            width: 95%;
        }
    </style>

    <script type="text/javascript" src="../JavaScript/jquery-1.10.2.js"></script>

    <script src="<%=ResolveUrl("../JavaScript/progressLoader.js")%>" type="text/javascript"></script>

    <script type="text/javascript">

        $(document).ready(function() {
            $("#btnAdd").click(function() {

                $("#<%=hdState.ClientID %>").val("1")
                $(".table input:text").val("");
            });
            $("btnUpdate").click(function() {
                $("#<%=hdState.ClientID %>").val("0")
            });
            var treeName = "<%=trvMenu.ClientID %>";

            loadProgress();

        });
        //        function btnDelete() {
        //            return confirm("确定删除此节点");
        //        }
        var sSelectMenuDivIDHdID = "<%=hdSelectMenuDivID.ClientID %>";
        function postBackByObject() {

            var o = window.event.srcElement;

            //debugger;

            if ($(o).parent("td").length > 0) {
                var oA = $(o).closest('table').find("tr:last td:last a:last");
                if (oA.length > 0) {
                    var sAId = oA.attr("id");
                    $("#" + sSelectMenuDivIDHdID).val(sAId);
                }

            }


            if (o.tagName == "INPUT" && o.type == "checkbox") {

                // __doPostBack("", "");
                //alert("checkbox");

                //设置子checkbox

                var childDiv = $(o).closest("table").next("div");
                if (childDiv != null && childDiv.length == 1) {
                    childDiv.find("INPUT:checkbox").each(function() { this.checked = o.checked; });

                }

                //设置父checkbox

                setTreeParentChecked(o);

                return false;

            }
            else {

                return true;
            }

        }
        function setTreeParent(treeName, oCurrent) {

            var treeName = "<%=trvMenu.ClientID %>";
            var oCloseDiv = $(oCurrent).closest("table").parent("div[id^='" + treeName + "'][id!='" + treeName + "']");

            if (oCloseDiv != null && oCloseDiv.length == 1) {
                var oParentTable = oCloseDiv.prev("table");
                if (oParentTable != null && oParentTable.length == 1) {
                    var oParent = oParentTable.find("INPUT:checkbox");
                    if (oParent != null && oParent.length == 1) {
                        if (oParent.get(0).checked != oCurrent.checked) {
                            oParent.get(0).checked = oCurrent.checked;
                            setTreeParent(oParent.get(0));
                        }
                    }
                }
            }
        }

        function setTreeParentChecked(optCheckBox) {

            var divCurrent = $(optCheckBox).parents("div").get(0); //所在节点div
            var parentNode = divCurrent.previousSibling; //父节点所在table

            if (parentNode) {
                var parentNodeCheckBox = $(parentNode).find("INPUT:checkbox").get(0);
                if (parentNodeCheckBox != undefined && parentNodeCheckBox != NaN && parentNodeCheckBox != null)
                    if (optCheckBox.checked) {
                    parentNodeCheckBox.checked = optCheckBox.checked;
                    setTreeParentChecked(parentNodeCheckBox);
                }
                else {
                    var flag = false;
                    var childNodeCheckBoxs = $(divCurrent).find("INPUT:checkbox");
                    for (var i = 0; i < childNodeCheckBoxs.length; i++) {
                        var childCheckBox = childNodeCheckBoxs[i];
                        if (childCheckBox != optCheckBox && childCheckBox.checked) {
                            break;
                        }
                        if (i == childNodeCheckBoxs.length - 1)
                            flag = true;
                    }
                    if (flag) {
                        parentNodeCheckBox.checked = optCheckBox.checked;
                        setTreeParentChecked(parentNodeCheckBox);
                    }
                }
            }

        }
        function DeleteNode() {

            var sAId = $("#" + sSelectMenuDivIDHdID).val();
            var oNode = $("#" + sAId);

            if (oNode.length > 0) {

                var oDiv = oNode.closest("table").next("div");
                if (oDiv.length > 0) {
                    alert("[" + oNode.text() + "]节点下面存在子菜单，无法删除。建议在清除该节点之前，清除它下面的子菜单");
                    return false;
                }
                else {
                    return confirm("你确定删除[" + oNode.text() + "]节点吗");

                    // return confirm("你确定删除[" + oNode.text() + "]节点吗");
                }
            }
            else {
                alert("请选择节点");
                return false;
            }

        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="MainDiv">
        <div class="LeftDiv">
            <asp:TreeView ID="trvGroup" runat="server" ShowLines="True" ExpandDepth="1" EnableViewState="true"
                CssClass="tree" ForeColor="Black" NoExpandImageUrl="~/images/tree/plus3.gif" CollapseImageUrl="~/images/tree/plus3.gif"
                ExpandImageUrl="~/images/tree/minus2.gif" NodeStyle-ImageUrl="~/images/tree/folderClosed.gif"
                LeafNodeStyle-ImageUrl="~/images/tree/leaf.gif" SelectedNodeStyle-ImageUrl="~/images/tree/folderOpen.gif"
                OnSelectedNodeChanged="trvGroup_SelectedNodeChanged">
                <SelectedNodeStyle  ForeColor="#FFFFFF" />
            </asp:TreeView>
        </div>
        <div class="RightDiv">
            <div class="MenuDiv">
                <div class="OperateDiv">
                    <table>
                        <tr>
                            <td>
                                <table cellpadding="0" cellspacing="0" class="button-blue xforms-control xforms-trigger xforms-appearance-minimal xui-button ">
                                    <tbody>
                                        <tr>
                                            <td class="xxf-value">
                                                <button class="xui-button-label xforms-trigger-image" type="button" id="btnAdd" style="width: 105px">
                                                    <i class="icon icon-system-plus"></i><span class="xforms-label">新增子节点</span>
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
                                                <button class="xui-button-label xforms-trigger-image" type="button" id="btnUpdate">
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
                                <table cellpadding="0" cellspacing="0" class="button-blue xforms-control xforms-trigger xforms-appearance-minimal xui-button ">
                                    <tbody>
                                        <tr>
                                            <td class="xxf-value">
                                                <button class="xui-button-label xforms-trigger-image" type="button" onclick="$('#<%=btnMoveUp.ClientID %>').click();">
                                                    <i class="icon icon-system-up"></i><span class="xforms-label ">上移</span>
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
                                                <button class="xui-button-label xforms-trigger-image" type="button" onclick="$('#<%=btnMoveDown.ClientID %>').click();">
                                                    <i class="icon icon-system-down"></i><span class="xforms-label ">下移</span>
                                                </button>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div class="menu">
                    <asp:TreeView ID="trvMenu" runat="server" ShowLines="True" ExpandDepth="1" EnableViewState="true"
                        CssClass="tree" ForeColor="Black" NoExpandImageUrl="~/images/tree/plus3.gif" CollapseImageUrl="~/images/tree/plus3.gif"
                        ExpandImageUrl="~/images/tree/minus2.gif" NodeStyle-ImageUrl="~/images/tree/folderClosed.gif"
                        LeafNodeStyle-ImageUrl="~/images/tree/leaf.gif" SelectedNodeStyle-ImageUrl="~/images/tree/folderOpen.gif"
                        OnSelectedNodeChanged="trvMenu_SelectedNodeChanged">
                        <SelectedNodeStyle BackColor="#000080" ForeColor="#FFFFFF" />
                    </asp:TreeView>
                </div>
            </div>
            <div class="EditMenuDiv">
                <%-- <div class="Operation">
                    <input type="button" id="btnAdd" value="增加子节点"  class="btn"/>
                     <input type="button" value="修改" id="btnUpdate"  class="btn"/>
                    <asp:Button runat="server" Text="保存" ID="btnSave" class="btn" 
                        onclick="btnSave_Click" />
                </div>--%>
                <div class="Info">
                    <table class="table" cellpadding="0" cellspacing="0">
                        <tr class="tr">
                            <td class="label">
                                名称:
                            </td>
                            <td class="text">
                                <asp:TextBox ID="txtName" runat="server" class="textbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="tr">
                            <td class="label">
                                页面:
                            </td>
                            <td class="text">
                                <asp:TextBox ID="txtPagePath" runat="server" class="textbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="tr" style="display: none">
                            <td class="label">
                                编码
                            </td>
                            <td class="text" colspan="3">
                                <asp:TextBox ID="txtCode" runat="server" class="textbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <table cellpadding="0" cellspacing="0" class="button-blue xforms-control xforms-trigger xforms-appearance-minimal xui-button ">
                                    <tbody>
                                        <tr>
                                            <td class="xxf-value">
                                                <button class="xui-button-label xforms-trigger-image" style="width: 90px" type="button" onclick="$('#<%=Button3.ClientID %>').click();">
                                                    <i class="icon icon-system-ok"></i><span class="xforms-label ">保存修改</span>
                                                </button>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table cellpadding="0" cellspacing="0" class="button-blue xforms-control xforms-trigger xforms-appearance-minimal xui-button ">
                                    <tbody>
                                        <tr>
                                            <td class="xxf-value">
                                                <button class="xui-button-label xforms-trigger-image" type="button" onclick="$('#<%=btnSaveLink.ClientID %>').click();"
                                                    style="width: 90px">
                                                    <i class="icon icon-system-ok"></i><span class="xforms-label ">保存关联 </span>
                                                </button>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        
                    </table>
                </div>
                <div class="Operation" style="display: none">
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField Value="0" ID="hdState" runat="server" />
    <input type="hidden" value="" id="hdSelectMenuDivID" runat="server" />
    <div style="display: none" id="hidden">
        <asp:Button ID="btnDelete" Text="删除" runat="server" OnClientClick="return confirm('你确定删除节点吗');"
            CssClass="CommonButton" OnClick="btnDelete_Click" />
        <asp:Button ID="btnSaveLink" Text="保存关联" runat="server" OnClick="btnSaveLink_Click"
            CssClass="CommonButton" />
        &nbsp;&nbsp;
        <asp:Button ID="btnMoveUp" Text="上移" runat="server" OnClick="btnMoveUp_Click" CssClass="CommonButton" />
        &nbsp;&nbsp;
        <asp:Button ID="btnMoveDown" Text="下移" runat="server" OnClick="btnMoveDown_Click"
            CssClass="CommonButton" />
        <asp:Button runat="server" Text="保存" ID="Button3" CssClass="CommonButton" OnClick="btnSave_Click" />
    </div>
    </form>
</body>
</html>
