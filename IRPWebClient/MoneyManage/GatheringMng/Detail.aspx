<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Detail.aspx.cs" Inherits="MoneyManage_GatheringMng_Detail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>详细信息</title>
      <link href="../../CSS/main.css"rel="stylesheet" type="text/css" />
    
    <script src="../../jquery/jquery.min.js" type="text/javascript"></script>                       <!-- jquery-1.11.1版本 -->
    
   <style type="text/css">
        .MasterTableTr{width:100%;  height:30px;   text-align:center; border:solid 1px blue;}
        .MasterTableTdLabel{width:13%;  height:100% ;  text-align:right;border:solid 1px blue;}
        .MasterTableTdText{width:20%;  height:100% ;  text-align:left; padding:auto 5px auto auto; padding-left:5px;border:solid 1px blue;}
   </style>
 
</head>
<body>
    <form id="form1" runat="server" style="width:100%; height:100%">
      <div  id="divMaster" style="height:50px; width:100%;">
        <table cellpadding="0" cellspacing="0" style=" width:100%; height:100%; margin-top:0px;  ">
             <tr class="MasterTableTr">
            <td nowrap="nowrap" class="MasterTableTdLabel" > 
                <label for="txtCode"  >单据编号：</label>
            </td>
            <td nowrap="nowrap" class=" MasterTableTdText">
                 <input type="text" id="txtCode"    style=" width:99%"/>
            </td>
             <td nowrap="nowrap" class="MasterTableTdLabel">
                <label for="txtTheCustomerName"  >单位：</label>
             </td>
            <td nowrap="nowrap" class="MasterTableTdText">
                 <input type="text" id="txtTheCustomerName"    style=" width:75%;"/>
                 <input type="hidden" id="hdTheCustomerID"   />
                 <input type="button" value="选择"    onclick="selectCustom({'hdTheCustomerID':'cusrelid','txtTheCustomerName':'name'})" />
            </td>
            <td nowrap="nowrap" class="MasterTableTdLabel">
                 <label for="txtCreateDate"   style=" width:99%">收款日期:</label>
            </td>
            <td nowrap="nowrap" class="MasterTableTdText">
                 <input type="text" id="txtCreateDate" class="  Wdate" style=" width:99%"  onfocus="new WdatePicker(this,'%Y-%M-%D',true)" />
            </td>
        </tr>
        </table>
     </div>
     <div id="divDetail" style=" width:100%;" >
        <div id="divGrid" style=" height:100% ; width:100%">123123</div>
     </div>
      <script type="text/javascript">
        $(window).ready( function(){
             $(window).resize(function(){
                debugger;
                 var w=$(window).width()-10;
                 var h=$(window).height()-10;
                 $("#divMaster").height(50).width(w);
                 $("#divDetail").height(h-50).width(w);
             });
              $(document).resize(); 
        });
  </script>
    </form>
</body>
</html>
