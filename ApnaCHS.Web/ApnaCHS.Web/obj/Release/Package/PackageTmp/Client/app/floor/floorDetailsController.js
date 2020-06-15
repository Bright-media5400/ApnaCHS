app.controller('floorDetailsController', [
    '$scope', '$rootScope', '$window', '$location', '$http', '$routeParams', '$anchorScroll', '$filter', 'DATA_URLS', 'appConstant', 'localStorageService',
function ($scope, $rootScope, $window, $location, $http, $routeParams, $anchorScroll, $filter, DATA_URLS, appConstant, localStorageService) {

    'use strict';
    $scope.editFormData = {};
    $scope.editParkingData = {};
    $scope.flatRecords = [];
    $scope.flatParkingRecords = [];
    $scope.floorId = null;
    $scope.facilityId = null;
    $scope.parkingTypes = appConstant.ParkingType;
    $scope.FloorTypes = appConstant.FloorType;

    if ($routeParams.fid != undefined) {
        $scope.floorId = $routeParams.fid;
        get($routeParams.fid);
    }

    $scope.submit = function () {

        var fdata = angular.copy($scope.formData);
        fdata.type = fdata.type.id;
        fdata.facilityId = fdata.facility.id;

        var promise = $http.post(DATA_URLS.UPDATE_FLOOR, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Floor updated successfully", "Success");
                $scope.cancelEdit();
                $scope.formData = null;
                get($routeParams.fid);

            }
        });
    }

    $scope.edit = function () {
        $scope.formData = angular.copy($scope.editFormData);
        $scope.formData.type = $filter('filter')($scope.FloorTypes, { id: $scope.formData.type })[0];
        $scope.formData.isEdit = true;
    }

    $scope.cancelEdit = function () {
        $scope.formData = null;
    }

    $scope.submitParking = function () {

        var fdata = angular.copy($scope.selectedParking);
        fdata.type = fdata.type.id;
        var promise = $http.post(DATA_URLS.UPDATE_FLATPARKING, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Parking updated successfully", "Success");
                $('#cancelEditFlatParkingBox').click();
                $scope.editFormData.flatParkings[$scope.selectedParking.index].type = $scope.selectedParking.type;
                $scope.editFormData.flatParkings[$scope.selectedParking.index].name = $scope.selectedParking.name;
                $scope.selectedParking = null;
            }
        });
    }


    $scope.onSelectParking = function (flatParking, index) {
        $scope.selectedParking = angular.copy(flatParking);
        if ($scope.selectedParking.type) {
            $scope.selectedParking.type = $filter('filter')($scope.parkingTypes, { id: $scope.selectedParking.type.id })[0];
        }
        $scope.selectedParking.isEdit = true;
        $scope.selectedParking.index = index;
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

    var onFlatCreate = $rootScope.$on('onFlatCreate', function (e, d) {
        toastr["success"]("Flat created successfully", "Success");
        $('#cancelNewFlatBox').click();
        get($scope.editFormData.id);
    });

    var onCommercialSpaceCreate = $rootScope.$on('onCommercialSpaceCreate', function (e, d) {
        toastr["success"]("Commercial Space created successfully", "Success");
        $('#cancelNewCommercialSpaceBox').click();
        get($scope.editFormData.id);
    });

    var onFlatParkingCreate = $rootScope.$on('onFlatParkingCreate', function (e, d) {
        get($scope.editFormData.id);
    });

    $scope.onBackClick = function () {
        //$window.history.back();
        $location.path('facility/details/' + $scope.editFormData.facility.id);
    }

    $scope.$on('$destroy', function () {
        onFlatCreate();
        onCommercialSpaceCreate();
        onFlatParkingCreate();
    });

    $scope.onSocietyDashboard = function () {
        var soc = JSON.parse(localStorageService.get('onLogSoc'));
        $location.path('/society/details/' + soc.id);
    }

}]);