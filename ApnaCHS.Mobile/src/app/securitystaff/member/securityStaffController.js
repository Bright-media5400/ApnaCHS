app.controller('securityStaffListController', ['$scope', 'authService', '$rootScope', '$window', '$location', 'DATA_URLS', '$http', '$routeParams', 'appConstant', '$filter','localStorageService',
function ($scope, authService, $rootScope, $window, $location, DATA_URLS, $http, $routeParams, appConstant, $filter,localStorageService) {

    'use strict';
    $scope.society =  JSON.parse(localStorageService.get('onLogSoc'));
    $scope.recordList = [];
    $scope.societyId = null;
    getList();
    
    if ($routeParams.sid != undefined) {
        $scope.societyId = $routeParams.sid;
    }
 
    function getList() {
        var fdata = { societyId: $scope.societyId };
        var promise = $http.post(DATA_URLS.LIST_SECURITYSTAFF, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.recordList = data;
            }
        });
    }
}]);