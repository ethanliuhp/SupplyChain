<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GridViewObjectMng.ascx.cs" Inherits="UserControls_GridViewObjectMng" %>
<script src="<%=ResolveUrl("../JavaScript/Util.js")%>" type="text/javascript"></script>
<script src="<%=ResolveUrl("../JavaScript/jquery-1.10.2.min.js")%>" type="text/javascript"></script>
 <style  type="text/css">
  
 </style>
<script type="text/javascript">
   
//   function singleClick1(iRowIndex,data){
// 
//        var gridID='<%=DestControl.ClientID %>';
//        var oGrid=$("#"+gridID);
//        oGrid.attr("selectRowIndex",iRowIndex);
//        oGrid.attr("selectRowData",data);
//       selectRowInfo[ gridID ]={rowIndex:iRowIndex,rowData:data};
//       if(parent.setOperateControl) parent.setOperateControl();
//       
//   }
// function _singleClick(oTr,gridClientID,pageDeleteBillIDsClientID,iRowIndex,clientClick){

//    var selectRowData=getDecodeRowData(oTr.Data||oTr.attributes["data"].value);
//    var selectRowIndex=iRowIndex;
//    var oGrid= document.getElementById(gridClientID);
//    _setSelectStyle(oTr,gridClientID);
//    oGrid.selectRow={index:selectRowIndex,data:selectRowData};
//    oGrid.deleteRow= function(rowIndex){
//         var oGrid= document.getElementById(gridClientID); 
//         if( oGrid.rows.length >rowIndex){
//                var data=oGrid.selectRow.data;
//                oGrid.rows[rowIndex+1].style.display='none';
//                var oDeleteDetailIds=document.getElementById(pageDeleteBillIDsClientID); 
//                var sIDs=oDeleteDetailIds.value; 
//                var arrIDs=[];
//                if(sIDs){
//                    arrIDs=sIDs.split(';'); 
//                }
//                if(data){
//                    arrIDs.push(data.ID);
//                }
//                oDeleteDetailIds.value= arrIDs.join(';');  
//         } 
//   }; 
//   if(clientClick) {
//        clientClick(selectRowIndex,selectRowData);
//   }
// }
// function _setSelectStyle(oTr,gridClientID){

//    var oGrid=document.getElementById(gridClientID);
//    if(oGrid && oGrid.selectRow && oGrid.selectRow.index<oGrid.rows.length-1){
//       oGrid.rows[oGrid.selectRow.index+1].className= oGrid.rows[oGrid.selectRow.index+1].className.replace("_SelectRowStyle",""); 
//    }
//    if(oTr.className.indexOf("_SelectRowStyle")<0){
//            oTr.className+=" _SelectRowStyle";
//    }
//    //oTr.style.color='#116a79';oTr.style.backgroundColor='#cef97b'
// }

// function _doubleClick(gridClientID,iRowIndex,clientClick){
//    _singleClick1(gridClientID,iRowIndex);
//   if(clientClick) {
//         var oGrid=document.getElementById(gridClientID);
//         //oGrid.selectRow={index:iRowIndex,data:selectRowData};
//        clientClick(oGrid.selectRow.index,oGrid.selectRow.data);
//   }
//  
// }
$(window).ready(function(){
var gridID='<%=DestControl.ClientID %>';
    //_AddRow(gridID);
});
function _AddRow(gridID){

   var oGrid= $("#"+gridID);
   //if(oGrid && oGrid.rows.length)
   var oDiv=$("<div style='width:100%; height:auto; text-align:center;'>添加</div>");
 
   oGrid.parent().append(oDiv);
}
 function _Oprate(gridID,flag,iRowIndex,_fun){
    var oGrid=document.getElementById(gridID);
    _singleClick1(gridID,iRowIndex);
    if(oGrid){
    flag=toLower(flag);
     switch(flag){
            case "new":
            {   
                alert("new");
                break;
            }
            case "delete":
            {
                if(!_fun){
                    _deleteGridRow(oGrid,iRowIndex);
                }
                break;
            }
            case "update":
            {
                 
                break;
            }
            case "singleclick":
            {
                break;
            }
            case "doubleclick":
            {
                alert(flag);
                break;
            }
            
        }
         if(_fun){
             if(oGrid.selectRow){
               _fun(oGrid, oGrid.selectRow.index,oGrid.selectRow.data,flag);
               }
               else{
                  _fun(oGrid, null,null,flag);
               }
         }
          
     }
     return false;
 }
 function _singleClick1(gridID,iRowIndex){
     var oGrid=document.getElementById(gridID);
     if(oGrid && iRowIndex>0 && iRowIndex>0 && oGrid.rows.length>iRowIndex ){
        if(!oGrid.deleteRow){//没有删除属性
            oGrid.deleteRow=function(rowIndex){
                _deleteGridRow(oGrid,rowIndex);
            }
        }
         var oTr=oGrid.rows[iRowIndex];
         _setSelectStyle1(oGrid,oTr);
         var selectRowData=getDecodeRowData(oTr.Data||oTr.attributes["data"].value);
          oGrid.selectRow={index:iRowIndex,data:selectRowData};
     }
 }
  function _setSelectStyle1(oGrid,oTr){
    if(oGrid && oTr && oGrid.selectRow){
       oGrid.rows[oGrid.selectRow.index].className= oGrid.rows[oGrid.selectRow.index].className.replace("_SelectRowStyle",""); 
    }
    if(oTr.className.indexOf("_SelectRowStyle")<0){
            oTr.className+=" _SelectRowStyle";
    }
    //oTr.style.color='#116a79';oTr.style.backgroundColor='#cef97b'
 }
 function _deleteGridRow(oGrid,iRowIndex){
     if( oGrid.rows.length >iRowIndex){
           oGrid.rows[iRowIndex].style.display='none';
           var oDeleteDetailIds=document.getElementById('<%=hdPageDeleteBillIDs.ClientID %>'); 
           var sIDs=oDeleteDetailIds.value; 
           var arrIDs=[];
           if(sIDs){
                arrIDs=sIDs.split(';'); 
           }
           var data=oGrid.selectRow.data;
           if(data){
                arrIDs.push(data.ID);
           }
           oDeleteDetailIds.value= arrIDs.join(';');  
         } 
 }
