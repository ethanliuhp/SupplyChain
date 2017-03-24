<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default3.aspx.cs" Inherits="test_Default3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script type="text/javascript">
  function dbClick(){  
    var selectRowData=getDecodeRowData(this.Data);  
    var selectRowIndex=0; 
    var oGrid= document.getElementById('gvMaster'); 
    oGrid.selectRow={index:selectRowIndex,data:selectRowData});  //2
    oGrid.deleteRow= function(rowIndex){ 
        var oGrid= document.getElementById('gvMaster'); 
        if( oGrid.rows.length >rowIndex){
            oGrid.rows[rowIndex].style.display='none';
            var oDeleteDetailIds=document.getElementById('System.Web.UI.WebControls.HiddenField'); 
            var sIDs=oDeleteDetailIds.value; 
            var arrIDs=[];
            if(sIDs){
                arrIDs=sIDs.split(';'); 
            }
            arrIDs.push();
            oDeleteDetailIds.value= arrIDs.join(';');  
       } 
     });  //1
     if(dbClick){ 
        dbClick(selectRowIndex,selectRowData);  
      } 
  }
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr 
            Data="[{NUM:&quot;1&quot;,ID:&quot;3x0JwIAaX8aPF9kyRaJixG&quot;,VERSION:&quot;6&quot;,CODE:&quot;%E8%B4%B9%E7%94%A8_20160100001&quot;,CREATEPERSON:&quot;428&quot;,CREATEPERSONNAME:&quot;admin&quot;,HANDLEPERSON:&quot;428&quot;,HANDLEPERSONNAME:&quot;admin&quot;,CREATEDATE:&quot;2016%2F1%2F22%200%3A00%3A00&quot;,REALOPERATIONDATE:&quot;2016%2F1%2F22%2016%3A00%3A37&quot;,STATE:&quot;5&quot;,OPGSYSCODE:&quot;91cf7bf193fb4824adbcfbf5360369b3.0_8J_eQfP068UvW6Eu_qkO.&quot;,OPERATIONORG:&quot;0_8J_eQfP068UvW6Eu_qkO&quot;,OPERORGNAME:&quot;%E4%B8%AD%E5%BB%BA%E4%B8%89%E5%B1%80%E6%80%BB%E6%89%BF%E5%8C%85%E5%85%AC%E5%8F%B8&quot;,QUANTITY:&quot;0&quot;,MONEY:&quot;0&quot;,DESCRIPT:&quot;131313123123123131312&quot;,PROJECTID:&quot;3GVJd4X_rAMAQzIk%24FYU7x&quot;,PROJECTNAME:&quot;%20&quot;,YEAR:&quot;2016&quot;,MONTH:&quot;1&quot;}]" onmouseover="curBak=this.style.backgroundColor;this.style.color='#116a79';this.style.backgroundColor='#cef97b'" 
            onmouseout="this.style.backgroundColor=curBak;this.style.color='#3a6381'" 
            onclick=" var selectRowData=getDecodeRowData(this.Data);  var selectRowIndex=0; var oGrid= document.getElementById('gvMaster'); oGrid.selectRow={index:selectRowIndex,data:selectRowData});  oGrid.deleteRow= function(rowIndex){ var oGrid= document.getElementById('gvMaster'); if( oGrid.rows.length >rowIndex){oGrid.rows[rowIndex].style.display='none';var oDeleteDetailIds=document.getElementById('System.Web.UI.WebControls.HiddenField'); var sIDs=oDeleteDetailIds.value; var arrIDs=[];if(sIDs){arrIDs=sIDs.split(';'); }arrIDs.push();oDeleteDetailIds.value= arrIDs.join(';');  } });  if(click){ click(selectRowIndex,selectRowData);  } " 
            ondblclick=" var selectRowData=getDecodeRowData(this.Data);  var selectRowIndex=0; var oGrid= document.getElementById('gvMaster'); oGrid.selectRow={index:selectRowIndex,data:selectRowData});  oGrid.deleteRow= function(rowIndex){ var oGrid= document.getElementById('gvMaster'); if( oGrid.rows.length >rowIndex){oGrid.rows[rowIndex].style.display='none';var oDeleteDetailIds=document.getElementById('System.Web.UI.WebControls.HiddenField'); var sIDs=oDeleteDetailIds.value; var arrIDs=[];if(sIDs){arrIDs=sIDs.split(';'); }arrIDs.push();oDeleteDetailIds.value= arrIDs.join(';');  } });  if(dbClick){ dbClick(selectRowIndex,selectRowData);  } " 
            style="color:Black;background-color:#FFFFFF;Cursor:pointer">
			
		</tr>
        </table>
    </div>
    </form>
</body>
</html>
