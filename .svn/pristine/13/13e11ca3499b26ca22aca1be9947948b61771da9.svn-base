// JScript 文件
function showDialog(url)
{
    var reValue=window.showModalDialog(url,window,'dialogWidth: 733px;dialogHeight: 550px;status:no;center: yes;resizable:on;');
    return reValue;
}

function showCommonDialog(url,feature)
{
   var reValue=window.showModalDialog(url,window,feature);
   return reValue;
}

//Sets value to html input text 
function setInputTextValue(inputTextName,value)
{
     var objs=document.getElementsByTagName("input");
     for(var i=0;i<objs.length;i++)
     {
        if(objs[i].type=="text")
        {
            
            if(objs[i].name.indexOf(inputTextName)>=0&&value!=null)
            {
                objs[i].value=value;
                break;
            }
       }
    }
    
}

//Sets value to html textarea
function setTextareaValue(textareaName,value)
{
      var objs=document.getElementsByTagName("textarea");
     for(var i=0;i<objs.length;i++)
     {      
           
            if(objs[i].name.indexOf(textareaName)>=0&&value!=null)
            {
                objs[i].value=value;
                break;
            }
       
    }
}

//Sets valuet to html input 
function setInputValue(inputType,inputName,value)
{
      var objs=document.getElementsByTagName("input");
     for(var i=0;i<objs.length;i++)
     {
        if(objs[i].type==inputType)
        {
            
            if(objs[i].name.indexOf(inputName)>=0&&value!=null)
            {
                objs[i].value=value;
                break;
            }
       }
    }
}