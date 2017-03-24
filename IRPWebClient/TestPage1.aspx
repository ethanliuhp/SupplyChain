<%@ Page Language="C#" MasterPageFile="~/WebMasterPage.master" AutoEventWireup="true"
    CodeFile="TestPage1.aspx.cs" Inherits="TestPage1" Title="饼图" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ubsContentPage" runat="Server">
    <link href="CSS/samples.css" rel="stylesheet" type="text/css" />
    <table cellpadding="0" cellspacing="0" border="0" style="margin-left: 100px">
        <tr>
            <td style="padding-top: 20px;">
                <h1>
                    饼状图：</h1>
            </td>
        </tr>
        <tr>
            <td width="412" class="tdchart">
                <asp:Chart ID="Chart2" runat="server" Palette="BrightPastel" BackColor="#D3DFF0"
                    EnableViewState="true" Height="400px" Width="600px" BorderDashStyle="Solid" BackGradientStyle="TopBottom"
                    BorderWidth="2" BorderColor="26, 59, 105" IsSoftShadows="False" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)">
                    <Titles>
                        <asp:Title ShadowColor="32, 0, 0, 0" ShadowOffset="3" Font="Trebuchet MS, 14.25pt, style=Bold"
                            Text="各地区年收入比例图" Name="Title1" ForeColor="26, 59, 105">
                        </asp:Title>
                    </Titles>
                    <Legends>
                        <asp:Legend TitleFont="Microsoft Sans Serif, 8pt, style=Bold" BackColor="Transparent"
                            IsEquallySpacedItems="True" Font="Trebuchet MS, 8pt, style=Bold" IsTextAutoFit="False"
                            Name="Default">
                        </asp:Legend>
                    </Legends>
                    <BorderSkin SkinStyle="Emboss"></BorderSkin>
                    <Series>
                        <asp:Series ChartArea="ChartArea1" XValueType="Double" Name="Default" ChartType="Pie"
                            Font="Trebuchet MS, 8.25pt, style=Bold" CustomProperties="DoughnutRadius=25, PieDrawingStyle=Concave, CollectedLabel=Other, MinimumRelativePieSize=20"
                            MarkerStyle="Circle" BorderColor="64, 64, 64, 64" Color="180, 65, 140, 240" YValueType="Double"
                            Label="#PERCENT{P1}">
                            <Points>
                                <asp:DataPoint LegendText="武汉" CustomProperties="OriginalPointIndex=0" YValues="39" />
                                <asp:DataPoint LegendText="北京" CustomProperties="OriginalPointIndex=1" YValues="18" />
                                <asp:DataPoint LegendText="上海" CustomProperties="OriginalPointIndex=2" YValues="15" />
                                <asp:DataPoint LegendText="天津" CustomProperties="OriginalPointIndex=3" YValues="12" />
                                <asp:DataPoint LegendText="济南" CustomProperties="OriginalPointIndex=6" YValues="2" />
                                <asp:DataPoint LegendText="郑州" CustomProperties="OriginalPointIndex=4" YValues="8" />
                                <asp:DataPoint LegendText="西安" CustomProperties="OriginalPointIndex=5" YValues="4.5" />
                            </Points>
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="Transparent"
                            BackColor="Transparent" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                            <AxisY2>
                                <MajorGrid Enabled="False" />
                                <MajorTickMark Enabled="False" />
                            </AxisY2>
                            <AxisX2>
                                <MajorGrid Enabled="False" />
                                <MajorTickMark Enabled="False" />
                            </AxisX2>
                            <Area3DStyle PointGapDepth="900" Rotation="162" IsRightAngleAxes="False" WallWidth="25"
                                IsClustered="False" />
                            <AxisY LineColor="64, 64, 64, 64">
                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                <MajorGrid LineColor="64, 64, 64, 64" Enabled="False" />
                                <MajorTickMark Enabled="False" />
                            </AxisY>
                            <AxisX LineColor="64, 64, 64, 64">
                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                <MajorGrid LineColor="64, 64, 64, 64" Enabled="False" />
                                <MajorTickMark Enabled="False" />
                            </AxisX>
                        </asp:ChartArea>
                    </ChartAreas>
                </asp:Chart>
            </td>
            <td valign="top">
                <table class="controls" cellpadding="4">
                    <tr>
                        <td class="label">
                            图形种类:
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownListPieType" runat="server" AutoPostBack="True" CssClass="spaceright"
                                Width="112px" OnSelectedIndexChanged="DropDownListPieType_SelectedIndexChanged">
                                <asp:ListItem Value="Doughnut">Doughnut</asp:ListItem>
                                <asp:ListItem Value="Pie">Pie</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            圆环半径(%):
                        </td>
                        <td>
                            <asp:DropDownList ID="HoleSizeList" runat="server" AutoPostBack="True" Enabled="False"
                                CssClass="spaceright" Width="112px" OnSelectedIndexChanged="HoleSizeList_SelectedIndexChanged">
                                <asp:ListItem Value="20">20</asp:ListItem>
                                <asp:ListItem Value="30">30</asp:ListItem>
                                <asp:ListItem Value="40">40</asp:ListItem>
                                <asp:ListItem Value="50">50</asp:ListItem>
                                <asp:ListItem Value="60" Selected="True">60</asp:ListItem>
                                <asp:ListItem Value="70">70</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            标注文本的样式:
                        </td>
                        <td>
                            <asp:DropDownList ID="LabelStyleList" runat="server" AutoPostBack="True" CssClass="spaceright"
                                Width="112px" OnSelectedIndexChanged="LabelStyleList_SelectedIndexChanged">
                                <asp:ListItem Value="Inside" Selected="True">Inside</asp:ListItem>
                                <asp:ListItem Value="Outside">Outside</asp:ListItem>
                                <asp:ListItem Value="Disabled">Disabled</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            突出显示的地区:
                        </td>
                        <td>
                            <asp:DropDownList ID="ExplodedPointList" runat="server" AutoPostBack="True" CssClass="spaceright"
                                Width="112px" OnSelectedIndexChanged="ExplodedPointList_SelectedIndexChanged">
                                <asp:ListItem Value="None" Selected="True">None</asp:ListItem>
                                <asp:ListItem Value="武汉">武汉</asp:ListItem>
                                <asp:ListItem Value="北京">北京</asp:ListItem>
                                <asp:ListItem Value="上海">上海</asp:ListItem>
                                <asp:ListItem Value="天津">天津</asp:ListItem>
                                <asp:ListItem Value="济南">济南</asp:ListItem>
                                <asp:ListItem Value="郑州">郑州</asp:ListItem>
                                <asp:ListItem Value="西安">西安</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            饼图样式:
                        </td>
                        <td>
                            <asp:DropDownList ID="DropdownlistPieDrawingStyle" runat="server" Width="112px" CssClass="spaceright"
                                AutoPostBack="True" OnSelectedIndexChanged="DropdownlistPieDrawingStyle_SelectedIndexChanged">
                                <asp:ListItem Value="Default">Default</asp:ListItem>
                                <asp:ListItem Value="SoftEdge" Selected="True">SoftEdge</asp:ListItem>
                                <asp:ListItem Value="Concave">Concave</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            3D显示:
                        </td>
                        <td>
                            <asp:CheckBox ID="CheckboxShow3D" runat="server" AutoPostBack="True" Text="" OnCheckedChanged="CheckboxShow3D_CheckedChanged">
                            </asp:CheckBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
