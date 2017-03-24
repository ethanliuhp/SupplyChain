<%@ Page Language="C#" AutoEventWireup="true" CodeFile="detailInfo.aspx.cs" Inherits="MoneyManage_CompanyAccountMng_detailInfo" %>
 
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
     <script type="text/javascript" src="../../JavaScript/upFile.js"></script>
    <script type="text/javascript">
    var EnumCostType={'间接费用':0, '其他应收':1,'其他应付':2,'管理费用':3};
    //对应的列表集合
    var arrGridID={
                      
                        '管理费用':'<%= gvManageCost.ClientID %>'
                  };
    function getGridClientID( sType){
        var sName="";
        switch(sType){
            case EnumCostType.其他应收:{sName='其他应收';break;}
            case EnumCostType.其他应付:{sName='其他应付';break;}
            case EnumCostType.管理费用:{sName='管理费用';break;}
        }
      return arrGridID[sName];
    }
     function getEnumCostType(sGridClientID){
        for(var key in arrGridID){
            if(arrGridID[key]==sGridClientID){
                switch(key){
                    case "其他应收":{return EnumCostType.其他应收;break;}
                    case "其他应付":{return  EnumCostType.其他应付;break;}
                    case "管理费用":{return  EnumCostType.管理费用;break;}
                }
            }
        }
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
        var pageState=$('#<%=hdPageState.ClientID %>').val();
        $("input[DataType='Decimal']").blur(function(){ 
            if(this.value && !IsFloat(this.value)){
                alert("请输入数值类型");
                this.focus();
            }
        });
        // intialDialog();
         $(window).ready(function(){
        if(parent.setDetailOperateControl) parent.setDetailOperateControl();
       // setReadyOnlyControl();
     });
     });
     function setReadyOnlyControl(){
         var arrReadyOnlyCtl=['<=txtBorrowSymbolAccountTtileTreeName.ClientID >',
                              '<=txtFinanceCostSymbolAccountTtileTreeName.ClientID %>',
                              '<=txtProfitSymbolAccountTtileTreeName.ClientID >', 
                              '<=txtHandOnSymbolAccountTtileTreeName.ClientID >']; 
         for(var key in arrReadyOnlyCtl){
            document.getElementById(arrReadyOnlyCtl[key]).readOnly=true ;
         }
        
     }
     function setDialogByType(iType){
        var sAccountTitleHtml="";
        var isShowOrg=false;
        var sActualMoneyHtml="";
        var isBudgetMoney=false;
        var isPartnerType=false;
         switch(iType){
            case EnumCostType.其他应付:
            {
                sAccountTitleHtml="费用类别:";
                isShowOrg=false;
                sActualMoneyHtml='金额(元):';
                isBudgetMoney=false;
                isPartnerType=true;
                break;
            }
            case EnumCostType.其他应收:
            {
               sAccountTitleHtml="费用类别:";
                isShowOrg=false;
                sActualMoneyHtml='金额(元):';
                isBudgetMoney=false;
                isPartnerType=true;
                break;
            }
           case EnumCostType.管理费用:
            {
               sAccountTitleHtml="费用类别:";
                isShowOrg=true;
                sActualMoneyHtml='实际支出(元):';
                isBudgetMoney=true;
                isPartnerType=false;
                break;
            }
            case EnumCostType.间接费用:
            {
                sAccountTitleHtml="会计科目:";
                isShowOrg=false;
                sActualMoneyHtml='金额(元):';
                break;
            }
         }
         $("#tabDetail tr:eq(0) td:eq(0)").html(sAccountTitleHtml);
         isShowOrg ? $("#tabDetail tr:eq(1)").show():$("#tabDetail  tr:eq(1)").hide();
         $("#tabDetail tr:eq(2) td:eq(0)").html(sActualMoneyHtml);
         isBudgetMoney? $("#tabDetail tr:eq(3)").show():$("#tabDetail  tr:eq(3)").hide();
         isPartnerType? $("#tabDetail tr:eq(4)").show():$("#tabDetail  tr:eq(4)").hide();
       
     }
     function intialDialog(iType){  
     var d=null;
     var state=pageReadOnly();
     setDialogByType(iType);
     document.getElementById("<%=hdCostType.ClientID %>").value=iType;
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
        operate(flag,getEnumCostType(oGrid.id));
    }
     function operate(flag,sType){   
      var selectRowInfo=  getSelectRow(sType);
        switch (flag){
            case "show":
            {  
                setControl(true);
                showDetail(sType);
                if(selectRowInfo && selectRowInfo.data){
                showDetailDialog(true,sType);}
                break;
            }
            case "new":{
                clearControl();
                setControl(false);
                showDetailDialog(true,sType);
                break;
            }
            case "delete":{
               
                break;
            }
            case "update":{
                showDetail(sType);
                setControl(false);
                if(selectRowInfo && selectRowInfo.data){
                    showDetailDialog(true,sType);
                }
                break;
            } 
        }
     }
     function showDetail(sType){
       var selectRowInfo= getSelectRow(sType);
       if(selectRowInfo && selectRowInfo.data){
           $("#<%=txtAccountTitle.ClientID %>").val(selectRowInfo.data[toUpper( "AccountTitleName")]);
           $("#<%=txtMoney.ClientID %>").val(selectRowInfo.data[toUpper( "Money")]);
           $("#<%=txtBudgetMoney.ClientID %>").val(selectRowInfo.data[toUpper( "BudgetMoney")]);
           $("#<%=hdAccountTitleID.ClientID %>").val(selectRowInfo.data[toUpper( "AccountTitle")]);
           $("#<%=hdDetailID.ClientID %>").val(selectRowInfo.data[toUpper( "ID")]);
           $("#<%=txtOrgName.ClientID %>").val(selectRowInfo.data[toUpper( "OrgInfoName")]);
           $("#<%=hdOrgID.ClientID %>").val(selectRowInfo.data[toUpper( "OrgInfoID")]);
           $("#<%=selPartnerType.ClientID %>").val(selectRowInfo.data[toUpper( "PartnerType")]);
           $("#<%=txtDescript.ClientID %>").val(selectRowInfo.data[toUpper( "Descript")]);
       }
       else{
            alert("请选择行");
       }
     }
     function showDetailDialog(show,iType)
     {
        intialDialog(iType);
        var dialog= $("#dialogDivDetail");
         if(show){
           dialog.dialog("open");
         }
         else{
          dialog.dialog("open");
         }
     }
      
      var arrControl=[ '<%=txtMoney.ClientID %>',
                    '<%=txtBudgetMoney.ClientID %>',
                    '<%=hdAccountTitleID.ClientID %>',"<%=hdDetailID.ClientID %>",
                    '<%=hdCostType.ClientID %>', 
                    '<%=hdOrgID.ClientID %>','<%=txtDescript.ClientID %>','<%=selPartnerType.ClientID %>',
                    'btnSelectAccountTitleTree','btnSelectOrg'
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
                    default:{
                        break;
                    }
                }
            }
        }
        //txtAccountTitle txtOrgName
        document.getElementById("<%=txtAccountTitle.ClientID %>").value="";
        document.getElementById("<%=txtOrgName.ClientID %>").value="";
     }
     function getSelectRow(iType)
     {
        
        return document.getElementById(getGridClientID(iType) ).selectRow;//={rowIndex:iRowIndex,rowData:rowData};
     }
     function clearSelectRow(iType){
        return document.getElementById( getGridClientID(iType) ).selectRow={};//={rowIndex:iRowIndex,rowData:rowData};        
     }
    function selectAccountTitleTree(sCtlIDAccountID,sCtlNameAccountName){
        var d=new SelectDialog("会计科目管理费用",
        function(data){
            if(data){
   
                document.getElementById(sCtlIDAccountID).value=data.ID;
                document.getElementById(sCtlNameAccountName).value=data.NAME;
            }
        },
        function(data){});
        d.open();
    }
     function selectOrgInfo(sCtlIDAccountID,sCtlNameAccountName){
        var d=new SelectDialog("组织",
        function(data){
            if(data){
   
                document.getElementById(sCtlIDAccountID).value=data.ID;
                document.getElementById(sCtlNameAccountName).value=data.NAME;
            }
        },
        function(data){});
        d.open();
    }
    function gvOtherReceiveDbClick(rowIndex,Data){
        rowDbClick(EnumCostType.其他应收);
    }
    function gvOtherPayoutDbClick(rowIndex,data){
         rowDbClick(EnumCostType.其他应付);
    }
     function gvManageCostDbClick(rowIndex,data){
         rowDbClick(EnumCostType.管理费用);
    }
    function rowDbClick(iType){
        operate(pageReadOnly()?"show":"update",iType);
    }
    function importExcel(){
       var filePath='<%=hdFilePath.ClientID %>';
       var btnImport='<%=btnImportExcel.ClientID %>';
       var oBtnImport=$("#"+btnImport);
       var oFilePath=  $("#"+filePath);
       oFilePath.val();
       var  oUpFile=new upFile({name:"excel",sure:function(data ){   oFilePath.val(data.FilePath);oBtnImport.click();}});
       oUpFile.open();
    }
    </script>
