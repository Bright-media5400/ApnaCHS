app.controller('flatDetails', ['$scope', 'authService', 'appConstant', '$rootScope', '$window', '$location', 'DATA_URLS',
'$http', '$routeParams', '$state', 'localStorageService',
function ($scope, authService, appConstant, $rootScope, $window, $location, DATA_URLS, $http, $routeParams, $state, localStorageService) {

    'use strict';
    $scope.editFormData = {};
    $scope.flatId = null;
    $scope.flatOwnerId = null;
    $scope.currentFlatOwner = null;
    $scope.vehicleList = {};
    $scope.recordList = {};

    if ($routeParams.fid != undefined) {
        $scope.flatId = $routeParams.fid;
        get($routeParams.fid);
    }

    function get(id) {

        var promise = $http.post(DATA_URLS.GET_FLAT, id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.editFormData = data;

                $scope.editFormData.flatOwners.sort(sort_by({ name: 'memberSinceDate', reverse: true }));
                var owners = $scope.editFormData.flatOwners.filter(function (e) {
                    return e.flatOwnerType == 1;
                });
                $scope.currentFlatOwner = owners[0];
                //$scope.currentFlatOwner = $scope.editFormData.flatOwners[0];

                //$scope.editFormData.tenants.sort(sort_by({ name: 'memberSinceDate', reverse: true }));
                //$scope.currentTenant = $scope.editFormData.tenants[0];

                $scope.editFormData.tenants
                $scope.tenantHistory = $scope.editFormData.tenants;

                $scope.editFormData.flatOwners
                $scope.flatOwnerHistory = $scope.editFormData.flatOwners;

                $scope.editFormData.flatParkings
                $scope.flatParkingsList = $scope.editFormData.flatParkings;


            }
        });
    }

    
    

    $scope.$watch(function ($scope) { return $scope.editFormData },
                              function (newValue, oldValue) {
                                  if (!!newValue && newValue != oldValue) {
                                      getVehicleList();
                                      getFamilyList();
                                  }
                              });

                              function getFamilyList() {
                                var soc = JSON.parse(localStorageService.get('onLogSoc'));
                                var fdata = { flatOwnerId: $scope.currentFlatOwner.flatOwner.id, societyId: soc.id };
                                var promise = $http.post(DATA_URLS.LIST_FLATOWNERFAMILIES, fdata);
                        
                                promise.then(function (data) {
                                    if (!data.status || data.status == 200) {
                                        $scope.recordList = data;
                                    }
                                });
                            }                          

    function getVehicleList() {
        var soc = JSON.parse(localStorageService.get('onLogSoc'));
        var fdata = { flatId: $scope.flatId, flatOwnerId: $scope.currentFlatOwner.flatOwner.id, societyId: soc.id };
        var promise = $http.post(DATA_URLS.LIST_VEHICLES, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.vehicleList = data;
            }
        });
    }


}]);





