<%@ Page Language="C#" AutoEventWireup="true" CodeFile="master.aspx.cs" Inherits="MoneyManage_GatheringMng_master" %>

<%@ Register Src="~/UserControls/GridViewDataMng.ascx" TagName="GridViewDataMng" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>  
        <style type="text/css">
            .divQuery{ width:100%;  height:100%; margin:0px; padding:0px; text-align:center; display:block;}
            .QueryTable{width:100%;  height:100%; margin-left:auto; margin-right:auto; padding:0px; text-align:center;}
            .QueryTableTr{width:100%;  height:30px;   text-align:center;}
            .QueryTableTdLabel{width:15%;  height:100% ;  text-align:right;}
            .QueryTableTdText{width:35%;  height:100% ;  text-align:left; padding:auto 5px auto auto;}
        </style>
    <link href="../../css/MainWeb.css" rel="Stylesheet" type="text/css" />
        <script type="text/javascript" src="../../JavaScript/Util.js"></script>
        <script src="../../JavaScript/jquery-1.10.2.min.js"></script>
        <script type="text/javascript" src="../../JavaScript/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript">
  var selectRowInfo={};
     $(window).ready(function(){
    
     //debugger;
        if(parent.setMasterOperateControl) parent.setMasterOperateControl();
     });
   //function dbClick1(iRowIndex,rowData){
 
//        var detailInfo=window.parent.arrIframe.detailInfo;
//        if(detailInfo){
//            detailInfo.obj.src=setParam(detailInfo.src, {ID:rowData.ID,PageState:0});
//        }
//         window.parent.showDetailInfo();
//         window.parent.setReadOnly(true);

       // window.parent.Operate("showDetail");
   //}
  // function click1(iRowIndex,rowData){
 
       
    // if(parent.setOperateControl) parent.setOperateControl();
  // }
   function getSelectRowData(){
        return document.getElementById('<%=gvMaster.ClientID %>').selectRow;
   }
  // function showDetailInfo()
  // {    
    //    var selectRow=getSelectRowData();//selectRowInfo['<%=gvMaster.ClientID %>'];
    //    if(selectRow && selectRow.data){
    //        dbClick(selectRow.index,selectRow.data)
    //    }
  // }
  // function mouseOut(iRowIndex){
    //alert("mouseOut");
  // }
  // function showQuery()
  // {
  ///     $("#dialog").dialog("open");
  // }
   function operateMaster(oGrid,rowIndex,data,flag){
    parent.Operate(flag); 
   }
//  function deleteMaster(oGrid,rowIndex,data,flag){
//    parent.Operate("delete");
//  }
//  function updateMaster(oGrid,rowIndex,data,flag){
//   parent.Operate("update");
//  }
//   function newMaster(oGrid,rowIndex,data,flag){
//   parent.Operate("new");
//  }
    </script>
</head>
<body  style=" padding:0px; margin:0px; border:solid 0px black; width:100%; height:100%;">
    <form id="form1" runat="server" >
     
     <div id="divMain"  style=" padding:0px; margin:0px; text-align:center;  ">
        <div id="divQuery" style=" width:100%; height:30px">
            <table cellpadding="0" cellspacing="0"  class="QueryTable">
                <tr class="QueryTableTr">
                    <td class="QueryTableTdLabel">收款日期:</td>
                    <td class=" QueryTableTdText"> 
                        <asp:TextBox runat="server" ID="txtStartDate" class="Wdate"  onfocus="new WdatePicker(this,'%Y-%M-%D',true)"></asp:TextBox>
                        至
                        <asp:TextBox runat="server" ID="txtEndDate" class="Wdate"  onfocus="new WdatePicker(this,'%Y-%M-%D',true)"></asp:TextBox>
                    </td>
                    <td class="QueryTableTdLabel" style=" text-align:left;">
                        <asp:Button runat="server" ID="btnQuery" Text="查询" onclick="btnQuery_Click"></asp:Button>
                    </td>
                    <td class=" QueryTableTdText"></td>
                </tr>
            </table>
        </div>
        <div id="divLst">
             <asp:GridView ID="gvMaster" runat="server" AutoGenerateColumns="false"   
                 Width="100%" Height="100%" EmptyDataText="没有符合条件数据"  >
                  
                  <EmptyDataRowStyle HorizontalAlign="Center" />
                  <Columns>
                            <asp:BoundField HeaderText="序号"  HeaderStyle-Width="40"   >
                             </asp:BoundField>
                              <asp:BoundField DataField="Code" HeaderText="编号"  >
                            </asp:BoundField>
                            <asp:BoundField DataField="accounttitlename" HeaderText="收款类别"   >
                            </asp:BoundField>
                          
                             <asp:BoundField DataField="TheCustomerName" HeaderText="单位"   >
                            </asp:BoundField>
                           <asp:BoundField DataField="summoney" HeaderText="金额(元)"  DataFormatString="{0:N2}"  >
                             </asp:BoundField>
                            
                             <asp:BoundField DataField="CreateDate" HeaderText="收款时间" DataFormatString="{0:yyyy-MM-dd}" >
                            </asp:BoundField>
                             <asp:TemplateField HeaderText="状态">
                                <ItemTemplate>
                                    <asp:Label ID="lblState" runat="server" Text='<%#  UtilClass.GetDocumentStateName(Eval("State").ToString()) %>'></asp:Label>
                                </ItemTemplate>
                               
                            </asp:TemplateField> 
                            <asp:BoundField DataField="CreatePersonName" HeaderText="制单人">  
                            </asp:BoundField>
                            <asp:BoundField DataField="RealOperationDate" HeaderStyle-Width="150" HeaderText="制单时间" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" >
                            </asp:BoundField>
                        </Columns>
             </asp:GridView>
        </div>
         <div id="divPageSize"     >
             <uc1:GridViewDataMng ID="gvMasterSource" runat="server"  />
        </div>
                  
    </div>
    <asp:HiddenField runat="server" ID="hdGatheringType" Value="1" />
   
        
  
    </form>
</body>
</html>
