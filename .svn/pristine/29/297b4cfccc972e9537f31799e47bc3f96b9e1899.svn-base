angular.module('starter.controllers', [])

// 财务关键信息统计
.controller("IndexCtrl",["$scope","$http","$ionicLoading","$timeout",function($scope,$http,$ionicLoading,$timeout){
  // grade:gs【公司】
  $scope.title = '公司财务信息统计';
  $scope.$parent.curDate = new Date();
  $scope.dateChange = function(date){
    $scope.$parent.curDate = date;
    loadData();
  };

  $scope.prev = function(){
    $scope.$parent.curDate = new Date(addDate($scope.$parent.curDate,-1));
    loadData();
  }

  $scope.next = function(){
    $scope.$parent.curDate = new Date(addDate($scope.$parent.curDate,1));
    loadData();
  }

  function addDate(date,days){
    date.setDate(date.getDate()+days);
    return date;
  }

  function loadData(){
    $scope.list = [];
    $ionicLoading.show({
      content: 'Loading',
      animation: 'fade-in',
      showBackdrop: true,
      maxWidth: 200,
      showDelay: 0
    });

    $http({url:"services/FundManagementHandler.ashx",method:"get",params:{key:"GetCompanyIndex",date:$scope.curDate}})
    .success(function(data){
      $timeout(function () {
        $ionicLoading.hide();
      }, 1000);
      $scope.list = data;
    });
  }
  $scope.prev();
}])
// 营业收入
.controller("BusinessincomeCtrl",["$scope","$http","$timeout",function($scope,$http,$timeout){
  $scope.title = '营业收入情况表';
  $scope.info = [];
    $scope.view = true;

    var status = 0;

    var zoption;//柱状图option
    var boption;//饼状图option


    $scope.drawImage = function(){
        var dom = document.getElementById("img");
        var myChart = echarts.init(dom);
        if (status == 1) {
            myChart.setOption(boption, true);
        }else if (status == 2) {
            myChart.setOption(zoption,true);
        }
        $scope.$broadcast('scroll.refreshComplete');
    };

    $scope.viewChange = function(){     // 视图切换
        switch (status) {
            case 0:
                $scope.view = false;
                status = 1;
                break;
            case 1:
                status = 2;
                $scope.view = false;
                break;
            case 2:
                status = 0;
                $scope.view = true;
                break;
        }
        $timeout(function () {
            $scope.drawImage();
        },50);
    };

    $http({url:"services/FundManagementHandler.ashx",params:{key:"BusinessPlannIcome",date:$scope.curDate},method:"get"})
    .success(function(data){
        var operorgname = [];
        var income = [];
        var planincome = [];
        angular.forEach(data,function(obj){
            operorgname.push(obj.Name);
            income.push((obj.Income/100000000).toFixed(2));
            planincome.push((obj.PlanIncome/100000000).toFixed(2));
        });
        // 指定图表的配置项和数据
       zoption = {
            title: {
                text: '营业收入对比分析图',
                x: 'left'
            },
            tooltip: {trigger: 'axis'},
            legend: {
                data: ['年度计划值', '年度累计值'],
                orient: 'horizontal',      // 布局方式，默认为水平布局，可选为：
                // 'horizontal' ¦ 'vertical'
                x: 'center',               // 水平安放位置，默认为全图居中，可选为：
                                           // 'center' ¦ 'left' ¦ 'right'
                                           // ¦ {number}（x坐标，单位px）
                y: 'top',                  // 垂直安放位置，默认为全图顶端，可选为：
                                           // 'top' ¦ 'bottom' ¦ 'center'
                                           // ¦ {number}（y坐标，单位px）
                //itemWidth: '10',
                //itemHeight: '10'
            },
            xAxis: {
                type: 'category',
                data: operorgname,
                //axisLabel: {
                //    interval: 0,
                //    rotate: 45
                //},
                splitLine: {show: false}
            },
            yAxis: {
//            name:'万元'
            },
            series: [{
                name: '年度计划值',
                type: 'bar',
                data: planincome,
                itemStyle: {
                    normal: {
//                    color: '#ffaabb',//如果需要设置颜色
                        label: {
                            show: true,
                            position: 'top'
                        }
                    }
                }
            }, {
                name: '年度累计值',
                type: 'bar',
                data: income,
                itemStyle: {
                    normal: {
                        label: {
                            show: true,
                            position: 'top'
                        }
                    }
                }
            }]
        };
    });

  $http({url:"services/FundManagementHandler.ashx",params:{key:"Businessincome",date:$scope.curDate},method:"get"})
  .success(function(data){
    $scope.info = data;

    var departmentArr = [];
    var total = 0;
    var rateArr = [];
    angular.forEach(data,function(obj){
      departmentArr.push(obj.Name);
      total+=obj.Year;
      rateArr.push({name:obj.Name,value:obj.Year});
    });

      boption = {
        title : {
        text: '本年营业收入' + (total/100000000).toFixed(1) + '亿元',
        subtext: '',
        x:'center'
    },
    tooltip : {
        trigger: 'item',
        // formatter: "{a} <br/>{b} : {c} ({d}%)"
        formatter:function(obj){
          return obj.seriesName + "<br>" + obj.name + '：' + (obj.value/100000000).toFixed(2) + "亿元" + "("+obj.percent+"%)";
        }
    },
    legend: {
        orient: 'horizontal',
        top:'50px',
        data: departmentArr
    },
    series : [
        {
          name: '本年收入',
          type: 'pie',
          radius : '55%',
          center: ['50%', '60%'],
          data:rateArr,
          itemStyle: {
            emphasis: {
                shadowBlur: 10,
                shadowOffsetX: 0,
                shadowColor: 'rgba(0, 0, 0, 0.5)'
            },
            normal:{
              label:{
                show:true,
                formatter: function(obj){
                  return obj.name + (obj.value/100000000).toFixed(2);
                },
                labelLine :{show:true}
              }
            }
          }
        }
      ]
    };
  });
}])
// 资金存量情况
.controller("StockmomeyCtrl",["$scope","$http",function($scope,$http){
  $scope.title = '资金存量情况表';
  $scope.info = [];
  $http({url:"services/FundManagementHandler.ashx",params:{key:"Stockmomey",date:$scope.curDate},method:"get"})
  .success(function(data){
    $scope.info = data;
  });
}])
// 当日、本月、本年收入
.controller("ReceivablesCtrl",["$scope","$http",function($scope,$http){
  $scope.title = '收款情况表';
  $scope.info = [];
  $http({url:"services/FundManagementHandler.ashx",params:{key:"ReceivableMoney",date:$scope.curDate},method:"get"}).success(function(data){
      $scope.info = data;
  });
}])
// 应收账款情况
.controller("AccountsReceivableCtrl",["$scope","$http",function($scope,$http){
  $scope.title = '应收账款情况';
  $http({url:"services/FundManagementHandler.ashx",params:{key:"AccountsReceivable",date:$scope.curDate},method:"get"}).success(function(data){
      $scope.info = data;
  });
}])
// 应收拖欠款情况表
.controller("ArrearsCtrl",["$scope","$http",function($scope,$http){
  $scope.title = '应收拖欠款情况表';
  $scope.info = [];
  $http({url:"services/FundManagementHandler.ashx",params:{key:"Arrears",date:$scope.curDate},method:"get"}).success(function(data){
    $scope.info = data;
    $scope.view = true;
    $scope.viewChange = function(){     // 视图切换
      $scope.view = !$scope.view;
    }

    var departmentArr = [];
    var total = 0;
    var rateArr = [];
    angular.forEach(data,function(obj){
      departmentArr.push(obj.Name);
      total+=obj.N;
      rateArr.push({name:obj.Name,value:obj.N});
    });
    var option = {
        title : {
        text: '累计拖欠款' + (total/100000000).toFixed(1) + '亿元',
        subtext: '',
        x:'center'
    },
    legend: {
        orient: 'horizontal',
        top:'50px',
        data: departmentArr
    },
    tooltip : {
        trigger: 'item',
        // formatter: "{a} <br/>{b} : {c} ({d}%)"
        formatter:function(obj){
          return obj.seriesName + "<br>" + obj.name + '：' + (obj.value/100000000).toFixed(2) + "亿元" + "("+obj.percent+"%)";
        }
    },
    series : [
        {
          name: '累计拖欠款',
          type: 'pie',
          radius : '55%',
          center: ['50%', '60%'],
          data:rateArr,
          itemStyle: {
            emphasis: {
              shadowBlur: 10,
              shadowOffsetX: 0,
              shadowColor: 'rgba(0, 0, 0, 0.5)'
            },
            normal:{
              label:{
                show:true,
                formatter: function(obj){
                  return obj.name + (obj.value/100000000).toFixed(2);
                },
                labelLine :{show:true}
              }
            }
          }
        }
      ]
    };
    $scope.drawImage = function(){
      var dom = document.getElementById("img");
      var myChart = echarts.init(dom);
      myChart.setOption(option, true);
      $scope.$broadcast('scroll.refreshComplete');
    };
    $scope.drawImage();
  });
}])
// 资金占用情况表
.controller("OccupyCtrl",["$scope","$http",function($scope,$http){
  $scope.title = '资金占用情况表';
  $scope.info = [];
  $scope.view = true;
  $scope.viewChange = function(){     // 视图切换
    $scope.view = !$scope.view;
  };
  $http({url:"services/FundManagementHandler.ashx",params:{key:"Occupy",date:$scope.curDate},method:"get"}).success(function(data){
    $scope.info = data;

    var departmentArr = [];
    var total = 0;
    var rateArr = [];
    angular.forEach(data,function(obj){
      departmentArr.push(obj.Name);
      total+=obj.N;
      rateArr.push({name:obj.Name,value:obj.N});
    });
    var dom = document.getElementById("img");
    $(dom).css("width",$(dom).parent().width());
    var myChart = echarts.init(dom);
    var option = {
        title : {
        text: '累计资金占用' + (total/100000000).toFixed(1) + '亿元',
        subtext: '',
        x:'center'
    },
    tooltip : {
        trigger: 'item',
        formatter:function(obj){
          return obj.seriesName + "<br>" + obj.name + '：' + (obj.value/100000000).toFixed(2) + "亿元" + "("+obj.percent+"%)";
        }
    },
    legend: {
        orient: 'horizontal',
        top:'50px',
        data: departmentArr
    },
    series : [
        {
          name: '资金占用',
          type: 'pie',
          radius : '55%',
          center: ['50%', '60%'],
          data:rateArr,
          itemStyle: {
            emphasis: {
              shadowBlur: 10,
              shadowOffsetX: 0,
              shadowColor: 'rgba(0, 0, 0, 0.5)'
            },
            normal:{
              label:{
                show:true,
                formatter: function(obj){
                  return obj.name + (obj.value/100000000).toFixed(2);
                },
                labelLine :{show:true}
              }
            }
          }
        }
      ]
    };
    $scope.drawImage = function(){
      var myChart = echarts.init(dom);
      myChart.setOption(option, true);
      $scope.$broadcast('scroll.refreshComplete');
    };
    $scope.drawImage();
  });
}])
// 累计收现率情况表
.controller("CashrateCtrl",["$scope","$http",function($scope,$http){
  $scope.title = '收现率情况表';
  $scope.info = [];
  $http({url:"services/FundManagementHandler.ashx",params:{key:"Cashrate",date:$scope.curDate},method:"get"}).success(function(data){
      $scope.info = data;
  });
}])
// 各单位营业收入详情
.controller("BusinessincomeDetailCtrl",["$scope","$http","$stateParams",function($scope,$http,$stateParams){
  var params = $stateParams.id.split('|');
  $scope.title = '['+params[1]+']营业收入详情';
  $http({url:"services/FundManagementHandler.ashx",params:{key:"BusinessincomeDetail",date:$scope.curDate,id:params[0]},method:"get"}).success(function(data){
      $scope.frontList = data.FrontList;
      $scope.backList = data.BackList;
  });
}])
// 收现率预警
.controller("CashrateDetailCtrl",["$scope","$http","$stateParams",function($scope,$http,$stateParams){
  var params = $stateParams.id.split('|');
  $scope.title = '['+params[1]+']收现率预警';
  $http({url:"services/FundManagementHandler.ashx",params:{key:"CashrateDetail",date:$scope.curDate,id:params[0]},method:"get"}).success(function(data){
      $scope.list = data;
  });
}])
// 各单位收款详情
.controller("ReceivablesDetailCtrl",["$scope","$http","$stateParams",function($scope,$http,$stateParams){
  var params = $stateParams.id.split('|');
  $scope.title = '['+params[1]+']年收款详情';
  $http({url:"services/FundManagementHandler.ashx",params:{key:"ReceivablesDetail",date:$scope.curDate,id:params[0]},method:"get"}).success(function(data){
      $scope.list = data;
  });
}])
// 各单位资金占用预警
.controller("OccupyDetailCtrl",["$scope","$http","$stateParams",function($scope,$http,$stateParams){
  var params = $stateParams.id.split('|');
  $scope.title = '['+params[1]+']资金占用详情';
  $http({url:"services/FundManagementHandler.ashx",params:{key:"OccupyDetail",date:$scope.curDate,id:params[0]},method:"get"}).success(function(data){
      $scope.list = data;
  });
}])
// 各单位拖欠款预警
.controller("ArrearsDetailCtrl",["$scope","$http","$stateParams",function($scope,$http,$stateParams){
  var params = $stateParams.id.split('|');
  $scope.title = '['+params[1]+']拖欠款情况';
  $http({url:"services/FundManagementHandler.ashx",params:{key:"ArrearsDetail",date:$scope.curDate,id:params[0]},method:"get"}).success(function(data){
      $scope.list = data;
  });
}])
// 保证金情况表
.controller("MarginCtrl",["$scope","$http",function($scope,$http){
  $scope.title = '保证金情况表';
  $http({url:"services/FundManagementHandler.ashx",params:{key:"Margin",date:$scope.curDate},method:"get"}).success(function(data){
      $scope.list = data;
  });
}])




