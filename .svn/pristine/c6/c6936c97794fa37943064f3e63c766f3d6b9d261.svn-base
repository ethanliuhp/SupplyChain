<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BaiduMap.aspx.cs" Inherits="Map_BaiduMap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>百度地图测试用例</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <style type="text/css">
        html
        {
            height: 100%;
            width: 100%;
        }
        #container
        {
            height: 100%;
            width: 100%;
            margin: 0px;
            padding: 0px;
        }
        .divColor
        {
            filter: progid:DXImageTransform.Microsoft.gradient(enable=true,startColorstr=#3F8BBC, endColorstr=#FFFFFF); /*9AC9DA*/
        }
    </style>

    <script type="text/javascript" src="http://api.map.baidu.com/api?v=1.4"></script>

    <script type="text/javascript" src="SearchInfoWindow_min.js"></script>

</head>
<body style="height: 100%; margin: 0px; padding: 0px; width: 100%" ondisposed="AddMark()">
    <div id="container" onload="AddMark()">
    </div>

    <script type="text/javascript">
        var arrInfo = new Array();
        var map; // 创建地图实例
        var clickCount = 0;
        window.onload = function() {

            InitData();

        }

        function InitData() {

            var isUsedDefineIcons = document.getElementById("txtProjectType").value.split("|");
            var projectNames = document.getElementById("txtProjectName").value.split("|");
            var tels = document.getElementById("txtTel").value.split("|");
            var places = document.getElementById("txtPlace").value.split("|");
            var citys = document.getElementById("txtCity").value.split("|");
            var pointXs = document.getElementById("txtPointX").value.split("|");
            var pointYs = document.getElementById("txtPointY").value.split("|");
            var projectIds = document.getElementById("txtProjectIdStr").value.split("|");
            var projectSyscodes = document.getElementById("txtProjectSyscodeStr").value.split("|");
            var projectTypes = document.getElementById("txtProjectType").value.split("|");

            var projectName = projectNames[0];
            var tel = tels[0];
            var place = places[0];
            var projectId = projectIds[0];
            var projectSyscode = projectSyscodes[0];
            var projectType = projectTypes[0];

            arrInfo[0] = { name: projectName, tel: tel, place: place, projectId: projectId, projectSyscode: projectSyscode, projectType: projectType };


            map = new BMap.Map("container"); // 创建地图实例
            map.enableScrollWheelZoom();
            map.enableKeyboard();
            map.addControl(new BMap.NavigationControl());
            map.addControl(new BMap.ScaleControl());
            map.addControl(new BMap.OverviewMapControl());
            map.addControl(new BMap.MapTypeControl());

            //先定位到北京，后面再添加每一个项目、公司、分公司坐标
            //            point = new BMap.Point(116.403694, 39.914714); //北京坐标
            //            map.centerAndZoom(point, 16); // 初始化地图，设置中心点坐标和地图级别
            //            map.setCurrentCity("北京市");
            //            map.setZoom(5);

            map.setZoom(5);


            for (var i = 1; i < projectNames.length; i++) {

                var projectName = projectNames[i];
                var isUsedDefineIcon = isUsedDefineIcons[i];
                var tel = tels[i];
                var place = places[i];
                var city = citys[i];
                var pointX = pointXs[i];
                var pointY = pointYs[i];
                var projectId = projectIds[i];
                var projectSyscode = projectSyscodes[i];
                var projectType = projectTypes[i];

                arrInfo[i] = { name: projectName, tel: tel, place: place, projectId: projectId, projectSyscode: projectSyscode, projectType: projectType };

                var point = new BMap.Point(pointX, pointY);

                //机构类型 h 总公司；hd 总公司部门；b 分公司；bd 分公司部门；zgxmb 直管项目部；fgsxmb 分公司项目部

                if (pointX != "" && pointY != "") {
                    if (isUsedDefineIcon == "h")
                        AddMarkAndNewIconByCompany(point, i);
                    else if (isUsedDefineIcon == "b")
                        AddMarkAndNewIconBySubCompany(point, i);
                    else
                        AddMarkAndNewIconByProject(point, i);
                }
            }

            point = new BMap.Point(114.33753, 30.539917); //武汉坐标
            AddMarkAndNewIconByCompany(point, 0);

            //            map.setCurrentCity("中国");
            //            map.setCurrentCity("武汉");
            map.centerAndZoom(point, 5); // 初始化地图，设置中心点坐标和地图级别
            map.centerAndZoom(point, 5); // 初始化地图，设置中心点坐标和地图级别
            map.setZoom(5);

            //            point = new BMap.Point(114.513458, 38.047821); //北京坐标
            //            map.centerAndZoom(point, 5); // 初始化地图，设置中心点坐标和地图级别
            //            map.setCurrentCity("石家庄");
            //            map.setZoom(5);
        }

        function AddMark(point, index) {

            var p = point;  //new BMap.Point(114.33753, 30.539917);
            //            var index = 0;
            var marker = new BMap.Marker(point);
            marker.enableDragging();
            marker.addEventListener("click", function(e) { MarkClick(marker, point, index) });
            var label = new BMap.Label(arrInfo[index]['name'], { offset: new BMap.Size(20, -10) });
            //marker.setLabel(label);
            marker.setTitle(arrInfo[index]['name']);

            map.addOverlay(marker); //在地图中添加marker

        }

        function AddMarkAndNewIconByCompany(point, index) {

            //            var index = 0;
            //添加自定义图标
            var myIcon = new BMap.Icon("../images/map/company.png", new BMap.Size(30, 41));
            var marker2 = new BMap.Marker(point, { icon: myIcon });  // 创建标注
            var label = new BMap.Label(arrInfo[index]['name'], { offset: new BMap.Size(20, -10) });
            //marker2.setLabel(label);
            marker2.setTitle(arrInfo[index]['name']);
            marker2.addEventListener("click", function(e) { MarkClick(marker2, point, index) });
            map.addOverlay(marker2); //在地图中添加marker
        }
        function AddMarkAndNewIconBySubCompany(point, index) {

            //            var index = 0;
            //添加自定义图标
            var myIcon = new BMap.Icon("../images/map/subcompany.png", new BMap.Size(25, 34));
            var marker2 = new BMap.Marker(point, { icon: myIcon });  // 创建标注
            var label = new BMap.Label(arrInfo[index]['name'], { offset: new BMap.Size(20, -10) });
            //marker2.setLabel(label);
            marker2.setTitle(arrInfo[index]['name']);
            marker2.addEventListener("click", function(e) { MarkClick(marker2, point, index) });
            map.addOverlay(marker2); //在地图中添加marker
        }
        function AddMarkAndNewIconByProject(point, index) {

            //            var index = 0;
            //添加自定义图标
            var myIcon = new BMap.Icon("../images/map/project.png", new BMap.Size(20, 28));
            var marker2 = new BMap.Marker(point, { icon: myIcon });  // 创建标注
            var label = new BMap.Label(arrInfo[index]['name'], { offset: new BMap.Size(20, -10) });
            //marker2.setLabel(label);
            marker2.setTitle(arrInfo[index]['name']);
            marker2.addEventListener("click", function(e) { MarkClick(marker2, point, index) });
            map.addOverlay(marker2); //在地图中添加marker
        }

        function AddIcon(point) {
            var pt = point; //new BMap.Point(x, y);
            var myIcon = new BMap.Icon("images/map/sanju.ico", new BMap.Size(30, 30));
            var marker2 = new BMap.Marker(pt, { icon: myIcon });  // 创建标注
            //  map.addOverlay(marker2);              // 将标注添加到地图中
            var label = new BMap.Label("中南国际", { offset: new BMap.Size(20, -10) });
            marker2.setLabel(label);
            marker2.setTitle("中南国际");
            var infoWindow2 = new BMap.InfoWindow("<p style='font-size:14px;'>分公司图标</p>");
            marker2.addEventListener("click", function() { this.openInfoWindow(infoWindow2); });
            map.addOverlay(marker2); //在地图中添加marker
        }

        function MarkClick(mark, point, index) {

            map.centerAndZoom(point, 7); // 初始化地图，设置中心点坐标和地图级别

            if (index > -1 && index <= arrInfo.length - 1) {
                //                var opts = {
                //                    width: 600, // 信息窗口宽度
                //                    height: 370, // 信息窗口高度
                //                    title: arrInfo[index]['name'] // 信息窗口标题 
                //                }
                var html = getInfo(mark, index).join("");
                var infoWin = new BMap.InfoWindow(html, { offset: new BMap.Size(0, -10) }); //, { offset: new BMap.Size(0, -10) }
//                infoWin.setWidth(730);
//                infoWin.setHeight(650);
//                infoWin.enableMaximize();
//                infoWin.setMaxContent(html);
                mark.openInfoWindow(infoWin);

            }
        }
        function getInfo(mark, index) {

            var projectId = arrInfo[index]['projectId'];
            var projectSyscode = arrInfo[index]['projectSyscode'];
            var projectType = arrInfo[index]['projectType'];

            var sHTML = [];
            sHTML.push('<table border="1" cellpadding="1" cellspacing="1" style="margin-top:15px;">');
            sHTML.push('  <tr>');
            sHTML.push('      <td align="right" class="common">名称</td>');
            sHTML.push('      <td  align="left"> ' + arrInfo[index]['name'] + '</td>');
            sHTML.push('  </tr>');
            sHTML.push('  <tr>');
            sHTML.push('      <td  align="right" class="common">电话</td>');
            sHTML.push('      <td  align="left"> ' + arrInfo[index]['tel'] + '</td>');
            sHTML.push('  </tr>');
            sHTML.push('  <tr>');
            sHTML.push('      <td  align="right" class="common">地址</td>');
            sHTML.push('      <td  align="left"> ' + arrInfo[index]['place'] + '</td>');
            sHTML.push('  </tr>');
            sHTML.push('  <tr>');
            sHTML.push('      <td colspan="2"><iframe scrolling="auto" width="595" height="350" id="frmMapChart" frameborder="0" marginwidth="0" marginheight="0" src="../MainPage/MainPageBottomMap.aspx?ProjectId=' + projectId + '&ProjectType=' + projectType + '&ProjectSyscode=' + projectSyscode + '&rand=' + Math.random() + '"></iframe></td>');
            sHTML.push('  </tr>');
            //            sHTML.push('  <tr>');
            //            sHTML.push('      <td  align="right" class="common">来源于</td>');
            //            sHTML.push('      <td  align="left"> <a href="http://map.baidu.com/">百度地图服务</a>  ' + " " + '</td>');
            //            sHTML.push('  </tr>');
            //            sHTML.push('  <tr>');
            //            sHTML.push('      <td   colspan=2 class="common"><input type="button" value ="点击我" id="Click" onclick="TS(' + index + ')"/> </td>');
            //            sHTML.push('  </tr>');
            sHTML.push('  </table>');

            return sHTML;
        }

        function TS(index) {
            //alert(index.toString());
            window.open("http://zhidao.baidu.com/question/256314449.html");
        }
        function GetPosition() {
            var dd = map.getZoom();

            //alert(dd.toString());
        }


    </script>

    <input type="button" id="btnClick" onclick="AddMark()" style="display: none" />
    <input type="hidden" id="txtProjectType" runat="server" value="h|" />
    <input type="hidden" id="txtProjectName" runat="server" value="中建三局总承包公司|" />
    <input type="hidden" id="txtTel" runat="server" value="&nbsp;|" />
    <input type="hidden" id="txtPlace" runat="server" value="武昌中南路口|" />
    <input type="hidden" id="txtCity" runat="server" value="武汉市|" />
    <input type="hidden" id="txtPointX" runat="server" value="114.33753|" />
    <input type="hidden" id="txtPointY" runat="server" value="30.539917|" />
    <input type="hidden" id="txtProjectIdStr" runat="server" value="0_8J_eQfP068UvW6Eu_qkO|" />
    <input type="hidden" id="txtProjectSyscodeStr" runat="server" value="91cf7bf193fb4824adbcfbf5360369b3.0_8J_eQfP068UvW6Eu_qkO.|" />
</body>
</html>
