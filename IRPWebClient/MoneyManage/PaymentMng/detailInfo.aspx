<%@ Page Language="C#" AutoEventWireup="true" CodeFile="detailInfo.aspx.cs" Inherits="MoneyManage_PaymentMng_detailInfo" %>
<%@ Register Src="~/UserControls/GridViewDataMng.ascx" TagName="GridViewDataMng" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server"> 
    <title>Untitled Page</title>
    <style  type="text/css">
        .divTool{   height:30px; margin:0px; padding:0px; border:solid 0px black; text-align:left; display:none;}
        .divMain{ width:100%; height:100%; margin:0px; padding:0px; border:solid 0px black;}
        .divMaster{ width:100%; height:30%; margin:0px; padding:0px; border:solid 0px black;}
        .divDetail{ width:100%; height:70%; margin:0px; padding:0px; border:solid 0px black;}
        .MasterTable{width:100%;  height:100%; margin-left:auto; margin-right:auto; padding:0px; text-align:center;}
        .MasterTableTr{width:100%;  height:30px;   text-align:center;}
        .MasterTableTdLabel{width:13%;  height:100% ;  text-align:right;}
        .MasterTableTdText{width:20%;  height:100% ;  text-align:left; padding:auto 5px auto auto; padding-left:5px;}
        
         .DetailTable{width:100%;   height:100%; margin-left:auto; margin-right:auto; padding:0px; text-align:center;}
        .DetailTableTr{width:100%;  height:30px;   text-align:center;}
        .DetailTableTdLabel{width:15%;  height:100% ;  text-align:left; padding-left:5px;}
        .DetailTableTdText{width:35%;  height:100% ;  text-align:left; padding:auto 5px auto auto;padding-left:5px}
        .dialogDivDetail{width:100%; height:100%; margin:0px; padding:0px; border:solid 0px black; display:none;}
    </style>
     <link href="../../css/MainWeb.css" rel="Stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../JavaScript/jquery-ui-1.11.4/jquery-ui.min.css">
    <link rel="stylesheet" href="../../JavaScript/jquery-ui-1.11.4/jquery-ui.theme.min.css">
    <script src="../../JavaScript/jquery-ui-1.11.4/external/jquery/jquery.js"></script>
    <script src="../../JavaScript/jquery-ui-1.11.4/jquery-ui.min.js"></script>
    <link rel="stylesheet" href="../../JavaScript/jquery-ui-1.11.4/jquery-ui.structure.min.css">
