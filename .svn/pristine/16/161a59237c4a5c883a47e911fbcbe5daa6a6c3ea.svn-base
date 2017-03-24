<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GridViewRow.ascx.cs" Inherits="Resource_GridViewRow" %>

<script type="text/javascript"  src="../javascript/jquery-1.10.2.js"></script>
<script type="text/javascript">
    $(document).ready(function() {
        //debugger;
        var ctlID = "<%=hdControlName.ClientID %>";
        var SaveCtlID = "<%=hdSelectRowIndex.ClientID %>";
        var btnCallBack = "<%=btnClick.ClientID %>";
        var IsCallBackID = "<%=hdIsCallBack.ClientID %>";
        var IsCallBack = $("#" + IsCallBackID).val();
        var hdButtonID =  "<%=hdButton.ClientID %>";
        var GridID = $("#" + ctlID).val();

        InitalGridView(GridID, SaveCtlID, btnCallBack, IsCallBackID, IsCallBack, hdButtonID);
        SelectRowByIndexStyle(GridID, SaveCtlID,hdButtonID);
    });
    function InitalGridView(GridID, SaveCtlID, btnCallBack, IsCallBackID, IsCallBack, hdButtonID) {
         
        //var ctlID = "<%=hdControlName.ClientID %>";
        // var SaveCtlID = "<%=hdSelectRowIndex.ClientID %>";
        //debugger;
        var v = $("#" + GridID + " tr:gt(0)");
        NoSelectGridViewRowStyle(v);
        v.each(function(key, data) {
            $(data).click(function() {
                $("#" + hdButtonID).val("1");
                SelectGridViewRow(GridID, data, SaveCtlID, btnCallBack, IsCallBack);
            });
            $(data).mouseout(function() {

                // NoSelectGridViewRowStyle(data);
            });

            $(data).mouseover(function() {
                // debugger;
                // OverGridViewRowStyle(data);
            })
        });
    }
    function NoSelectGridViewRowStyle(o) {

       // $(o).css({ "background-color": "White", cursor: "pointer", "text-align": "left" });
        var d = $(o);
        d.css("background-color", "White");
        d.css("cursor", "pointer");
        d.css("text-align", "left");
       // $(o).css("cursor", "pointer");
    }
    function SelectGridViewRowStyle(o) {
       //debugger;
        var d = $(o);
        d.css("background-color", "#FFFFCC");
        d.css("cursor", "pointer");
        d.css("text-align", "center" );
    }
    function SelectGridViewRowByIndexStyle(sCtlID, index) {
       // debugger;
        var trContext = $("#" + sCtlID + " tr:gt(0)");
        SelectGridViewRowStyle(trContext.get(index));
    }
    function OverGridViewRowStyle(o) {
        //debugger;
        //  $(o).css({ "background-color": "#99CCFF", cursor: "pointer", "text-align": "center" });
        var d = $(o);
        d.css("background-color", "#99CCFF");
        d.css("cursor", "pointer");
        d.css("text-align", "center");
    }
    
    function SelectGridViewRow(ctlID, o,SaveCtlID, btnCallBack, IsCallBack) {
       // debugger;
        //        var trContext = $("#" + ctlID + " tr:gt(0)");
        //        trContext.removeClass("SelectRow");
        //        trContext.addClass("OutRow");
        //        $(o).removeClass("OutRow");
        //        $(o).addClass("SelectRow");
        // $("#" + SaveCtlID).val(trContext.index(o));
        //alert(trContext.index(o)+1);
        var trContext = $("#" + ctlID + " tr:gt(0)");
       
       // alert(trContext.index(o) );
        $("#" + SaveCtlID).val(trContext.index(o));

        if (IsCallBack) {
            $("#" + btnCallBack).click();
        }
        else {
            NoSelectGridViewRowStyle(trContext);
            SelectGridViewRowStyle(o);
        }
    }
    function OverGridViewRow(o) {
       // debugger;
//        var trContext = $("#" + ctlID + " tr:gt(0)");
//        NoSelectGridViewRowStyle(trContext);
        //        OverGridViewRowStyle(o);
        OverGridViewRowStyle(o);
    }
    function OutGridViewRow( o) {
        
//        var trContext = $("#" + ctlID + " tr:gt(0)");
        //        NoSelectGridViewRowStyle(trContext);
       // debugger;
        NoSelectGridViewRowStyle(o);
    }
    function SelectRowByIndexStyle(ctlID, SaveCtlID,hdButtonID) {
      
        var trContext = $("#" + ctlID + " tr:gt(0)");
        var index = $("#" + SaveCtlID).val();
        var IsShowSelect = $("#" + hdButtonID);
        var o = trContext.get(index);
        if (o!=null && o!=undefined) {
            OutGridViewRow(trContext);

            NoSelectGridViewRowStyle(trContext);
            //setTimeout(" SelectGridViewRowByIndexStyle('" + ctlID + "'," + index + ")", 30);
            //if (IsShowSelect.val() == "1") {
                SelectGridViewRowStyle(o);

                //alert(trContext.index(o));
                $("#" + SaveCtlID).val(index);
           // }
        }
    }
</script>
<div style=" display:none">
    <asp:HiddenField ID="hdControlName" Value=""  runat="server"/>
    <asp:HiddenField ID="hdSelectRowIndex" Value=""  runat="server" />
    <asp:Button ID="btnClick" runat="server" Text="Event"   onclick="btnClick_Click" OnClientClick="return true;" />
    <asp:HiddenField ID="hdIsCallBack" Value="1" runat="server" />
    <asp:HiddenField ID="hdButton" Value="0" runat="server" />
</div>