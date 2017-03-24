<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CostAnalysisFigure.ascx.cs"
    Inherits="UserControls_CostAnalysisFigure" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<link href="../CSS/MainWeb.css" rel="stylesheet" type="text/css" />
<table style="height: 200px; margin: 0px auto; padding: 0px auto;">
    <tr>
        <td>
            <%--<h1>
                收入成本情况分析图</h1>--%>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Chart ID="Chart1" runat="server" Height="330px" Width="700px" EnableViewState="true"
                ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)" ImageType="Png" BackColor="#D3DFF0"
                Palette="BrightPastel" BorderDashStyle="Solid" BackSecondaryColor="White" BackGradientStyle="TopBottom"
                BorderWidth="2" BorderColor="26, 59, 105">
                <Titles>
                    <asp:Title ShadowColor="32, 0, 0, 0" ShadowOffset="3" Font="Trebuchet MS, 14.25pt, style=Bold"
                        Text="月度成本分析图" Name="Title1" ForeColor="26, 59, 105">
                    </asp:Title>
                </Titles>
                <Legends>
                    <asp:Legend Enabled="true" IsTextAutoFit="false" Name="Default" BackColor="Transparent"
                        Alignment="Near" Docking="Right" Font="Trebuchet MS, 8.25pt, style=Bold">
                    </asp:Legend>
                </Legends>
                <%--<Annotations>
                    <asp:AnnotationGroup Name="lblContractTotalPriceGroup" X="30" Y="75" AnchorAlignment="BottomCenter">
                        <Annotations>
                            <asp:TextAnnotation Name="lblContractTotalPrice" X="3" Y="10" Text="累计合同收入：100.4563 万元">
                            </asp:TextAnnotation>
                        </Annotations>
                    </asp:AnnotationGroup>
                    <asp:AnnotationGroup Name="lblResonsibleTotalPrice" AnchorX="50" AnchorY="75" AnchorAlignment="BottomCenter">
                        <Annotations>
                            <asp:TextAnnotation Name="lblResonsibleTotalPrice" AnchorX="50" AnchorY="150" Text="累计责任成本：100.4563 万元">
                            </asp:TextAnnotation>
                        </Annotations>
                    </asp:AnnotationGroup>
                    <asp:AnnotationGroup Name="lblRealTotalPrice" AnchorX="70" AnchorY="75" AnchorAlignment="BottomCenter">
                        <Annotations>
                            <asp:TextAnnotation Name="lblRealTotalPrice" AnchorX="70" AnchorY="150" Text="累计实际成本：100.4563 万元">
                            </asp:TextAnnotation>
                        </Annotations>
                    </asp:AnnotationGroup>
                    <asp:AnnotationGroup Name="lblRealColltionsummoney" AnchorX="20" AnchorY="80" AnchorAlignment="BottomCenter">
                        <Annotations>
                            <asp:TextAnnotation Name="lblRealColltionsummoney" AnchorX="90" AnchorY="150" Text="累计收款：100.4563 万元">
                            </asp:TextAnnotation>
                        </Annotations>
                    </asp:AnnotationGroup>
                    <asp:AnnotationGroup Name="lblProfit" AnchorX="50" AnchorY="75" AnchorAlignment="BottomCenter">
                        <Annotations>
                            <asp:TextAnnotation Name="lblProfit" AnchorX="50" AnchorY="150" Text="利润率：100.4563 万元">
                            </asp:TextAnnotation>
                        </Annotations>
                    </asp:AnnotationGroup>
                </Annotations>--%>
                <%--<BorderSkin SkinStyle="Emboss"></BorderSkin>--%>
                <Series>
                    <asp:Series XValueType="String" YValueType="String" Name="Series1" LegendText="合同收入"
                        ChartType="Line" BorderColor="180, 26, 59, 105" Color="Blue" BorderWidth="2">
                    </asp:Series>
                    <asp:Series XValueType="String" YValueType="String" Name="Series2" LegendText="责任成本"
                        ChartType="Line" BorderColor="180, 26, 59, 105" Color="Orange" BorderWidth="2">
                    </asp:Series>
                    <asp:Series XValueType="String" YValueType="String" Name="Series3" LegendText="实际成本"
                        ChartType="Line" BorderColor="180, 26, 59, 105" Color="Red" BorderWidth="2">
                    </asp:Series>
                    <asp:Series XValueType="String" YValueType="String" Name="Series4" LegendText="实际收款"
                        ChartType="Line" BorderColor="180, 26, 59, 105" Color="Green" BorderWidth="2">
                    </asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid"
                        BackSecondaryColor="White" BackColor="64, 165, 191, 228" ShadowColor="Transparent"
                        BackGradientStyle="TopBottom">
                        <Area3DStyle Rotation="9" Perspective="5" LightStyle="Realistic" Inclination="38"
                            PointDepth="200" IsRightAngleAxes="False" WallWidth="0" IsClustered="False" />
                        <Position X="0" Y="12" Height="85" Width="84"></Position>
                        <AxisY LineColor="64, 64, 64, 64" Title="万元" TextOrientation="Horizontal" TitleAlignment="Far">
                            <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                            <MajorGrid LineColor="64, 64, 64, 64" />
                        </AxisY>
                        <AxisX LineColor="64, 64, 64, 64" Title="年/月" Interval="1" TitleAlignment="Far" LabelAutoFitStyle="IncreaseFont"
                            Maximum="12" Minimum="1">
                            <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                            <MajorGrid LineColor="64, 64, 64, 64" />
                        </AxisX>
                    </asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
        </td>
    </tr>
    <tr>
        <td valign="top" style="padding-bottom: 10px; display: none;">
            <fieldset>
                <legend id="lblCostInfoTitle" runat="server">成本信息</legend>
                <table style="width: 100%; margin: 0px auto; text-align: left;" cellspacing="5" cellpadding="5">
                    <tr>
                        <td class="tdBaseLeft">
                            合同收入(开工累计)：
                        </td>
                        <td>
                            <label id="lblContractTotalPrice" runat="server" class="tdBaseRight">
                            </label>
                            万元
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            责任成本(开工累计)：
                        </td>
                        <td>
                            <label id="lblResonsibleTotalPrice" runat="server" class="tdBaseRight">
                            </label>
                            万元
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            实际成本(开工累计)：
                        </td>
                        <td>
                            <label id="lblRealTotalPrice" runat="server" class="tdBaseRight">
                            </label>
                            万元
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            实际收款(开工累计)：
                        </td>
                        <td>
                            <label id="lblRealColltionsummoney" runat="server" class="tdBaseRight">
                            </label>
                            万元
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            利润率：
                        </td>
                        <td>
                            <label id="lblProfit" runat="server" class="tdBaseRight">
                            </label>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </td>
    </tr>
</table>
