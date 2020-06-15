app.controller('facilityDetailsController', ['$scope', 'authService', 'appConstant', '$rootScope', '$window','$location', 'DATA_URLS', 
'$http','$routeParams','$filter','$state', 'localStorageService',
function ($scope, authService, appConstant, $rootScope, $window,$location, DATA_URLS, $http, $routeParams,$filter, $state,localStorageService) {

    'use strict';
    $scope.society =  JSON.parse(localStorageService.get('onLogSoc'));
    $scope.editFormData = {};
    $scope.facilityId = null;
    $scope.parkingTypes = appConstant.ParkingType;
    $scope.FloorTypes = appConstant.FloorType;
    $scope.FacilityType = appConstant.FacilityType;
    $scope.isFloorVisible = false;

    if ($routeParams.sid != undefined) {
        $scope.facilityId = $routeParams.sid;

        get($routeParams.sid);
    }

        function get(id) {

            var promise = $http.post(DATA_URLS.GET_FACILITY, id);
    
            promise.then(function (data) {
                if (!data.status || data.status == 200) {
                    $scope.editFormData = data;
    
                    if ($scope.editFormData.type == 1) {
                        localStorageService.set('onLogSoc', JSON.stringify($scope.editFormData.societies[0].society));
                    }
    
                    angular.forEach($scope.editFormData.floors, function (v) {
                        v.floorType = $filter('filter')($scope.FloorTypes, { id: v.type }, true)[0];
    
                    });
                    angular.forEach($scope.editFormData.flatParkings, function (v) {
                        if (v.type) {
                            v.type = $filter('filter')($scope.parkingTypes, { id: v.type }, true)[0];
                        }
                    });
                    $scope.facilityType = $filter('filter')($scope.FacilityType, { id: data.type }, true)[0];
                    $scope.isFloorVisible = isFloor($scope.editFormData.type);
                }
            });
        }
        function isFloor(type) {
            return type == 1 || type == 4 || type == 6 || type == 7 || type == 9 || type == 12 || type == 13 || type == 14 || type == 15;
        }
   
}]);
        
        
                
    
