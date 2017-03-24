  ///////页面状态
  var PageState={  
    Show:0,
    Update:1,
    Delete:2,
    New:3,
    Empty:4
    };
   var DocumentState=
    {
        Edit : 0,//     编辑
        Valid : 1,//     有效
        Invalid : 2, //     无效
        InAudit : 3, //     审批中
        Suspend : 4, //     挂起
        InExecute : 5, //     执行中
        Freeze : 6, //     冻结
        Completed : 7, //     结束
        Blankness : 9
    };
    ///单据是否可以编辑
  function canEditBill(rowData){
      var result=false;
      var state=null;
       state=getBillState(rowData);
       if(state){
           result=(state==DocumentState.Edit||state==DocumentState.Valid);
        }
        return result;
  }
  function canDelete(rowData)
  {
    var result=false;
      var state=null;
       state=getBillState(rowData);
       if(state){
           result=(state==DocumentState.Edit||state==DocumentState.Valid||state==DocumentState.Invalid);
        }
        return result;
  }
  //获取单据状态
  function getBillState(rowData){
    return  state= rowData? (rowData["DOCSTATE"]||rowData["STATE"]): null;
  }
  ////
  //url后面追加参数
  function appendParam(sUrl,Params){
    return setUrl(sUrl,Params,true);
  }
  //url清除以前url参数并添加
  function setParam(sUrl,Params){
    return setUrl(sUrl,Params,false);
  }
  function setUrl(sUrl,Params,isAppend){
    Params["randTime"]=new Date().getTime();
        var urlInfo=getUrlParam(sUrl);
        var arrParam=[];
        if(isAppend){//是否追加参数
            for(var key in urlInfo.params){
                if(Params[key]== undefined){
                    Params[key]=urlInfo.params[key];
                }
            }
        }
        for(var key in Params){
            arrParam.push(key+"=" +encodeURIComponent(Params[key]));
        }
        sUrl=urlInfo.url+"?"+arrParam.join("&");
      return sUrl;
    }
    function getUrlParam(sUrl){
        var result={url:sUrl,params:{}};
        var key,value;
        if(sUrl){
            var iStart=sUrl.indexOf("?");
            if(iStart>-1){
                result.url=sUrl.substr(0,iStart);
                var sTemp=sUrl.substr(iStart+1);
                var arrPara=sTemp.split("&");
                for(var i in arrPara) {
                    iStart=arrPara[i].indexOf("=");
                    if(iStart>-1){
                        key=arrPara[i].substr(0,iStart);
                        value=arrPara[i].substr(iStart+1);
                        result.params[key]=decodeURIComponent(value);
                    }
                }
            }
        }
        return result;
    }
     function decodeComponent(value){
        value=decodeURIComponent(value);
        return value;
    }
    function getDecodeRowData(data){
        var obj=[];
        try{
            var oData=eval(data)[0];
            for(var   key in oData){
                obj[key]=decodeComponent(oData[key]);
            }
        }
        catch(e){
        }
        return obj;
    }
    function toLower(value){
        if(value){
            value=value.toLowerCase();
        }
        return value;
    }
    function toUpper(value){
        if(value){
            value=value.toUpperCase();
        }
        return value;
    }
  
    function fadeOut(sHideID,sShowID){
        var obj= $("#"+sHideID);
        obj.fadeOut("slow",function(){$("#"+sShowID).fadeIn("slow");});
    }
    function showOut(sShowID,sHideID){
        var obj= $("#"+sHideID);
        obj.fadeOut("slow",function(){$("#"+sShowID).fadeIn("slow");});
    }
    
    function showMessage(sMsg){
        if(sMsg){
            alert(sMsg);
        }
    }
 
    