<%--    <script src="../../JavaScript/jquery-1.10.2.min.js"></script>--%>
     <script src="../../JavaScript/Util.js"></script>
     <script src="../../JavaScript/DataType.js"></script>
     <script type="text/javascript" src="../../JavaScript/My97DatePicker/WdatePicker.js"></script>
     <script type="text/javascript" src="../../JavaScript/SelectDialog.js"></script>
    <script type="text/javascript">
     function validateData()
     {
        var bResult=false;
        var payType=document.getElementById("<%=cmbPaymentType.ClientID %>");
        if(payType.selectedIndex>0 &&payType.options[payType.selectedIndex].value){
            bResult=true;
        }
        else{
            showMessage("保存失败:[付款类别]不能为空!");
            payType.focus();
        }
        return bResult;
     }
     function getMasterID(){
        var sID=document.getElementById("<%=hdMasterID.ClientID %>").value;
        return sID;
    }
    //获取当前页面状态
    function pageUpdated(){
        return $("#<%=chkIsRefresh.ClientID %>")[0].checked;
    }
    function pageReadOnly(){
        var isPageReadOnly=<%=IsReadyOnly?"true":"false" %>;
        return isPageReadOnly;
    }
     $(window).ready(function(){
      
        $("input[DataType='Decimal']").blur(function(){ 
            if(this.value && !IsFloat(this.value)){
                alert("请输入数值类型");
                this.focus();
            }
        });
        // intialDialog();
         $(window).ready(function(){
        if(parent.setDetailOperateControl) parent.setDetailOperateControl();
        setReadyOnlyControl();
     });
     });
     function setReadyOnlyControl(){
         var arrReadyOnlyCtl=[ '<%=txtSupply.ClientID %>']; 
         for(var key in arrReadyOnlyCtl){
         var oCtl= document.getElementById(arrReadyOnlyCtl[key]);
           oCtl.readOnly=true ;
         }
        
     }
     
     function intialDialog(){  
     var d=null;
     var state=pageReadOnly();
     //setDialogByType(iType);
     
     var divDialog=$( "#dialogDivDetail" );
     divDialog.attr("title",state? "浏览":"编辑");
     divDialog.attr("icon",state? "icon-show":"icon-save");
     if(!pageReadOnly()){
        d=divDialog.dialog({
              autoOpen: false,
            width:400,
            modal: true,
            draggable: true,  
            resizable: true,  
            closed:true,  
            show: 'Transfer',  
            hide: 'Transfer',
            buttons: {
                    "保存": function() {
                        $("#<%=btnAddOrUpdateDetail.ClientID %>").click();
                        $(this).dialog("close");
                    },
                    "取消": function() {
                        $(this).dialog("close");
                    }
                }
          });
         }
         else{
          d=divDialog.dialog({
            autoOpen: false,
            width:400,
            modal: true 
          });
         }
         $('.ui-dialog').appendTo('form:first');
     }
     
     function saveMaster()
     {
        $("#<%=btnSaveMaster.ClientID %>").click();
       parent.setOperateControl();
     }
     function submitMaster()
     {
      $("#<%=btnSubmitMaster.ClientID %>").click();
      
     }
  
    function operateGrid(oGrid,rowIndex,data,flag)
    {
        flag=flag=="doubleclick"?(pageReadOnly()? "show":"update"):flag;//处理是只读还是
        operate(flag);
    }
     function operate(flag){   
      var selectRowInfo=  getSelectRow();
        switch (flag){
            case "show":
            {  
                setControl(true);
                showDetail( );
                if(selectRowInfo && selectRowInfo.data){
                showDetailDialog(true);}
                break;
            }
            case "new":{
                clearControl();
                setControl(false);
                showDetailDialog(true);
                break;
            }
            case "delete":{
               
                break;
            }
            case "update":{
                showDetail( );
                setControl(false);
                if(selectRowInfo && selectRowInfo.data){
                    showDetailDialog(true);
                }
                break;
            } 
        }
     }
     function showDetail( ){
       var selectRowInfo= getSelectRow();
       if(selectRowInfo && selectRowInfo.data){
         
           $("#<%=txtMoney.ClientID %>").val(selectRowInfo.data[toUpper( "Money")]);
           $("#<%=hdDetailID.ClientID %>").val(selectRowInfo.data[toUpper( "ID")]);
           $("#<%=txtDescript.ClientID %>").val(selectRowInfo.data[toUpper( "Descript")]);
       }
       else{
            alert("请选择行");
       }
     }
     function showDetailDialog(show)
     {
        intialDialog();
        var dialog= $("#dialogDivDetail");
         if(show){
           dialog.dialog("open");
         }
         else{
          dialog.dialog("open");
         }
     }
      
      var arrControl=[ '<%=txtMoney.ClientID %>','<%=txtDescript.ClientID %>','<%=hdDetailID.ClientID %>'
                    ];
     function setControl(readOnly){
        var oControl=null;
        for(var key in arrControl){
            oControl=document.getElementById(arrControl[key]);
            if(oControl){
                switch(toLower(oControl.tagName) ){
                    case "input":
                    {
                        switch(toLower(oControl.type)){
                            case 'text':{
                            readOnly ? oControl.disabled="disabled" : oControl.removeAttribute("disabled");
                           }
                            case 'button':{
                            readOnly ? oControl.disabled="disabled":oControl.removeAttribute("disabled") ;
                              break;}
                            default: {
                                break;
                            }
                        }
                        break;
                    }
                    case "textarea":{readOnly ? oControl.disabled="disabled":oControl.removeAttribute("disabled") ; break;}
                    case "select":{ readOnly ? oControl.disabled="disabled":oControl.removeAttribute("disabled") ; break;}
                    default:{
                        break;
                    }
                }
            }
        }
     }
     function clearControl()
     {
        var oControl=null;
        for(var key in arrControl){
            oControl=document.getElementById(arrControl[key]);
            if(oControl){
                switch(toLower(oControl.tagName) ){
                    case "input":
                    {
                        switch(toLower(oControl.type)){
                            case 'hidden': 
                            case 'text':{
                                oControl.value="";
                                break;
                            }
                            default: {
                                break;
                            }
                        }
                        break;
                    }
                    case "select":
                    {
                       if( oControl.options.length>0){
                            oControl.selectedIndex=0
                       }
                       break;
                    }
                    case "textarea":
                    {
                        oControl.value="";
                        break;
                     }
                    default:{
                        break;
                    }
                }
            }
        }
        //txtAccountTitle txtOrgName
   
     }
     function getSelectRow()
     {
        return document.getElementById('<%=gvDetail.ClientID %>' ).selectRow;//={rowIndex:iRowIndex,rowData:rowData};
     }
     function clearSelectRow(iType){
        return document.getElementById( getGridClientID(iType) ).selectRow={};//={rowIndex:iRowIndex,rowData:rowData};        
     }
    
    
 
    function rowDbClick(iType){
        operate(pageReadOnly()?"show":"update",iType);
    }
    function selectSupply(arrHT){
         var d=new SelectDialog("供应商",
        function(data){
            if(data){
                for(var key in arrHT){
                  document.getElementById(key).value=data[toUpper(arrHT[key])];
                }
            }
        },
        function(data){});
        d.open();
    }
    </script>
