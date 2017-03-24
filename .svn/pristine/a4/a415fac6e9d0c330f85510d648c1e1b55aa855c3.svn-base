<%@ Page Language="C#" AutoEventWireup="true" CodeFile="main.aspx.cs" Inherits="test_menu_main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        #divMain{ padding:0px; margin:0px; overflow:hidden; padding-left:5px; padding-right:5px;  padding-top:5px;  border-width:0px;  width:100%; text-align:center;}
        #divTool{padding:0px; margin:0px; overflow:hidden; border-width:0px;  width:100%; height:30px; text-align:left; vertical-align:middle;}
        #divContent{padding:0px ; margin:0px; overflow:hidden; border-width:0px;  width:100%;  height:auto; }
        #divList,#divDetailInfo{padding:0px; margin:0px;  border:none;  width:100%;  height:100%; display:none; }
        #divListTool ,#divDetailInfoTool{padding:0px; margin:0px; overflow:hidden; border:none;  width:100%;  height:auto; display:none; height:100%;   text-align:left; vertical-align:middle;}
        #divListTool input,#divDetailInfoTool input{   width:50px; height:22px; border:#F00 1px solid; background:#69F; color:#FFF; margin-left:5px; margin-right:5px; margin-top:3px;}
        #ifrDetailInfo{ margin:0px; padding:0px; border:solid 0px blue;height:100%;  width:100%;}
        #ifrMaster,#ifrDetail{ margin:0px; padding:0px; border:solid 0px blue;   width:100%;  padding-top:5px; padding-bottom:5px;}
        fieldset{width:100%; height:100%; margin:0px; padding:0px; border:solid 1px blue; }
        legend{margin:0px; padding:0px; font-size:18px;font-style:normal; font-family:@楷体; }
        #divMasterList{margin:0px; padding:0px;  border-width:0px; width:100%;   padding-bottom:10px; }
        #divDetailList{margin:0px; padding:0px;   border-width:0px; width:100%;  }
    </style>
    <script type="text/javascript" src="../../JavaScript/jquery-1.10.2.js"></script>
    <script type="text/javascript">
        $(window).ready(function(){
           $(window).resize(function(){
                var w=$(window).width()-10;
                var h=$(window).height();
                var main=$("#divMain");
                var tool=$("#divTool");
                var content=$("#divContent");
                var masterList=$("#divMasterList");
                var detailList=$("#divDetailList");
                if(main.length>0){
                    main.width(w);
                    main.height(h);
                   content.width(w-10);
                   content.height(h-tool.height());
                   masterList.height(parseInt( (h-tool.height())*0.3));
                    masterList.width(w-10);
                    detailList.height(parseInt( (h-tool.height())*0.7));
                    detailList.width(w-10);
                   //设置主表列表大小
                   
              
                  var masterListFieldset= $("#divMasterList fieldset");
                  var masterListFrame=$("#divMasterList fieldset iframe");
                  masterListFieldset.height(masterList.height());
                   masterListFieldset.width(w-12);
                  masterListFrame.height(masterList.height()-30);
                   masterListFrame.width(w-14);
                   //设置明细列表大小
                   var detailListFieldset= $("#divDetailList fieldset");
                  var detailListFrame=$("#divDetailList fieldset iframe");
                  detailListFieldset.height(detailList.height()-15);
                   detailListFieldset.width(w-12);
                  detailListFrame.height(detailList.height()-50);
                detailListFrame.width(w-14);
                }
           });
           $(document).resize();
           $("#divList").show();
           $("#divListTool").show();
           $("#btnBack").click(function(){
                $("#divList").show();
                $("#divDetailInfo").hide();
                $("#divListTool").show();
                $("#divDetailInfoTool").hide();
           });
           $("#btnDetailInfo").click(function(){
                $("#divList").hide();
                $("#divDetailInfo").show();
                $("#divListTool").hide();
                $("#divDetailInfoTool").show();
           });
        });
    </script>
</head>
<body style=" width:100%; height:100%; margin:0px; padding:0px; ">
    <form id="form1" runat="server">
    <div id="divMain">
        <div id="divTool">
            <div id="divListTool">
                <input  type="button" id="btnQuery" value="查询"/>
                <input  type="button" id="btnNew" value="新增"/>
                <input  type="button" id="btnDetailInfo" value="详细"/>
            </div>
            <div id="divDetailInfoTool">
                <input  type="button" id="btnSubmit"   value="提交" />
                <input  type="button" id="btnSave" value="保存"/>
                <input  type="button" id="btnDelete" value="删除"/>
                <input  type="button" id="btnBack" value="返回"/>
            </div>
        </div>
        <div id="divContent">
            <div id="divList"  >
                 <div id="divMasterList">
                    <fieldset>
                        <legend>主表列表</legend>
                        <iframe id="ifrMaster"   src="master.aspx" frameborder="0" scrolling="auto" marginheight="0" marginwidth="0"></iframe>
                    </fieldset>
                 </div>
                 <div id="divDetailList">
                    <fieldset>
                        <legend>明细列表</legend>
                        <iframe id="ifrDetail" frameborder="0" scrolling="auto" marginheight="0" marginwidth="0"  src="detail.aspx"></iframe>
                    </fieldset>
                 </div>
            </div>
            <div id="divDetailInfo">
                <iframe  id="ifrDetailInfo" src="master.aspx" frameborder="0" scrolling="auto" marginheight="0" marginwidth="0"  ></iframe>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
