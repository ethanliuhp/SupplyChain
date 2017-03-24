<%@ Page Language="C#" AutoEventWireup="true" CodeFile="main.aspx.cs" Inherits="MoneyManage_PaymentMng_main" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
     <link href="../../css/MainWeb.css" rel="Stylesheet" type="text/css" />
    <link href="../../CSS/main.css"rel="stylesheet" type="text/css" />
   
    <script src="../../JavaScript/jquery-1.10.2.min.js"></script>
        <script src="../../JavaScript/Util.js"></script>
      <script src="<%=ResolveUrl("../../JavaScript/progressLoader.js")%>" type="text/javascript"></script>
      <script type="text/javascript" >
       var arrIframe=[];
        $(window).ready(function(){
           $(window).resize(function(){
                var w=$(window).width()-10;
                var h=$(window).height()-10;
                var main=$("#divMain");
                var tool=$("#divTool");
                var content=$("#divContent");
                var masterList=$("#divMasterList");
                var detailList=$("#divDetailList");
                if(main.length>0){
                    main.width(w);
                    main.height(h);
                   content.width(w);
                   content.height(h-tool.height());
                   
                   
                   //设置主表列表大小
                    masterList.height(parseInt( (h-tool.height())*1));
                    masterList.width(w);
                  var masterListFieldset= $("#divMasterList fieldset");
                  var masterListFrame=$("#divMasterList fieldset iframe");
                  masterListFieldset.height(masterList.height()-10);
                   masterListFieldset.width(w-10);
                  masterListFrame.height(masterList.height()-30);
                   masterListFrame.width(w-14);
                   //设置明细列表大小
                    detailList.height(parseInt( (h-tool.height())*1));
                    detailList.width(w);
                   //var detailListFieldset= $("#divDetailList fieldset");
                  var detailListFrame=$("#divDetailList fieldset iframe");
                  //detailListFieldset.height(detailList.height()-10);
                  // detailListFieldset.width(w-10);
                  detailListFrame.height(detailList.height()-30);
                detailListFrame.width(w-14);
                }
           });
          
           $(document).resize();
           $("#divList").show();
           $("#divListTool").show();
           $("#btnBack").click(function(){
                showList();
           });
           //$("#btnDetailInfo").click(function(){
            //    var masterWindow= $("#ifrMaster")[0].contentWindow;
             //   masterWindow.showDetailInfo();
              //   showDetailInfo();
          // });
          // $("#btnQuery").click(function(){
          //  debugger;
           //var masterWindow= $("#ifrMaster")[0].contentWindow;
           //  if(masterWindow && masterWindow.showQuery){
            //    masterWindow.showQuery();
            //    }
          // });
           
            loadProgress();
         
            arrIframe={  mainInfo:{obj:$("#ifrMaster")[0],src:$("#ifrMaster")[0].src},
                detailInfo:{obj:$("#ifrDetailInfo")[0],src:$("#ifrDetailInfo")[0].src} };
            
        });
        function Operate(flag){
        var oIfrDetail=  arrIframe.detailInfo.obj ;
        var oIfrMaster=arrIframe.mainInfo.obj;
        var selectRow=oIfrMaster.contentWindow.getSelectRowData();
            switch(flag){
                case "new":{
                     oIfrDetail.src=setParam(oIfrDetail.src,{PageState:PageState.New});
                     showDetailInfo();
                    break;
                }
                case "save":{
                    if(oIfrDetail.contentWindow.validateData && oIfrDetail.contentWindow.validateData("保存")){
                        if(confirm('是否保存?')){
                            oIfrDetail.contentWindow.saveMaster();
                        }
                    }
                    break;
                }
                case "delete":{
                    if(selectRow&&selectRow.data ){
                        if( confirm('是否删除?')){
                             oIfrDetail.src=setParam(oIfrDetail.src,{ID:selectRow.data.ID,PageState:PageState.Delete});
                             //showDetailInfo();
                             }
                         }
                     else{
                        alert("请选择行");
                      }
                    break;
                }
                  case "deleteCur":{
                    var sID=oIfrDetail.contentWindow.getMasterID();
                    if(sID ){
                        if( confirm('是否删除?')){
                             oIfrDetail.src=setParam(oIfrDetail.src,{ID: sID,PageState:PageState.Delete});
                             //showDetailInfo();
                             }
                         }
                     else{//如果删除是为保存的单据
                          showList(false);
                      }
                    break;
                }
                case "update":{
                  if(selectRow&&selectRow.data){
                    oIfrDetail.src=setParam(oIfrDetail.src, {ID:selectRow.data.ID,PageState:PageState.Update});
                    showDetailInfo();
                    }
                    else{
                         alert("请选择行");
                    }
                    break;
                }
                case "submit":{
                    if(oIfrDetail.contentWindow.validateData && oIfrDetail.contentWindow.validateData("提交")){
                        if(confirm('是否提交?')){
                            oIfrDetail.contentWindow.submitMaster();
                        }
                    }
                    break;
                }
                case "showDetail":
                case "doubleclick":
                {
                    // oIfrMaster.contentWindow.showDetailInfo();
                    oIfrDetail.src=setParam(oIfrDetail.src, {ID:selectRow.data.ID,PageState:PageState.Show});
                    showDetailInfo();
                    break;
                }
                
                case "back":{
                    showList(oIfrDetail.contentWindow.pageUpdated());
                    break;
                }
                case "refresh":{
                    oIfrMaster.src=setParam(oIfrMaster.src, {});
                    break;
                }
                case "query":{
                    break;
                }
                default:{
                    break;
                }
            }
            setOperateControl();
        }
        var arrControl=['btnSubmit', 'btnSave' ,'btnUpdate1' ,'btnDelete1'];
      function setReadOnly(flag){
        var oIfrMaster=arrIframe.mainInfo.obj;
        var selectRow=oIfrMaster.contentWindow.getSelectRowData();
         var arrShowControl=[];
         var arrHideControl=[];
        if(canEditBill(selectRow.data)){
           arrControl=['btnSubmit', 'btnSave' ,'btnUpdate1' ,'btnDelete1'];
        }
        else{
            
        }
      }
      function setOperateControl()
      {
            setDetailOperateControl();
            setMasterOperateControl();
      }
       // 'btnNew1' 'btnSubmit', 'btnSave' ,'btnUpdate1' ,'btnDelete1' 'btnBack'
      function setDetailOperateControl(){
          var arrHideControl=[];
          var arrShowControl=[];
          if(arrIframe.detailInfo==null) return;
          var detailPageReadOnly=arrIframe.detailInfo.obj.contentWindow.pageReadOnly();
          if(detailPageReadOnly){
             arrShowControl.push('btnBack');arrShowControl.push('btnNew1');arrHideControl.push('btnDelete1');
             arrHideControl.push('btnSubmit');arrHideControl.push('btnSave'); //arrHideControl.push('btnUpdate1');
          }
          else{
             arrShowControl.push('btnBack');arrShowControl.push('btnNew1');arrShowControl.push('btnSubmit');
             arrShowControl.push('btnSave');arrShowControl.push('btnDelete1');//arrShowControl.push('btnUpdate1');
          }
          for(var key in arrHideControl){$("#"+arrHideControl[key]).hide()}
          for(var key in arrShowControl){$("#"+arrShowControl[key]).show()}
      }
      
  
      function setMasterOperateControl(){
        
      //'btnQuery'  'btnReflesh' 'btnDetailInfo' 'btnNew'   'btnUpdate'   'btnDelete'
      var arrHideControl=[];
      var arrShowControl=[];
        if(arrIframe.mainInfo){
            var oIfrMaster=arrIframe.mainInfo.obj;
            var selectRow=oIfrMaster.contentWindow.getSelectRowData();
            
           
            if(selectRow && selectRow.data){
                var billData=selectRow.data;
                if(canEditBill(billData)){
                      arrShowControl.push('btnReflesh');arrShowControl.push('btnDetailInfo'); arrShowControl.push('btnNew');
                      arrShowControl.push('btnUpdate');arrShowControl.push('btnDelete'); 
                      
                }
                else if(canDelete(billData)){
                       arrShowControl.push('btnReflesh');arrShowControl.push('btnDetailInfo'); arrShowControl.push('btnNew');
                      arrHideControl.push('btnUpdate');arrShowControl.push('btnDelete');    
                }
                else{
                     arrShowControl.push('btnReflesh');arrShowControl.push('btnDetailInfo'); arrShowControl.push('btnNew');
                     arrHideControl.push('btnUpdate');arrHideControl.push('btnDelete'); 
                }
            }
            else
            {
               arrShowControl.push('btnReflesh');arrHideControl.push('btnDetailInfo'); arrShowControl.push('btnNew');
               arrHideControl.push('btnUpdate');arrHideControl.push('btnDelete');      
            }
        }
        else{
              arrShowControl.push('btnReflesh');arrHideControl.push('btnDetailInfo'); arrShowControl.push('btnNew');
               arrHideControl.push('btnUpdate');arrHideControl.push('btnDelete'); 
        }
        for(var key in arrHideControl){$("#"+arrHideControl[key]).hide()}
        for(var key in arrShowControl){$("#"+arrShowControl[key]).show()}
      }
         function showList(refresh)
         {
           if(refresh){
      
               var oIfrMaster=arrIframe.mainInfo.obj;
               oIfrMaster.src=setParam(oIfrMaster.src,{});
            }
           // $("#divList").show();
            //$("#divDetailInfo").hide();
            //$("#divListTool").show();
            //$("#divDetailInfoTool").hide();
              fadeOut("divDetailInfo","divList");
              fadeOut("divDetailInfoTool","divListTool");
         }  
         function showDetailInfo(refresh)
         {
            if(refresh){
                 var oIfrDetail=  arrIframe.detailInfo.obj ;
                oIfrDetail.src=setParam(oIfrDetail.src,{});
             }
//            $("#divList").hide();
//            $("#divDetailInfo").show();
//            $("#divListTool").hide();
//            $("#divDetailInfoTool").show();
              showOut("divDetailInfo","divList");
              showOut("divDetailInfoTool","divListTool");
         }
      function btnRefreshClick(){
      alert("");
      }
      
  
  
      </script>
