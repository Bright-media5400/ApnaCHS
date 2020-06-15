app.controller('flatListController', [
    '$scope', '$rootScope', '$window', '$location', '$http', '$routeParams', '$anchorScroll', '$filter', 'DATA_URLS', 'appConstant',
function ($scope, $rootScope, $window, $location, $http, $routeParams, $anchorScroll, $filter, DATA_URLS, appConstant) {

    'use strict';
    $scope.flatList = [];
    $scope.searchFormData = {};
    if ($routeParams.sid != undefined) {
        $scope.searchFormData.societyId = { id: $routeParams.sid };
    }


    $scope.clearSearchBoxes = function () {
        $scope.searchFormData = {};
        if ($routeParams.sid != undefined) {
            $scope.searchFormData.societyId = { id: $routeParams.sid };
        }
        clearSearchFilters();

        $scope.flatList = [];
    }

    function clearSearchFilters() {
        $scope.searchFilters = {};
    }

    function setFilters() {
        clearSearchFilters();
        $scope.searchFilters.societyId = $scope.searchFormData.societyId;
    }

    $scope.clearSearch = function () {
        clearSearchFilters();
        $scope.search();
    }

    $scope.search = function () {

        setFilters();
        var filters = angular.copy($scope.searchFilters);

        if (filters.societyId)
            filters.societyId = filters.societyId.id;

        $scope.flatList = [];

        var promise = $http.post(DATA_URLS.REPORT_FLATS, filters);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.flatList = data;
            }
        });
    }

    $scope.onBackClick = function () {
        $window.history.back();
    }

    $scope.exportToExcel = function () {

        if ($scope.flatList.length == 0) {
            toastr.info('No result found to export.', 'Info', {});
            return;
        }

        toastr.success('Export started please wait.', 'Success', {});

        //$("#exportReport").table2xlsx({
        //    filename: "Society List" + ".xlsx",
        //    sheetName: "SocietyList"
        //});

        var toexport = [];
        angular.forEach($scope.flatList, function (item) {

            toexport.push({
                "Building": item.building,
                "Floor": item.floor,
                "Flat": item.flat,
                "Current Owner": item.currentOwner,
                "Current Tenant": item.currentTenant
            });
        });

        data2xlsx({
            filename: "Flat List" + ".xlsx",
            sheetName: "FlatList",
            data: toexport
        });
    }
}]);