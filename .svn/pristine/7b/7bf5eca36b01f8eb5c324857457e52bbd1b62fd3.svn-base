<%@ Page Language="C#" MasterPageFile="~/WebMasterPage.master" AutoEventWireup="true"
    CodeFile="ProjectWarnQuery.aspx.cs" Inherits="MainPage_ProjectWarnQuery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ubsContentPage" runat="Server">

    <script type="text/javascript">
        window.onload = function() {
            if (window.parent && window.parent.document) {
                var frmContentMain = GetParentObject("iframe", "frmProjectWarnQuery_Content");

                if (frmContentMain) {
                    var pagesize = getPageSize();
                    var pageHeight = pagesize[1];
                    frmContentMain.setAttribute("height", (pageHeight + 50) + "px");
                }
            }
        }
        function Query() {
            document.getElementById("trDisplay").style.display = "block";

            if (window.parent && window.parent.document) {
                var frmContentMain = GetParentObject("iframe", "frmProjectWarnQuery_Content");

                if (frmContentMain) {
                    frmContentMain.setAttribute("height", (window.screen.height - 400) + "px");
                }
            }
        }
        function GetParentObject(tagName, objId) {
            var inputs = parent.document.getElementsByTagName(tagName);
            for (var i = 0; i < inputs.length; i++) {
                var inp = inputs[i];
                if (inp.id.indexOf(objId) > -1) {
                    return inp;
                    break;
                }
            }
            return null;
        }
        function getPageSize() {

            var xScroll, yScroll;

            if (window.innerHeight && window.scrollMaxY) {
                xScroll = document.body.scrollWidth;
                yScroll = window.innerHeight + window.scrollMaxY;
            } else if (document.body.scrollHeight > document.body.offsetHeight) { // all but Explorer Mac
                xScroll = document.body.scrollWidth;
                yScroll = document.body.scrollHeight;
            } else { // Explorer Mac...would also work in Explorer 6 Strict, Mozilla and Safari
                xScroll = document.body.offsetWidth;
                yScroll = document.body.offsetHeight;
            }

            var windowWidth, windowHeight;
            if (self.innerHeight) {	// all except Explorer
                windowWidth = self.innerWidth;
                windowHeight = self.innerHeight;
            } else if (document.documentElement && document.documentElement.clientHeight) { // Explorer 6 Strict Mode
                windowWidth = document.documentElement.clientWidth;
                windowHeight = document.documentElement.clientHeight;
            } else if (document.body) { // other Explorers
                windowWidth = document.body.clientWidth;
                windowHeight = document.body.clientHeight;
            }

            // for small pages with total height less then height of the viewport
            if (yScroll < windowHeight) {
                pageHeight = windowHeight;
            } else {
                pageHeight = yScroll;
            }

            // for small pages with total width less then width of the viewport
            if (xScroll < windowWidth) {
                pageWidth = windowWidth;
            } else {
                pageWidth = xScroll;
            }


            arrayPageSize = new Array(pageWidth, pageHeight, windowWidth, windowHeight)
            return arrayPageSize;
        }
    </script>

    <div id="divMain" runat="server" style="margin: 10px 0px 0px 10px;  background-color: #F0F8FF;">
        <table class="tableWarnQuery">
            <tr>
                <td>
                    工程名称:<input id="txtProjectName" runat="server" />
                </td>
                <td>
                    项目类型:<select id="selProjectType" runat="server">
                        <option value=""></option>
                        <option value="建筑工程">建筑工程</option>
                        <option value="安装工程">安装工程</option>
                        <option value="市政桥梁">市政桥梁</option>
                        <option value="公路工程">公路工程</option>
                        <option value="装饰装修工程">装饰装修工程</option>
                        <option value="园林绿化工程">园林绿化工程</option>
                        <option value="大型土石方工程">大型土石方工程</option>
                        <option value="钢结构工程">钢结构工程</option>
                        <option value="轻轨">轻轨</option>
                        <option value="其它">其它</option>
                    </select>
                </td>
                <td>
                    工程状态:<select id="selProjectState" runat="server">
                        <option value=""></option>
                        <option value="编辑">编辑</option>
                        <option value="发布">发布</option>
                    </select>
                </td>
                <td>
                    施工阶段:<select id="selProjectStage" runat="server">
                        <option value=""></option>
                        <option value="投标阶段">投标阶段</option>
                        <option value="策划阶段">策划阶段</option>
                        <option value="施工阶段">施工阶段</option>
                        <option value="竣工结算阶段">竣工结算阶段</option>
                        <option value="维护阶段">维护阶段</option>
                    </select>
                </td>
                <td>
                    <asp:Button ID="btnQuery" runat="server" Text="查询" Width="100px" OnClientClick="return Query();"
                        OnClick="btnQuery_Click" />
                </td>
            </tr>
            <tr id="trDisplay" style="display: none; color: Blue; font-weight: bold;">
                <td colspan="5">
                    <img alt="正在查询" src="../images/menu/loader.gif" />正在查询，请稍候..........
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:GridView ID="gridProject" runat="server" CssClass="gridProject" DataKeyNames="Id" AutoGenerateColumns="False"
                        Style="width: 100%; text-align: center;" CellPadding="3" GridLines="Vertical"
                        EmptyDataText="项目预警信息列表" OnRowDataBound="gridProject_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="序号">
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="工程名称" />
                            <asp:BoundField DataField="HandlePersonName" HeaderText="项目经理" />
                            <asp:TemplateField HeaderText="项目类型">
                                <ItemTemplate>
                                    <%# Enum.GetName(typeof(Application.Business.Erp.SupplyChain.Basic.Domain.EnumProjectType), VirtualMachine.Component.Util.ClientUtil.ToInt(Eval("ProjectType")))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="工程状态">
                                <ItemTemplate>
                                    <%# Enum.GetName(typeof(Application.Business.Erp.SupplyChain.Basic.Domain.EnumProjectInfoState), VirtualMachine.Component.Util.ClientUtil.ToInt(Eval("ProjectInfoState")))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="施工阶段">
                                <ItemTemplate>
                                    <%# Enum.GetName(typeof(Application.Business.Erp.SupplyChain.Basic.Domain.EnumProjectLifeCycle), VirtualMachine.Component.Util.ClientUtil.ToInt(Eval("ProjectLifeCycle")))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="成本">
                                <ItemTemplate>
                                    <%--    <div id="divCost" runat="server" title="成本信息" style="">
                                    </div>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="工期">
                                <ItemTemplate>
                                    <div title="工期信息">
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="质量">
                                <ItemTemplate>
                                    <div title="质量信息">
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="安全">
                                <ItemTemplate>
                                    <div title="安全信息">
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="#DCDCDC" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
