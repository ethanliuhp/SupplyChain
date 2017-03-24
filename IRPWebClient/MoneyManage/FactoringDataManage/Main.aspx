<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="Main.aspx.cs" Inherits="MoneyManage_FactoringDataManage_Main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>保理业务</title>
    <link rel="stylesheet" href="../../css/bootstrap.css">
    <link href="../../CSS/bootstrap-datetimepicker.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="css/style.css">
    <script type="text/javascript" src="../../JavaScript/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="../../JavaScript/json2.js"></script>
    <script src="../../JavaScript/bootstrap.js" type="text/javascript"></script>
    <script src="../../JavaScript/bootstrap-datetimepicker.js" type="text/javascript"></script>
    <script src="../../JavaScript/bootstrap-datetimepicker.zh-CN.js" type="text/javascript"></script>
    <script src="../../js/componentinit.js" type="text/javascript"></script>
    <style type="text/css">
        .recordstyle{
        	float:right;
        	font-size:15px;
        	font-weight:bold;
        	padding:8px;
        	color:#000;
        	margin-top:25px;
        }
        .side{
        	overflow:hidden;
        }
        #detail{
            display:none;
        }
        
        tbody tr:last-child{
            background-color:#eee;
        }
        .glyphicon{
            opacity:0.5;
            transition:all 0.5s;
        }
        .glyphicon:hover{
            opacity:1;
        }

    </style>
    <script type="text/javascript">
        $(function(){
            // 注册时间控件
            var startDate = $("input[id*=txtStartDate]").datetimepicker({
                format: "yyyy-mm-dd",
                autoclose: true,
                minView: "month",
                maxView: "decade",
                todayBtn: true,
                pickerPosition: "bottom-left"
            }).val();
            var endDate = $("input[id*=txtEndDate]").datetimepicker({
                format: "yyyy-mm-dd",
                autoclose: true,
                minView: "month",
                maxView: "decade",
                todayBtn: true,
                pickerPosition: "bottom-left"
            }).val();
            $("#StartDate").datetimepicker({format: "yyyy-mm-dd",autoclose: true,minView: "month",maxView: "decade",todayBtn: true,pickerPosition: "bottom-center"});
            $("#EndDate").datetimepicker({format: "yyyy-mm-dd",autoclose: true,minView: "month",maxView: "decade",todayBtn: true,pickerPosition: "bottom-center"});
            $("#StartChargingDate").datetimepicker({format: "yyyy-mm-dd",autoclose: true,minView: "month",maxView: "decade",todayBtn: true,pickerPosition: "bottom-center"});
            $("#EndChargingDate").datetimepicker({format: "yyyy-mm-dd",autoclose: true,minView: "month",maxView: "decade",todayBtn: true,pickerPosition: "bottom-center"});
            
            // 分页控制
            $(".pagination a[href*=pageindex]").each(function(item){
                $(this).attr("href",$(this).attr("href") + "&startDate="+startDate+"&endDate="+endDate);
            });
            
            
            // 子主表操作
            var height,                 // 记录主表的高度
            masterEditId,               // 编辑主表数据id
            masterDeleteId,             // 删除主表数据id
            detailEditId,               // 编辑子表数据id
            detailDeleteId,             // 删除子表数据id
            masterSearchId,             // 正在查看的主表id    
            newDetailArr = [];          // 新增加的子表记录

            
            $("#master tbody")
            .on("dblclick","tr",function(){                         // 主表行双击事件
                var id = $(this).data("id");
                var projectName = $(this).find("td:eq(1)").text();
                if($(this).find(".label-info").length > 0){
                    var alertInfo = $('<div class="alert alert-info fade in" id="alert_master"><a href="javascript:void(0)" class="close" data-dismiss="alert">&times;</a><strong>警告！</strong>请保存后再双击查看！</div>');
                    $("#master .panel-body").after(alertInfo);
                    setTimeout(function(){
                        alertInfo.alert("close");
                    },3000);
                }
                if(!id)return;
                masterSearchId = id;
                $("#detail tbody").html("");
                showModal();

                // 判断是否已经加载过子表数据
                var targetInfo;
                $("#detail_title").text(projectName);
                $.each(detailInfoArr,function(){
                    if(this.Id == masterSearchId){
                        targetInfo = this;
                    }
                })
                if(!targetInfo){
                    $.get("MainHandler.ashx",{key:"GetDetailById",id:id},function(data){
                        if(data){
                            data = $.parseJSON(data);
                        }
                        buildDetail(data);
                        detailInfoArr.push({Id:id,info:data});
                    });
                }
                else{
                    buildDetail(targetInfo.info);
                }
            })
            .on("click",".master-edit",function(e){                 // 主表数据编辑
                masterEditId = $(this).data("id");
                $("#hid_operateState").val("Edit");
                $("#modal_Add_Marster").modal();
                $("#txtCode").attr("disabled","disabled");
                $("#hid_edit_id").val(masterEditId);
                $.get("MainHandler.ashx",{key:"GetMasterInfo",id:masterEditId},function(data){
                    var data = $.parseJSON(data)
                    $("#txtCode").val(data.Code);
                    $("#txtHandler").val(data.HandlePersonName);
                    $("#txtDescrip").val(data.Descript);
                });
            })
            .on("click",".master-delete",function(e){               // 主表数据删除
                $("#hidMasterDeleteId").val($(this).data("id"));
                $("#modal_master_delete").modal();
            });

            $("#master tr:last").on("click",function(){             // 主表添加新记录
                $("#hid_operateState").val("Add");
                $("#modal_Add_Marster :text").val("");
                $("#txtCode").removeAttr("disabled");
                $("#modal_Add_Marster").modal();
            });
            
            $(".btn-cancel").on("click",function(){                 // 取消操作
                $(".modal").modal("hide");
            });

            function buildDetail(data){
                if(data.length > 0){
                    createDetail(data);
                }
                height = $("#master").height();
                $("#master").animate({height:"0px"},300);
                $("#detail").show();
                $("#detail tbody").append('<tr><td colspan="14" style="text-align: center;"><span class="glyphicon glyphicon-plus" style="font-size: 20px;"></span></td></tr>');
                hideModal();
            }
            
            
            // 返回主表
            $(".csces-back").on("click",function(){
                // 返回主表时提示
                if(newDetailArr.length > 0){
                    $("#modal_detail_back").modal();
                }
                else{
                    $("#modal_detail_back .btn-submit").click();
                }
            });

            // 返回列表模态框
            $("#modal_detail_back")
            .on("click",".btn-cancel",function(){
                $(".modal").modal("hide");
            })
            .on("click",".btn-submit",function(){
                $("#master").animate({height:height},300,function(){
                    $("#detail").hide();
                });
                newDetailArr = [];
                $(".modal").modal("hide");
            })
            // .find(".btn-submit").click(function(){
            //     $("#master").animate({height:height},300,function(){
            //         $("#detail").hide();
            //     });
            //     newDetailArr = [];
            //     $(".modal").modal("hide");
            // });
            
            // 保存新增主表数据
            $("#btnSave").click(function(){
                if($("#master .label-info").length == 0)return;
                $("#modal_master_add").modal();
            });

            var detailOperaType;                                    // 操作方式【add、update】
            $("#detail tbody")
            .on("click","tr:last",function(){                       // 子表添加
                detailOperaType = "add";
                $("#myModalAddDetail input[type=text]").val("");
                $("#myModalAddDetail input[type=number]").val("");
                $("#myModalAddDetail").modal();
            })
            .on("click",".glyphicon-edit",function(){               // 子表数据编辑
                detailOperaType = "edit";
                var trInfo = $(this).closest("tr");
                var id = trInfo.data("id");
                var targetInfo;
                if(trInfo.find(".label-danger").length > 0){
                    $.each(newDetailArr,function(){
                        if(id == this.Id){
                            targetInfo = this;
                        }
                    });
                }
                else{
                    $.each(detailInfoArr,function(){
                        if(this.Id == masterSearchId){
                            $.each(this.info,function(){
                                if(this.Id == id){
                                    targetInfo = this;
                                }
                            })
                        }
                    });
                }
                $.each(detailColumns,function(){
                    if(this != "PayType"){
                        $("input[name="+this+"]").val(targetInfo[this]);
                    }
                    else{
                        $("input:radio[value='"+this+"']").attr("checked",true);
                    }
                });
                $("#myModalAddDetail").modal();
                detailEditRow = trInfo;                             // 记录正在编辑的行
            })
            .on("click",".glyphicon-remove",function(){
                $("#modal_detail_delete").modal();
                deleteRow = $(this).closest("tr");
            });

            // 子表添加确认
            var detailRecord = 0;                                   // 新增加子表自增长id
            var detailEditRow;                                      // 正在编辑的子表行 
            var deleteRow;                                          // 准备删除的记录
            $("#btn_add_subDetail").click(function(){
                var info = {};
                info.DepartmentName = $("input[name='DepartmentName']").val();
                info.ProjectName = $("input[name='ProjectName']").val();
                info.BankName = $("input[name='BankName']").val();
                info.Balance = $("input[name='Balance']").val();
                info.Rate = $("input[name='Rate']").val();
                info.StartDate = $("input[name='StartDate']").val();
                info.EndDate = $("input[name='EndDate']").val();
                info.PayType = $("input[name='PayType']:checked").val();
                info.StartChargingDate = $("input[name='StartChargingDate']").val();
                info.EndChargingDate = $("input[name='EndChargingDate']").val();
                info.TotalDay = $("input[name='TotalDay']").val();
                info.AmountPayable = $("input[name='AmountPayable']").val();
                
                if(detailOperaType == "add"){
                    // 添加到新增子表数组
                    info.Id = detailRecord++;
                    newDetailArr.push(info);
                    // 创建一个tr
                    var trInfo = $("<tr>");
                    trInfo.attr("data-id",info.Id);
                    trInfo.append('<td><span class="label label-danger">new</span></td>');
                    trInfo.append('<td>'+info.DepartmentName+'</td>');
                    trInfo.append('<td>'+info.ProjectName+'</td>');
                    trInfo.append('<td>'+info.BankName+'</td>');
                    trInfo.append('<td>'+info.Balance+'</td>');
                    trInfo.append('<td>'+info.Rate+'</td>');
                    trInfo.append('<td>'+info.StartDate+'</td>');
                    trInfo.append('<td>'+info.EndDate+'</td>');
                    trInfo.append('<td>'+info.PayType+'</td>');
                    trInfo.append('<td>'+info.StartChargingDate+'</td>');
                    trInfo.append('<td>'+info.EndChargingDate+'</td>');
                    trInfo.append('<td>'+info.TotalDay+'</td>');
                    trInfo.append('<td>'+info.AmountPayable+'</td>');
                    trInfo.append('<td><span class="glyphicon glyphicon-edit detail-edit"></span>&nbsp;&nbsp; <span class="glyphicon glyphicon-remove detail-delete"></span></td>');
                    $("#detail tr:eq(1)").before(trInfo);
                    $("#myModalAddDetail").modal("hide");
                }
                else if(detailOperaType == "edit"){
                    var rowId = detailEditRow.data("id");
                    // 判断是否是新增的行
                    if(detailEditRow.find(".label").length > 0){
                        // 新增加的行
                        $.each(newDetailArr,function(){
                            if(rowId == this.Id){
                                $.extend(this, info);
                                return;
                            }
                        });
                    }
                    else{
                        // 原始数据
                        info.Id = rowId;
                        $.post("MainHandler.ashx",{key:"EditDetail",id:masterSearchId,data:JSON.stringify(info)},function(data){
                            $.each(detailInfoArr,function(){
                                if(this.Id == masterSearchId){
                                    $.each(this.info,function(){
                                        if(rowId == this.Id){
                                            $.extend(this,info);
                                            return false;
                                        }
                                    })
                                    return false;
                                }
                            });
                        });
                    }
                    var tds = detailEditRow.find("td");
                    for(var i = 1;i < detailColumns.length;i++){
                        $(tds[i]).text(info[detailColumns[i]]);
                    }
                    $("#myModalAddDetail").modal("hide");
                }
            });
            
            // 子表新添加记录保存
            $("#btnDetailSave").click(function(){
                if (newDetailArr.length == 0) return;
                $("#modal_detail_save").modal({backdrop:"static"});
            });

            $("#modal_detail_save")
            .on("click",".btn-submit",function(){
                $("#modal_detail_save").modal("hide");
                showModal();
                var data = JSON.stringify(newDetailArr);
                $.post("MainHandler.ashx",{key:"SaveDetail",data:data,id:masterSearchId},function(data){
                    data = $.parseJSON(data);
                    for (var i =  0; i < detailInfoArr.length; i++) {
                        if(detailInfoArr[i].Id == masterSearchId){
                            detailInfoArr[i].info = data;
                            break;
                        }
                    };
                    newDetailArr = [];
                    hideModal();
                    // createDetail(data);
                    buildDetail(data);
                });
            })

            // 子表模态窗口事件
            $("#modal_detail_delete")
            .on("click",".btn-submit",function(){
                var rowId = deleteRow.data("id");
                if(deleteRow.find(".label").length > 0){
                    // 新增加的行
                    $.each(newDetailArr,function(n){
                         if(this.Id == rowId){
                            newDetailArr.splice(n,1);
                         }
                    });
                }
                else{
                    // 原始数据行
                    var row = searchDetailRowInMasterArr(detailInfoArr,masterSearchId,rowId);
                    $.post("MainHandler.ashx",{key:"DeleteDetail",id:masterSearchId,detailId:rowId});
                    row.detail.info.splice(row.index,1);
                }
                deleteRow.remove();
                $(".modal").modal("hide");
            })
            
        });

        // 找到数组对象中对应的数据行
        function searchDetailRowInMasterArr(arr,masterId,detailId){
            var info = {};
            $.each(arr,function(){
                if(this.Id == masterId){
                    $.each(this.info,function(n){
                        if(this.Id == detailId){
                            info.row = this;
                            info.index = n;
                        }
                    });
                    info.detail = this;
                    return false;
                }
            });
            return info;
        }

        // 页面跳转
        function changePageIndex(num){
            $("input:hidden[id*=hidPageIndex]").val(num);
            __doPostBack('btnJump','');
        }
        
        // 显示模态框
        function showModal(){
            if($(".modalcsces").length > 0){
                $(".modalcsces").show();
            }
            else{
                var modal = $("<div class='modalcsces'>");
                var img = $("<img class='modal-img' src='../../images/loading.gif' />");
                modal.append(img);
                $("body").append(modal);
            }
        }
        // 隐藏模态框
        function hideModal(){
            $(".modalcsces").hide();
        }
        var detailColumns = ["Row_Number","DepartmentName","ProjectName","BankName","Balance","Rate","StartDate","EndDate","PayType","StartChargingDate","EndChargingDate","TotalDay","AmountPayable"];
        var detailInfoArr = [];
        // 创建子表数据
        function createDetail(data){
            var body = $("#detail tbody");
            body.html("");
            $(data).each(function(n){
                var tr = $("<tr>");
                tr.attr("data-id",this.Id);
                for(var k=0;k < detailColumns.length;k++){
                    if(k == 0) {
                        tr.append($("<td>"+(n+1)+"</td>"));
                        continue;
                    }
                    tr.append($("<td>"+this[detailColumns[k]]+"</td>"));
                }
                tr.append($('<td><span class="glyphicon glyphicon-edit master-edit"></span>&nbsp;&nbsp; <span class="glyphicon glyphicon-remove master-delete"></span></td>'))
                body.append(tr);
            });
        }
        
        
        
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="master" class="side">
            <div class="panel panel-info">
                <div class="panel-heading">
                    <h3 class="panel-title">保理业务</h3>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1" style="height: 100%;">
                            <span class="label panel-search">截止日期</span>
                        </div>
                        <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4">
                            <div class="input-group">
                                <span class="input-group-addon">开始日期</span>
                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" placehodler="开始日期..."></asp:TextBox>
                                <span class="input-group-addon">-</span>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" placehodler="结束日期..."></asp:TextBox>
                                <span class="input-group-addon">结束日期</span>
                            </div>
                        </div>
                        <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2">
                            <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" CssClass="btn btn-default" />
                        </div>
                        <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                        </div>
                        <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2">
                            <input type="button" value="保存" id="btnSave" class="btn btn-default" />
                        </div>
                    </div>
                </div>
                <table class="table table-striped table-hover">
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
                                备注
                            </th>
                            <th>
                                状态
                            </th>
                            <th>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <%for (int i = 0; i < NewMasterList.Count; i++)
                        {
                          %>
                        <tr data-id="<%=NewMasterList[i].Id %>">
                            <td><span class="label label-info">new</span></td>
                            <td><%=NewMasterList[i].Code%></td>
                            <td><%=NewMasterList[i].CreatePersonName%></td>
                            <td><%=NewMasterList[i].HandlePersonName%></td>
                            <td><%=NewMasterList[i].CreateDate.ToString("yyyy-MM-dd")%></td>
                            <td><%=NewMasterList[i].Descript%></td>
                            <td><%=NewMasterList[i].Temp1%></td>
                            <td>
                                <span class="glyphicon glyphicon-edit master-edit" data-id="<%=NewMasterList[i].Id %>"></span>&nbsp;&nbsp; <span class="glyphicon glyphicon-remove master-delete" data-id="<%=i %>"></span>
                            </td>
                        </tr>
                        <%
                    } %>
                        <%for (int i = 0; i < ListData.Count; i++)
                        {
                          %>
                        <tr data-id="<%=ListData[i].Id %>">
                            <td><%=ListData[i].Temp2%></td>
                            <td><%=ListData[i].Code%></td>
                            <td><%=ListData[i].CreatePersonName%></td>
                            <td><%=ListData[i].HandlePersonName%></td>
                            <td><%=ListData[i].CreateDate.ToString("yyyy-MM-dd")%></td>
                            <td><%=ListData[i].Descript%></td>
                            <td><%=ListData[i].Temp1%></td>
                            <td>
                                <span class="glyphicon glyphicon-edit master-edit" data-id="<%=ListData[i].Id %>"></span>&nbsp;&nbsp; <span class="glyphicon glyphicon-remove master-delete" data-id="<%=i %>"></span>
                            </td>
                        </tr>
                        <%
                    } %>
                        <tr>
                            <td colspan="8" style="text-align: center;">
                                <span class="glyphicon glyphicon-plus table-add" style="font-size: 20px;"></span>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <span  class="label recordstyle">共&nbsp;<lable class="text-primary"><%=RecordCount%></lable>&nbsp;条记录</span>
                <ul class="pagination" style='float:right;<%=AllPageNumber <= 1 ? "display:none;" : "" %>'>
                    <li><asp:LinkButton ID="btnPrev" runat="server" Text="&laquo;" OnClick="btnPrev_Click" /></li>
                      <%for (int i = 1; i < AllPageNumber + 1; i++)
                      {
                        %>
                        <li<%=i == PageIndex ? " class='active'" : "" %>><a href="javascript:changePageIndex('<%=i %>')"><%=i%></a></li>
                        <%
                    } %>
                    <li><asp:LinkButton ID="btnNext" runat="server" Text="&raquo;" OnClick="btnNext_Click" /></li>
                </ul>
                <asp:LinkButton ID="btnJump" runat="server" OnClick="btnJump_Click"></asp:LinkButton>
                <asp:HiddenField ID="hidPageIndex" runat="server" />
            </div>
        </div>

        <div id="detail" class="side">                      <!-- 子表框 -->
            <div class="panel panel-warning">
                <div class="panel-heading">
                    <div class="panel-title">
                        <div class="row">
                            <div class="col-md-2">
                                子表信息 【<label id="detail_title"></label>】
                            </div>
                            <div class="col-md-offset-9 col-md-1">
                                <span class="csces-back">
                                    <span class="glyphicon glyphicon-chevron-left"></span>返回
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-body">
                    <input type="button" id="btnDetailSave" class="btn btn-default" value="保存" />
                </div>
                <table class="table table-hover table-striped">
                    <thead>
                        <tr>
                            <th>序号</th>
                            <th>单位名称</th>
                            <th>项目</th>
                            <th>银行</th>
                            <th>目前余额(元)</th>
                            <th>费率</th>
                            <th>起始日期</th>
                            <th>终止日期</th>
                            <th>付费</th>
                            <th>计费起始日期</th>
                            <th>计费终止日期</th>
                            <th>天数</th>
                            <th>应付金额</th>   
                            <th></th>          
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
        </div>



        <div class="modal fade" id="modal_Add_Marster" tabindex="-1">       <!-- 主表添加框 -->
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header bg-primary">
                        <button class="close" data-dismiss="modal">
                            <span>&times;</span>
                        </button>
                        <h4 class="modal-title">
                            填写明细
                        </h4>
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
                        <asp:Button ID="btn_Add_Master" runat="server" OnClick="btn_Add_Master_Click" CssClass="btn btn-primary right" Text="确定"></asp:Button>
                        <asp:HiddenField ID="hid_edit_id" runat="server" />
                        <asp:HiddenField ID="hid_operateState" runat="server" />
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal_master_delete" tabindex="-1">    <!-- 删除记录提示 -->
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header bg-primary">
                        <button class="close" data-dismiss="modal">
                            <span>&times;</span>
                        </button>
                        <h4 class="modal-title">
                            提示
                        </h4>
                    </div>
                    <div class="modal-body">
                        <h4>
                            确定删除吗？
                        </h4>
                    </div>
                    <div class="modal-footer">
                        <div class="btn-group">
                            <!-- <button type="button" class="btn btn-primary right" id="">确定</button> -->
                            <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" CssClass="btn btn-primary right" Text="确定"></asp:LinkButton>
                            <asp:HiddenField ID="hidMasterDeleteId" runat="server"></asp:HiddenField>
                            <button type="button" class="btn btn-primary right btn-cancel">取消</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal_master_add" tabindex="-1">    <!-- 保存主表新增记录提示 -->
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header bg-primary">
                        <button class="close" data-dismiss="modal">
                            <span>&times;</span>
                        </button>
                        <h4 class="modal-title">
                            提示
                        </h4>
                    </div>
                    <div class="modal-body">
                        <h4>
                            确定保存吗？
                        </h4>
                    </div>
                    <div class="modal-footer">
                        <div class="btn-group">
                            <asp:Button ID="btnSaveMaster" runat="server" Text="保存" OnClick="btnSave_Click" CssClass="btn btn-primary" />
                            <button type="button" class="btn btn-primary right btn-cancel">
                                取消</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="myModalAddDetail" tabindex="-1">    <!-- 子表添加明细 -->
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
                                <input type="text" id="StartDate" name="StartDate" class="form-control" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                终止日期
                            </div>
                            <div class="col-md-8">
                                <input type="text" id="EndDate" name="EndDate" class="form-control" />
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
                                <input type="text" id="StartChargingDate" name="StartChargingDate" class="form-control" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                计费终止日
                            </div>
                            <div class="col-md-8">
                                <input type="text" id="EndChargingDate" name="EndChargingDate" class="form-control" />
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
                            <button type="button" class="btn btn-primary" id="btn_add_subDetail">
                                确定</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal_detail_delete" tabindex="-1">    <!-- 删除记录提示 -->
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header bg-primary">
                        <button class="close" data-dismiss="modal">
                            <span>&times;</span>
                        </button>
                        <h4 class="modal-title">
                            提示
                        </h4>
                    </div>
                    <div class="modal-body">
                        <h4>
                            确定删除吗？
                        </h4>
                    </div>
                    <div class="modal-footer">
                        <div class="btn-group">
                            <button type="button" class="btn btn-primary right btn-submit">确定</button>
                            <button type="button" class="btn btn-primary right btn-cancel">取消</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal_detail_save" tabindex="-1">    <!-- 保存记录提示 -->
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header bg-primary">
                        <button class="close" data-dismiss="modal">
                            <span>&times;</span>
                        </button>
                        <h4 class="modal-title">
                            提示
                        </h4>
                    </div>
                    <div class="modal-body">
                        <h4>
                            确定保存吗？
                        </h4>
                    </div>
                    <div class="modal-footer">
                        <div class="btn-group">
                            <button type="button" class="btn btn-primary right btn-submit">确定</button>
                            <button type="button" class="btn btn-primary right btn-cancel">取消</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modal_detail_back" tabindex="-1">    <!-- 保存记录提示 -->
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header bg-primary">
                        <button class="close" data-dismiss="modal">
                            <span>&times;</span>
                        </button>
                        <h4 class="modal-title">
                            提示
                        </h4>
                    </div>
                    <div class="modal-body">
                        <h4>
                            您有尚未保存的编辑项，返回后将会丢失，确定返回吗？
                        </h4>
                    </div>
                    <div class="modal-footer">
                        <div class="btn-group">
                            <button type="button" class="btn btn-primary right btn-submit">确定</button>
                            <button type="button" class="btn btn-default right btn-cancel">取消</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hidSearchMasterId" runat="server"></asp:HiddenField>
    </form>
</body>
</html>
