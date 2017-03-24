<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DailyCashFlowReporter.aspx.cs" Inherits="Rpt_MoneyManage_DailyCashFlowManage_DailyCashFlowReporter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>日现金流统计</title>
    <meta charset="UTF-8">
    <style type="text/css">
        body{
        	margin:0;
        	padding:0px;
        	background:#fff;
        }
        .panel{
        	width:100%;
        	height:auto;
        	margin:0 auto;
        	
        }
        
        .table{
        	border-collapse:collapse;
        	width:100%;
        	font-size:9px;
        	font-family : 微软雅黑;
        }
        .table th{
        	padding:3px;
        	background:#eee;
        	height:22px;
        	font-size:14px;
        }
        .table td{
        	padding:0px;
        	text-align:center;
        	height:18px;
        	white-space:nowrap;
        }
        .table .table-rowTitle{
            width:80px;
        	padding:5px;
        	background:#eee;
        	text-align:left;
        	font-size:11px;
        }
        .table .table-row-footer{
        	
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="panel">
        <div class="panel-title"></div>
        <div class="panel-body">
            <table border="1" class="table">
                <tbody>
                    <tr>
                        <th rowspan="2">项目类别</th>
                        <th rowspan="2">期初</th>
                        <th colspan="4">资金流入</th>
                        <th colspan="4">资金流出</th>
                        <th colspan="6">结余</th>
                    </tr>
                    <tr>
                        <th>当日</th>
                        <th>本月</th>
                        <th>本年</th>
                        <th>累计</th>
                        <th>当日</th>
                        <th>本月</th>
                        <th>本年</th>
                        <th>累计</th>
                        <th>当日</th>
                        <th>日均</th>
                        <th>本月</th>
                        <th>月均</th>
                        <th>本年</th>
                        <th>累计</th>
                    </tr>
                    <tr>
                        <td class="table-rowTitle">上交</td>
                        <td rowspan="19">192,192,394</td>
                        <td>192,192,394</td>
                        <td>192,192,394</td>
                        <td>192,192,394</td>
                        <td>192,192,394</td>
                        <td>192,192,394</td>
                        <td>192,192,394</td>
                        <td>192,192,394</td>
                        <td>192,192,394</td>
                        <td rowspan="19">192,192,394</td>
                        <td rowspan="19">192,192,394</td>
                        <td rowspan="19">192,192,394</td>
                        <td rowspan="19">192,192,394</td>
                        <td rowspan="19">192,192,394</td>
                        <td rowspan="19">192,192,394</td>
                    </tr>
                    <tr>
                        <td class="table-rowTitle">借款</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="table-rowTitle">保理金额</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="table-rowTitle">工程款</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="table-rowTitle">以房（物）抵债</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="table-rowTitle">投标保证金</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="table-rowTitle">履约保证金</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="table-rowTitle">农民工工资保证金</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="table-rowTitle">其他保证金</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="table-rowTitle">押金</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="table-rowTitle">备用金</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="table-rowTitle">政府规费</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="table-rowTitle">间接费用</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="table-rowTitle">管理费用</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="table-rowTitle">财务费用</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="table-rowTitle">税金</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="table-rowTitle">租赁费</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="table-rowTitle">材料</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="table-rowTitle">其他</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="table-row-footer">合计</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="panel-footer">
           <asp:Button ID="btnExport" runat="server" Text="导出" OnClick="btnExport_Click" />
        </div>
    </div>
    </form>
</body>
</html>
