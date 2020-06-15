app.controller('userGroupListController', ['$scope', 'authService', '$rootScope', '$window', '$location', 'DATA_URLS', '$http', 'appConstant',
function ($scope, authService, $rootScope, $window, $location, DATA_URLS, $http, appConstant) {

    'use strict';
    $scope.recordList = [];
    $scope.formData = {};

    getList();

    function getList() {

        var promise = $http.post(DATA_URLS.List_APPLICATIONROLELIST);
        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                $scope.recordList = data;
            }
        });
    }

    $scope.new = function (item) {
        $scope.formData = null;
    }

    $scope.edit = function (item) {
        $scope.formData = item;
        $scope.formData.isEdit = true;
    }

    $scope.onDelete = function (item, index) {
        $scope.selectedItem = item;
        $scope.selectedItem.index = index;
    }

    $scope.delete = function () {
        var promise = $http.post(DATA_URLS.DELETE_APPLICATIONROLE, $scope.selectedItem.id);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]("Role deleted", "Success");
                $scope.recordList.splice($scope.selectedItem.index, 1);
            }
            $('#cancelConfirmDelete').click();
        });
    }

    $scope.submit = function () {

        var fdata = angular.copy($scope.formData);
        var promise = $http.post(DATA_URLS.ADD_APPLICATIONROLE, fdata);

        promise.then(function (data) {
            if (!data.status || data.status == 200) {
                toastr["success"]($scope.formData.isEdit  ? "Role updated" : "New role created successfully", "Success");
                getList();
            }
            $('#cancelAddNew').click();
        });
    }
}]);