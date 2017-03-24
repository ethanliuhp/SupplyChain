<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowSupply.aspx.cs" Inherits="Show_ShowSupply" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/GridViewSource.ascx" TagName="GridViewDataMng" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<base target="_self" />
<head id="Head1" runat="server">
    <title>供应商选择</title>
    <link href="../css/MainWeb.css" rel="Stylesheet" type="text/css" />
    <link href="../css/MainWeb.css" rel="Stylesheet" type="text/css" />
    <link href="../css/xforms.css" rel="stylesheet" type="text/css" />
    <link href="../css/system.icon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #form1
        {
            width: 700px;
            margin: 0px;
            padding: 0px;
            top: 0px;
            left: 0px;
            text-align: left;
        }
        .mainDiv
        {
            width: 100%;
            margin: 0px;
            padding: 0px;
            top: 0px;
            left: 0px;
            text-align: left;
        }
        .QueryDiv
        {
            width: 100%;
            height: 30px;
            margin: 0px;
            padding: 0px;
            top: 0px;
            left: 0px;
            text-align: left;
        }
        .ContextDiv
        {
            width: 100%;
            margin: 0px;
            padding: 0px;
            top: 0px;
            left: 0px;
            text-align: left;
        }
        .OperateDiv
        {
            width: 100%;
            height: 30px;
            margin: 0px;
            padding: 0px;
            top: 0px;
            left: 0px;
            text-align: left;
        }
        .QueryTable
        {
            width: 100%;
            height: 30px;
            margin: 0px;
            padding: 0px;
            top: 0px;
            left: 0px;
            text-align: left;
        }
        .QueryTdLabel
        {
            width: 10%;
            height: 100%;
            margin: 0px;
            padding: 0px;
            top: 0px;
            left: 0px;
            text-align:right;
        }
        .QueryTdText
        {
            width: 20%;
            height: 100%;
            margin: 0px;
            padding: 0px;
            top: 0px;
            left: 0px;
            text-align: left;
        }
        .QueryTdBtn
        {
            width: 10%;
            height: 100%;
            margin: 0px;
            padding: 0px;
            top: 0px;
            left: 0px;
            text-align: center;
        }
    </style>

    <script type="text/javascript" src="../javascript/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="../JavaScript/Util.js"></script>
    <script type="text/javascript">
        var sPersonID = "<%=hdPersonID.ClientID %>";
        var gvPersonID = "<%=gvPerson.ClientID %>";
        var sState = "<%=hdSelectMethod.ClientID %>";

        $(document).ready(function() {
            IntialCheckBox();

        });
        function IntialCheckBox() {

            var sPersonIDs = $("#" + sPersonID).val();
            if (sPersonIDs == null || sPersonIDs == '') {
            }
            else {
                var sIDs = sPersonIDs.split(",");
                for (var id in sIDs) {
                    try {
                        document.getElementById(sIDs[id]).checked = true;
                    }
                    catch (e)
                { }
                }
            }
            return false;
        }
        function Select(sID, o) {
            if (o != null && sID != null && sID != '') {
                try {
                    if (o.checked) {
                        AddPerson(sID);
                    }
                    else {
                        DeletePerson(sID);
                    }
                }
                catch (e) {
                    alert("选择出错:" + e.message);
                }

            }

        }

        function AddPerson(sID) {
            if (sID == null || sID == "") {
                throw "选择项的人员ID为空！";
            }
            else {
                var oPersonIDs = $("#" + sPersonID);
                var oState = $("#" + sState);
                var sIDs = "";

                if (oPersonIDs == null) {
                    throw "找不到保存ID控件！";
                }
                else {
                    if (oState.val() == "1") {
                        sIDs = sID;
                        $("#" + gvPersonID + " input:checkbox").attr("checked", false);
                        document.getElementById(sID).checked = true;
                    }
                    else {
                        sIDs = oPersonIDs.val();

                        if (sIDs != "" && sIDs != null) {
                            sIDs = sIDs + "," + sID;
                        }
                        else {
                            sIDs = sID;
                        }
                    }
                    oPersonIDs.val(sIDs);
                }
            }

        }
        function DeletePerson(sID) {
            if (sID == null || sID == "") {
                throw "选择项的人员ID为空！";
            }
            else {
                var oPersonIDs = $("#" + sPersonID);
                var sIDs = "";

                if (oPersonIDs == null) {
                    throw "找不到保存ID控件！";
                }
                else {
                    sIDs = oPersonIDs.val();
                    if (sIDs != "" && sIDs != null) {
                        sIDs = sIDs.replace(sID + ",", "");
                        sIDs = sIDs.replace(sID, "");
                    }

                    oPersonIDs.val(sIDs);
                }
            }

        }
        function validate() {
            //debugger;
            var oPersonIDs = $("#" + sPersonID);
            var sIDs = oPersonIDs.val();
            if (sIDs == null || sIDs == '') {
                alert("请选择供应商");
                return false;
            }
            else {
                return true;
            }

        }
        function Sure(){
          var data=document.getElementById("<%=hdJosn.ClientID %>").value;
            if(data){
                 if(parent && parent.window){
                //parent.window.frames[0]==window.location
                    var div=parent.window.document.getElementById("_dialogDivDetail");
                    if( div &&div.sure){
                     var  data1=getDecodeRowData(data);
                        div.sure(data1);
                    }
                }
            }
            else{
                alert("请选择供应商");
            }
     }
      function Close(){
        if(parent && parent.window){
        //parent.window.frames[0]==window.location

            var div=parent.window.document.getElementById("_dialogDivDetail");
            if( div &&div.close){
                div.close("取消");
            }
        }
     }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="mainDiv">
        <div class="QueryDiv">
            <table style="width: 100%; text-align: left;">
                <tr style="width: 100%; text-align: left;">
                    <td class="QueryTdLabel">
                        供应方
                    </td>
                    <td class="QueryTdText">
                        <asp:TextBox runat="server" ID="txtName" Width="95%"></asp:TextBox>
                    </td>
                    <td class="QueryTdLabel">
                        编码
                    </td>
                    <td class="QueryTdText">
                        <asp:TextBox runat="server" ID="txtCode" Width="95%"></asp:TextBox>
                    </td>
                    <td class="QueryTdLabel">
                        分类
                    </td>
                    <td class="QueryTdText">
                        <asp:DropDownList runat="server" ID="dpLstCata" Width="95%"></asp:DropDownList>
                    </td>
                    <td class="QueryTdBtn">
                        <table cellpadding="0" cellspacing="0" class="button-blue xforms-control xforms-trigger xforms-appearance-minimal xui-button ">
                            <tbody>
                                <tr>
                                    <td class="xxf-value">
                                        <button class="xui-button-label xforms-trigger-image" type="button" onclick="$('#<%=btnQuery.ClientID %>').click();">
                                            <i class="icon icon-system-search"></i><span class="xforms-label ">查询 </span>
                                        </button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="ContextDiv"  >
            <asp:GridView ID="gvPerson" runat="server" AutoGenerateColumns="False" DataKeyNames="suprelid" Width="100%"
                EmptyDataText="当前没有相关数据显示" CssClass="grid">
                <AlternatingRowStyle BackColor="#F7FAFF" />
                <Columns>
                    <asp:TemplateField  ItemStyle-CssClass="colSelect" HeaderStyle-Width="50">
                        <HeaderStyle VerticalAlign="Middle" HorizontalAlign="Center"/>
                        <HeaderTemplate>
                            选择</HeaderTemplate>
                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <input type="checkbox" value="选择" id='<%#Eval("suprelid") %>'  onclick="return Select('<%#Eval("suprelid") %>',this);" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField  HeaderText="id" DataField="suprelid" Visible="false"  ItemStyle-CssClass="colContent_min">
                        <ItemStyle CssClass="colContent_min"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderStyle-Width="80" HeaderText="编码" DataField="CODE"  ItemStyle-CssClass="colContent_min">
                        <ItemStyle CssClass="colContent_min"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="orgname" HeaderStyle-Width="200" HeaderText="供应方"  ItemStyle-CssClass="colContent_min">
                        <ItemStyle CssClass="colContent_min"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="address" HeaderText="地址" HeaderStyle-Width="200"  ItemStyle-CssClass="colContent_min">
                        <ItemStyle CssClass="colContent_min"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="linkman" HeaderText="联系人"  HeaderStyle-Width="80" ItemStyle-CssClass="colContent_min">
                        <ItemStyle CssClass="colContent_min"></ItemStyle>
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            <uc1:GridViewDataMng ID="GridViewSource_person" runat="server" />
        </div>
        <div class="OperateDiv">
            <table style="width: 100%; text-align: center">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" class="button-blue xforms-control xforms-trigger xforms-appearance-minimal xui-button ">
                            <tbody>
                                <tr>
                                    <td class="xxf-value">
                                        <button class="xui-button-label xforms-trigger-image" type="button" onclick="$('#<%=btnSure.ClientID %>').click();">
                                            <i class="icon icon-system-ok"></i><span class="xforms-label ">确定 </span>
                                        </button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                    <td>
                        <table cellpadding="0" cellspacing="0" class="button-blue xforms-control xforms-trigger xforms-appearance-minimal xui-button ">
                            <tbody>
                                <tr>
                                    <td class="xxf-value">
                                        <button class="xui-button-label xforms-trigger-image" type="button" onclick="Close('')">
                                            <i class="icon icon-system-cancel"></i><span class="xforms-label ">取消 </span>
                                        </button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </table>
            <div style="display: none" id="hidden">
                <asp:Button ID="btnQuery" runat="server" Text="查询" OnClick="btnQuery_Click" CssClass="CommonButton" />
                <asp:Button ID="btnSure" Text="确定" CssClass="CommonButton" runat="server" OnClientClick="return validate()"
                    OnClick="btnSure_Click" />
                <input type="button" value="取消" onclick="Close('')" class="CommonButton" />
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdPersonID" Value="" runat="server" />
    <asp:HiddenField ID="hdSelectMethod" Value="1" runat="server" />
    <asp:HiddenField ID="hdJosn" Value="" runat="server" />
    <asp:HiddenField ID="hdPersonType" Value="" runat="server" />
    <asp:HiddenField ID="hdTeachType" Value="" runat="server" />
    </form>
</body>
</html>
