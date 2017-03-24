<%@ Page Language="C#" MasterPageFile="~/WebMasterPage.master" AutoEventWireup="true"
    CodeFile="TestPage.aspx.cs" Inherits="TestPage" Title="线性条形图" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ubsContentPage" runat="Server">
    <link href="CSS/samples.css" rel="stylesheet" type="text/css" />
    <asp:ScriptManager ID="sm1" runat="server">
    </asp:ScriptManager>
    <table cellpadding="0" cellspacing="0" border="0" style="margin-left: 100px">
        <tr>
            <td style="padding-top: 10px;">
                <h1>
                    线性条形图：</h1>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Chart ID="Chart1" runat="server" Height="400px" Width="600px" EnableViewState="true"
                    ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)" ImageType="Png" BackColor="#D3DFF0"
                    Palette="BrightPastel" BorderDashStyle="Solid" BackSecondaryColor="White" BackGradientStyle="TopBottom"
                    BorderWidth="2" BorderColor="26, 59, 105">
                    <Legends>
                        <asp:Legend Enabled="true" IsTextAutoFit="false" Name="Default" BackColor="Transparent"
                            Font="Trebuchet MS, 8.25pt, style=Bold">
                        </asp:Legend>
                    </Legends>
                    <BorderSkin SkinStyle="Emboss"></BorderSkin>
                    <Series>
                        <asp:Series XValueType="Int32" YValueType="Double" Name="Series1" LegendText="武汉"
                            ChartType="Line" BorderColor="180, 26, 59, 105">
                        </asp:Series>
                        <asp:Series XValueType="Int32" YValueType="Double" Name="Series2" LegendText="上海"
                            ChartType="Line" BorderColor="180, 26, 59, 105">
                        </asp:Series>
                        <asp:Series XValueType="Int32" YValueType="Double" Name="Series3" LegendText="北京"
                            ChartType="Line" BorderColor="180, 26, 59, 105">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid"
                            BackSecondaryColor="White" BackColor="64, 165, 191, 228" ShadowColor="Transparent"
                            BackGradientStyle="TopBottom">
                            <Area3DStyle Rotation="9" Perspective="5" Enable3D="True" LightStyle="Realistic"
                                Inclination="38" PointDepth="200" IsRightAngleAxes="False" WallWidth="0" IsClustered="False" />
                            <Position X="2" Y="2" Height="94" Width="94"></Position>
                            <AxisY LineColor="64, 64, 64, 64" Title="收入金额(亿元)" TextOrientation="Horizontal" TitleAlignment="Far">
                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                <MajorGrid LineColor="64, 64, 64, 64" />
                            </AxisY>
                            <AxisX LineColor="64, 64, 64, 64" Title="年份">
                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                <MajorGrid LineColor="64, 64, 64, 64" />
                            </AxisX>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </td>
            <td valign="top">
                <table class="controls" cellpadding="4">
                    <tr>
                        <td colspan="2" style="text-align: center; padding: 20px;">
                            <asp:Button ID="btnChangeData" runat="server" Text="换一组数据" OnClick="btnChangeData_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            图形种类:
                        </td>
                        <td>
                            <asp:DropDownList ID="ChartTypeList" runat="server" AutoPostBack="True" CssClass="spaceright"
                                OnSelectedIndexChanged="ChartTypeList_SelectedIndexChanged">
                                <asp:ListItem Value="Line">Line</asp:ListItem>
                                <asp:ListItem Value="Spline" Selected="True">Spline</asp:ListItem>
                                <asp:ListItem Value="StepLine">StepLine</asp:ListItem>
                                <asp:ListItem Value="Column">Column</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            标记收入点:
                        </td>
                        <td>
                            <asp:DropDownList ID="PointLabelsList" runat="server" AutoPostBack="True" CssClass="spaceright"
                                OnSelectedIndexChanged="PointLabelsList_SelectedIndexChanged">
                                <asp:ListItem Value="None" Selected="True">None</asp:ListItem>
                                <asp:ListItem Value="Auto">Auto</asp:ListItem>
                                <asp:ListItem Value="TopLeft">TopLeft</asp:ListItem>
                                <asp:ListItem Value="Top">Top</asp:ListItem>
                                <asp:ListItem Value="TopRight">TopRight</asp:ListItem>
                                <asp:ListItem Value="Right">Right</asp:ListItem>
                                <asp:ListItem Value="BottomRight">BottomRight</asp:ListItem>
                                <asp:ListItem Value="Bottom">Bottom</asp:ListItem>
                                <asp:ListItem Value="BottomLeft">BottomLeft</asp:ListItem>
                                <asp:ListItem Value="Left">Left</asp:ListItem>
                                <asp:ListItem Value="Center">Center</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            显示年份标记线:
                        </td>
                        <td>
                            <asp:CheckBox ID="ShowMarkers" runat="server" AutoPostBack="True" Text="" OnCheckedChanged="ShowMarkers_CheckedChanged">
                            </asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnExport" runat="server" Text="导出为Excel" OnClick="btnExport_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="900" Height="600">
        <LocalReport EnableExternalImages="True" ReportPath="Rpt/Report2.rdlc">
        </LocalReport>
    </rsweb:ReportViewer>
</asp:Content>
