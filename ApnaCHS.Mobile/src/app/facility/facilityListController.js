app.controller('facilityListController', ['$scope', 'authService', 'appConstant', '$rootScope', '$window','$location', 'DATA_URLS', 
'$http','$routeParams','$state', 'localStorageService',
function ($scope, authService, appConstant, $rootScope, $window,$location, DATA_URLS, $http, $routeParams, $state,localStorageService) {

    'use strict';
    $scope.society =  JSON.parse(localStorageService.get('onLogSoc'));
    $scope.facilityRecords = [];
    $scope.societyId = null;

    if ($routeParams.sid != undefined) {
        $scope.societyId = $routeParams.sid;

        loadFacilities($routeParams.sid);
    }

     function loadFacilities(id) {
        var fdata = { societyId: $scope.societyId };
        var promise = $http.post(DATA_URLS.SOCIETYWISELIST_FACILITIES,fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.facilityRecords = data;
            }
        });
    }
}]);
                
        
        
                
    
