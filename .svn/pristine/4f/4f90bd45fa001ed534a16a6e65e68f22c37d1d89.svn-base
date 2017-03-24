<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ListSource.ascx.cs" Inherits="UserControls_ListSource" %>

<script src="../JavaScript/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="../JavaScript/Util.js" type="text/javascript"></script>
<script type="text/javascript">
   
   function singleClick(iRowIndex,data){
 
        var gridID='<%=DestControl.ClientID %>';
        var oGrid=$("#"+gridID);
        oGrid.attr("selectRowIndex",iRowIndex);
        oGrid.attr("selectRowData",data);
       selectRowInfo[ gridID ]={rowIndex:iRowIndex,rowData:data};
       if(parent.setOperateControl) parent.setOperateControl();
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
<div id="divPager" style="height: 22px;width: 99%;  border:solid 0px blue; top: 0px; margin: 0px 0px 0px 0px; padding: 8px 0px 0px 0px; <%=this.RowCount==0?"display:none;": "" %>   " align="right">
    <table cellpadding="0" cellspacing="0"  style="width:auto;white-space: nowrap; border:solid 0px blue; ">
 
            <tr>
                <td  style="padding-left: 5px; padding-right: 5px; width: 80px;">
                    <input type="button" style="width: 70px;"  value="首页"  <%=this.linkFirst.Enabled?"": "disabled='disabled'" %>  onclick="$('#<%=linkFirst.ClientID %>').click();" />   
                  
                </td>
                <td  style="padding-left: 5px; padding-right: 5px; width: 80px;">
                    <input  type="button" value="上一页" style="width: 70px;"  <%=this.linkPrev.Enabled?"": " disabled='disabled'" %> onclick="$('#<%=linkPrev.ClientID %>').click();" />
                </td>
                <td  style="padding-left: 5px; padding-right: 5px; width: 80px;">
                    <input type="button" value="下一页" style="width: 70px;"   <%=this.linkNext.Enabled?"": "disabled='disabled'" %> onclick="$('#<%=linkNext.ClientID %>').click();" />
                   
                </td>
                <td   style="padding-left: 5px; padding-right: 5px; width: 80px;">
                    <input  type="button" value="尾页" style="width: 70px;" <%=this.linkLast.Enabled? "": "disabled='disabled'" %> onclick=" $('#<%=linkLast.ClientID %>').click();" />

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
                    <asp:DropDownList ID="selPager" runat="server" Style="width: 40px" AutoPostBack="true"
                        OnSelectedIndexChanged="selPager_SelectedIndexChanged">
                    </asp:DropDownList>
                    页
                </td>
            </tr>
     
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
