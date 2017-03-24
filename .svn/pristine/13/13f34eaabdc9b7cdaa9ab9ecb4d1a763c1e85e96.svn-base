<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ReceivablesAnalysisFigure.ascx.cs"
    Inherits="UserControls_ReceivablesAnalysisFigure" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<table style="height: 200px;">
    <tr>
        <td>
            <%--      <h1>
                收款情况分析图</h1>--%>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Chart ID="Chart1" runat="server" Height="350px" Width="700px" EnableViewState="true"
                ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)" ImageType="Png" BackColor="#D3DFF0"
                Palette="BrightPastel" BorderDashStyle="Solid" BackSecondaryColor="White" BackGradientStyle="TopBottom"
                BorderWidth="2" BorderColor="26, 59, 105">
                <Titles>
                    <asp:Title ShadowColor="32, 0, 0, 0" ShadowOffset="3" Font="Trebuchet MS, 14.25pt, style=Bold"
                        Text="收款情况分析图" Name="Title1" ForeColor="26, 59, 105">
                    </asp:Title>
                </Titles>
                <Legends>
                    <asp:Legend Enabled="true" IsTextAutoFit="false" Name="Default" BackColor="Transparent"
                        Font="Trebuchet MS, 8.25pt, style=Bold">
                    </asp:Legend>
                </Legends>
                <BorderSkin SkinStyle="Emboss"></BorderSkin>
                <Series>
                    <asp:Series XValueType="String" YValueType="Double" Name="Series1" LegendText="合同收入"
                        ChartType="Column" BorderColor="180, 26, 59, 105">
                    </asp:Series>
                    <asp:Series XValueType="String" YValueType="Double" Name="Series2" LegendText="实际收款"
                        ChartType="Column" BorderColor="180, 26, 59, 105">
                    </asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid"
                        BackSecondaryColor="White" BackColor="64, 165, 191, 228" ShadowColor="Transparent"
                        BackGradientStyle="TopBottom">
                        <Area3DStyle Rotation="9" Perspective="5" LightStyle="Realistic" Inclination="38"
                            PointDepth="200" IsRightAngleAxes="False" WallWidth="0" IsClustered="False" />
                        <Position X="2" Y="40" Height="90" Width="82"></Position>
                        <AxisY LineColor="64, 64, 64, 64" Title="单位：万元" TextOrientation="Stacked" TitleAlignment="Far">
                            <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                            <MajorGrid LineColor="64, 64, 64, 64" />
                        </AxisY>
                        <AxisX LineColor="64, 64, 64, 64" Title="月份" Interval="1" LabelAutoFitStyle="StaggeredLabels"
                            Maximum="12">
                            <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                            <MajorGrid LineColor="64, 64, 64, 64" />
                        </AxisX>
                    </asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
        </td>
    </tr>
</table>
