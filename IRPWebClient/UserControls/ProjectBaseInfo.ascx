<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProjectBaseInfo.ascx.cs"
    Inherits="UserControls_ProjectBaseInfo" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<link href="../CSS/MainWeb.css" rel="stylesheet" type="text/css" />
<table style="height: 300px; width: 700px; margin: 0px auto;">
    <tr>
        <td valign="top">
            <fieldset>
                <legend>工程简介</legend>
                <table style="width: 100%; margin: 0px auto; text-align: left;" cellspacing="5" cellpadding="5">
                    <tr>
                        <td class="tdBaseLeft">
                            工程名称：
                        </td>
                        <td id="txtProjectName" runat="server" class="tdBaseRight">
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            项目名称：
                        </td>
                        <td id="txtProName" runat="server" class="tdBaseRight">
                        </td>
                    </tr>
                    <%--    <tr>
                        <td class="tdBaseLeft">
                            项目类型：
                        </td>
                        <td id="txtProjectType" runat="server" class="tdBaseRight">
                        </td>
                        <td class="tdBaseLeft">
                            承包方式：
                        </td>
                        <td id="txtContractWay" runat="server" class="tdBaseRight">
                        </td>
                    </tr>--%>
                    <tr>
                        <td class="tdBaseLeft">
                            工程地点：
                        </td>
                        <td id="txtProvince" runat="server" class="tdBaseRight">
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            承包范围：
                        </td>
                        <td id="txtContracte" runat="server" class="tdBaseRight">
                        </td>
                    </tr>
                    <tr>
                        <%--          <td class="tdBaseLeft">
                            结构类型：
                        </td>
                        <td id="txtStructType" runat="server" class="tdBaseRight">
                        </td>--%>
                        <td class="tdBaseLeft">
                            基础形式：
                        </td>
                        <td id="txtBace" runat="server" class="tdBaseRight">
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            生命周期状态：
                        </td>
                        <td id="txtLifeCycleState" runat="server" class="tdBaseRight">
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            创建时间：
                        </td>
                        <td id="txtCreateDate" runat="server" class="tdBaseRight">
                        </td>
                    </tr>
                </table>
            </fieldset>
        </td>
    </tr>
    <tr>
        <td valign="top" style="padding-top: 15px; padding-bottom: 15px;">
            <fieldset>
                <legend>商务信息</legend>
                <table style="width: 100%; margin: 0px auto; text-align: left;" cellspacing="5" cellpadding="5">
                    <tr>
                        <td class="tdBaseLeft">
                            资金来源：
                        </td>
                        <td id="txtMoneySource" runat="server" class="tdBaseRight">
                        </td>
                    </tr>
                    <%--<tr>
                        <td class="tdBaseLeft">
                            资金到位情况：
                        </td>
                        <td id="txtMoneyStates" runat="server" class="tdBaseRight">
                        </td>
                    </tr>--%>
                    <tr>
                        <td class="tdBaseLeft">
                            工程造价：
                        </td>
                        <td>
                            <label id="txtProjectCost" runat="server" class="tdBaseRight">
                            </label>
                            万元
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            实际预算总额：
                        </td>
                        <td>
                            <label id="txtRealPreMoney" runat="server" class="tdBaseRight">
                            </label>
                            万元
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            土建合同总额：
                        </td>
                        <td>
                            <label id="txtConstractMoney" runat="server" class="tdBaseRight">
                            </label>
                            万元
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            安装合同总额：
                        </td>
                        <td>
                            <label id="txtInstallOrderMoney" runat="server" class="tdBaseRight">
                            </label>
                            万元
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            合同收款比例：
                        </td>
                        <td>
                            <label id="txtCollectProport" runat="server" class="tdBaseRight">
                            </label>
                            %
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            责任上缴比例：
                        </td>
                        <td>
                            <label id="txtTurnProport" runat="server" class="tdBaseRight">
                            </label>
                            %
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            大包模板木枋地上造价：
                        </td>
                        <td>
                            <label id="txtGroundPrice" runat="server" class="tdBaseRight">
                            </label>
                            元
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            大包模板木枋地下造价：
                        </td>
                        <td>
                            <label id="txtUnderPrice" runat="server" class="tdBaseRight">
                            </label>
                            元
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBaseLeft">
                            备注信息：
                        </td>
                        <td id="txtExplain" runat="server" class="tdBaseRight">
                        </td>
                    </tr>
                </table>
            </fieldset>
        </td>
    </tr>
</table>
