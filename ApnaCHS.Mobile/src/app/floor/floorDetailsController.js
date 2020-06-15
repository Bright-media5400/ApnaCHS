app.controller('floorDetailsController', ['$scope', 'authService', 'appConstant', '$rootScope', '$window','$location', 'DATA_URLS', 
'$http','$routeParams','$filter','$state', 'localStorageService',
function ($scope, authService, appConstant, $rootScope, $window,$location, DATA_URLS, $http, $routeParams,$filter, $state,localStorageService) {

    'use strict';
    $scope.society =  JSON.parse(localStorageService.get('onLogSoc'));
    $scope.editFormData = {};
    $scope.floorId = null;   
    $scope.parkingTypes = appConstant.ParkingType;
    $scope.FloorTypes = appConstant.FloorType;

    if ($routeParams.sid != undefined) {
        $scope.floorId = $routeParams.sid;
        get($routeParams.sid);
    }

    function get(id) {

        var promise = $http.post(DATA_URLS.GET_FLOOR, id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.editFormData = data;

                $scope.floorType = $filter('filter')($scope.FloorTypes, { id: data.type }, true)[0];
                $scope.facilityId = $scope.editFormData.facility.id;

                angular.forEach($scope.editFormData.flatParkings, function (v) {
                    if (v.type) {
                        v.type = $filter('filter')($scope.parkingTypes, { id: v.type }, true)[0];
                    }
                });
            }
        });
    }
   
}]);
        
        
                
    
