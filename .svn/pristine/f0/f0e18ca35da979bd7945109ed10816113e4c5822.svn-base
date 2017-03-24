<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompanyMng.aspx.cs" Inherits="CompanyMng_CompanyMng" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>公司信息管理</title>
    <link href="../css/MainWeb.css" rel="Stylesheet" type="text/css" />
    <link type="text/css" href="../css/SystemMng.css" rel="Stylesheet" />
    <link href="../css/MainWeb.css" rel="Stylesheet" type="text/css" />
    <link href="../css/xforms.css" rel="stylesheet" type="text/css" />
    <link href="../css/system.icon.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../JavaScript/jquery-1.10.2.js"></script>
   <script src="<%=ResolveUrl("../JavaScript/GetResource.js")%>" type="text/javascript"></script>
    <script src="<%=ResolveUrl("../JavaScript/progressLoader.js")%>" type="text/javascript"></script>
    <script src="<%=ResolveUrl("../javascript/DataType.js")%>" type="text/javascript"></script>

    <style type="text/css">
        .LableClass
        {
            text-align: right;
            width:8%;
        }
        .DisableTextClass
        {
            text-align: left;
            width: 99%;
            background-color: White;
        }
        .ableTextClass
        {
            text-align: left;
            width: 99%;
            background-color: White;
        }
        .btnClass
        {
            height: 23px;
            border: solid 1px #465670; /*filter: progid:DXImageTransform.Microsoft.gradient(enable=true,startColorstr=#FDFDFD, endColorstr=#7CB3FF);color: White;background-color: #465670;*/
            background-image: url(../img/Control/btn_bg.bmp);
            background-repeat: repeat-x;
        }
        .MainDiv
        {
            width: 100%;
            height: 100%;
            text-align: left;
            margin: 0px 0px 0px 0px;
            padding: 0px 0px 0px 0px;
            
        }
        .LeftDiv
        {
            width: 22%;
            height: 520px;
            text-align: left;
            margin: 0px 0px 0px 0px;
            padding: 0px 0px 0px 0px;
            float: left;
            border: 1px solid #D3D3D3;
        }
        .RightDiv
        {
            width: 77.5%;
            height: 520px;
            text-align: left;
            float: right;
            margin: 0px 0px 0px 0px;
            padding: 0px 0px 0px 0px;
            border: 1px solid #D3D3D3;
        }
        .OperationDiv
        {
            width: 100%;
            height: 7%;
        }
        .ShowDiv
        {
            width: 100%;
            height: 515px;
        
        }
        .ShowTable
        {
            width: 100%;
            margin: 0px auto;
            padding: 0px 0px 0px 0px;
        }
    </style>

    <script language="javascript" type="text/javascript">
        window.onload = function() {
            loadProgress();
        }

        function SubmitValid() {
            if ($("#txtName").val() == null || jQuery.trim($("#txtName").val()) == '') {
                alert("无法保存:该节点名称不能为空");
                // $("#txtName").select();
                return false;
            }
            if ($("#txtAddress").val() == null || jQuery.trim($("#txtAddress").val()) == '') {

                alert("无法保存:该节点地址不能为空");
                // $("#txtAddress").select();
                return false;
            }
            var personNum=$("#txtPersonNum").val();
            if(  !IsInt(personNum) ||( personNum.length>0 &&  personNum.indexOf("-")==0)){
            alert("无法保存:人数为正数类型");
            }
            return true;
        }
        //value='disable'  禁止编辑 ''可以编辑
        function Edit(value) {
            value = "";

            if (value == "") {
                SetStatue("修改");

            }
            else {
                SetStatue("显示");

            }

        }
        function ClearText() {
            Edit("");
            $(".ShowTable input:text").val("");
            SetStatue("添加");
        }
        function SetStatue(value) {
            var Flag = "";
            if (value == "修改") {
                Flag = "1";

            }
            else if (value == "添加") {
                Flag = "2";

            }
            else if (value == "显示") {
                Flag = "0";

            }
            else {
                Flag = "0";
            }

            $("#hdOperationState").attr("value", Flag);

        }
           function GetPersonInfo(flag) {
            var vReturnVlaues = GetPerson("true");
            if (vReturnVlaues != null) {
                if (flag == 1) {
                    document.getElementById("txtPersonMngID").value = vReturnVlaues[0].ID;
                    document.getElementById("txtPersonMngName").value = vReturnVlaues[0].Name;
                }
               
            }
            return false;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server" style="width: 98%; height:95%;">
    <div valign="top" class="MainDiv">
        <div class="LeftDiv" valign="top">
            <asp:TreeView ID="trvOrg" runat="server" ShowLines="True" ExpandDepth="1" EnableViewState="true"
                CssClass="tree" ForeColor="Black" NoExpandImageUrl="~/images/tree/plus3.gif" CollapseImageUrl="~/images/tree/plus3.gif"
                ExpandImageUrl="~/images/tree/minus2.gif" NodeStyle-ImageUrl="~/images/tree/folderClosed.gif"
                LeafNodeStyle-ImageUrl="~/images/tree/leaf.gif" SelectedNodeStyle-ImageUrl="~/images/tree/folderOpen.gif"
                OnSelectedNodeChanged="trvOrg_SelectedNodeChanged">
                <SelectedNodeStyle BackColor="#000080" ForeColor="#FFFFFF" />
                <Nodes>
                    <asp:TreeNode Text="培训场所" Value="培训场所"></asp:TreeNode>
                </Nodes>
            </asp:TreeView>
        </div>
        <div class="RightDiv" valign="top">
            <div class="ShowDiv" valign="top">
                <div style="width: 100%; text-align: left; padding-top: 10px; padding-left: 10px;
                    padding-bottom: 10px">
                    <table cellpadding="0" cellspacing="0" class="button-blue xforms-control xforms-trigger xforms-appearance-minimal xui-button ">
                        <tbody>
                            <tr>
                                <td class="xxf-value">
                                    <button class="xui-button-label xforms-trigger-image" style="width: 90px" type="button" onclick="ClearText()">
                                        <i class="icon icon-system-plus"></i><span class="xforms-label ">添加子节点 </span>
                                    </button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    &nbsp;&nbsp;
                    <table cellpadding="0" cellspacing="0" class="button-yellow xforms-control xforms-trigger xforms-appearance-minimal xui-button ">
                        <tbody>
                            <tr>
                                <td class="xxf-value">
                                    <button class="xui-button-label xforms-trigger-image" style="width: 90px" type="button" onclick="$('#<%=btnDel.ClientID %>').click();">
                                        <i class="icon icon-system-trash"></i><span class="xforms-label ">删除子节点 </span>
                                    </button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    &nbsp;&nbsp;
                    <%--<input type="button" value ="修改此节点" id="btnUpdateChild" onclick="Edit('')" visible="false" class="btnClass"/>--%>
                </div>
                <table class="ShowTable">
                    <tr>
                        <td style="width: 20%;" class="LableClass">
                            名称:
                        </td>
                        <td style="width: 79%">
                            <input type="text" id="txtName" runat="server" value=""   />
                        </td>
                    </tr>
                    <tr>
                        <td class="LableClass">
                            员工数量:
                        </td>
                        <td>
                            <input type="text" id="txtPersonNum" runat="server" value=""  />
                        </td>
                    </tr>
                    <tr>
                        <td class="LableClass">
                            地址:
                        </td>
                        <td>
                            <input type="text" id="txtAddress" runat="server" value=""  />
                        </td>
                    </tr>
                    <tr>
                        <td class="LableClass">
                            编码:
                        </td>
                        <td>
                            <input type="text" id="txtCode" runat="server" value=""   />
                        </td>
                    </tr>
                     <tr>
                        <td class="LableClass">
                            负责人:
                        </td>
                        <td>
                            <input type="text" id="txtPersonMngName" runat="server" value=""   />
                            <input type="text" id="txtPersonMngID" runat="server" value=""  style=" display:none"/>
                            <input type="button" id="btnSelectPersonMng" value="选择"  onclick="return GetPersonInfo(1)"/>
                            
                            
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="width: 100%; text-align: right" colspan="2">
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
                        </td>
                    </tr>
                </table>
                <div style="display: none" id="hidden">
                    <input type="button" value="添加子节点" id="btnAddChild" onclick="ClearText()" class="btnClass" />
                    <asp:Button ID="btnDel" runat="server" Text="删除子节点" OnClick="btnDel_Click" OnClientClick=" return confirm('确定删除此节点吗？');"
                        CssClass="CommonButton" />
                    <asp:Button ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click" OnClientClick="return SubmitValid();"
                        CssClass="CommonButton" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdOperationState" runat="server" Value="0" />
    </form>
</body>
</html>

