<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GridViewSource.ascx.cs" Inherits="UserControls_GridViewSource" %>

<script src="<%=ResolveUrl("../JavaScript/jquery-1.10.2.js")%>" type="text/javascript"></script>

<script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.core.js")%>" type="text/javascript"></script>

<script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.widget.js")%>" type="text/javascript"></script>

<script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.button.js")%>" type="text/javascript"></script>

<script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.mouse.js")%>" type="text/javascript"></script>

<script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.draggable.js")%>" type="text/javascript"></script>

<script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.resizable.js")%>" type="text/javascript"></script>

<script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.position.js")%>" type="text/javascript"></script>

<script src="<%=ResolveUrl("../JavaScript/jQueryUI/jquery.ui.dialog.js")%>" type="text/javascript"></script>

<script type="text/javascript">

    $(document).ready(function() {

        var sHdGridID = "<%=hdControl.ClientID %>";
        var sGridID = $("#" + sHdGridID).val();
        var sHdSelectRowID = "<%=hdSelectRow.ClientID %>";
        var sBtnSelectID = "<%=btnSelect.ClientID %>";
        var sIsSelectRowID = "<%=hdIsSelect.ClientID %>";
        SetStyle();
        var v = $("#" + sGridID + " tr:gt(0)");

        v.each(function(key, data) {
            $(data).click(function() {
                var iSelect = $("#" + sHdSelectRowID).val();
                if (iSelect != key) {
                    $("#" + sHdSelectRowID).val(key);
                    if ($("#" + sIsSelectRowID).val() == "1") {
                        $("#" + sBtnSelectID).click();
                    }
                    else {
                        SetStyle();
                    }
                }
            });
        });
    });
    function SetStyle() {
        var sHdGridID = "<%=hdControl.ClientID %>";
        var sGridID = $("#" + sHdGridID).val();
        var sHdSelectRowID = "<%=hdSelectRow.ClientID %>";


        var v = $("#" + sGridID + " tr:gt(0)");
        v.css("text-align", "<%=hdAlign.Value %>");
        //font-weight:normal
        v.css("font-weight", "normal");
        var iSelectRow = parseInt($("#" + sHdSelectRowID).val());
        if (iSelectRow != undefined && iSelectRow != NaN && iSelectRow > -1) {
            var d = $("#" + sGridID + " tr:eq(" + (iSelectRow + 1) + ")");
            d.css("text-align", "center");
            d.css("font-weight", "bold");
        }

        v.css("cursor", "pointer");
    }
    //    function Page(iPage) {

    //        //    var value = $("<%=hdShowPageCount.ClientID %>").val() + iPage;
    //        //    $("<%=hdShowPageCount.ClientID %>").val(value);
    //        //    $("<%=btnQuery.ClientID %>")[0].onclick();
    //        // $("#<%=btnQuery.ClientID %>")[0].click();
    //        var hd = document.getElementById("<%=hdShowPageCount.ClientID %>");
    //        hd.value = iPage;
    //        document.getElementById("<%=btnQuery.ClientID %>").click();
    //    }

    //    function change() {

    //        // var value = $("#<%=selPager.ClientID %>").val();
    //        var value = document.getElementById("<%=selPager.ClientID %>").value;
    //        Page(value);
    //    }
</script>

<%--<div id="divPager" style="height: 22px; width: 100%; text-align: center; top: 0px;
    margin: 3px 0px 0px 0px; padding: 0px 0px 0px 0px; <%=this.RowCount==0?"display:none": "" %>">
    <asp:LinkButton ID='linkFirst' runat="server" Style="cursor: pointer;" Text="首  页"
        OnClick="btnQuery_Click" CommandName="first"></asp:LinkButton>
    <asp:LinkButton Style='margin-left: 2px; cursor: pointer; color: Black' ID='linkPrev'
        runat="server" Text="上一页" OnClick="btnQuery_Click" CommandName="prev"></asp:LinkButton>
    <asp:LinkButton Style='margin-left: 2px; cursor: pointer;' ID='linkNext' runat="server"
        Text="下一页" OnClick="btnQuery_Click" CommandName="next">
    </asp:LinkButton>
    <asp:LinkButton Style='margin-left: 2px; cursor: pointer;' ID='linkLast' runat="server"
        Text="尾  页" OnClick="btnQuery_Click" CommandName="last"></asp:LinkButton>
    <span style="margin-left: 2px;">共<span style="color: red; font-weight: bold;"><%=this.RowCount %></span>行
        每页<span style="color: red; font-weight: bold;"><%=this.PageSize %></span>行，共<span
            style="color: red; font-weight: bold;"><%=this.PageCount %></span>页 当前第<span style="color: red;
                font-weight: bold;"><%=this.CurrentPage %></span>页</span> <a style='margin-left: 2px;'>
                    跳到</a>
    <asp:DropDownList ID="selPager" runat="server" Style="width: 30px" AutoPostBack="true"
        OnSelectedIndexChanged="selPager_SelectedIndexChanged">
    </asp:DropDownList>
    页
