angular.module("myApp",[])
.controller("FactoringDataMasterCtrl",["$scope","$http",function($scope,$http){
	var now = new Date();
	$scope.params = {
		key:"GetMasterList",
		startTime:now.getFullYear() + "-" + (now.getMonth()+1) + "-" + now.getDay(),
		endTime:new Date(),
		pageIndex:1,
		pageSize:15	
	};
	var path = 'Main.ashx';
	$http({method:"get",params:$scope.params,url:path})
	.success(function(data){
		$scope.masterList = data;
	});
}])




