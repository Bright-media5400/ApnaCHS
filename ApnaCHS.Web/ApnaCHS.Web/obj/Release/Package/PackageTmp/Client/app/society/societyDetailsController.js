app.controller('societyDetailsController', ['$scope', 'authService', '$rootScope', '$window', '$location', 'DATA_URLS', '$http', '$routeParams', 'appConstant', '$filter', 'localStorageService',
function ($scope, authService, $rootScope, $window, $location, DATA_URLS, $http, $routeParams, appConstant, $filter, localStorageService) {

    'use strict';
    $scope.editFormData = {};
    $scope.facilityRecords = [];
    $scope.societyId = null;
    $scope.totalFlats = null;
    $scope.FacilityTypes = appConstant.FacilityType;

    if ($routeParams.sid != undefined) {
        $scope.societyId = $routeParams.sid;

        get($routeParams.sid);
        loadFacilities($routeParams.sid);
        getList()
    }

    $scope.todayDate = new Date();

    $scope.submit = function () {

        var fdata = angular.copy($scope.editFormData);
        var promise = $http.post(DATA_URLS.UPDATE_SOCIETY, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Society updated successfully", "Success");
                $('#canceleditSocietyBox').click();
            }
        });
    }

    $scope.submitSocietySetting = function () {

        var fdata = angular.copy($scope.editFormData);

        var promise = $http.post(DATA_URLS.UPDATESETTING_SOCIETY, fdata);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Society Setting updated successfully", "Success");
                $('#cancelsocietySettingBox').click();
            }
        });
    }

    $scope.updateLoginDetails = function () {

        var fdata = angular.copy($scope.editFormData);

        var promise = $http.post(DATA_URLS.UPDATELOGINDETAILS_SOCIETY, fdata);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Login details updated successfully", "Success");
                $('#cancelLoginBox').click();
            }
        });
    }

    $scope.generate = function () {

        var fdata = {
            id: $scope.editFormData.id,
            generation: $filter('date')(new Date(), appConstant.ConversionDateFormat)
        };
        var promise = $http.post(DATA_URLS.GENERATE_BILLING, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Bill generation process for this month completed successfully", "Success");
            }
        });
    }

    function get(id) {

        var promise = $http.post(DATA_URLS.GET_SOCIETY, id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.editFormData = data;

                angular.forEach($scope.editFormData.facilities, function (v) {
                    v.facilityType = $filter('filter')($scope.FacilityTypes, { id: v.type }, true)[0];

                });

                $scope.editFormData.dateOfIncorporation = new Date($filter('date')($scope.editFormData.dateOfIncorporation, appConstant.ConversionDateFormat));
                $scope.editFormData.dateOfRegistration = new Date($filter('date')($scope.editFormData.dateOfRegistration, appConstant.ConversionDateFormat));

                localStorageService.set('onLogSoc', JSON.stringify($scope.editFormData));
                getTotalFlats(id);
            }
        });
    }

    $scope.edit = function () {

        $scope.editFormData.isEdit = true;
    }

    function getTotalFlats(id) {

        var promise = $http.post(DATA_URLS.GET_SOCIETY_FLATCOUNT, id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.totalFlats = data;
            }
        });
    }

    var onFacilityCreate = $rootScope.$on('onFacilityCreate', function (e, d) {

        toastr["success"]("Facility created successfully", "Success");
        $('#cancelNewFacilityBox').click();
        loadFacilities($routeParams.sid);
    });

    function loadFacilities(id) {

        var promise = $http.post(DATA_URLS.SOCIETYWISELIST_FACILITIES, { societyId: id });

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.facilityRecords = data;
                $scope.count = $scope.facilityRecords.length;

                angular.forEach($scope.facilityRecords, function (v) {
                    v.facilityType = $filter('filter')($scope.FacilityTypes, { id: v.type }, true)[0];

                });
            }
        });
    }

    $scope.$on('$destroy', function () {
        onFacilityCreate();
    });

    $scope.onBackClick = function () {
        $window.history.back();
    }

    var onSocirtyAssetCreate = $rootScope.$on('onSocirtyAssetCreate', function (e, d) {
        toastr["success"]("Asset created successfully", "Success");
        $('#cancelNewAssetBox').click();
        getList()
    });

    $scope.$on('$destroy', function () {
        onSocirtyAssetCreate();
    });

    $scope.editAsset = function (item) {
        $scope.editSocietyAsset = angular.copy(item);
        $scope.editSocietyAsset.purchaseDate = new Date($filter('date')($scope.editSocietyAsset.purchaseDate, appConstant.ConversionDateFormat));
    }

    $scope.submitAsset = function () {

        var fdata = angular.copy($scope.editSocietyAsset);
        var promise = $http.post(DATA_URLS.UPDATE_SOCIETYASSET, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Asset updated successfully", "Success");
                $('#cancelAssetBox').click();
                getList();
            }
        });
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

    $scope.onDelete = function (item, index) {
        $scope.selectedItem = item;
        $scope.selectedItem.index = index;
    }

    $scope.delete = function (item, index) {
        var promise = $http.post(DATA_URLS.DELETE_SOCIETYASSET, $scope.selectedItem.id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Assets deleted successfully", "Success");
                $scope.recordList.splice($scope.selectedItem.index, 1);
                $('#cancelConfirmDelete').click();
            }
        });

    }

}]);
