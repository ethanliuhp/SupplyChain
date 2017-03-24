<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dialog.aspx.cs" Inherits="test_Dialog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link rel="stylesheet" href="../css/bootstrap.css"/>
    
    <script type="text/javascript" src="../JavaScript/jquery-1.10.2.js"></script>
    <script src="../JavaScript/bootstrap.js" type="text/javascript"></script>
    <script type="text/javascript">
    function addDialog1(){
        $("#example").modal( );
    }
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="button" value="click"   onclick="addDialog1()"/><br />
    <div id="example" class="modal hide fade in" style="display: none; ">
        <div class="modal-header">
            <a class="close" data-dismiss="modal">×</a>
            <h3>This is a Modal Heading</h3>
        </div>
        <div class="modal-body">
            <h4>Text in a modal</h4>
            <p>You can add some text here.</p>		        
        </div>
       <div class="modal-footer">
            <a href="#" class="btn btn-success">Call to action</a>
            <a href="#" class="btn" data-dismiss="modal">Close</a>
    </div>
    </div>
    <div>1212</div>
    <div id="dialog">
          <table>
        <tr>
            <td>1</td>
            <td>2</td>
            <td>3</td>
        </tr>
     </table>
    </div>
   
    </form>
</body>
</html>
