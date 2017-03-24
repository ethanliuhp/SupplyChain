angular.module("myApp",[])
.service('MasterList', ["$http","$q",function($http,$q){
	this.data = {
		key:"GetMasterList",
		startTime:new Date(),
		endTime:new Date(),
		pageIndex:1,
		pageSize:15
	};
	this.getData = function (id) {
		var deferred = $q.defer();
		var path = 'Main.ashx';

		return $http(path).then(function (d) {
			this.data = d;
			return $q.when(d);
		}, function (d) {
			return $q.reject(d);
		});
	}
}])