angular.module('starter.services', [])
.directive('hideTabs', function($rootScope) {
    return {
        restrict: 'A',
        link: function(scope, element, attributes) {

            scope.$on('$ionicView.beforeEnter', function() {

                scope.$watch(attributes.hideTabs, function(value){
                    $rootScope.hideTabs = 'tabs-item-hide';
                });

            });

            scope.$on('$ionicView.beforeLeave', function() {
                scope.$watch(attributes.hideTabs, function(value){
                    $rootScope.hideTabs = 'tabs-item-hide';
                });
                scope.$watch('$destroy',function(){
                    $rootScope.hideTabs = false;
                })
            });
        }
    };
})
.filter("NumConvert",function(){                        // 转化为亿元
  return function(input){
    var temp = (input/100000000 + 1).toFixed(2);
    temp = new Number(temp - 1).toFixed(2);
    return temp == 0.00 ? '-' : temp;
  };
})
.filter("FundConversion",function(){                    // 转化为亿元
  return function(input,value,unit){
    if(input.Title && input.Title.indexOf("保证金")>-1){
      var temp = (input[value]/10000 + 1).toFixed(2)
      return  new Number(temp - 1).toFixed(2);
    }
    if(input[unit] == "元"){
      var temp = (input[value]/100000000 + 1).toFixed(2)
      return new Number(temp - 1).toFixed(2);
    }
    return input[value];
  };
})
.filter("UnitConversion",function(){                    // 转化为亿元
  return function(input){
    if(input.Title && input.Title.indexOf('保证金') > -1)return '万元';
    if(input.MeasurementUnitName == "元")return "亿元";
    if(input.MeasurementUnitName == "百分比")return "%";
    return input.MeasurementUnitName;
  };
})
.filter("IntegerConversion",function(){                    
  return function(input){
    if(typeof input == "number")return input.toFixed(0);
    return input;
  };
})
.filter("NumberConversion",function(){                // 为零的数值采用-表示
  return function(input,unit){
    return input == 0 ? '-' : (input + unit);
  }
})
.filter('MillionConversion',function(){
  return function(input){
    var temp = (input/10000 + 1).toFixed(2)
    return new Number(temp - 1).toFixed(2);
  }
})
.filter('WarningConversion',function(WargingDescripe){
  return function(input){
    return WargingDescripe[input];
  }
})
.filter("millionConvert",function(){                        // 转化为亿元
  return function(input){
    var temp = (input/10000 + 1).toFixed(2);
    temp = new Number(temp - 1).toFixed(2);
    return temp == 0.00 ? '-' : temp;
  };
})
.constant('WargingDescripe',['正常','黄色','橙色','红色'])