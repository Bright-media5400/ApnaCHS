app.controller('complexListController', ['$scope', 'authService', '$rootScope', '$window', '$location', 'DATA_URLS'
    , '$http', '$filter', 'appConstant',
function ($scope, authService, $rootScope, $window, $location, DATA_URLS, $http, $filter, appConstant) {

    'use strict';
    $scope.recordList = [];
    $scope.facilityTypes = appConstant.FacilityType;
    $scope.searchFormData = {};

    getList();
    getCityList();
    getStateList();

    function getList() {

        var filters = setfilters();
        var promise = $http.post(DATA_URLS.LIST_COMPLEX, filters);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.recordList = data;

                if ($scope.recordList.length == 0) {
                    toastr["info"]("No result found", "info");
                }

                angular.forEach($scope.recordList, function (v) {
                    angular.forEach(v.facilities, function (v1) {
                        v1.facilityType = $filter('filter')($scope.facilityTypes, { id: v1.type }, true)[0];
                    });
                });
            }
        });
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

    $scope.new = function (item) {
        $location.path('complex/new');
    }

    $scope.edit = function (item) {
        $location.path('complex/details/' + item.id);
    }

    $scope.onDelete = function (item, index) {
        $scope.selectedItem = item;
        $scope.selectedItem.index = index;
    }

    $scope.delete = function (item, index) {
        var promise = $http.post(DATA_URLS.DELETE_COMPLEX, $scope.selectedItem.id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Complex deleted successfully", "Success");
                $scope.recordList.splice($scope.selectedItem.index, 1);
                $('#cancelConfirmDelete').click();
            }
        });

    }

    $scope.exportToExcel = function () {

        if ($scope.recordList.length == 0) {
            toastr.success('No result found to export.', 'Success', {});
            return;
        }

        toastr.success('Export started please wait.', 'Success', {});

        //$("#exportReport").table2xlsx({
        //    filename: "Society List" + ".xlsx",
        //    sheetName: "SocietyList"
        //});

        var toexport = [];
        angular.forEach($scope.recordList, function (item) {
            item.dateOfIncorporation = $filter('date')(item.dateOfIncorporation, appConstant.ExcelFormat);
            item.dateOfRegistration = $filter('date')(item.dateOfRegistration, appConstant.ExcelFormat);
            toexport.push({
                "ID": item.id,
                "Complex": item.name + '\r\n' + item.address + '\r\n' + item.registrationNo,
                "City": item.area + ',' + item.city.text,
                "State": item.state.text,
                "Societies": item.noOfSocieties + '#\r\n' + getSocieties(item),
                "Buildings": item.noOfBuilding + '#\r\n' + getBuildings(item),
                "Amenities": item.noOfAmenities + '#\r\n' + getAmenities(item),
            });
        });

        data2xlsx({
            filename: "Society List" + ".xlsx",
            sheetName: "SocietyList",
            data: toexport
        });
    }

    function getSocieties(item) {
        var res = '';

        angular.forEach(item.societies, function (v) {
            res = res + v.name + '\r\n';
        });

        return res;
    }

    function getBuildings(item) {
        var res = '';

        angular.forEach(item.facilities, function (v) {
            if (v.type == 1)
                res = res + v.name + '\r\n';
        });

        return res;
    }

    function getAmenities(item) {
        var res = '';

        angular.forEach(item.facilities, function (v) {
            if (v.type != 1)
                res = res + v.name + '\r\n';
        });

        return res;
    }

    $scope.search = function () {
        getList();
    }

    function setfilters() {
        var searchFilter = {};

        if ($scope.searchFormData.city)
            searchFilter.city = $scope.searchFormData.city.id;

        if ($scope.searchFormData.state)
            searchFilter.state = $scope.searchFormData.state.id;

        if ($scope.searchFormData.amenitytype)
            searchFilter.amenitytype = $scope.searchFormData.amenitytype.id;

        searchFilter.complexname = $scope.searchFormData.complexname;
        searchFilter.societyname = $scope.searchFormData.societyname;

        return searchFilter;
    }

    $scope.clearSearch = function () {
        $scope.searchFormData = {};
        $scope.search();
    }

}]);
