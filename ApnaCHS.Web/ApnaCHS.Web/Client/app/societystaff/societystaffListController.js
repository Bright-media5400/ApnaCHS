app.controller('societystaffListController', ['$scope', 'authService', '$rootScope', '$window', '$location', 'DATA_URLS', '$http', '$routeParams', 'appConstant', '$filter',
function ($scope, authService, $rootScope, $window, $location, DATA_URLS, $http, $routeParams, appConstant,$filter) {

    'use strict';
    $scope.recordList = [];
    $scope.societyId = null;
    $scope.todayDate = new Date();
    getUDFList();

    if ($routeParams.sid != undefined) {
        $scope.societyId = $routeParams.sid;
        getList()
    }

    function getUDFList() {
                    var promise = $http.post(DATA_URLS.GET_MASTERVALUELISTALL, 4);
                    promise.then(function (data) {
                        if (!data.status || data.status == 200) {
                            $scope.udfList = data;
                        }
                    });
                };

    function getList() {

        var fdata = { societyId: $scope.societyId };
        var promise = $http.post(DATA_URLS.LIST_SOCIETYSTAFF, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.recordList = data;
            }
        });
    }

    $scope.submitEndDate = function () {

        var fdata = angular.copy($scope.editStaff);
        fdata.lastWorkingDay = $filter('date')(fdata.lastWorkingDay, appConstant.ConversionDateFormat);
        var promise = $http.post(DATA_URLS.UPDATE_STAFFLASTWORKINGDAY, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Society Staff last working day updated successfully", "Success");
                $('#cancelEditWorkingDayBox').click();
                getList();
            }
        });
    }

    $scope.submit = function () {

        var fdata = angular.copy($scope.editStaff);
        fdata.dateOfBirth = $filter('date')(fdata.dateOfBirth, appConstant.ConversionDateFormat);
        fdata.joiningDate = $filter('date')(fdata.joiningDate, appConstant.ConversionDateFormat);
        fdata.lastWorkingDay = $filter('date')(fdata.lastWorkingDay, appConstant.ConversionDateFormat);
        var promise = $http.post(DATA_URLS.UPDATE_SOCIETYSTAFF, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Society Staff updated successfully", "Success");
                $('#cancelEditStaffBox').click();
                getList();
            }
        });
    }

    $scope.edit = function (item) {
        $scope.editStaff = angular.copy(item);
        if ($scope.editStaff.dateOfBirth)
        $scope.editStaff.dateOfBirth = new Date($filter('date')($scope.editStaff.dateOfBirth, appConstant.ConversionDateFormat));
        $scope.editStaff.joiningDate = new Date($filter('date')($scope.editStaff.joiningDate, appConstant.ConversionDateFormat));
        if ($scope.editStaff.lastWorkingDay)
        $scope.editStaff.lastWorkingDay = new Date($filter('date')($scope.editStaff.lastWorkingDay, appConstant.ConversionDateFormat));
        $scope.editStaff.staffType = $filter('filter')($scope.udfList, { id: $scope.editStaff.staffType.id })[0];
    }

    //$scope.edit = function (item) {
    //    $location.path('societyStaff/details/' + item.id);
    //}

    $scope.onDelete = function (item, index) {
        $scope.selectedItem = item;
        $scope.selectedItem.index = index;
    }

    $scope.delete = function (item, index) {
        var promise = $http.post(DATA_URLS.DELETE_SOCIETYSTAFF, $scope.selectedItem.id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Society Staff deleted successfully", "Success");
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

            toexport.push({
                "ID": item.id,
                "Name": item.name
            });
        });

        data2xlsx({
            filename: "SocietyStaff List" + ".xlsx",
            sheetName: "SocietyStaffList",
            data: toexport
        });
    }
   
    $scope.onBackClick = function () {
        $window.history.back();
    }

    var onSocietyStaffCreate = $rootScope.$on('onSocietyStaffCreate', function (e, d) {
        toastr["success"]("Society staff created successfully", "Success");
        $('#cancelNewSocietystaffBox').click();
        getList($routeParams.sid);
    });

    $scope.$on('$destroy', function () {
        onSocietyStaffCreate();
    });
}]);