</head>
<body style=" width:99%; height:100%;">
    <form id="form1" runat="server">
      
    <div id="divMain"   class="divMain "   >
        <div id="divMaster" class="divMaster">
         
            <table cellpadding="0" cellspacing="0" class="MasterTable"  >
                <tr class="MasterTableTr">
                    <td nowrap="nowrap" class="MasterTableTdLabel">截至日期:</td>
                    <td nowrap="nowrap" class="MasterTableTdText">
                        <asp:TextBox runat="server" ID="txtEndTime" class="Wdate"  onfocus="new WdatePicker(this,'%Y-%M-%D',true)"  Width="99%"></asp:TextBox>
                    </td>
                    <td nowrap="nowrap" class="MasterTableTdLabel"   >
                         年份: 
                    </td>
                    <td nowrap="nowrap" class="MasterTableTdText"   >
                    <asp:DropDownList runat="server" ID="dpLstYear" Width="36%"></asp:DropDownList>
                         &nbsp; &nbsp; 月份:<asp:DropDownList runat="server" ID="dpLstMonth" Width="36%"></asp:DropDownList>
                     
                    </td>
                     <td nowrap="nowrap" class="MasterTableTdLabel">借款(元):</td>
                    <td nowrap="nowrap" class="MasterTableTdText">
                        <asp:TextBox ID="txtBorrowSymbolMoney" runat="server" DataType='Decimal'  Width="99%"></asp:TextBox>
                        
                    </td>
                    
                 </tr>
                <tr class="MasterTableTr">
                    <td nowrap="nowrap" class="MasterTableTdLabel">财务费用(元):</td>
                    <td nowrap="nowrap" class="MasterTableTdText">
                        <asp:TextBox ID="txtFinanceCostSymbolMoney" DataType='Decimal'  runat="server"  Width="99%"></asp:TextBox>
                        
                    </td>
                
                     <td nowrap="nowrap" class="MasterTableTdLabel">货币上交(元):</td>
                    <td nowrap="nowrap" class="MasterTableTdText">
                        <asp:TextBox ID="txtHandOnSymbolMoney" runat="server" DataType='Decimal' Width="99%"></asp:TextBox>
                        
                    </td>
                    <td nowrap="nowrap" class="MasterTableTdLabel" colspan="2">
                     
                        <input type="button" value="导入Excel" onclick="importExcel()" />
                    </td>
                    
                 </tr>
            </table>
        </div>
        <div id="divDetail" class="divDetail"> 
            <table style=" width:100%; height:100%;  text-align:left;" cellpadding="0" cellspacing="0" >
               
                <tr>
                    <td style=" width:100% ; vertical-align:top;" colspan="2" >
                         <fieldset style=" width:99%; height:100% ;text-align: center;">
                            <legend>管理费用信息</legend>
                            <div id="divManageCost">
                            <div id="divManageCostTool" class="divTool">    
                                <input type="button" value="新增" <%=PageState==PageState.New ||PageState==PageState.Update ? "":"disabled='disabled'" %>  onclick="operate('new',EnumCostType.管理费用)" />
                                <input type="button" value="修改" <%=PageState==PageState.New ||PageState==PageState.Update ? "":"disabled='disabled'" %> onclick="operate('update',EnumCostType.管理费用)" />
                                <input type="button" value="删除" <%=PageState==PageState.New ||PageState==PageState.Update ? "":"disabled='disabled'" %> onclick="operate('delete',EnumCostType.管理费用)" />
                                <input type="button" value="详细"  onclick="operate('show',EnumCostType.管理费用)" />
                            </div>
                             <asp:GridView ID="gvManageCost" runat="server" EnableViewState="false" AutoGenerateColumns="false"    Width="99.5%" Height="100%"  >
                                   
                                  <Columns>
                                      <asp:BoundField HeaderText="ID"   Visible="false"  ></asp:BoundField>
                                      <asp:BoundField HeaderText="序号" HeaderStyle-Width="40"    ></asp:BoundField>
                                      <asp:BoundField HeaderText="会计科目ID"   DataField="AccountTitle"  Visible="false"  ></asp:BoundField>
                                      <asp:BoundField HeaderText="费用类别"    DataField="AccountTitleName" ></asp:BoundField>
                                      <asp:BoundField HeaderText="所属部门ID"   DataField="OrgInfo"  Visible="false"  ></asp:BoundField>
                                      <asp:BoundField HeaderText="所属部门"    DataField="OrgInfoName" ></asp:BoundField>
                                      <asp:BoundField HeaderText="预算金额(元)"    DataField="BudgetMoney"  DataFormatString="{0:N2}"></asp:BoundField>
                                      <asp:BoundField HeaderText="实际支出(元)"    DataField="Money" DataFormatString="{0:N2}"></asp:BoundField>
                                      <asp:BoundField HeaderText="比率(%)"    DataField="Rate" DataFormatString="{0:F2}" ></asp:BoundField>
                                      <asp:BoundField HeaderText="备注"    DataField="Descript" ></asp:BoundField>
                                   </Columns>
                             </asp:GridView>
                             <uc1:GridViewDataMng ID="gvManageCostSource" runat="server"  /> 
                          </div>
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
                    <td nowrap="nowrap" class="DetailTableTdLabel" >费用类别:</td>
                    <td nowrap="nowrap" class="DetailTableTdText">
                        <asp:TextBox runat="server" ID="txtAccountTitle"      Enabled="false"  Width="75%"></asp:TextBox>
                        <asp:HiddenField runat="server" ID="hdAccountTitleID" />
                        <input  type="button"   id="btnSelectAccountTitleTree"      value="选择" onclick="selectAccountTitleTree('<%=hdAccountTitleID.ClientID %>','<%=txtAccountTitle.ClientID %>')" />
                    </td>
                 </tr>
                  <tr class="DetailTableTr">
                    <td nowrap="nowrap" class="DetailTableTdLabel" >所属部门:</td>
                    <td nowrap="nowrap" class="DetailTableTdText">
                        <asp:TextBox runat="server" ID="txtOrgName"     Enabled="false"   Width="75%"></asp:TextBox>
                        <asp:HiddenField runat="server" ID="hdOrgID" />
                        <input  type="button"  id="btnSelectOrg"    value="选择"  onclick="selectOrgInfo('<%=hdOrgID.ClientID %>','<%=txtOrgName.ClientID %>')" />
                    </td>
                 </tr>
                 <tr class="DetailTableTr">
                     <td  nowrap="nowrap" class="DetailTableTdLabel">实际支出(元):</td>
                    <td nowrap="nowrap" class="DetailTableTdText">
                        <asp:TextBox runat="server" ID="txtMoney"  DataType='Decimal' Width="100%"  ></asp:TextBox>
                    </td>
                  </tr>
                  <tr  class="DetailTableTr">
                    <td nowrap="nowrap" class="DetailTableTdLabel">预算金额(元):</td>
                    <td nowrap="nowrap" class="DetailTableTdText">
                        <asp:TextBox runat="server" ID="txtBudgetMoney" DataType='Decimal'  Width="100%"  ></asp:TextBox>
                    </td>
                    </tr>
                    <tr  class="DetailTableTr">
                        <td nowrap="nowrap" class="DetailTableTdLabel">伙伴类型:</td>
                        <td nowrap="nowrap" class="DetailTableTdText">
                            <asp:DropDownList runat="server" ID="selPartnerType"  Width="80px"  ></asp:DropDownList>
                        </td>
                    </tr>
                    <tr  class="DetailTableTr">
                        <td nowrap="nowrap" class="DetailTableTdLabel">备注:</td>
                        <td nowrap="nowrap" class="DetailTableTdText">
                            <asp:TextBox runat="server" ID="txtDescript"   Width="100%"  ></asp:TextBox>
                        </td>
                    </tr>
                 
               
            </table>
            </div>
    
    </div>
    <div style=" display:none" >
    <asp:HiddenField runat="server" ID="hdFilePath" />
        <asp:HiddenField runat="server" ID="hdMasterID" />
        <asp:HiddenField runat="server" ID="hdPageState" />
        <asp:HiddenField runat="server" ID="hbSelectDetailID" />
         <asp:HiddenField runat="server" ID="hdPageDeleteDetailIDs" />
        <asp:Button runat="server" ID="btnSaveMaster"  />
        <asp:Button  runat="server" ID="btnDeleteMaster"/>
        <asp:Button runat="server" ID="btnSubmitMaster" />
        <asp:Button runat="server" ID="btnAddOrUpdateDetail" />
        <asp:CheckBox runat="server" ID="chkIsRefresh" Checked="false" />
        <asp:Button runat="server" ID="btnImportExcel" OnClick="btnImportExcel_Click" />
    </div>
    </form>
</body>
</html>
