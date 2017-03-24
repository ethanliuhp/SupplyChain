<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FactoringDataDetail.aspx.cs"
    Inherits="MoneyManage_FactoringDataDetail" %>

<div class="modal-header bg-primary">
    <button class="close" data-dismiss="modal">
        <span>&times;</span></button>
    <h4 class="modal-title">
        单据【<%=Request["id"] %>】详情</h4>
</div>
<div class="modal-body">
    <table class="table table-hover" id="table_subDetail">
        <thead>
            <tr style="font-size:9px;">
                <th>
                    序号
                </th>
                <th>
                    单位名称
                </th>
                <th> 
                    项目
                </th>
                <th>
                    银行
                </th>
                <th>
                    目前余额（元）
                </th>
                <th>
                    费率
                </th>
                <th>
                    起始日期
                </th>
                <th>
                    终止日期
                </th>
                <th>
                    付费
                </th>
                <th>
                    计费起始日
                </th>
                <th>
                    计费终止日
                </th>
                <th>
                    天数
                </th>
                <th>
                    应付金额（元）
                </th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <%for (int i = 0; i < ListData.Count; i++)
              {
                  var item = ListData[i];
            %>
                <tr data-id="<%=item.Id %>">
                    <td><%=i+1 %></td>
                    <td name="DepartmentName"><%=item.DepartmentName%></td>
                    <td name="ProjectName"><%=item.ProjectName%></td>
                    <td name="BankName"><%=item.BankName%></td>
                    <td name="Balance"><%=item.Balance%></td>
                    <td name="Rate"><%=item.Rate%></td>
                    <td name="StartDate"><%=item.StartDate.ToShortDateString()%></td>
                    <td name="EndDate"><%=item.EndDate.ToShortDateString()%></td>
                    <td name="PayType"><%=item.PayType%></td>
                    <td name="StartChargingDate"><%=item.StartChargingDate.ToShortDateString()%></td>
                    <td name="EndChargingDate"><%=item.EndChargingDate.ToShortDateString()%></td>
                    <td name="TotalDay"><%=item.TotalDay%></td>
                    <td name="AmountPayable"><%=item.AmountPayable%></td>
                    <td>
                        <span class="glyphicon glyphicon-edit" name="detail_edit"></span>&nbsp;&nbsp;
                        <span class="glyphicon glyphicon-remove" name="detail_delete"></span>
                    </td>
                </tr>
            <%
                } %>
        </tbody>
    </table>
</div>
<div class="modal-footer">
    <div class="btn-group">
        <button class="btn btn-primary" id="add_SubDetail">
            添加</button>
    </div>
</div>



<script>
	$("#add_SubDetail").on("click",function(){
		$("#myModal").modal("hide");
		$("#myModalAddDetail").modal({
			show:true
		});
	});
    var a;
    $("span[name=detail_edit]").on("click",function(){
        $("#myModal").modal("hide");

        var data_tr = $(this).closest("tr");
        editDetailId = data_tr.data("id");
        var tds = data_tr;
        a = tds;
        console.log(tds.find("td[name=DepartmentName]").text());
        $("input[name=DepartmentName]").val($.trim(tds.find("td[name=DepartmentName]").text()));
        $("input[name=ProjectName]").val($.trim(tds.find("td[name=ProjectName]").text()));
        $("input[name=BankName]").val($.trim(tds.find("td[name=BankName]").text()));
        $("input[name=Balance]").val($.trim(tds.find("td[name=Balance]").text()));
        $("input[name=Rate]").val($.trim(tds.find("td[name=Rate]").text()));
        $("input[name=StartDate]").val($.trim(tds.find("td[name=StartDate]").text()));
        $("input[name=EndDate]").val($.trim(tds.find("td[name=EndDate]").text()));
        var payId;
        if($.trim(tds.find("td[name=EndDate]").text()) == "一次")payId = "c1";else payId = "c2";
        $("#"+payId).attr("checked","checked");
        $("#"+payId).parent().addClass("active").siblings("label").removeClass('active');
        $("input[name=StartChargingDate]").val($.trim(tds.find("td[name=StartChargingDate]").text()));
        $("input[name=EndChargingDate]").val($.trim(tds.find("td[name=EndChargingDate]").text()));
        $("input[name=TotalDay]").val($.trim(tds.find("td[name=TotalDay]").text()));
        $("input[name=AmountPayable]").val($.trim(tds.find("td[name=AmountPayable]").text()));

        $("#myModalAddDetail").modal({show:true});
    });


    $("span[name=detail_delete]").on("click",function(){
        deleteDetailId = $(this).closest("tr").data("id");
        $("#myModal").modal("hide");
        $("#modal_delete_detail").modal({show:true});
    });
</script>

