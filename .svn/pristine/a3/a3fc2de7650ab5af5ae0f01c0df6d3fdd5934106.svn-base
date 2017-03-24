<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProfitChart.ascx.cs" Inherits="UserControls_ProfitChart" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<table style="height: 200px;">
    <tr>
        <td colspan="2">
            <%--  <h1>
                利润图</h1>--%>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Chart ID="Chart1" runat="server" Height="330px" Width="700px" EnableViewState="true"
                ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)" ImageType="Png" BackColor="#D3DFF0"
                Palette="BrightPastel" BorderDashStyle="Solid" BackSecondaryColor="White" BackGradientStyle="TopBottom"
                BorderWidth="2" BorderColor="26, 59, 105">
                <Titles>
                    <asp:Title ShadowColor="32, 0, 0, 0" ShadowOffset="3" Font="Trebuchet MS, 14.25pt, style=Bold"
                        Text="累计成本分析图" Name="Title1" ForeColor="26, 59, 105">
                    </asp:Title>
                </Titles>
                <Legends>
                    <asp:Legend Enabled="true" IsTextAutoFit="true" Name="Default" BackColor="Transparent"
                        Alignment="Near" Docking="Right" Font="Trebuchet MS, 8.25pt, style=Bold">
                    </asp:Legend>
                </Legends>
                <%--<BorderSkin SkinStyle="Emboss"></BorderSkin>--%>
                <Series>
                    <asp:Series XValueType="String" YValueType="String" Name="Series1" LegendText="2012累计合同收入"
                        ChartType="Line" BorderColor="180, 26, 59, 105" Color="Blue" BorderWidth="1">
                    </asp:Series>
                    <asp:Series XValueType="String" YValueType="String" Name="Series2" LegendText="2012累计责任成本"
                        ChartType="Line" BorderColor="180, 26, 59, 105" Color="Orange" BorderWidth="1"
                        Enabled="false">
                    </asp:Series>
                    <asp:Series XValueType="String" YValueType="String" Name="Series3" LegendText="2012累计实际成本"
                        ChartType="Line" BorderColor="180, 26, 59, 105" Color="Red" BorderWidth="1">
                    </asp:Series>
                    <asp:Series XValueType="String" YValueType="String" Name="Series4" LegendText="2012累计收款"
                        ChartType="Line" BorderColor="180, 26, 59, 105" Color="Green" BorderWidth="1">
                    </asp:Series>
                    <asp:Series XValueType="String" YValueType="String" Name="Series20" LegendText=" "
                        ChartType="Line" BorderColor="180, 26, 59, 105" Color="Transparent" BorderWidth="2">
                    </asp:Series>
                    <asp:Series XValueType="String" YValueType="String" Name="Series5" LegendText="2013累计合同收入"
                        ChartType="Line" BorderColor="180, 26, 59, 105" Color="Black" BorderWidth="2">
                    </asp:Series>
                    <asp:Series XValueType="String" YValueType="String" Name="Series6" LegendText="2013累计责任成本"
                        ChartType="Line" BorderColor="180, 26, 59, 105" Color="Orange" BorderWidth="2"
                        Enabled="false">
                    </asp:Series>
                    <asp:Series XValueType="String" YValueType="String" Name="Series7" LegendText="2013累计实际成本"
                        ChartType="Line" BorderColor="180, 26, 59, 105" Color="Yellow" BorderWidth="2">
                    </asp:Series>
                    <asp:Series XValueType="String" YValueType="String" Name="Series8" LegendText="2013累计收款"
                        ChartType="Line" BorderColor="180, 26, 59, 105" Color="Purple" BorderWidth="2">
                    </asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid"
                        BackSecondaryColor="White" BackColor="64, 165, 191, 228" ShadowColor="Transparent"
                        BackGradientStyle="TopBottom">
                        <Area3DStyle Rotation="9" Perspective="5" LightStyle="Realistic" Inclination="38"
                            PointDepth="200" IsRightAngleAxes="False" WallWidth="0" IsClustered="False" />
                        <Position X="0" Y="12" Height="85" Width="76"></Position>
                        <AxisY LineColor="64, 64, 64, 64" LineDashStyle="Dash" Title="万元" TextOrientation="Stacked"
                            TitleAlignment="Far">
                            <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                            <MajorGrid LineColor="64, 64, 64, 64" />
                        </AxisY>
                        <AxisX LineColor="64, 64, 64, 64" Title="月份" Interval="1" TitleAlignment="Far" LabelAutoFitStyle="IncreaseFont"
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
        <td valign="top" style="padding-bottom: 10px; width: 50%; padding-right: 3px; display: none;">
            <fieldset>
                <legend id="lblCostInfoTitle" runat="server">成本信息</legend>
                <table style="width: 100%; margin: 0px auto; text-align: left;" cellspacing="5" cellpadding="5">
                    <tr>
                        <td class="tdBaseLeft">
                            合同收入(年度累计)：
                        </td>
                        <td>
                            <label id="lblContractTotalPrice" runat="server" class="tdBaseRight">
                                &nbsp;
                            </label>
                            万元
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            责任成本(年度累计)：
                        </td>
                        <td>
                            <label id="lblResonsibleTotalPrice" runat="server" class="tdBaseRight">
                                &nbsp;
                            </label>
                            万元
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            实际成本(年度累计)：
                        </td>
                        <td>
                            <label id="lblRealTotalPrice" runat="server" class="tdBaseRight">
                                &nbsp;
                            </label>
                            万元
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            实际收款(年度累计)：
                        </td>
                        <td>
                            <label id="lblRealColltionsummoney" runat="server" class="tdBaseRight">
                                &nbsp;
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
                                &nbsp;
                            </label>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </td>
        <td valign="top" style="padding-bottom: 10px; width: 50%; padding-left: 3px; display: none;">
            <fieldset>
                <legend id="lblCostInfoTitle2" runat="server">成本信息</legend>
                <table style="width: 100%; margin: 0px auto; text-align: left;" cellspacing="5" cellpadding="5">
                    <tr>
                        <td class="tdBaseLeft">
                            合同收入(年度累计)：
                        </td>
                        <td>
                            <label id="lblContractTotalPrice2" runat="server" class="tdBaseRight">
                                &nbsp;
                            </label>
                            万元
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            责任成本(年度累计)：
                        </td>
                        <td>
                            <label id="lblResonsibleTotalPrice2" runat="server" class="tdBaseRight">
                                &nbsp;
                            </label>
                            万元
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            实际成本(年度累计)：
                        </td>
                        <td>
                            <label id="lblRealTotalPrice2" runat="server" class="tdBaseRight">
                                &nbsp;
                            </label>
                            万元
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            实际收款(年度累计)：
                        </td>
                        <td>
                            <label id="lblRealColltionsummoney2" runat="server" class="tdBaseRight">
                                &nbsp;
                            </label>
                            万元
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            利润率：
                        </td>
                        <td>
                            <label id="lblProfit2" runat="server" class="tdBaseRight">
                                &nbsp;
                            </label>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </td>
    </tr>
</table>