</head>
<body style=" width:100%; height:100%; margin:0px; padding:0px; ">
    <form id="form1" runat="server">
    <div id="divMain"   >
        <div id="divTool">
            <div id="divListTool">
                <input  type="button" id="btnQuery" value="查询"  style=" display:none"  onclick="Operate('query')" />
                <input  type="button" id="btnReflesh" value="刷新"  onclick="Operate('refresh')" />
                <input  type="button" id="btnDetailInfo" value="详细" onclick="Operate('showDetail')" />
                <input  type="button" id="btnNew" value="新建" onclick="Operate('new')" />
                <input  type="button" id="btnUpdate" value="修改" onclick="Operate('update')" />
                <input  type="button" id="btnDelete" value="删除" onclick="Operate('delete')" />
                
            </div>
            <div id="divDetailInfoTool">
                <input  type="button" id="btnNew1" value="新建" onclick="Operate('new')" />
                <input  type="button" id="btnSubmit"   value="提交"  onclick="Operate('submit')" />
                <input  type="button" id="btnSave" value="保存" onclick="Operate('save')" />
                <input  type="button" id="btnUpdate1" value="修改"  style=" display:none;" onclick="Operate('update')" />
                <input  type="button" id="btnDelete1" value="删除" onclick="Operate('deleteCur')" />
                <input  type="button" id="btnBack" value="返回" onclick="Operate('back')" />
            </div>
        </div>
        <div id="divContent">
            <div id="divList"  >
                 <div id="divMasterList">
                    <fieldset>
                        <legend align="left" >付款单</legend>
                        <iframe id="ifrMaster"   src="master.aspx" frameborder="0" scrolling="auto" marginheight="0" marginwidth="0" style=" margin:0px;  padding:0px;" ></iframe>
                    </fieldset>
                 </div>
                 <%--<div id="divDetailList">
                    <fieldset>
                        <legend>明细列表</legend>
                        <iframe id="ifrDetail" frameborder="0" scrolling="auto" marginheight="0" marginwidth="0"  src="detail.aspx"></iframe>
                    </fieldset>
                 </div>--%>
            </div>
            <div id="divDetailInfo">
                <iframe  id="ifrDetailInfo"   src="detailInfo.aspx" frameborder="0" scrolling="auto" marginheight="0" marginwidth="0"  ></iframe>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
