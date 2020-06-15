app.controller('complexDetailsController', ['$scope', 'authService', '$rootScope', '$window', '$location', 'DATA_URLS', '$http', '$routeParams', 'appConstant', '$filter', 'societyService',
function ($scope, authService, $rootScope, $window, $location, DATA_URLS, $http, $routeParams, appConstant, $filter, societyService) {

    'use strict';
    $scope.editFormData = {};
    $scope.facilityRecords = [];
    $scope.userRecords = [];
    $scope.societyId = null
    $scope.FacilityTypes = appConstant.FacilityType;

    getCityList();
    getStateList();
    if ($routeParams.sid != undefined) {

        get($routeParams.sid);
        loadUsers($routeParams.sid);
    }

    $scope.todayDate = new Date();

    function get(id) {

        var promise = $http.post(DATA_URLS.GET_COMPLEX, id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {

                $scope.editFormData = data;

                angular.forEach($scope.editFormData.facilities, function (v) {
                    v.facilityType = $filter('filter')($scope.FacilityTypes, { id: v.type }, true)[0];

                });
                $scope.editFormData.dateOfIncorporation = new Date($filter('date')($scope.editFormData.dateOfIncorporation, appConstant.ConversionDateFormat));
                $scope.editFormData.dateOfRegistration = new Date($filter('date')($scope.editFormData.dateOfRegistration, appConstant.ConversionDateFormat));
            }

        });
    }

    function loadUsers(id) {

        var promise = $http.post(DATA_URLS.LIST_USERS_COMPLEX, id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.userRecords = data;
            }
        });

    }

    var onFacilityCreate = $rootScope.$on('onFacilityCreate', function (e, d) {

        toastr["success"]("Facility created successfully", "Success");
        $('#cancelNewFacilityBox').click();
        loadFacilities($routeParams.sid);
    });

    $scope.$on('$destroy', function () {
        onFacilityCreate();
    });

    $scope.submit = function () {

        var fdata = angular.copy($scope.editFormData);
        var promise = $http.post(DATA_URLS.UPDATE_COMPLEX, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Complex updated successfully", "Success");
                $('#canceleditComplexBox').click();
            }
        });
    }

    $scope.updateLoginDetails = function () {

        var fdata = angular.copy($scope.editFormData);

        var promise = $http.post(DATA_URLS.UPDATELOGINDETAILS_COMPLEX, fdata);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Login details updated successfully", "Success");
                $('#cancelLoginBox').click();
            }
        });
    }

    $scope.edit = function () {
        $scope.editFormData.city = $filter('filter')($scope.cityList, { id: $scope.editFormData.city.id })[0];
        $scope.editFormData.state = $filter('filter')($scope.stateList, { id: $scope.editFormData.state.id })[0];
        $scope.editFormData.isEdit = true;
    }

    function getCityList() {
        var promise = $http.post(DATA_URLS.GET_MASTERVALUELISTALL, 5);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.cityList = data;
            }
        });
    };

    function getStateList() {
        var promise = $http.post(DATA_URLS.GET_MASTERVALUELISTALL, 6);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.stateList = data;
            }
        });
    };

    $scope.navToSociety = function (society) {

        societyService.setSociety(society);
        $location.path('society/details/' + society.id);
    }
}]);