</script>
 
 
<div id="divPager" style="height: 22px;width: 99%;  border:solid 0px blue; top: 0px; margin: 0px 0px 0px 0px; padding: 8px 0px 0px 0px; <%=this.RowCount==0?"display:none;": "" %>   " align="right">
    <table cellpadding="0" cellspacing="0"  style="width:auto;white-space: nowrap; border:solid 0px blue; ">
 
            <tr>
                <td  style="padding-left: 5px; padding-right: 5px; width: 80px; ">
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
    <asp:HiddenField Value="15" ID="hdPageSize" runat="server" />
    <asp:HiddenField Value="true" ID="hdState" runat="server" />
    <asp:Button ID="btnQuery" runat="server" Text="查询" OnClick="btnQuery_Click" Style="visibility: hidden" />
    <asp:HiddenField Value="-1" ID="hdSelectRowIndex" runat="server" />
    <asp:HiddenField Value="1" ID="hdIsSelect" runat="server" />
    <asp:Button ID="btnSelect" runat="server" OnClick="btnSelectClick" Text="选择" />
    <asp:HiddenField ID="hdAlign" Value="Left" runat="server" />
    <%--客户端单击事件--%>
    <asp:HiddenField ID="hdEnabledClientSingleClick" Value="False"  runat="server"/>
    <asp:HiddenField ID="hdClientClick"  runat="server"/><%--客户端单击事件脚本函数--%>
    <%--客户端双击事件--%>
     <asp:HiddenField ID="hdEnabledClientDoubleClick" Value="False"  runat="server"/>
    
    <%--客户端修改事件--%>
    <asp:HiddenField ID="hdEnabledClientUpdateClick" Value="False"  runat="server"/>
   
    <%--客户端删除事件--%>
    <asp:HiddenField ID="hdEnabledClientDeleteClick" Value="False"  runat="server"/>
   
    <%--客户端新增事件--%>
    <asp:HiddenField ID="hdEnabledClientNewClick" Value="False"  runat="server"/>
    
    
    <asp:HiddenField ID="hdUserControlKey"  runat="server"/><%--当前控件的唯一标识--%>
    <asp:HiddenField ID="hdPageDeleteBillIDs"  runat="server"/><%--页面删除ID集合--%>
 
    
    <asp:Button ID='linkFirst'  runat="server" Style="cursor: pointer;" Text="首  页" OnClick="btnQuery_Click"
        CommandName="first"></asp:Button>
    <asp:Button Style='margin-left: 2px; cursor: pointer;' ID='linkPrev' runat="server"
        Text="上一页" OnClick="btnQuery_Click" CommandName="prev"></asp:Button>
    <asp:Button Style='margin-left: 2px; cursor: pointer;' ID='linkNext' runat="server"
        Text="下一页" OnClick="btnQuery_Click" CommandName="next"></asp:Button>
    <asp:Button Style='margin-left: 2px; cursor: pointer;' ID='linkLast' runat="server"
        Text="尾  页" OnClick="btnQuery_Click" CommandName="last"></asp:Button>
</div>
