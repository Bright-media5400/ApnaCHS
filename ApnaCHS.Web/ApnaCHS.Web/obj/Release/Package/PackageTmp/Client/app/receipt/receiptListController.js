app.controller('receiptListController', ['$scope', 'authService', '$rootScope', '$window', '$location', 'DATA_URLS'
    , '$http', '$filter', 'appConstant', '$routeParams','localStorageService',
function ($scope, authService, $rootScope, $window, $location, DATA_URLS, $http, $filter, appConstant, $routeParams, localStorageService) {

    'use strict';
    $scope.recordList = [];
    $scope.facilityRecords = [];
    $scope.floorRecords = [];
    $scope.months = appConstant.Months;
    $scope.searchFormData = {};

    if ($routeParams.sid != undefined) {
        getList();
        loadFacilities();
        loadFloors();
    }

    function loadFacilities() {

        var soc = JSON.parse(localStorageService.get('onLogSoc'));
        var promise = $http.post(DATA_URLS.SOCIETYWISELIST_FACILITIES, { societyId: soc.id });

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.facilityRecords = data;
                $scope.count = $scope.facilityRecords.length;
            }
        });
    }

    function loadFloors() {

        var soc = JSON.parse(localStorageService.get('onLogSoc'));
        var promise = $http.post(DATA_URLS.FLOOR_SOCIETYWISELIST, { societyId: soc.id });

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.floorRecords = data;
            }
        });
    }

    function getList(socId) {

        var filters = setfilters();

        var soc = JSON.parse(localStorageService.get('onLogSoc'));
        filters.societyId = soc.id;

        var promise = $http.post(DATA_URLS.LIST_BILLING, filters);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.recordList = data;

                if ($scope.recordList.length == 0) {
                    toastr["info"]("No result found", "Info");
                }
            }
        });
    }

    $scope.downloadFile = function (bill) {
        bill.isLoading = true;
        $http({
            method: 'GET',
            url: DATA_URLS.GET_BILLING_PDF,
            params: { id: bill.flat.id, month: bill.month, year: bill.year, socid: bill.society.id },
            responseType: 'arraybuffer'
        }).then(function (response) {

            var headers = response.headers();
            var filename = headers['x-filename'];
            var contentType = headers['content-type'];

            var myBlob = new Blob([response.data], { type: contentType })
            var blobURL = (window.URL || window.webkitURL).createObjectURL(myBlob);
            var anchor = document.createElement("a");
            //anchor.download = "myfile.txt";
            anchor.href = blobURL;
            anchor.target = "_blank";
            anchor.click();
            bill.isLoading = false;
        });
    }

    $scope.showMonth = function (month) {
        var monthNames = ["January", "February", "March", "April", "May", "June",
          "July", "August", "September", "October", "November", "December"
        ];

        return monthNames[month - 1];
    }

    $scope.onBackClick = function () {
        $window.history.back();
    }

    $scope.search = function () {
        getList();
    }

    function setfilters() {
        var searchFilter = {};

        searchFilter.year = $scope.searchFormData.year;

        if ($scope.searchFormData.month)
            searchFilter.month = $scope.searchFormData.month.id;

        if ($scope.searchFormData.facility)
            searchFilter.facility = $scope.searchFormData.facility.id;

        if ($scope.searchFormData.floor)
            searchFilter.floor = $scope.searchFormData.floor.id;

        return searchFilter;
    }
    
    $scope.clearSearch = function () {
        $scope.searchFormData = {};
        $scope.search();
    }
}]);
