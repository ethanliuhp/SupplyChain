<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test1.aspx.cs" Inherits="test_test1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        body,ul,li{margin: 0;padding: 0;font: 12px normal "宋体", Arial, Helvetica, sans-serif;list-style: none;}
        a{text-decoration: none;color: #000;font-size: 14px;}

        #divMenu{   width:100%; height:auto; overflow:hidden; margin:0 auto;}
        .tabBox{border: 1px solid #999;border-top: none;}
        .tab_con{ display:none;}

        .tabs{height: 32px; width:100%; border-bottom :1px solid #999;border-left: 1px solid #999;width: 100%;}
        .tabs li{height:31px;line-height:31px;float:left;border:1px solid #999;border-left:none;margin-bottom: -1px;background: #e0e0e0;overflow: hidden;position: relative;}
        .tabs li a {display: block;padding: 0 20px;border: 1px solid #fff;outline: none;}
        .tabs li a:hover {background: #ccc;}    
        .tabs .thistab,.tabs .thistab a:hover{background: #fff;border-bottom: 1px solid #fff;}

        .tab_con {padding:12px;font-size: 14px; line-height:175%;}
        .divCoommon{padding:5px 5px 5px 5px; margin:0px; border:0px;}
    </style>
       <script type="text/javascript" src="../JavaScript/jquery-1.10.2.js"></script>
       <script type="text/javascript">
       var divMenuID="divMenu";
       var divMenu_TabsID="tabs";
       var divMenu_TabBox="tabBox";
       function initialTab(){
           var divMenu,tabs,tabBox;
           divMenu= $("#"+divMenuID);
           if(divMenu.length==0){   
                $(document).append("<div id='"+divMenuID+"'></div>");
                divMenu= $("#"+divMenuID);
           }
           
           tabs= $("#"+divMenuID+" .tabs");
           if(tabs.length==0){
               divMenu.append(" <ul class='tabs'  ></ul>"); 
               tabs= $("#divMenu .tabs");
           }
           tabBox=$("#divMenu .tabBox");
           if(tabBox.length==0){
               divMenu.append(" <ul class='tabBox'  ></ul>"); 
               tabBox= $("#divMenu .tabBox");
           }
       }
       function addTab(tabID,sUrl){
          
           
           
       }
        addTab("tab1","master.aspx");
        addTab("tab2","detial.aspx");
       </script>
      
</head>
<body>
    <form id="form1" runat="server">
     <div id="divMenu">
     </div>
    </form>
</body>
</html>
