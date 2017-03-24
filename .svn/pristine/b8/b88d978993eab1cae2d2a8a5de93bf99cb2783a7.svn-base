<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FactoringDataManage.aspx.cs"
    Inherits="MoneyManage_FactoringDataManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="/IRPWebClient/CSS/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/bootstrap-datetimepicker.css" rel="stylesheet" type="text/css" />
    <style>
        .table-hover:hover
        {
            cursor: pointer;
        }
        .modal .row
        {
            margin-top: 3px;
        }
        .table .glyphicon
        {
            transition: opacity 0.5s;
            opacity: 0.3;
        }
        .table .glyphicon:hover
        {
            opacity: 1;
        }
        .modal-dialog,.modal-content{
            margin:30px auto;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container">
        <div class="panel panel-primary">
            <!-- Default panel contents -->
            <div class="panel-heading">
                理保业务</div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-xs-6 col-sm-6 col-md-2">
                        <span style="line-height:30px;">单据编号</span>
                    </div>
                    <div class="col-xs-6 col-sm-6 col-md-4">
                        <asp:TextBox ID="txtNumber" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="row" style="margin-top:10px">
                    <div class="col-xs-12 col-sm-12 col-md-2">
                        <span style="line-height:30px;">起始日期</span>
                    </div>
                    <div class="col-xs-12 col-sm-12 col-md-4">
                        <div class="input-group">
                            <asp:TextBox ID="txtStartDate" placeholder="起始开始日期..." runat="server" CssClass="form-control" data-date-format="yyyy-mm-dd"></asp:TextBox>
                            <span class="input-group-addon">
                                <i class="glyphicon glyphicon-time"></i>
                            </span>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-12 col-md-4">
                        <div class="input-group">
                            <asp:TextBox ID="txtEndDate" placeholder="起始结束日期..." runat="server" CssClass="form-control" data-date-format="yyyy-mm-dd"></asp:TextBox>
                            <span class="input-group-addon">
                                <i class="glyphicon glyphicon-time"></i>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top:10px">
                    <div class="col-md-offset-8">
                        <asp:Button ID="btn_Search" runat="server" OnClick="btn_Search_Click" CssClass="btn btn-primary" Text="确定"></asp:Button>
                    </div>
                </div>
            </div>
            <table class="table table-striped table-hover" id="table_Data_Master">
                <thead>
                    <tr>
                        <th>
                            序号
                        </th>
                        <th>
                            单据编号
                        </th>
                        <th>
                            制单人
                        </th>
                        <th>
                            业务员
                        </th>
                        <th>
                            制单日期
                        </th>
                        <th>
                            所属组织
                        </th>
                        <th>
                            备注
                        </th>
                        <th>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <%for (int i = 0; i < ListData.Rows.Count; i++)
                      {
                          var item = ListData.Rows;
                    %>
                    <tr data-id="<%=item[i]["id"] %>">
                        <td>
                            <%=i + 1 %>
                        </td>
                        <td>
                            <%=(item[i]["code"]+"").Trim() %>
                        </td>
                        <td>
                            <%=(item[i]["createpersonname"]+"").Trim()%>
                        </td>
                        <td>
                            <%=(item[i]["handlepersonname"]+"").Trim()%>
                        </td>
                        <td>
                            <%=(item[i]["createdate"]+"").Trim()%>
                        </td>
                        <td>
                            <%=(item[i]["operorgname"]+"").Trim()%>
                        </td>
                        <td>
                            <%=(item[i]["descript"]+"").Trim()%>
                        </td>
                        <td>
                            <span class="glyphicon glyphicon-edit"></span>&nbsp;&nbsp; <span class="glyphicon glyphicon-remove">
                            </span>
                        </td>
                    </tr>
                    <%
                        } %>
                    <tr>
                        <td colspan="9" style="text-align: center;">
                            <span class="glyphicon glyphicon-plus" style="font-size: 20px;"></span>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div class="modal fade" id="modal_Add_Marster" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-primary">
                    <button class="close" data-dismiss="modal">
                        <span>&times;</span>
                    </button>
                    <h4 class="modal-title">
                        填写明细</h4>
                </div>
                <div class="modal-body" id="info_subDetail">
                    <div class="row">
                        <div class="col-md-4">
                            单据编号
                        </div>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtCode" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            业务员
                        </div>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtHandler" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            备注
                        </div>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtDescrip" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="btn-group">
                        <asp:Button ID="btn_Add_Master" runat="server" OnClick="btn_Add_Master_Click" CssClass="btn btn-primary right"
                            Text="确定"></asp:Button>
                        <asp:HiddenField ID="hid_edit_id" runat="server" />
                        <asp:HiddenField ID="hid_operateState" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="modal_delete" tabindex="-1">
        <div class="modal-dialog">
            <!-- modal-lg modal-sm -->
            <div class="modal-content">
                <div class="modal-header bg-primary">
                    <button class="close" data-dismiss="modal">
                        <span>&times;</span>
                    </button>
                    <h4 class="modal-title">
                        提示</h4>
                </div>
                <div class="modal-body">
                    <h4>
                        确定删除吗？</h4>
                </div>
                <div class="modal-footer">
                    <div class="btn-group">
                        <asp:Button ID="btn_delete" runat="server" OnClick="btn_Delete_Click" CssClass="btn btn-primary right"
                            Text="确定"></asp:Button>
                        <asp:HiddenField ID="hid_delete_id" runat="server" />
                        <button class="btn btn-primary right" id="btn_delete_cancle">
                            取消</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="modal_add" tabindex="-1">
        <div class="modal-dialog">
            <!-- modal-lg modal-sm -->
            <div class="modal-content">
                <div class="modal-header bg-primary">
                    <button class="close" data-dismiss="modal">
                        <span>&times;</span>
                    </button>
                    <h4 class="modal-title">
                        提示</h4>
                </div>
                <div class="modal-body">
                    <h4>
                        确定添加吗？</h4>
                </div>
                <div class="modal-footer">
                    <div class="btn-group">
                        <button class="btn btn-primary" id="btn_add_submit">
                            确定</button>
                        <button class="btn btn-default" id="btn_add_cancle">
                            取消</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
    <div class="modal fade" id="myModal" tabindex="-1">
        <div class="modal-dialog modal-lg" style="width: 1000px;">
            <!-- modal-lg modal-sm -->
            <div class="modal-content">
            </div>
        </div>
    </div>
    <div class="modal fade" id="myModalAddDetail" tabindex="-1">
        <div class="modal-dialog">
            <!-- modal-lg modal-sm -->
            <form action="" id="form_add_subDetail">
            <div class="modal-content">
                <div class="modal-header bg-primary">
                    <button class="close" data-dismiss="modal">
                        <span>&times;</span>
                    </button>
                    <h4 class="modal-title">
                        填写明细</h4>
                </div>
                <div class="modal-body" id="info_subDetail">
                    <div class="row">
                        <div class="col-md-4">
                            单位名称
                        </div>
                        <div class="col-md-8">
                            <input type="text" name="DepartmentName" class="form-control" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            项目
                        </div>
                        <div class="col-md-8">
                            <input type="text" name="ProjectName" class="form-control" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            银行
                        </div>
                        <div class="col-md-8">
                            <input type="text" name="BankName" class="form-control" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            目前余额（元）
                        </div>
                        <div class="col-md-8">
                            <input type="text" name="Balance" class="form-control" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            费率（%）
                        </div>
                        <div class="col-md-8">
                            <input type="number" name="Rate" class="form-control" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            起始日期
                        </div>
                        <div class="col-md-8">
                            <input type="date" name="StartDate" class="form-control" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            终止日期
                        </div>
                        <div class="col-md-8">
                            <input type="date" name="EndDate" class="form-control" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            付费方式
                        </div>
                        <div class="col-md-8">
                            <div class="btn-group" data-toggle="buttons">
                                <label for="c1" class="btn btn-default active">
                                    <input id="c1" type="radio" name="PayType" value="一次" checked>一次</label>
                                <label for="c2" class="btn btn-default">
                                    <input id="c2" type="radio" name="PayType" value="两次">两次</label>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            计费起始日
                        </div>
                        <div class="col-md-8">
                            <input type="date" name="StartChargingDate" class="form-control" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            计费终止日
                        </div>
                        <div class="col-md-8">
                            <input type="date" name="EndChargingDate" class="form-control" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            天数
                        </div>
                        <div class="col-md-8">
                            <input type="number" name="TotalDay" class="form-control" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            应付金额（元）
                        </div>
                        <div class="col-md-8">
                            <input type="number" name="AmountPayable" class="form-control" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="btn-group">
                        <button class="btn btn-primary" id="btn_add_subDetail">
                            确定</button>
                    </div>
                </div>
            </div>
            </form>
        </div>
    </div>
    <div class="modal fade" id="modal_delete_detail" tabindex="-1">
        <div class="modal-dialog">
            <!-- modal-lg modal-sm -->
            <div class="modal-content">
                <div class="modal-header bg-primary">
                    <button class="close" data-dismiss="modal">
                        <span>&times;</span>
                    </button>
                    <h4 class="modal-title">
                        提示</h4>
                </div>
                <div class="modal-body">
                    <h4>
                        确定删除吗？</h4>
                </div>
                <div class="modal-footer">
                    <div class="btn-group">
                        <button class="btn btn-primary right" id="btn_detail_delete_submit">
                            确定</button>
                        <button class="btn btn-primary right" id="btn_detail_delete_cancle">
                            取消</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="/IRPWebClient/JavaScript/angular.js" type="text/javascript"></script>

    <script type="text/javascript" src="/IRPWebClient/JavaScript/jquery-1.10.2.js"></script>

    <script src="/IRPWebClient/JavaScript/bootstrap.js" type="text/javascript"></script>

    <script src="/IRPWebClient/js/app.js" type="text/javascript"></script>

    <script src="/IRPWebClient/JavaScript/jquery.serializejson.js" type="text/javascript"></script>

    <script src="../../JavaScript/bootstrap-datetimepicker.js" type="text/javascript"></script>

    <script src="../../JavaScript/bootstrap-datetimepicker.zh-CN.js" type="text/javascript"></script>

    <script src="/IRPWebClient/js/controller.js" type="text/javascript"></script>

    <script src="/IRPWebClient/js/componentinit.js" type="text/javascript"></script>

    <script type="text/javascript">
		var infoId,editId,editDetailId,deleteDetailId;
		$("#table_Data_Master tr:not(:first):not(:last)").on("click",function(){
			infoId = $(this).data("id");
			if(!infoId)return;
			var url = "FactoringDataDetail.aspx?id="+infoId;

			$("#myModal").modal({
				backdrop:"static",
				show:true,
				remote:url
			});
		});
		$("#myModal").on("hidden.bs.modal",function(){
			$(this).removeData("bs.modal");
		});
		$("#table_Data_Master tr:last").on("click",function(){
            $("#hid_operateState").val("Add");
			$("#modal_Add_Marster").modal({show:true});
		});
		// 防止时间冒泡
		$("#table_Data_Master>tbody>tr:not(:last)").find(">td:last").on("click",function(e){
			e.stopPropagation();
		});
		$("#btn_add_subDetail").on("click",function(){
			$("#myModalAddDetail").modal("hide");
			$("#modal_add").modal({
				show:true
			});
			return false;
		});
		$("#btn_add_submit").on("click",function(){
			var formData = $("#form_add_subDetail").serializeJSON();
			var info = {};
			info = $.extend(info,formData);
			info.ParentId = infoId;
			info.columns = $.formatColumns(info);
			info.key = "Add_SubDetail";
			$.post("FactoringDataHandler.ashx",info,function(data){
				var table = $("#table_subDetail");
				var row = $("<tr>");
				var index = table.find("tr").length + 1;
				row.append("<td>"+index+"</td>");
				for(var item in formData){
					row.append("<td>"+formData[item]+"</td>");
				}
				table.append(row);
				$("#modal_add").modal("hide");
				$("#myModal").model({show:true});
			});
		});
		$("#btn_add_cancle").on("click",function(){
			$(".modal").modal("hide");
			$("#myModalAddDetail").modal({
				show:true
			});
		});

		// 修改主表
		$(".glyphicon-edit").on("click",function(){
            $("#hid_operateState").val("Edit");
			$("#modal_Add_Marster").modal({show:true});
            var data_tr = $(this).closest("tr");
            $("#hid_edit_id").val(data_tr.data("id"));
            var tds = data_tr.find("td");
            console.log(tds);
            $("#txtCode").val($.trim($(tds[1]).text()));
            $("#txtHandler").val($.trim($(tds[3]).text()));
            $("#txtDescrip").val($.trim($(tds[6]).text()));
		});
		// 删除
		$(".glyphicon-remove").on("click",function(){
			$("#modal_delete").modal({
				show:true
			});
			var id = $(this).closest("tr").data("id");
			$("#hid_delete_id").val(id)
		});
        // 取消删除
		$("#btn_delete_cancle").on("click",function(){
			$("#modal_delete").modal("hide");
			return false;
		});

        $("#btn_detail_delete_cancle").on("click",function(){
            $("#modal_delete_detail").modal("hide");
            $("#myModal").modal({show:true});
        });
        $("#btn_detail_delete_submit").on("click",function(){
            var url = "FactoringDataHandler.ashx?key=Detail_Delete&id="+deleteDetailId;
            $.get(url,{},function(data){
                $("#myModal").modal({show:true});
                $("#modal_delete_detail").modal("hide");
                var target = $("#myModal table").find("tr[data-id="+deleteDetailId+"]");
                target.hide();
            });
        });

        $("#txtStartDate").datetimepicker({
            format: "yyyy-mm-dd",
            autoclose: true,
            minView: "month",
            maxView: "decade",
            todayBtn: true,
            pickerPosition: "bottom-left"
        });
        $("#txtEndDate").datetimepicker({
            format: "yyyy-mm-dd",
            autoclose: true,
            minView: "month",
            maxView: "decade",
            todayBtn: true,
            pickerPosition: "bottom-left"
        });


    </script>

</body>
</html>
