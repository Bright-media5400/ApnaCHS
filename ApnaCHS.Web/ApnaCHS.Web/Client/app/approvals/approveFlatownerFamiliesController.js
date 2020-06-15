app.controller('approveFlatownerFamiliesController', [
    '$scope', '$rootScope', '$window', '$location', '$http', '$routeParams', '$anchorScroll', '$filter', 'DATA_URLS', 'appConstant', 'localStorageService', '$sce', 'authService', 'societyService',
function ($scope, $rootScope, $window, $location, $http, $routeParams, $anchorScroll, $filter, DATA_URLS, appConstant, localStorageService, $sce, authService, societyService) {

    'use strict';
    $scope.recordList = [];
    $scope.searchFormData = {};
    $scope.selectedList = [];
    $scope.searchFormData.isApproved = "";
    $scope.commentTableName = "FlatOwnerFamily";

    getList();

    function getList() {

        var filters = setfilters();
        var promise = $http.post(DATA_URLS.REPORT_FLATOWNERFAMILIES, filters);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.recordList = data;

                if ($scope.recordList.length == 0) {
                    toastr["info"]("No result found", "info");
                }

                angular.forEach($scope.recordList, function (v) {

                    v.userApproved = !!($filter('filter')(v.approvals, { approvedBy: authService.authentication.loginData.userName }, true)[0]);
                });
            }
        });
    }

    $scope.search = function () {
        getList();
    }

    function setfilters() {
        var searchFilter = {};

        var soc = societyService.getSociety();
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
        var soc = societyService.getSociety();
        var fdata = { id: $scope.selectedItem.id, societyId: soc.id, note: $scope.note };
        var promise = $http.post(DATA_URLS.APPROVE_FLATOWNERFAMILIES, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.recordList[$scope.selectedItem.index].userApproved = true;
                $scope.recordList[$scope.selectedItem.index].isApproved = data.isApproved;
                $scope.recordList[$scope.selectedItem.index].approvals.push({ approvedName: authService.authentication.loginData.userFullName });
                toastr["success"]("Flat owner family approved by you", "Success");
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
        var promise = $http.post(DATA_URLS.REJECT_FLATOWNERFAMILIES, fdata);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.recordList[$scope.selectedItem.index].isRejected = true;
                toastr["success"]("Flat owner family rejected", "Success");
                $('#cancelConfirmReject').click();
                clearNote();
            }
            else {
                $scope.recordList[index].isRejected = false;
            }
        });
    }

    $scope.approveAll = function () {
        var soc = societyService.getSociety();
        var fdata = { ids: $scope.selectedList, societyId: soc.id, note: $scope.note };
        var promise = $http.post(DATA_URLS.BULKAPPROVE_FLATOWNERFAMILIES, fdata);

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

    $scope.onSelectClick = function (item) {
        item.selected = !item.selected;
        selectMc();
    }

    $scope.onBackClick = function () {
        $window.history.back();
    }

    function clearNote() {
        $scope.note = null;
    };

}]);