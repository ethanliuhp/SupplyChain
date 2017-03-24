<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Main.aspx.cs" Inherits="MoneyManage_FactoringDataMng_Main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>保理业务</title>
    <link href="../../css/MainWeb.css" rel="Stylesheet" type="text/css" />
    <link href="../../CSS/main.css" rel="stylesheet" type="text/css" />
    <script src="../../JavaScript/jquery-1.10.2.min.js"></script>
    <script src="<%=ResolveUrl("../../JavaScript/progressLoader.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        // 重新加载子窗体
        var ifrMaster,ifrDetail;
        $(function(){
            ifrMaster = $("#ifrMaster");
            ifrDetail = $("#ifrDetailInfo");
            
            // 子表操作栏功能定义
            $("#myBack").click(function(){
                $(ifrDetail[0].contentWindow.document).find("#back").click();
            });
            // 保存主子表
            $("#mySave").click(function(){
                ifrDetail[0].contentWindow.mySave();
            });
            // 新增子表
            $("#btnAdd").click(function(){
                $(ifrDetail[0].contentWindow.document).find("#add").click();
            });
            // 提交主子表数据
            $("#mySubmit").click(function(){
                ifrDetail[0].contentWindow.mySubmit();
            });
            // 删除主表数据
            $("#myDelete").click(function(){
                ifrDetail[0].contentWindow.myDelete();
            });
            // 刷新主表
            $("#refresh").click(function(){
                // ifrMaster[0].contentWindow.location.reload();
                window.location.reload();
            });
            // 创建新主表
            $("#btnNew").click(function(){
                reloadDetail("","add");
            });
            // 主表删除
            $("#btnDel").click(function(){
                ifrMaster[0].contentWindow.deleteMaster();
            });
        });
        
        // 显示与隐藏删除按钮
        function delVisible(state){
            if(state == 0){
                $("#btnDel").show();
            }
            else{
                $("#btnDel").hide();
            }
        }
        
        // 重新加载子表
        function reloadDetail(id,pageState){
            ifrDetail.attr("src","Detail.html?id=" + id + "&pageState=" + pageState);
            winVisible(false);
        }
        
        // 主表与子表的相互切换
        function winVisible(state){
            // state为true时，显示主表，隐藏子表；state为false时，隐藏主表，显示子表
            if(state){
                $("#divDetailInfo").fadeOut("slow",function(){
                    $("#divList").fadeIn("slow");
                    $("#divDetailInfoTool").hide();
                    $("#divListTool").show();
                });
            }
            else{
            
                $("#divList").fadeOut("slow",function(){
                    $("#divDetailInfo").fadeIn("slow");
                    $("#divDetailInfoTool").show();
                    $("#divListTool").hide();
                });
            }
        }
        
    </script>

    <script type="text/javascript">
       var arrIframe=[];
        $(window).ready(function(){
           $(window).resize(function(){
                var w=$(window).width()-10;
                var h=$(window).height()-10;
                var main=$("#divMain");
                var tool=$("#divTool");
                var content=$("#divContent");
                var masterList=$("#divMasterList");
                //var detailList=$("#divDetailList");
                if(main.length>0){
                    main.width(w);
                    main.height(h);
                   content.width(w);
                   content.height(h-tool.height());
                   masterList.height(parseInt( (h-tool.height())*1));
                    masterList.width(w);
                    //detailList.height(parseInt( (h-tool.height())*0.7));
                    //detailList.width(w-10);
                   //设置主表列表大小
                   
              
                  var masterListFieldset= $("#divMasterList fieldset");
                  var masterListFrame=$("#divMasterList fieldset iframe");
                  masterListFieldset.height(masterList.height()-10);
                   masterListFieldset.width(w-10);
                  masterListFrame.height(masterList.height()-30);
                   masterListFrame.width(w-14);
                   //设置明细列表大小
//                   var detailListFieldset= $("#divDetailList fieldset");
//                  var detailListFrame=$("#divDetailList fieldset iframe");
//                  detailListFieldset.height(detailList.height()-15);
//                   detailListFieldset.width(w-12);
//                  detailListFrame.height(detailList.height()-50);
//                detailListFrame.width(w-14);
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
                    if(confirm('是否保存！')){
                        oIfrDetail.contentWindow.saveMaster();
                    }
                    break;
                }
                case "delete":{
                    if(selectRow&&selectRow.data ){
                        if( confirm('是否删除！')){
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
                        if( confirm('是否删除！')){
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
                if(confirm('是否提交！')){
                        oIfrDetail.contentWindow.submitMaster();
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
              showOut("divDetailInfo","divList");
              showOut("divDetailInfoTool","divListTool");
         }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="divMain">
        <div id="divTool">
            <div id="divListTool">
                <input type="button" id="refresh" value="刷新" />
                <input type="button" id="btnNew" value="新建" />
                <input type="button" id="btnDel" value="删除" style="display:none;" />
            </div>
            <div id="divDetailInfoTool">
                <input type="button" id="mySubmit" value="提交" class="item_edit" />
                <!-- <input type="button" id="btnAdd" value="新增" class="item_edit" /> -->
                <input type="button" id="mySave" value="保存" class="item_edit" />
                <input type="button" id="myDelete" value="删除" class="item_edit" />
                <input type="button" id="myBack" value="返回" />
            </div>
        </div>
        <div id="divContent">
            <div id="divList">
                <div id="divMasterList">
                    <fieldset>
                        <legend align="left">保理业务</legend>
                        <iframe id="ifrMaster" src="Master.aspx" class="ifr-master" frameborder="0" scrolling="auto"
                            marginheight="0" marginwidth="0"></iframe>
                    </fieldset>
                </div>
            </div>
            <div id="divDetailInfo">
                <%--<iframe id="ifrDetailInfo" class="ifr-detail" src="DetailInfo.aspx" frameborder="0"
                    scrolling="auto" marginheight="0" marginwidth="0"></iframe>--%>
                <iframe id="ifrDetailInfo" class="ifr-detail" src="Detail.html" frameborder="0"
                    scrolling="auto" marginheight="0" marginwidth="0"></iframe>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