</div>--%>
<div id="divPager" style="height: 22px;width: 99%; 
    text-align: center; top: 0px; margin: 0px 0px 0px 0px; padding: 8px 0px 0px 0px;
    <%=this.RowCount==0?"display:none": "" %>">
    <table cellpadding="0" cellspacing="0" class="button-blue xforms-control xforms-trigger xforms-appearance-minimal xui-button "
        style="width: 700px; margin: 0px auto;white-space: nowrap;">
        <tbody>
            <tr>
                <td class="xxf-value" style="padding-left: 5px; padding-right: 5px; width: 80px;">
                    <button class="xui-button-label xforms-trigger-image" type="button" style="width: 70px;"
                        <%=this.linkFirst.Enabled?"": "disabled" %> onclick="$('#<%=linkFirst.ClientID %>').click();">
                        <i class="icon icon-system-left-dir"></i><span class="xforms-label ">首页 </span>
                    </button>
                </td>
                <td class="xxf-value" style="padding-left: 5px; padding-right: 5px; width: 80px;">
                    <button class="xui-button-label xforms-trigger-image" type="button" style="width: 70px;"
                        <%=this.linkPrev.Enabled?"": "disabled" %> onclick="$('#<%=linkPrev.ClientID %>').click();">
                        <i class="icon icon-system-angle-double-left"></i><span class="xforms-label ">上一页
                        </span>
                    </button>
                </td>
                <td class="xxf-value" style="padding-left: 5px; padding-right: 5px; width: 80px;">
                    <button class="xui-button-label xforms-trigger-image" type="button" style="width: 70px;"
                        <%=this.linkNext.Enabled?"": "disabled" %> onclick="$('#<%=linkNext.ClientID %>').click();">
                        <i class="icon icon-system-angle-double-right"></i><span class="xforms-label ">下一页
                        </span>
                    </button>
                </td>
                <td class="xxf-value" style="padding-left: 5px; padding-right: 5px; width: 80px;">
                    <button class="xui-button-label xforms-trigger-image" type="button" style="width: 70px;"
                        <%=this.linkLast.Enabled?"": "disabled" %> onclick="$('#<%=linkLast.ClientID %>').click();">
                        <i class="icon icon-system-right-dir"></i><span class="xforms-label ">尾页 </span>
                    </button>
                </td>
                <td >
                    <span style="margin-left: 2px;">共<span style="color: red; font-weight: bold;"><%=this.RowCount %></span>行</span>
                </td>
                <td >
                    每页<span style="color: red; font-weight: bold;"><%=this.PageSize %></span>行
                </td>
                <td >
                    共<span style="color: red; font-weight: bold;"><%=this.PageCount %></span>页
                </td>
                <td >
                    当前第<span style="color: red; font-weight: bold;"><%=this.CurrentPage %></span>页
                </td>
                <td>
                    跳到
                    <asp:DropDownList ID="selPager" runat="server" Style="width: 30px" AutoPostBack="true"
                        OnSelectedIndexChanged="selPager_SelectedIndexChanged">
                    </asp:DropDownList>
                    页
                </td>
            </tr>
        </tbody>
    </table>
</div>
<div style="display: none">
    <asp:HiddenField Value="0" ID="hidRowCount" runat="server" />
    <asp:HiddenField Value="1" ID="hdShowPageCount" runat="server" />
    <asp:HiddenField Value="" ID="hdSQL" runat="server" />
    <asp:HiddenField Value="" ID="hdControl" runat="server" />
    <asp:HiddenField Value="20" ID="hdPageSize" runat="server" />
    <asp:HiddenField Value="true" ID="hdState" runat="server" />
    <asp:Button ID="btnQuery" runat="server" Text="查询" OnClick="btnQuery_Click" Style="visibility: hidden" />
    <asp:HiddenField Value="-1" ID="hdSelectRow" runat="server" />
    <asp:HiddenField Value="0" ID="hdIsSelect" runat="server" />
    <asp:Button ID="btnSelect" runat="server" OnClick="btnSelectClick" Text="选择" />
    <asp:HiddenField ID="hdAlign" Value="Left" runat="server" />
    <asp:Button ID='linkFirst' runat="server" Style="cursor: pointer;" Text="首  页" OnClick="btnQuery_Click"
        CommandName="first"></asp:Button>
    <asp:Button Style='margin-left: 2px; cursor: pointer;' ID='linkPrev' runat="server"
        Text="上一页" OnClick="btnQuery_Click" CommandName="prev"></asp:Button>
    <asp:Button Style='margin-left: 2px; cursor: pointer;' ID='linkNext' runat="server"
        Text="下一页" OnClick="btnQuery_Click" CommandName="next"></asp:Button>
    <asp:Button Style='margin-left: 2px; cursor: pointer;' ID='linkLast' runat="server"
        Text="尾  页" OnClick="btnQuery_Click" CommandName="last"></asp:Button>
</div>
