app.controller('approveMaintenancecostsController', [
    '$scope', '$rootScope', '$window', '$location', '$http', '$routeParams', '$anchorScroll', '$filter', 'DATA_URLS', 'appConstant',
    'localStorageService', '$sce', 'authService',
function ($scope, $rootScope, $window, $location, $http, $routeParams, $anchorScroll, $filter, DATA_URLS, appConstant,
    localStorageService, $sce, authService) {

    'use strict';
    $scope.recordList = [];
    $scope.selectedList = [];
    $scope.facilityTypes = appConstant.FacilityType;
    $scope.searchFormData = {};
    $scope.searchFormData.isApproved = "";
    $scope.commentTableName = "MC";

    getList();

    function getList() {

        var filters = setfilters();
        var promise = $http.post(DATA_URLS.LIST_MAINTENANCECOSTS, filters);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.recordList = data;

                if ($scope.recordList.length == 0) {
                    toastr["info"]("No result found", "info");
                }

                angular.forEach($scope.recordList, function (v) {

                    v.userApproved = !!($filter('filter')(v.approvals, { approvedBy: authService.authentication.loginData.userName }, true)[0]);

                    angular.forEach(v.facilities, function (v1) {
                        v1.facilityType = $filter('filter')($scope.facilityTypes, { id: v1.type }, true)[0];                        
                    });
                });
            }
        });
    }

    $scope.onApproveSelected = function () {
    }

    $scope.onBackClick = function () {
        $window.history.back();
    }

    $scope.onAllSelectedToggle = function () {
        $scope.allSelected = !$scope.allSelected;
        $scope.onToggleSelected();
    }

    $scope.onToggleSelected = function () {
        angular.forEach($scope.recordList, function (v) {
            if (!v.isApproved)
                v.selected = $scope.allSelected;
        });
        selectMc();
    }

    $scope.selectedItem = null;
    $scope.onCommentClick = function (mc) {
        $scope.selectedItem = angular.copy(mc);
    }

    function selectMc() {

        var sel = [];
        angular.forEach($scope.recordList, function (v) {
            if (v.selected) {
                sel.push(v.id);
            }
        });
        $scope.selectedList = sel;
        //------------------------------------------------------------------//
    }

    $scope.onApprove = function (item, index) {
        $scope.selectedItem = angular.copy(item);
        $scope.selectedItem.index = index;
    }

    $scope.approve = function () {
        var fdata = { id: $scope.selectedItem.id, note: $scope.note };
        var promise = $http.post(DATA_URLS.APPROVE_MAINTENANCECOST, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.recordList[$scope.selectedItem.index].userApproved = true;
                $scope.recordList[$scope.selectedItem.index].isApproved = data.isApproved;
                $scope.recordList[$scope.selectedItem.index].approvals.push({ approvedName: authService.authentication.loginData.userFullName });
                toastr["success"]("Maintenance cost approved by you", "Success");
                $('#cancelConfirmDelete').click();
                clearNote();
            }
        });

    }

    $scope.onReject = function (item, index) {
        $scope.selectedItem = item;
        $scope.selectedItem.index = index;
    }

    $scope.reject = function (item, index) {
        var fdata = { id: $scope.selectedItem.id, note: $scope.note };
        var promise = $http.post(DATA_URLS.REJECT_MAINTENANCECOST, fdata);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.recordList[$scope.selectedItem.index].isRejected = true;
                toastr["success"]("Maintenance cost rejected", "Success");
                $('#cancelConfirmReject').click();
                clearNote();
            }
            else {
                $scope.recordList[index].isRejected = false;
            }
        });
    }

    $scope.approveAll = function () {
        var fdata = { ids: $scope.selectedList, note: $scope.note };
        var promise = $http.post(DATA_URLS.BULKAPPROVE_MAINTENANCECOST, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Bulk approval succesful", "Success");
                $('#cancelApproveSelected').click();
                clearNote();
                angular.forEach(data, function (v) {
                    angular.forEach($scope.recordList, function (v1) {
                        if (v.id == v1.id) {
                            v1.message = v.message;
                            v1.isSucces = v.isSucces;
                            v1.approvals.push({ approvedName: authService.authentication.loginData.userFullName });
                            v1.isApproved = true;
                        }
                    });
                });
            }
        });
    }

    $scope.search = function () {
        getList();
    }

    function setfilters() {
        var searchFilter = {};

        var soc = JSON.parse(localStorageService.get('onLogSoc'));
        searchFilter.societyId = soc.id;

        searchFilter.isApproved = $scope.searchFormData.isApproved;
        searchFilter.isRejected = $scope.searchFormData.isRejected;

        return searchFilter;
    }

    $scope.clearSearch = function () {
        $scope.searchFormData = {};
        $scope.searchFormData.isApproved = "";

        $scope.search();
    }

    $scope.viewMC = function (item) {
        // $location.path('/society/details/' + item.society.id);
        $location.path('/maintenancecost/list');
    }

    $scope.assignFlats = function (item, index) {
        $scope.selectedMCLine = item;
        loadFacilities($scope.selectedMCLine.society.id);
    }

    function loadFacilities(id) {

        var promise = $http.post(DATA_URLS.LOADFLATS_FACILITIES, { societyId: id });

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.facilityRecords = data;

                angular.forEach($scope.facilityRecords, function (v1) {
                    angular.forEach(v1.floors, function (v2) {
                        angular.forEach(v2.flats, function (v3) {

                            var flat = $filter('filter')($scope.selectedMCLine.flats, { id: v3.id }, true)[0];
                            if (flat) {
                                v3.selected = true;
                            }
                        });
                    });
                });
            }
        });
    }

    $scope.onSelectClick = function (item) {
        item.selected = !item.selected;
        selectMc();
    }

    function clearNote() {
        $scope.note = null;
    };

}]);