app.controller('assetListController', ['$scope', 'authService', 'appConstant', '$rootScope', '$window','$location', 'DATA_URLS', 
'$http','$routeParams','$state', 'localStorageService',
function ($scope, authService, appConstant, $rootScope, $window,$location, DATA_URLS, $http, $routeParams, $state,localStorageService) {

    'use strict';
    $scope.recordList = [];
    $scope.societyId = null;

    if ($routeParams.sid != undefined) {
        $scope.societyId = $routeParams.sid;
        getList();
    }

    function getList() {
        var fdata = { societyId: $scope.societyId };
        var promise = $http.post(DATA_URLS.LIST_SOCIETYASSETS, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.recordList = data;
            }
        });
    }
}]);
                
        
        
                
    
