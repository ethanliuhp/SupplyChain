<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpFile.aspx.cs" Inherits="UpFile_UpFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
  
<head id="Head1" runat="server">
    <title></title>
     <script src="../JavaScript/jquery-1.10.2.min.js" type="text/javascript"></script>  
    <script src="../JavaScript/jquery-form.js" type="text/javascript"></script>  
       <script src="../JavaScript/Util.js"></script>
    <script type="text/javascript">
        $(function () {
         
            $("#btn").click(function () {
           
                var file=$("#btnfile");
                if(file.val()){
                    $("#fm1").ajaxSubmit({
                        url: "UpFileHandler.ashx",
                        type: "post",
                        success: function (data) {
                            //IE显示图片会默认加上<PRE></PRE>，着必须要把去除掉才能在低版本ie显示  
                            data = data.replace("<PRE>", "").replace("</PRE>", "");
                            data = data.replace("<pre>", "").replace("</pre>", "");
                            $("#hdFilePath").val(data);
                             if(parent && parent.window){
                             
                                //parent.window.frames[0]==window.location
                                var div=parent.window.document.getElementById("_dialogDivUpfile");
                                if( div &&div.sure){
                                    var  data1=getDecodeRowData(data);
                                    div.sure(data1);
                                }
                          }
                        }
                    });
                }
            });
        }) ;
        function changeData(){
           var file=$("#btnfile");
           var oBtn=$("#btn");
           if(file.val()){
                oBtn.show();
           }
           else{
                oBtn.hide();
           } 
        } 
    </script>
</head>
<body>
    <form id="fm1" method="post">
    <!--method="post"不能省略，在ie里面必不可少-->
    <div style="   text-align:center; margin-top:10px;">   
       导入文件:<input type="file" id="btnfile" name="btnfile"   onchange="changeData()"   />
        <input type="button" id="btn"  value="上传" style=" display:none;"/>
    </div>
    
    </form>
     
     <%-- 文件存放目录 --%>
    <input type="hidden" id="hdFolder" value="<%=folderPath %>"/>
    <input type="hidden" id="hdFilePath" value=""/>
</body>
</html>