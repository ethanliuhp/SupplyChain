<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test4.aspx.cs" Inherits="test_Default4" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script type="text/javascript">
    var iCount=1;
    function addContent()
    {
        //parent.insertChildContent();
        var iRowIndex=document.getElementById('tab').rows.length-iCount;
        if(iRowIndex>-1)
         document.getElementById("divContent").insert( iRowIndex   );
         iCount++;
    }
    function insertContent(content){
    document.getElementById("divContent").innerHTML+=content;
    }
    window.onload=function(){
        document.getElementById("divContent").insert=deleteRow;
    }
    function deleteRow(rowIndex){ var oGrid= document.getElementById('tab'); if( oGrid.rows.length >rowIndex){oGrid.rows[rowIndex].style.display='none';}}
    </script>
</head>
<body>
    <form id="form1" runat="server">    
    <div>
        <input type="button" value="子点击" id=""  onclick="addContent()"  />
    </div>
    <div id="divContent">
    
    </div>
    <table id="tab">
        <tr><td>0</td></tr>
        <tr><td>1</td></tr>
         <tr><td>2</td></tr>
          <tr><td>3</td></tr>
           <tr><td>4</td></tr>
            <tr><td>5</td></tr>
             <tr><td>6</td></tr>
              <tr><td>7</td></tr>
               <tr><td>8</td></tr>
               <tr><td>9</td></tr>
        <tr><td>10</td></tr>
        <tr><td>11</td></tr>
         <tr><td>12</td></tr>
          <tr><td>13</td></tr>
           <tr><td>14</td></tr>
            <tr><td>15</td></tr>
             <tr><td>16</td></tr>
              <tr><td>17</td></tr>
               <tr><td>18</td></tr>
               <tr><td>19</td></tr>
    </table>
    
    </form>
</body>
</html>
