<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test3.aspx.cs" Inherits="test_test3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
 <link href="../jquery/themes/bootstrap/easyui.css" rel="stylesheet" type="text/css" />       <!-- 整体样式包 -->
    <link href="../jquery/themes/color.css" rel="stylesheet" type="text/css" />                  <!-- 字体样式包 -->
    <link href="../jquery/themes/icon.css" rel="stylesheet" type="text/css" />                   <!-- 图标样式 -->
    <script src="../jquery/jquery.min.js" type="text/javascript"></script>                       <!-- jquery-1.11.1版本 -->
    <script src="../jquery/jquery.easyui.min.js" type="text/javascript"></script>                <!-- jquery-easyui-1.4.2版本 -->
    <script src="../jquery/jquery.plugin.js" type="text/javascript"></script>                    <!-- jquery其他方法扩展 -->
    <script src="../jquery/locale/easyui-lang-zh_CN.js" type="text/javascript"></script>         <!-- 本地语言包 -->
      
</head>
<body>
    <h2>Basic Layout</h2>
	<p>The layout contains north,south,west,east and center regions.</p>
	<div style="margin:20px 0;"></div>
	<div class="easyui-layout" style="width:100%;height:100%;">
		<div data-options="region:'north',title:'top Title',collapsible:false" style="height:50px"></div>
 
		<div data-options="region:'south',title:'Main Title',collapsible:false"  >
		 
		</div>
	</div>
 
</body>
</html>
