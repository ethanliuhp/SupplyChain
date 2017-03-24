<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowAccountTitleTree.aspx.cs" Inherits="Show_ShowAccountTitleTree" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>选择会计科目</title>
     <link href="../css/MainWeb.css" rel="Stylesheet" type="text/css" />
     <script type="text/javascript" src="../JavaScript/jquery-1.10.2.min.js"></script>
     <script type="text/javascript" src="../JavaScript/Util.js"></script>
     <style type="text/css">
     .divMain{ margin:0px; padding:0px; overflow:hidden; text-align:center; width:300px; height:400px; border:solid 0px black;}
     .divTree{ margin:0px; padding:0px; text-align:center; overflow:auto; width:100%; height:90%; border:solid 0px black;}
     .divOperate {margin:0px; padding:0px; text-align:center; overflow:hidden;width:100%; height:10%; border:solid 0px black;}
     .tableOperate{margin:0px; padding:0px;  width:100%; height:100%; border:solid 0px black;}
     .tdOperate{margin:0px; padding:0px;  width:50%; height:100%; text-align:center;}
     </style>
     <script type="text/javascript">
     $(document).ready(function(){
       initalCheck();
     
     });
     function initalCheck(){
         $("input[type='checkbox']").each(function(){
            
            $(this).change(function(obj){
                  if(obj.currentTarget && obj.currentTarget.checked){
                      $("input[type='checkbox']:checked").each(function(){
                           if(obj.currentTarget!=this){
                                this.checked=false;
                           }
                      });
                 }
            });
        });
     }
     function checkNode(obj){
        
     }
     function Sure(){
        var data=document.getElementById("<%=hdSelectData.ClientID %>").value;
        if(data){
             if(parent && parent.window){
            //parent.window.frames[0]==window.location
                var div=parent.window.document.getElementById("_dialogDivDetail");
                if( div &&div.sure){
                 var  data1=getDecodeRowData(data);
                    div.sure(data1);
                }
            }
        }
        else{
            alert("请选择会计科目");
        }
     }
     function validate(){
        if( $("input[type='checkbox']:checked").length>0){
            return true;
        }
        else{
        alert("请选择会计科目");
            return false;
        }
     }
     function Close(){
        if(parent && parent.window){
        //parent.window.frames[0]==window.location

            var div=parent.window.document.getElementById("_dialogDivDetail");
            if( div &&div.close){
                div.close("取消");
            }
        }
     }
     </script>
</head>
<body>
    <form id="form1" runat="server">
   
        <div  class="divMain" >
            <div class="divTree">
                 <asp:TreeView ID="tvTitle" runat="server" ShowLines="True" ExpandDepth="1" EnableViewState="true"
                    CssClass="tree" ForeColor="Black" NoExpandImageUrl="~/images/tree/plus3.gif" CollapseImageUrl="~/images/tree/plus3.gif"
                    ExpandImageUrl="~/images/tree/minus2.gif" NodeStyle-ImageUrl="~/images/tree/folderClosed.gif"
                    LeafNodeStyle-ImageUrl="~/images/tree/leaf.gif"  >
                     
                </asp:TreeView>
            </div>
            <div class="divOperate">
                <table class="tableOperate">
                    <tr>
                        <td class="tdOperate"> 
                            <asp:Button runat="server" ID="btnSure"  Text="确认"  OnClick="btnSureClick" OnClientClick="return validate();" />
                             
                        </td>
                        <td class="tdOperate">
                             <input type="button" value="取消" onclick="Close()"/>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField runat="server" ID="hdSelectData" Value="" />
            <asp:HiddenField runat="server" ID="hdCode" />
        </div>
    </form>
</body>
</html>
