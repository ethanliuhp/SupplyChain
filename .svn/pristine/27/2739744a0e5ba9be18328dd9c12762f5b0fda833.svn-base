<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DetailInfo.aspx.cs" Inherits="MoneyManage_FactoringDataMng_DetailInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>子表</title>
    <style type="text/css">
            body{font-size:9pt}
            .divQuery{ width:100%;  height:100%; margin:0px; padding:0px; text-align:center; display:block;}
            .QueryTable{width:100%;  height:100%; margin-left:auto; margin-right:auto; padding:0px; text-align:center;font-size:9pt;}
            .QueryTableTr{width:100%;  height:30px;   text-align:center;}
            .QueryTableTdLabel{width:15%;  height:100% ;  text-align:right;}
            .QueryTableTdText{width:35%;  height:100% ;  text-align:left; padding:auto 5px auto auto;}
            .table{
            	width:100%;border-collapse:collapse;word-break:break-all;
            }
            .table tr:hover{
            	color:#116a79 !important;
            	background-color:#cef97b !important;
            	cursor:pointer;
            }
            .table th{
            	color:Black;background-color:#F2F4F6;height:30px;
            	font-size:9pt;
            }
            .table td{
            	height:24px;
            	text-align:center;
            	font-size:9pt;
            }
            .dialog{
	            top:100px;
	            left:200px;
	            position: absolute;
	            padding:20px;
	            display: none;
	            width:400px;
	            z-index: 1001;
	            background-color: #fff;
	            height:auto;
	            border:1px solid #777;
	            border-radius: 5px;
	            box-shadow: 1px 1px 10px 0px #000
            }
            .dialog table{
            }
            .page-shadow{
	            top:0;
	            left:0;
	            position: absolute;
	            width:100%;
	            background-color:#000;
	            opacity: 0.3;
	            filter:alpha(opacity=30);
	            z-index: 1000;
            }
            
            img{
                border:none;	
            }
            	
    </style>
    <style  type="text/css">
         input[readonly]{
	        background-color: #ccc;
         } 
         textarea[readonly]{
	        background-color: #ccc;
         }
        .divTool{   height:30px; margin:0px; padding:0px; border:solid 0px black; text-align:left; display:none;}
        .divMain{ width:100%; height:100%; margin:0px; padding:0px; border:solid 0px black;}
        .divMaster{ width:100%; height:30%; margin:0px; padding:0px; border:solid 0px black;}
        .divDetail{ width:100%; height:70%; margin:0px; padding:0px; border:solid 0px black;}
        .MasterTable{width:100%;  height:100%; margin-left:auto; margin-right:auto; padding:0px; text-align:center;}
        .MasterTableTr{width:100%;  height:30px;   text-align:center;}
        .MasterTableTdLabel{width:13%;  height:100% ;  text-align:right;}
        .MasterTableTdText{width:20%;  height:100% ;  text-align:left; padding:auto 5px auto auto; padding-left:5px;}
        
         .DetailTable{width:100%;   height:100%; margin-left:auto; margin-right:auto; padding:0px; text-align:center;}
        .DetailTableTr{width:100%;  height:30px;   text-align:center;}
        .DetailTableTdLabel{width:15%;  height:100% ;  text-align:right;}
        .DetailTableTdText{width:35%;  height:100% ;  text-align:left; padding:auto 5px auto auto;padding-left:5px}
        .dialogDivDetail{width:100%; height:100%; margin:0px; padding:0px; border:solid 0px black; display:none;}
    </style>
     <link href="../../css/MainWeb.css" rel="Stylesheet" type="text/css" />
    <script src="../../JavaScript/jquery-1.10.2.min.js"></script>
     <script src="../../JavaScript/Util.js"></script>
     <script type="text/javascript" src="../../JavaScript/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript">
        var editState,                      // 区分操作是新增还是编辑
            isEdit,
            editId,
            deleteId,
            isShowForm;
        $(function(){
            editState = $("#<%=hidEditState.ClientID %>");
            isEdit = $("#<%=hidIsEdit.ClientID %>");
            editId = $("#<%=hidEditId.ClientID %>");
            deleteId = $("#<%=hidDeleteId.ClientID %>");
            isShowForm = $("#<%=hidIsShowForm.ClientID %>");
            // 返回主表
            $("#back").click(function(){
                if(isEdit.val() == "True")
                    if(!confirm("您修改了表格但尚未保存，确定返回后编辑的内容将会丢失，确定吗？"))return false;
                window.parent.winVisible(true);
                return false;
            });
            
            // 如果项目已经被删除，则跳转回主表界面
            if($("#hidIsDeleted").val() == "True"){
                alert("项目已经被删除");
                $("#back").click();
            }
            
            // 添加
            $("#add").click(function(){
                clearInput();
                editState.val("add");
                inputShow();
            });
            $(".table")
            // 编辑
            .on("click",".edit",function(){
                isShowForm.val("True");
                editState.val("edit");
                var id = $(this).data("id");
                editId.val(id);
               __doPostBack('btnEdit',''); 
            })
            // 删除
            .on("click",".delete",function(){
                if(!confirm("确定删除吗？"))return;
                var id = $(this).data("id");
                deleteId.val(id);
               __doPostBack('btnDelete',''); 
            })
            .on("click",".item-add",function(){
                $("#add").trigger("click");
            })
            // 取消添加或编辑
            $("#btnCancel").click(function(){
                $(".page-shadow").hide();
                $("#dialog").hide();
                return false;
            });
            // 格式化保存按钮
            $("#<%=btnSave.ClientID %>").click(function(){
                if(!confirm("确定保存吗？"))return false;
                __doPostBack('btnSave',''); 
            }).removeAttr("href");
            // 提交
            $("#submit").click(function(){
                if(!confirm("此操作不可逆，确定提交吗？"))return false;
            });
            // 加载显示输入表单
            if(isShowForm.val() == "True"){
                inputShow();
                isShowForm.val("False");
            }
            
            var parentBody = $(window.parent.document.body);
            // 新增并且尚未保存的项目
            if($("#<%=hidIsNew.ClientID %>").val() == "True"){
                parentBody.find(".item_edit").hide();
                parentBody.find("#btnAdd").show();
                parentBody.find("#mySave").show();
                createAdd();
            }
            else{
                // 已经提交的项目
                if("<%=Model.DocState %>" == "Completed"){
                    parentBody.find(".item_edit").hide();
                }
                else{
                    parentBody.find(".item_edit").show();
                    createAdd();
                }
            }

        });
        
        // 列表最后新增添加行
        function createAdd(){
            var table = $(".panel-detail .table:eq(0)");
            table.append("<tr><td class='item-add' colspan='13'><img src='../../images/grid/add1.png' /></td></tr>");
        }
        
        function mySave(){
            if(!confirm("确定保存吗？"))return false;
            __doPostBack('btnSave',''); 
        }
        function mySubmit(){
            if(!confirm("此操作不可逆，确定提交吗？"))return false;
            __doPostBack('submit',''); 
        }
        function myDelete(){
            if(!confirm("确定删除吗？"))return false;
            __doPostBack('btnMasterDelete',''); 
        }
        
        
        // 清除输入表单数据
        function clearInput(){
            $("#dialog").find(":text").val("");
        }
        
        function inputShow(){
            var modal = $("<div class='page-shadow'>");
            modal.css("height",$(window).height() + "px");
            $("body").append(modal);
            $("#dialog").show();
        }
        
        $.fn.Number = function(){
            console.log(this);
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="divMain" class="divMain">
        <div id="divMaster" class="divMaster">
            <table cellpadding="0" cellspacing="0" class="MasterTable"  >
                <tr class="MasterTableTr">
                    <td nowrap="nowrap" class="MasterTableTdLabel">单据编号：</td>
                    <td nowrap="nowrap" class="MasterTableTdText"><asp:TextBox ID="txtCode" runat="server"></asp:TextBox></td>
                    <td nowrap="nowrap" class="MasterTableTdLabel">制单人：</td>
                    <td nowrap="nowrap" class="MasterTableTdText"><asp:TextBox ID="txtCreatePerson" runat="server"></asp:TextBox></td>
                    <td nowrap="nowrap" class="MasterTableTdLabel">业务员：</td>
                    <td nowrap="nowrap" class="MasterTableTdText"><asp:TextBox ID="txtHandlerPerson" runat="server"></asp:TextBox></td>
                 </tr>
                <tr class="MasterTableTr">
                    <td nowrap="nowrap" class="MasterTableTdLabel" style="display:none;">业务日期：</td>
                    <td nowrap="nowrap" class="MasterTableTdText" style="display:none;"><asp:TextBox ID="txtCreateDate" runat="server"></asp:TextBox></td>
                    <td nowrap="nowrap" class="MasterTableTdLabel">备注：</td>
                    <td nowrap="nowrap" class="MasterTableTdText"><asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" style="width:400px;height:50px;"></asp:TextBox></td>
                 </tr>
            </table>
        </div>
        <div class="panel-tool" style="display:none;">
            <h3 class="panel-title">
                功能区：</h3>
            <ul>
                <li id="back">返回</li>
                <li id="add">新增</li>
                <li id="save"><asp:LinkButton ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click"></asp:LinkButton></li>
                <li id="edit" style="display:none;"><asp:LinkButton ID="btnEdit" runat="server" Text="保存" OnClick="btnEdit_Click"></asp:LinkButton></li>
                <li id="delete" style="display:none;"><asp:LinkButton ID="btnDelete" runat="server" Text="删除" OnClick="btnDelete_Click"></asp:LinkButton><asp:LinkButton ID="submit" runat="server" Text="提交" OnClick="submit_Click"></asp:LinkButton><asp:LinkButton ID="btnMasterDelete" runat="server" OnClick="btnMasterDelete_Click"></asp:LinkButton></li>
            </ul>
        </div>
        <hr style="clear: both" />
        <div class="panel-content">
            <div class="panel-detail">
                <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="false" CssClass="table"
                    EmptyDataText="没有详细数据...">
                    <AlternatingRowStyle BackColor="#F7FAFF" />
                    <Columns>
                        <asp:TemplateField HeaderText="操作">
                            <ItemTemplate>
                            <%if (Model.DocState == VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit)
                              { %>
                                <a href="#" class="edit" data-id='<%#Eval("Id") %>'><img src="../../images/grid/edit1.png" title="修改" /></a> <a href="#" class="delete" data-id='<%#Eval("Id") %>'><img src="../../images/grid/del1.png" title="删除" /></a>
                                <%} %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="单位名称" DataField="DepartmentName" />
                        <asp:BoundField HeaderText="项目" DataField="ProjectName" />
                        <asp:BoundField HeaderText="银行" DataField="BankName" />
                        <asp:BoundField HeaderText="目前余额(元)" DataField="Balance" />
                        <asp:BoundField HeaderText="费率" DataField="Rate" />
                        <asp:TemplateField HeaderText="起始日期" HeaderStyle-Width="120px">
                            <ItemTemplate>
                                <%#Convert.ToDateTime(Eval("StartDate")).ToString("yyyy-MM-dd")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="终止日期" HeaderStyle-Width="120px">
                            <ItemTemplate>
                                <%#Convert.ToDateTime(Eval("EndDate")).ToString("yyyy-MM-dd")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="付费" DataField="PayType" />
                        <asp:TemplateField HeaderText="计费起始日期" HeaderStyle-Width="160px">
                            <ItemTemplate>
                                <%#Convert.ToDateTime(Eval("StartChargingDate")).ToString("yyyy-MM-dd")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="计费终止日期" HeaderStyle-Width="160px">
                            <ItemTemplate>
                                <%#Convert.ToDateTime(Eval("EndChargingDate")).ToString("yyyy-MM-dd")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="天数" DataField="TotalDay" />
                        <asp:BoundField HeaderText="应付金额" DataField="AmountPayable" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <div class="dialog" id="dialog">
            <table width="400px">
                <caption style="text-align:left">
                    <b>信息输入...</b>
                    <hr />
                </caption>
                <tbody>
                    <tr>
                        <td>单位名称</td><td><asp:TextBox ID="txtDepartmentName" runat="server" CssClass="panel-input"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>项目</td><td><asp:TextBox ID="txtProjectName" runat="server" CssClass="panel-input"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>银行</td><td><asp:TextBox ID="txtBankName" runat="server" CssClass="panel-input"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>目前余额（元）</td><td><asp:TextBox ID="txtBalance" runat="server" CssClass="panel-input"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>费率（%）</td><td><asp:TextBox ID="txtRate" runat="server" CssClass="panel-input"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>起始日期</td><td><asp:TextBox ID="txtStartDate" runat="server" CssClass="panel-input" onfocus="new WdatePicker(this,'%Y-%M-%D',true)"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>终止日期</td><td><asp:TextBox ID="txtEndDate" runat="server" CssClass="panel-input" onfocus="new WdatePicker(this,'%Y-%M-%D',true)"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>付费方式</td><td>
                            <asp:DropDownList ID="ddlPayType" runat="server">
                                <asp:ListItem>一次</asp:ListItem>
                                <asp:ListItem>两次</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>计费起始日</td><td><asp:TextBox ID="txtStartChargingDate" runat="server" CssClass="panel-input" onfocus="new WdatePicker(this,'%Y-%M-%D',true)"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>计费终止日</td><td><asp:TextBox ID="txtEndChargingDate" runat="server" CssClass="panel-input" onfocus="new WdatePicker(this,'%Y-%M-%D',true)"></asp:TextBox></td>
                    </tr>
                    <!-- <tr>
                        <td>天数</td><td><asp:TextBox ID="txtTotalDay" runat="server" CssClass="panel-input"></asp:TextBox></td>
                    </tr> -->
                    <tr>
                        <td>应付金额（元）</td><td><asp:TextBox ID="txtAmountPayable" runat="server" CssClass="panel-input"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:Button ID="txtbtnSubmit" runat="server" Text="确定" OnClick="btnSubmit_Click" CssClass="panel-button" />
                        <button id="btnCancel" class="panel-button">取消</button></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <asp:HiddenField ID="hidEditState" runat="server" />                <!-- 区分操作是新增还是编辑 -->
    <asp:HiddenField ID="hidIsEdit" runat="server" />                   <!-- 标识是否有修改过 -->
    <asp:HiddenField ID="hidEditId" runat="server" />                   <!-- 正在修改的列表ID -->
    <asp:HiddenField ID="hidDeleteId" runat="server" />                 <!-- 正在删除的列表ID -->
    <asp:HiddenField ID="hidIsShowForm" runat="server" />               <!-- 是否加载时显示 -->
    <asp:HiddenField ID="hidIsNew" runat="server" />                    <!-- 是否是新增 -->
    <asp:HiddenField ID="hidIsDeleted" runat="server" />                <!-- 是否是新增 -->
    </form>
</body>
</html>
<script type="text/javascript">

    $.fn.extend({
        number : function(){
            this.keydown(function(e){
                if(e.which == 8 || e.which == 46)return true;
                if(e.which < 48 || (e.which > 57)){
                   return false; 
                } 
            })
        },
        float : function(){
            this.keydown(function(e){
                if(e.which == 8 || e.which == 46)return true;
                if(e.which < 48 || (e.which > 57 && e.which != 190)){
                    return false; 
                }
                if($(this).val().indexOf(".") > -1 && e.which == 190){
                    return false;
                }
            })
        }
    });
    
    $("#txtBalance").number();
    $("#txtRate").float();
    $("#txtAmountPayable").number();
        
</script>