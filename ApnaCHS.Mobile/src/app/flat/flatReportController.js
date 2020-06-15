app.controller('flatReportController', ['$scope', 'authService', '$rootScope', '$window', '$location', 'DATA_URLS', '$http', '$routeParams', 'appConstant', '$filter',
function ($scope, authService, $rootScope, $window, $location, DATA_URLS, $http, $routeParams, appConstant, $filter) {

    'use strict';
    $scope.flatList = [];
    $scope.societyId = null;
  
    if ($routeParams.sid != undefined) {
        $scope.societyId = $routeParams.sid;
        search();
    }
       
    function search() {
     var fdata = { societyId: $scope.societyId };
     var promise = $http.post(DATA_URLS.REPORT_FLATS,fdata);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.flatList = data;
            }
        });
    }

    $scope.onBackClick = function () {
        $window.history.back();
    }
}]);