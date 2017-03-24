<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Test" %>

<%@ Register Src="~/UserControls/ubsWebHeader.ascx" TagName="ubsWebHeader" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/CostAnalysisFigure.ascx" TagName="CostAnalysisFigure"
    TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc:CostAnalysisFigure ID="costAnalysisFigure1" runat="server" />
        <uc:ubsWebHeader ID="ubsWebHeader1" runat="server" />
    </div>
    </form>
</body>
</html>
