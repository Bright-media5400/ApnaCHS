app.controller('mastervalueListController', [
    '$scope', '$rootScope', '$window', '$location', '$http', '$routeParams', '$anchorScroll', '$filter', 'DATA_URLS', 'appConstant',
function ($scope, $rootScope, $window, $location, $http, $routeParams, $anchorScroll, $filter, DATA_URLS, appConstant) {

    'use strict';
    $scope.selectedItem = null;
    getDefaultValues();

    $scope.searchField = function () {
        getUDFList($scope.typeVal.id);
    }

    $scope.new = function () {
        $scope.selectedItem = {};
        if ($scope.typeVal) {
            $scope.selectedItem.type = $filter('filter')($scope.udfDefaultList, { id: $scope.typeVal.id })[0];
        }
    }

    $scope.editUDF = function (row) {

        $scope.selectedItem = angular.copy(row);
        $scope.selectedItem.isEdit = true;
        $scope.selectedItem.type = $filter('filter')(appConstant.MasterTypeList, { id: $scope.selectedItem.type })[0];
    }

    $scope.onDelete = function (row,index) {
        $scope.selectedItem = row;
        $scope.selectedItem.index = index;
    }

    $scope.delete = function (row) {

        var promise = $http.post(DATA_URLS.DELETE_MASTERVALUE, row);
        promise.then(function (data) {

            if (!data.status || data.status == 200) {
                
                $('#cancelConfirmDelete').click();
                toastr["success"]("Application master deleted", "Success");
                getUDFList(row.type);
            }
        });
    }

    $scope.onRestore = function (row, index) {
        $scope.selectedItem = row;
        $scope.selectedItem.index = index;
    }

    $scope.restore = function (row) {

        var promise = $http.post(DATA_URLS.UPDATE_UDFSTATUS, row);
        promise.then(function (data) {

            if (!data.status || data.status == 200) {

                $('#cancelConfirmRestore').click();
                toastr["success"]("Application master restored.", "Success");
                getUDFList(row.type);
            }
        });
    }

    $scope.onBackClick = function () {

    };

    $scope.submit = function () {

        var fdata = angular.copy($scope.selectedItem);
        fdata.type = fdata.type.id;

        var promise = $http.post(fdata.isEdit ? DATA_URLS.UPDATE_MASTERVALUE : DATA_URLS.NEW_MASTERVALUE, fdata);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {

                toastr["success"](fdata.isEdit ? "Master value updated" : "New master value created successfully", "Success");
                $('#cancelAddNew').click();
                getUDFList(data.type);
            }
        });
    };

    $scope.typeChage = function (type) {
    };

    function getDefaultValues() {
        $scope.udfDefaultList = appConstant.MasterTypeList;

        $scope.udfDefaultList.sort(sort_by(
            { name: 'text' }));
    }

    function getUDFList(id) {
        var promise = $http.post(DATA_URLS.GET_MASTERVALUELISTALL, id);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.udfList = data;
            }
        });
    };

    function InitVaribales() {
        $scope.name = "";
        $scope.description = "";
        $scope.isEditMode = false;
        $scope.customFieldList = [];
        $scope.isOutComeMeaurType = false;
    }

    $scope.onListTypeChange = function () {
        if ($scope.typeVal != undefined) {
            $scope.searchField();
        }
    }
}]);