</head>
<body style=" width:99%; height:100%;">
    <form id="form1" runat="server">

    <div id="divMain"   class="divMain "   >
        <div id="divMaster" class="divMaster">
         
            <table cellpadding="0" cellspacing="0" class="MasterTable"  >
                <tr class="MasterTableTr">
                    <td nowrap="nowrap" class="MasterTableTdLabel">单据号:</td>
                    <td nowrap="nowrap" class="MasterTableTdText">
                        <asp:TextBox runat="server" ID="txtCode" Enabled="false"   Width="99%"></asp:TextBox>
                    </td>
                    <td nowrap="nowrap" class="MasterTableTdLabel">付款类别:</td>
                    <td nowrap="nowrap" class="MasterTableTdText">
                       <asp:DropDownList runat="server" ID="cmbPaymentType"></asp:DropDownList>
                    </td>
                    <td nowrap="nowrap" class="MasterTableTdLabel">付款日期:</td>
                    <td nowrap="nowrap" class="MasterTableTdText">
                        <asp:TextBox runat="server" ID="txtCreateDate" class="Wdate"  onfocus="new WdatePicker(this,'%Y-%M-%D',true)"  Width="99%"></asp:TextBox>
                    </td>
                 </tr>
                  <tr class="MasterTableTr">   
                     <td nowrap="nowrap" class="MasterTableTdLabel">单位:</td>
                    <td nowrap="nowrap" class="MasterTableTdText">
                        <asp:TextBox runat="server"   ID="txtSupply"    Width="75%"></asp:TextBox>
                        <asp:HiddenField runat="server" ID="hdSupplyID" Value="" />
                        <input type="button" value="选择" <%=IsReadyOnly?"disabled='disabled' ":"" %>  onclick="selectSupply({'<%=txtSupply.ClientID %>':'name','<%=hdSupplyID.ClientID %>':'suprelid','<%=txtBankAcctNo.ClientID %>':'bankaccount','<%=txtBankNo.ClientID %>':'bankname'})"/>
                    </td> 
                     
                     <td nowrap="nowrap" class="MasterTableTdLabel">帐号:</td>
                    <td nowrap="nowrap" class="MasterTableTdText">
                        <asp:TextBox runat="server" ID="txtBankAcctNo"   Width="99%"></asp:TextBox>
                    </td>
                    <td nowrap="nowrap" class="MasterTableTdLabel">开户行及行号:</td>
                    <td nowrap="nowrap" class="MasterTableTdText">
                        <asp:TextBox runat="server" ID="txtBankNo"   Width="99%"></asp:TextBox>
                    </td>
                 </tr>
                 <tr class="MasterTableTr">
                     <td nowrap="nowrap" class="MasterTableTdLabel">备注:</td>
                    <td nowrap="nowrap" class="MasterTableTdText" colspan="5">
                      <asp:TextBox runat="server" ID="txtRemark"  Width="99%"></asp:TextBox>
                    </td>
                 </tr>
              
            </table>
        </div>
        <div id="divDetail" class="divDetail"> 
            <table style=" width:100%; height:100%;  text-align:left;" cellpadding="0" cellspacing="0" >
               
                <tr>
                    <td style=" width:100% ; vertical-align:top;" colspan="2" >
                         <fieldset style=" width:99%; height:100% ;text-align: center;">
                            <legend>借款明细信息</legend>
                             
                         
                             <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="false"    Width="99.5%" Height="100%"  >
                                   
                                  <Columns>
                                      <asp:BoundField HeaderText="ID"   Visible="false"  ></asp:BoundField>
                                      <asp:BoundField HeaderText="序号"   HeaderStyle-Width="40"   ></asp:BoundField>
                                      
                                      <asp:BoundField HeaderText="金额(元)"    DataField="Money" DataFormatString="{0:N2}"></asp:BoundField>
                                      
                                       
                                      <asp:BoundField HeaderText="备注"    DataField="Descript" ></asp:BoundField>
                                   </Columns>
                             </asp:GridView>
                             <uc1:GridViewDataMng ID="gvDetailSource" runat="server"  /> 
                         
                          </fieldset>
                    </td>
                    
                </tr>
            </table> 
           
        </div>
    </div>
    <div class="dialogDivDetail" id="dialogDivDetail" tabindex="-1" >      
    <div  >
      <asp:HiddenField runat="server" ID="hdDetailID" />
      <asp:HiddenField runat="server" ID="hdCostType" />
         <table cellpadding="0" cellspacing="0" id="tabDetail" class=" DetailTable" >
                
                  
                 <tr class="DetailTableTr">
                     <td  nowrap="nowrap" class="DetailTableTdLabel">金额(元):</td>
                    <td nowrap="nowrap" class="DetailTableTdText">
                        <asp:TextBox runat="server" ID="txtMoney"  DataType='Decimal' Width="100%"  ></asp:TextBox>
                    </td>
                  </tr>
                  
                    <tr  class="DetailTableTr">
                        <td nowrap="nowrap" class="DetailTableTdLabel">备注:</td>
                        <td nowrap="nowrap" class="DetailTableTdText">
                            <asp:TextBox runat="server" ID="txtDescript" TextMode="MultiLine"  Height="50"   Width="100%"  ></asp:TextBox>
                        </td>
                    </tr>
                 
               
            </table>
            </div>
    
    </div>
    <div style=" display:none" >
        <asp:HiddenField runat="server" ID="hdMasterID" />
        <asp:HiddenField runat="server" ID="hdPageState" />
        <asp:HiddenField runat="server" ID="hbSelectDetailID" />
         <asp:HiddenField runat="server" ID="hdPageDeleteDetailIDs" />
         <asp:HiddenField runat="server" ID="hdPaymentType" Value="1" />
        <asp:Button runat="server" ID="btnSaveMaster"  />
        <asp:Button  runat="server" ID="btnDeleteMaster"/>
        <asp:Button runat="server" ID="btnSubmitMaster" />
        <asp:Button runat="server" ID="btnAddOrUpdateDetail" />
        <asp:CheckBox runat="server" ID="chkIsRefresh" Checked="false" />
    </div>
    </form>
</body>
</html>
